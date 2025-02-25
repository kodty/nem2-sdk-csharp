using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Model.Network;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace io.nem2.sdk.src.Infrastructure.Mapping
{
    internal static class ObjectComposer
    {
        internal static T GenerateObject<T>(string data)
        {
            return (T)GenerateObject(typeof(T), JToken.Parse(data));
        }

        internal static object GenerateObject(Type type, JToken jObject)
        {
            var actualObject = Activator.CreateInstance(type);

            var nameToValueMap = GetPropNamesValues(type, jObject);

            return ValueMapToObject(nameToValueMap, actualObject, type);
        }

        private static Dictionary<string, object> GetPropNamesValues(Type obj, JToken objList, bool recursion = false)
        {
            Dictionary<string, object> nameToValueMap = new Dictionary<string, object>();

            obj?.GetProperties().ToList().ForEach(op =>
            {
                var lwrCase = (char.ToLower(op.Name[0]) + op.Name.Substring(1)).ToString();

                if (!nameToValueMap.ContainsKey(op.Name) && ((op.PropertyType == typeof(ushort) || op.PropertyType == typeof(int) || op.PropertyType == typeof(ulong) || op.PropertyType == typeof(string) || op.PropertyType == typeof(List<string>) || op.PropertyType == typeof(bool))))
                {
                    nameToValueMap.Add(op.Name, GetTypedValue(op.PropertyType, objList, lwrCase));
                    return;
                }
                else if (!nameToValueMap.ContainsKey(op.Name))
                {
                    if (objList.Contains(lwrCase))
                    {                  
                        var o = objList.Children().ToList().First(i => {

                            return i.Path == lwrCase || (i.Path == i.Parent.Path + "." + lwrCase);

                        });

                        if (o.Path == lwrCase || o.Path == o.Parent.Path + "." + lwrCase)
                        {
                            nameToValueMap.Add(op.Name, GenerateObject(op.PropertyType, o.Single()));
                            return;
                        }
                    }
                    else if(!nameToValueMap.ContainsKey(op.Name) && !objList.Contains(lwrCase))
                    {                   
                        foreach (var obj in objList.Children().Children())
                        {
                           if (obj.Path.Contains(lwrCase))
                           {                       
                                nameToValueMap.Add(op.Name, GenerateObject(op.PropertyType, obj));
                                break;
                           }          
                        }  
                    }                 
                }
            });

            return nameToValueMap;
        }

        private static dynamic? GetTypedValue(Type type, JToken ob, string path = null)
        {
            if (type == typeof(int))
            {
                return (int)ob[path];
            }
            if (type == typeof(ushort))
            {
                return (ushort)((JProperty)ob[path]).Value;
            }
            if (type == typeof(ulong))
            {
                return (ulong)((JProperty)ob[path]).Value;
            }
            if (type == typeof(string))
            {
                return (string)(ob[path]);
            }
            if (type == typeof(bool))
            {
                return (bool)ob[path];
            }
            if (type == typeof(List<string>))
            {
                return ob[path].Values<string>().ToList();
            }
            if (type == typeof(NetworkType.Types))
            {
                return NetworkType.GetRawValue((ushort)((JProperty)ob[path]).Value);
            }
            if (type == typeof(TransactionTypes.Types))
            {
                return TransactionTypes.GetRawValue((ushort)((JProperty)ob[path]).Value);
            }
            throw new NotImplementedException(typeof(Type).ToString());
        }

        internal static object ValueMapToObject(Dictionary<string, object> nameToValueMap, object actualObject, Type type)
        {
            foreach (var prop in nameToValueMap)
            {

                var actualObjProp = actualObject.GetType().GetProperties()?
                      .First(m =>
                      {
                          return (char.ToLower(m.Name[0]) + m.Name.Substring(1)).ToString() == (char.ToLower(prop.Key[0]) + prop.Key.Substring(1)).ToString();
                      });

                actualObjProp.SetValue(actualObject, prop.Value);
            }

            return Convert.ChangeType(actualObject, type);
        }
    }
}
