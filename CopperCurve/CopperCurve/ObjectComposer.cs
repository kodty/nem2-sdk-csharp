using System.Collections;
using System.Reflection;
using System.Text.Json.Nodes;

namespace CopperCurve
{
    public class ObjectComposer
    {
        private object[] TypeArgs { get; set; }

        internal Func<string, bool, Type> GetTransactionType { get; set; }

        public ObjectComposer(object[] args, Func<string, bool, Type> getTransactionType)
        {
            TypeArgs = args;
            GetTransactionType = getTransactionType;
        }

        public List<T> FilterEvents<T>(string data, string path = null)
        {
            var evs = path == null ? JsonNode.Parse(data) : JsonNode.Parse(data)[path];

            List<T> events = new List<T>();

            foreach (var e in evs.AsArray())
            {
                events.Add(GenerateObject(typeof(T), e.AsObject()));
            }

            return events;
        }

        public List<T> FilterTransactions<T>(string data, string path = null, bool embedded = false)
        {
            var tx = path == null ? JsonNode.Parse(data) : JsonNode.Parse(data)[path];

            List<T> txs = new List<T>();

            foreach (var t in tx.AsArray())
            {
                txs.Add(FilterSingle(typeof(T), t.ToString(), embedded));
            }

            return txs;
        }

        public T FilterSingle<T>(string data, bool embedded = false)
        {
            return FilterSingle(typeof(T), data, embedded);
        }

        public T GenerateObject<T>(string data)
        {
            return (T)GenerateObject(typeof(T), JsonObject.Parse(data));
        }

        private dynamic GenerateObject(Type type, JsonNode jObject)
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

        private dynamic ValueMapToObject(Dictionary<string, object> nameToValueMap, object actualObject, Type type)
        {
            foreach (var prop in nameToValueMap)
            {
                var actualObjProp = actualObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)?
                      .First(m =>
                      {
                          return (char.ToLower(m.Name[0]) + m.Name.Substring(1)).ToString() == (char.ToLower(prop.Key[0]) + prop.Key.Substring(1)).ToString();
                      });

                actualObjProp.SetValue(actualObject, prop.Value);
            }

            return Convert.ChangeType(actualObject, type);
        }

        private dynamic FilterSingle(Type genType, string data, bool embedded = false)
        {
            var tx = JsonObject.Parse(data).AsObject();

            var type = GetTransactionType(data, embedded);

            dynamic shell = GenerateObject(genType, tx.AsObject());

            shell.Transaction = GenerateObject(type, tx["transaction"].AsObject());

            return shell;
        }

        private IList GetListTypeValue(Type type, JsonNode ob, string path)
        {
            var values = (IList)Activator.CreateInstance(type);

            var genType = type.GetGenericArguments().SingleOrDefault();

            if (ob.AsObject().ContainsKey(path))
            {
                if (genType.Name == "EmbeddedTransactionData")
                {
                    var tx = path == null ? ob.AsArray() : ob[path];

                    foreach (var t in tx.AsArray())
                        values.Add(FilterSingle(genType, t.ToString(), true));

                    return values;
                }

                foreach (var item in ob[path].AsArray())
                {
                    if (genType.IsPrimitive)
                    {
                        values.Add(Convert.ChangeType(item.ToString(), genType));
                    }
                    if (genType == typeof(string))
                    {
                        values.Add((string)item);
                    }
                    else if (!genType.IsPrimitive && genType != typeof(string))
                    {
                        values.Add(GenerateObject(genType, item.AsObject()));
                    }
                }
            }

            return values;
        }

        private bool IsNativeProperty(System.Reflection.PropertyInfo op)
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

            if (TypeArgs.Contains(type.GetGenericArguments().SingleOrDefault()))
                return GetListTypeValue(type, ob, path);

            else throw new NotImplementedException(type.ToString());
        }
    }
}
