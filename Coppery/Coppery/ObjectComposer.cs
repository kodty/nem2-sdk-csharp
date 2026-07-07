using System.Collections;
using System.Text.Json.Nodes;

namespace Coppery
{
    public class ObjectComposer
    {
        internal int Depth = 0;

        public Func<dynamic, Type, ObjectComposer, JsonNode, dynamic> Function { get; set; }

        public ObjectComposer(Func<dynamic, Type, ObjectComposer, JsonNode, dynamic> function)
        {
            Function = function;
        }

        public ObjectComposer()
        {

        }

        public T GenerateObject<T>(JsonNode ob)
        {
            return (T)GenerateObject(typeof(T), ob);
        }
        
        public dynamic GenerateObject(Type type, JsonNode ob)
        {
            var actualObject = Activator.CreateInstance(type);

            actualObject.GetType().GetProperties().ToList().ForEach(op =>
            {               
                var path = char.ToLower(op.Name[0]) + op.Name.Substring(1);

                if (!ob.AsObject().ContainsKey(path)) 
                    return;

                if (op.PropertyType.IsGenericType && op.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    var values = (IList)Activator.CreateInstance(op.PropertyType);

                    var argType = op.PropertyType.GetGenericArguments().SingleOrDefault();

                    foreach (var item in ob[path].AsArray())
                        values.Add(Convert_Compose(argType, item));

                    op.SetValue(actualObject, values);

                    return;
                }
            
                op.SetValue(actualObject, Convert_Compose(op.PropertyType, ob[path]));
                                                           
            });

            return actualObject;
        }    
        
        private dynamic Convert_Compose(Type type, JsonNode ob)
        {
            if (type.IsPrimitive || type == typeof(string))
            {
                return Convert.ChangeType(ob.ToString(), type);
            }
            else
            {
                var comp_o = GenerateObject(type, ob);

                if (Function != null)
                    comp_o = Function(comp_o, type, this, ob);

                return comp_o;
            }
        }    
    }
}