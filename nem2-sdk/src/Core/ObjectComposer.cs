using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Collections;
using System.Text.Json.Nodes;

namespace io.nem2.sdk.src.Export
{
    internal class ObjectComposer
    {
        internal object[] TypeArgs { get; set; }

        Func<string, bool, Type> GetTransactionType { get; set; }

        internal ObjectComposer(object[] args)
        {
            TypeArgs = args;
        }

        internal ObjectComposer(object[] args, Func<string, bool, Type> getTransactionType)
        {
            TypeArgs = args;
            GetTransactionType = getTransactionType;
        }

        internal T GenerateObject<T>(string data)
        {
            return (T)GenerateObject(typeof(T), JsonObject.Parse(data));
        }

        internal dynamic GenerateObject(Type type, JsonNode jObject)
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
                var lwrCase = (char.ToLower(op.Name[0]) + op.Name.Substring(1)).ToString();

                if (!nameToValueMap.ContainsKey(op.Name))
                {
                    if (IsNativeProperty(op))
                    {
                        
                        nameToValueMap.Add(op.Name, GetTypedValue(op.PropertyType, objList.AsObject(), lwrCase));
                        return;
                    }
                    else
                    {                       
                        foreach (var obj in objList.AsObject())
                        {
                            if (obj.Key.Contains(lwrCase))
                            {
                                nameToValueMap.Add(op.Name, GenerateObject(op.PropertyType, obj.Value));
                                break;
                            }
                        }
                    }
                }
            });

            return nameToValueMap;
        }
        internal dynamic ValueMapToObject(Dictionary<string, object> nameToValueMap, object actualObject, Type type)
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

        private dynamic GetEmbeddedListType(JsonNode ob, string path)
        {
            return new ResponseFilters<EmbeddedTransactionData>(TypeArgs).FilterTransactions(GetTransactionType, ob.ToString(), path, true);
        }

        private IList GetListTypeValue(Type type, JsonNode ob, string path)
        {     
            var values = (IList)Activator.CreateInstance(type);

            foreach (var item in ob[path].AsArray())
            {
                var t = type.GetGenericArguments().SingleOrDefault();

                if (type.IsPrimitive)
                    values.Add(Convert.ChangeType(item.ToString(), t));

                if (t == typeof(string))
                    values.Add((string)item);

                else values.Add(GenerateObject(type.GetGenericArguments().SingleOrDefault(), item.AsObject()));
            }

            return values;
        }

        private bool IsNativeProperty(System.Reflection.PropertyInfo op)
        {
            if (TypeArgs.Contains(op.PropertyType)) return true;

            foreach(var arg in TypeArgs)
            {
                if (op.PropertyType.GetGenericArguments().Contains(arg))
                {
                    return true;
                }
            }

            return false;
        }

        private dynamic? GetTypedValue(Type type, JsonObject ob, string path)
        {
            if (type.IsPrimitive)
                return Convert.ChangeType(ob[path].ToString(), type);

            if (type == typeof(string))
                return (string)ob[path];

            if (type.GetGenericArguments().SingleOrDefault().Name == "EmbeddedTransactionData")
                return GetEmbeddedListType(ob, path);
               
            if (TypeArgs.Contains(type.GetGenericArguments().SingleOrDefault()))
                return GetListTypeValue(type, ob, path);

            else throw new NotImplementedException(type.ToString());
        }
    }
}
