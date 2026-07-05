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

                if (op.PropertyType.IsPrimitive)
                {
                    keyValueMap.Add(op.Name.ToLower(), Convert.ChangeType(ob[path].ToString(), op.PropertyType));
                    return;
                }

                if (op.PropertyType == typeof(string))
                {
                    keyValueMap.Add(op.Name.ToLower(), (string)ob[path]);
                    return;
                }

                if (op.PropertyType.IsGenericType && op.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    var values = (IList)Activator.CreateInstance(op.PropertyType);

                    var argType = op.PropertyType.GetGenericArguments().SingleOrDefault();

                    if (ob.AsObject().ContainsKey(path))
                    {
                        foreach (var item in ob[path].AsArray())
                        {
                            if (argType.IsPrimitive || argType == typeof(string))
                            {
                                values.Add(Convert.ChangeType(item.ToString(), argType));
                            }
                            else
                            {
                                var T = GenerateWithCustomPostProcessing(argType, item);

                                values.Add(T);
                            }
                        }
                    }

                    keyValueMap.Add(op.Name.ToLower(), values);
                    return;
                }

                if (ob.AsObject().ContainsKey(path))
                {
                    var T = GenerateWithCustomPostProcessing(op.PropertyType, ob[path]);

                    keyValueMap.Add(op.Name.ToLower(), T);    
                }
          
                return;   
            });

            return keyValueMap;
        }    

        private dynamic GenerateWithCustomPostProcessing(Type argType, JsonNode item)
        {
            var T = GenerateObject(argType, item);

            if (Function != null)
                T = Function(T, argType, this, item);

            return T;
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