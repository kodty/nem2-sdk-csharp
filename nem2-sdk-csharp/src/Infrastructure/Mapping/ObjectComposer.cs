using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Infrastructure.Buffers.Model;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Model.Network;
using Newtonsoft.Json.Linq;



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

        private static Dictionary<string, object> GetPropNamesValues(Type type, JToken objList)
        {
            Dictionary<string, object> nameToValueMap = new Dictionary<string, object>();

            type?.GetProperties().ToList().ForEach(op =>
            {
                var lwrCase = (char.ToLower(op.Name[0]) + op.Name.Substring(1)).ToString();

                if (!nameToValueMap.ContainsKey(op.Name))
                {

                    if (IsNativeProperty(op))
                    {
                        nameToValueMap.Add(op.Name, GetTypedValue(op.PropertyType, objList, lwrCase));
                        return;
                    }
                    else
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

        private static List<EmbeddedTransactionData> GetEmbeddedListType(Type type, JToken ob, string path)
        {
            List<EmbeddedTransactionData> events = new List<EmbeddedTransactionData>();

            if (ob[path] != null) foreach (var e in ob[path])
                {
                    events.Add(ResponseFilters<EmbeddedTransactionData>.FilterSingle(e.ToString()));
                }

            return events;     
        }

        private static List<T> GetListTypeValue<T>(Type type, JToken ob, string path)
        {
            if(typeof(T) == typeof(string))
            {              
                  return ob[path].Values<T>().ToList();
            }
            else
            {
                List<T> events = new List<T>();

                if(ob[path] != null) foreach (var e in ob[path])
                {
                    events.Add((T)GenerateObject(typeof(T), e));
                }

                return events;
            }
        }

        private static bool IsNativeProperty(System.Reflection.PropertyInfo op)
        {
            if ((op.PropertyType == typeof(ushort)
                || op.PropertyType == typeof(int)
                || op.PropertyType == typeof(ulong)
                || op.PropertyType == typeof(string)
                || op.PropertyType == typeof(List<string>)
                || op.PropertyType == typeof(List<ActivityBucket>)
                || op.PropertyType == typeof(List<VotingKeys>)
                || op.PropertyType == typeof(List<MosaicTransfer>)
                || op.PropertyType == typeof(List<MessageGroup>)
                || op.PropertyType == typeof(List<Signature>)
                || op.PropertyType == typeof(List<Tree>)
                || op.PropertyType == typeof(List<LinkBit>)
                || op.PropertyType == typeof(List<RestrictionData>)
                || op.PropertyType == typeof(List<Restrictions>)
                || op.PropertyType == typeof(List<MosaicEvent>)
                || op.PropertyType == typeof(List<MosaicName>)
                || op.PropertyType == typeof(List<AccountName>) 
                || op.PropertyType == typeof(List<ReceiptDatum>)
                || op.PropertyType == typeof(List<AddressDatum>)
                || op.PropertyType == typeof(List<MosaicDatum>) 
                || op.PropertyType == typeof(List<Receipt>)
                || op.PropertyType == typeof(List<ResolutionEntry>)
                || op.PropertyType == typeof(List<MRestrictionData>)
                || op.PropertyType == typeof(List<Cosignature>)
                || op.PropertyType == typeof(List<EmbeddedTransactionData>)
                || op.PropertyType == typeof(List<MosaicRestriction>)
                || op.PropertyType == typeof(bool)
                || op.PropertyType == typeof(TransactionTypes.Types)
                || op.PropertyType == typeof(NetworkType.Types)))
            {
                return true;
            }
            else return false;
        }

        private static dynamic? GetTypedValue(Type type, JToken ob, string path = null)
        {
            if (type == typeof(int))
            {
                return (int)ob[path];
            }
            if (type == typeof(ushort))
            {
                return (ushort)ob[path];
            }
            if (type == typeof(ulong))
            {
                return (ulong)ob[path];
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
                return GetListTypeValue<string>(type, ob, path);
            }
            if (type == typeof(List<ActivityBucket>))
            {
                return GetListTypeValue<ActivityBucket>(type, ob, path);
            }
            if (type == typeof(List<MosaicTransfer>))
            {
                return GetListTypeValue<MosaicTransfer>(type, ob, path);
            }
            if (type == typeof(List<MosaicEvent>))
            {
                return GetListTypeValue<MosaicEvent>(type, ob, path);
            }
            if (type == typeof(List<MessageGroup>))
            {
                return GetListTypeValue<MessageGroup>(type, ob, path);
            }
            if (type == typeof(List<Signature>))
            {
                return GetListTypeValue<Signature>(type, ob, path);
            }
            if (type == typeof(List<Tree>))
            {
                return GetListTypeValue<Tree>(type, ob, path);
            }
            if (type == typeof(List<LinkBit>))
            {
                return GetListTypeValue<LinkBit>(type, ob, path);
            }
            if (type == typeof(List<RestrictionData>))
            {
                return GetListTypeValue<RestrictionData>(type, ob, path);
            }
            if (type == typeof(List<Restrictions>))
            {
                return GetListTypeValue<Restrictions>(type, ob, path);
            }
            if (type == typeof(List<MosaicName>)) 
            {
                return GetListTypeValue<MosaicName>(type, ob, path);
            }
            if (type == typeof(List<AccountName>)) 
            {
                return GetListTypeValue<AccountName>(type, ob, path);
            }
            if (type == typeof(List<ReceiptDatum>))
            {
                return GetListTypeValue<ReceiptDatum>(type, ob, path);
            }
            if (type == typeof(List<Receipt>))
            {
                return GetListTypeValue<Receipt>(type, ob, path);
            }
            if (type == typeof(List<AddressDatum>))
            {
                return GetListTypeValue<AddressDatum>(type, ob, path);
            }
            if (type == typeof(List<ResolutionEntry>))
            {
                return GetListTypeValue<ResolutionEntry>(type, ob, path);
            }
            if (type == typeof(List<MosaicDatum>))
            {
                return GetListTypeValue<MosaicDatum>(type, ob, path);
            }
            if (type == typeof(List<MRestrictionData>)) 
            {
                return GetListTypeValue<MRestrictionData>(type, ob, path);
            }
            if (type == typeof(List<MosaicRestriction>))
            {
                return GetListTypeValue<MosaicRestriction>(type, ob, path);
            }
            if (type == typeof(List<Cosignature>))
            {
                return GetListTypeValue<Cosignature>(type, ob, path);
            }
            if (type == typeof(List<VotingKeys>))
            {
                return GetListTypeValue<VotingKeys>(type, ob, path);
            }
            if (type == typeof(List<EmbeddedTransactionData>))
            {
                return GetEmbeddedListType(type, ob, path);
            }
            if (type == typeof(NetworkType.Types))
            {
                return NetworkType.GetRawValue((ushort)ob[path]);
            }
            if (type == typeof(TransactionTypes.Types))
            {
                return TransactionTypes.GetRawValue((ushort)ob[path]);
            }
            throw new NotImplementedException(type.ToString());
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
