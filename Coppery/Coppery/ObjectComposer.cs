using System.Collections;
using System.Reflection;
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

        public T GenerateObject<T>(string data)
        {
            return (T)GenerateObject(typeof(T), JsonObject.Parse(data));
        }

        public dynamic GenerateObject(Type type, JsonNode ob)
        {
            if (Depth < 64)
            {
                var actualObject = Activator.CreateInstance(type);

                    Depth++;

                var nameToValueMap = GetPropNamesValues(type, ob);

                    Depth--;

                return ValueMapToObject(nameToValueMap, actualObject, type);         
            }
            else throw new Exception("Max depth limit exceeded");
        }

        private Dictionary<string, object> GetPropNamesValues(Type type, JsonNode ob)
        {
            Dictionary<string, object> keyValueMap = new Dictionary<string, object>();

            type?.GetProperties().ToList().ForEach(op =>
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
                    
                    keyValueMap.Add(op.Name.ToLower(), values);

                    return;
                }
                
                keyValueMap.Add(op.Name.ToLower(), Convert_Compose(op.PropertyType, ob[path]));    
                     
                return;   
            });

            return keyValueMap;
        }    
        
        private dynamic Convert_Compose(Type type, JsonNode ob)
        {
            if (type.IsPrimitive || type == typeof(string))
            {
                return Convert.ChangeType(ob.ToString(), type);
            }
            else
            {
                 return GenerateWithCustomPostProcessing(type, ob);
            }
        }
        private dynamic GenerateWithCustomPostProcessing(Type type, JsonNode item)
        {
            var comp_o = GenerateObject(type, item);

            if (Function != null)
                comp_o = Function(comp_o, type, this, item);

            return comp_o;
        }

        private dynamic ValueMapToObject(Dictionary<string, object> keyValueMap, object actualObject, Type type)
        {
            foreach (var prop in keyValueMap)
            {
                var actualObjProp = actualObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)?
                      .First(m =>
                      {
                          return m.Name.ToLower() == prop.Key.ToLower();
                      });

                actualObjProp.SetValue(actualObject, prop.Value);
            }

            return Convert.ChangeType(actualObject, type);
        }     
    }
}