using System.Collections;
using System.Reflection;
using System.Text.Json.Nodes;

namespace Coppery
{
    public class ObjectComposer
    {
        public Func<dynamic, Type, ObjectComposer, JsonNode, dynamic> GetEmbedded { get; set; }

        public ObjectComposer(Func<dynamic, Type, ObjectComposer, JsonNode, dynamic> getEmbedded)
        {
            GetEmbedded = getEmbedded;
        }

        public ObjectComposer()
        {
        }

        public T GenerateObject<T>(string data)
        {
            return (T)GenerateObject(typeof(T), JsonObject.Parse(data));
        }

        public dynamic GenerateObject(Type type, JsonNode jObject)
        {
            var actualObject = Activator.CreateInstance(type);

            var nameToValueMap = GetPropNamesValues(type, jObject);

            return ValueMapToObject(nameToValueMap, actualObject, type);
        }

        private Dictionary<string, object> GetPropNamesValues(Type type, JsonNode ob)
        {
            Dictionary<string, object> nameToValueMap = new Dictionary<string, object>();

            type?.GetProperties().ToList().ForEach(op =>
            {               
                var path = char.ToLower(op.Name[0]) + op.Name.Substring(1);

                if (op.PropertyType.IsPrimitive)
                {
                    nameToValueMap.Add(op.Name.ToLower(), Convert.ChangeType(ob[path].ToString(), op.PropertyType));
                    return;
                }

                if (op.PropertyType == typeof(string))
                {
                    nameToValueMap.Add(op.Name.ToLower(), (string)ob[path]);
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
                                var T = GenerateObject(argType, item.AsObject());

                                if (GetEmbedded != null)
                                    T = GetEmbedded(T, argType, this, item);

                                values.Add(T);
                            }
                        }
                    }

                    nameToValueMap.Add(op.Name.ToLower(), values);
                    return;
                }

                if(ob.AsObject().ContainsKey(path))
                    nameToValueMap.Add(op.Name.ToLower(), GenerateObject(op.PropertyType, ob[path]));

                return;   
            });

            return nameToValueMap;
        }

        private dynamic ValueMapToObject(Dictionary<string, object> nameToValueMap, object actualObject, Type type)
        {
            foreach (var prop in nameToValueMap)
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