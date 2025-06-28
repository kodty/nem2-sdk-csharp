using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Infrastructure.Buffers.Model;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.src.Model.Network;
using System.Text.Json.Nodes;

namespace io.nem2.sdk.src.Export
{
    internal class ObjectComposer
    {
        internal Type[] TypeArgs { get; set; }

        internal ObjectComposer(Type[] args)
        {
            TypeArgs = args;
        }
        internal T GenerateObject<T>(string data)
        {
            return (T)GenerateObject(typeof(T), JsonObject.Parse(data));
        }

        internal object GenerateObject(Type type, JsonNode jObject)
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
                        nameToValueMap.Add(op.Name, GetTypedValue(op.PropertyType, objList, lwrCase));
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
        internal object ValueMapToObject(Dictionary<string, object> nameToValueMap, object actualObject, Type type)
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

        private List<RestrictionTypes.Types> ExtractRestrictionFlags(JsonNode ob, string path)
        {
            var values = new List<RestrictionTypes.Types>();

            int actualInt = (int)ob[path];

            char[] actualBitwise = Convert.ToString(actualInt, 2).PadLeft(16, '0').ToCharArray(0, 16);

            for (var x = 0; x < actualBitwise.Length; x++)
            {
                if (actualBitwise[x] == '1')
                {
                    string bitwiseType = new string('0', x) + '1' + new string('0', actualBitwise.Length - (1 + x));

                    values.Add(Convert.ToInt32(bitwiseType, 2).GetRawValue());
                }
            }

            return values;
        }

        private List<EmbeddedTransactionData> GetEmbeddedListType(JsonNode ob, string path)
        {
            List<EmbeddedTransactionData> embeddedTransactions = new List<EmbeddedTransactionData>();

            if (ob[path] != null) foreach (var e in ob[path].AsArray())
                    embeddedTransactions.Add(ResponseFilters<EmbeddedTransactionData>.FilterSingle(e.ToString()));

            return embeddedTransactions;
        }

        private List<T> GetListTypeValue<T>(JsonNode ob, string path)
        {
            if (typeof(T) == typeof(string) || typeof(T) == typeof(int))
                return ob[path].AsArray().GetValues<T>().ToList();

            else
            {
                List<T> events = new List<T>();

                if (ob[path] != null) foreach (var e in ob[path].AsArray())
                        events.Add((T)GenerateObject(typeof(T), e.AsObject()));

                return events;
            }
        }

        private List<TransactionTypes.Types> ExtractTransactionTypes(JsonNode ob, string path)
        {
            List<TransactionTypes.Types> types = new List<TransactionTypes.Types>();

            if (ob[path] != null) foreach (var e in ob[path].AsArray())
                    types.Add(((ushort)e).GetRawValue());

            return types;
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

        private dynamic? GetTypedValue(Type type, JsonNode ob, string path = null)
        {
            if (type == typeof(ushort))
                return UInt16.Parse(ob[path].ToString());

            if (type == typeof(int))
                return Int32.Parse(ob[path].ToString());

            if ( type == typeof(uint))
                return UInt32.Parse(ob[path].ToString());

            if (type == typeof(ulong))
                return UInt64.Parse(ob[path].ToString());

            if (type == typeof(string))
                return (string)ob[path];

            if (type == typeof(bool))
                return (bool)ob[path];

            if (type == typeof(List<string>))
                return GetListTypeValue<string>(ob, path);

            if (type == typeof(List<int>))
                return GetListTypeValue<int>(ob, path);

            if (type == typeof(List<ActivityBucket>))
                return GetListTypeValue<ActivityBucket>(ob, path);

            if (type == typeof(List<MosaicTransfer>))
                return GetListTypeValue<MosaicTransfer>(ob, path);

            if (type == typeof(List<MosaicEvent>))
                return GetListTypeValue<MosaicEvent>(ob, path);

            if (type == typeof(List<MessageGroup>))
                return GetListTypeValue<MessageGroup>(ob, path);

            if (type == typeof(List<Signature>))
                return GetListTypeValue<Signature>(ob, path);

            if (type == typeof(List<Tree>))
                return GetListTypeValue<Tree>(ob, path);

            if (type == typeof(List<LinkBit>))
                return GetListTypeValue<LinkBit>(ob, path);

            if (type == typeof(List<RestrictionData>))
                return GetListTypeValue<RestrictionData>(ob, path);

            if (type == typeof(List<Restrictions>))
                return GetListTypeValue<Restrictions>(ob, path);

            if (type == typeof(List<MosaicName>))
                return GetListTypeValue<MosaicName>(ob, path);

            if (type == typeof(List<AccountName>))
                return GetListTypeValue<AccountName>(ob, path);

            if (type == typeof(List<ReceiptDatum>))
                return GetListTypeValue<ReceiptDatum>(ob, path);

            if (type == typeof(List<Receipt>))
                return GetListTypeValue<Receipt>(ob, path);

            if (type == typeof(List<AddressDatum>))
                return GetListTypeValue<AddressDatum>(ob, path);

            if (type == typeof(List<ResolutionEntry>))
                return GetListTypeValue<ResolutionEntry>(ob, path);

            if (type == typeof(List<MosaicDatum>))
                return GetListTypeValue<MosaicDatum>(ob, path);

            if (type == typeof(List<MosaicRestrictionData>))
                return GetListTypeValue<MosaicRestrictionData>(ob, path);

            if (type == typeof(List<MosaicRestriction>))
                return GetListTypeValue<MosaicRestriction>(ob, path);

            if (type == typeof(List<Cosignature>))
                return GetListTypeValue<Cosignature>(ob, path);

            if (type == typeof(List<VotingKeys>))
                return GetListTypeValue<VotingKeys>(ob, path);

            if (type == typeof(List<EmbeddedTransactionData>))
                return GetEmbeddedListType(ob, path);

            if (type == typeof(NetworkType.Types))
                return NetworkType.GetRawValue((ushort)ob[path]);

            if (type == typeof(List<RestrictionTypes.Types>))
                return ExtractRestrictionFlags(ob, path);

            if (type == typeof(List<TransactionTypes.Types>))
                return ExtractTransactionTypes(ob, path);

            if (type == typeof(TransactionTypes.Types))
                return ((ushort)ob[path]).GetRawValue();

            else throw new NotImplementedException(type.ToString());
        }
    }
}
