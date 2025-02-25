using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Model.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;

namespace io.nem2.sdk.src.Infrastructure.Mapping
{
    internal static class ObjectComposer2
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

        private static Dictionary<string, object> GetPropNamesValues(Type obj, JToken objList)
        {
            Dictionary<string, object> nameToValueMap = new Dictionary<string, object>();

            var oProp = obj?.GetProperties().ToList();

            

            oProp.ForEach(o => {

                var lwrCase = (char.ToLower(o.Name[0]) + o.Name.Substring(1)).ToString();

                if ((o.PropertyType == typeof(ushort) || o.PropertyType == typeof(int) || o.PropertyType == typeof(ulong) || o.PropertyType == typeof(string) || o.PropertyType == typeof(List<string>) || o.PropertyType == typeof(bool)))
                {
                    nameToValueMap.Add(o.Name, GetTypedValue(o.PropertyType, (JProperty)objList[lwrCase]));
                }
            });
            
            return nameToValueMap;
        }

        private static dynamic? GetTypedValue(Type type, JProperty ob)
        {
            if (type == typeof(int))
            {
                return (int)ob.Value;
            }
            if (type == typeof(ushort))
            {
                return (ushort)ob.Value;
            }
            if (type == typeof(ulong))
            {
                return (ulong)ob.Value;
            }
            if (type == typeof(string))
            {
                return (string)ob.Value;
            }
            if (type == typeof(bool))
            {
                return (bool)ob.Value;
            }
            if (type == typeof(List<string>))
            {
                return ob.Value.Values<string>().ToList();
            }
            if (type == typeof(NetworkType.Types))
            {
                return NetworkType.GetRawValue((ushort)ob.Value);
            }
            if (type == typeof(TransactionTypes.Types))
            {
                return TransactionTypes.GetRawValue((ushort)ob.Value);
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
                          var rootKey = prop.Key.Substring(prop.Key.LastIndexOf('.') + 1);

                          return (char.ToLower(m.Name[0]) + m.Name.Substring(1)).ToString() == rootKey;
                      });
                Debug.WriteLine(actualObjProp.PropertyType);
                actualObjProp.SetValue(actualObject, prop.Value);
            }

            return Convert.ChangeType(actualObject, type);
        }
    }
}
