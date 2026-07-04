using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json.Nodes;

namespace Coppery
{
    public class ObjectComposer
    {
        private Type[] TypeArgs { get; set; }

        public Func<dynamic, Type, ObjectComposer, JsonNode, dynamic> GetEmbedded { get; set; }

        public ObjectComposer(Type[] args, Func<dynamic, Type, ObjectComposer, JsonNode, dynamic> getEmbedded)
        {
            TypeArgs = args;
            GetEmbedded = getEmbedded;
        }

        public ObjectComposer(Type[] args)
        {
            TypeArgs = args;
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

        private Dictionary<string, object> GetPropNamesValues(Type type, JsonNode objList)
        {
            Dictionary<string, object> nameToValueMap = new Dictionary<string, object>();

            type?.GetProperties().ToList().ForEach(op =>
            {
                if (!nameToValueMap.ContainsKey(op.Name.ToLower()))
                {
                    
                    if (IsNativeProperty(op))
                    {
                        nameToValueMap.Add(op.Name.ToLower(), GetTypedValue(op.PropertyType, objList.AsObject(), char.ToLower(op.Name[0]) + op.Name.Substring(1)));
                        return;
                    }
                    else
                    {                       
                        foreach (var obj in objList.AsObject())
                        {
                            if (obj.Key.ToLower().Contains(op.Name.ToLower()))
                            {
                                nameToValueMap.Add(op.Name.ToLower(), GenerateObject(op.PropertyType, obj.Value));
                                break;
                            }
                        }
                    }
                }
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

        private IList GetListTypeValue(Type type, JsonNode ob, string path)
        {
            var values = (IList)Activator.CreateInstance(type);

            var genType = type.GetGenericArguments().SingleOrDefault();

            if (ob.AsObject().ContainsKey(path))
            {
                foreach (var item in ob[path].AsArray())
                {
                    if (genType.IsPrimitive || genType == typeof(string))
                    {
                        values.Add(Convert.ChangeType(item.ToString(), genType));
                    }    
                    else if (!genType.IsPrimitive && genType != typeof(string))
                    {
                        var T = GenerateObject(genType, item.AsObject());

                        if(GetEmbedded != null)
                            T = GetEmbedded(T, genType, this, item);

                        values.Add(T);
                    }
                }
            }

            return values;
        }

        private bool IsNativeProperty(PropertyInfo op)
        {
            if (TypeArgs.Contains(op.PropertyType) || TypeArgs.Contains(op.PropertyType.GetGenericArguments().SingleOrDefault()))
                return true;

            else return false;
        }

        private dynamic? GetTypedValue(Type type, JsonObject ob, string path)
        {
            if (type.IsPrimitive)
                return Convert.ChangeType(ob[path].ToString(), type);             

            if (type == typeof(string))
                return (string)ob[path];

            
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                return GetListTypeValue(type, ob, path);

            if (TypeArgs.Contains(type))
                return GenerateObject(type, ob[path]);
                
            else throw new NotImplementedException(type.ToString());
        }
    }
}