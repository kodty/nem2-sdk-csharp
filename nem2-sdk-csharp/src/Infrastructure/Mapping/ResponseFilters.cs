using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using io.nem2.sdk.src.Infrastructure.Buffers.Model.JsonConverters;


namespace io.nem2.sdk.src.Infrastructure.Mapping
{
    internal static class ResponseFilters<T> where T : class
    {
        public static T GetTransaction(TransactionData tx)
        {
            return (T)Convert.ChangeType(tx, typeof(T));
        }

        internal static List<T> FilterEvents(string data)
        {
            var evs = JObject.Parse(data);

            List<T> events = new List<T>();

            foreach (var e in evs)
            {
                events.Add(JsonConvert.DeserializeObject<T>(e.ToString(), new EventConverter(typeof(T))));
            }
           
            return events;
        }

        internal static T FilterEvent(string data)
        {
            return ObjectComposer.GenerateObject<T>(data); //  JsonConvert.DeserializeObject<T>(data, new EventConverter(typeof(T)));
        }

        internal static List<T> FilterType(string data)
        {
            var tx = JObject.Parse(data)["data"].ToList();

            List<T> txs = new List<T>();

            foreach (var t in tx)
            {
                    txs.Add(JsonConvert.DeserializeObject<T>(t.ToString()));
            }
            return txs;
        }

        internal static T FilterSingle(string data, bool embedded = false)
        {
            var tx = JObject.Parse(data);

            var type = TransactionTypes.GetRawValue((ushort)tx["transaction"]["type"]);

            if (type == TransactionTypes.Types.TRANSFER)
                return JsonConvert.DeserializeObject<T>(tx.ToString(), [new TransactionConverter(embedded ? typeof(EmbeddedSimpleTransfer) : typeof(SimpleTransfer))]);

            if (type == TransactionTypes.Types.NAMESPACE_REGISTRATION)
            {
                if ((int)tx["transaction"]["registrationType"] == 0)
                {
                    return JsonConvert.DeserializeObject<T>(tx.ToString(), [new TransactionConverter(embedded ? typeof(EmbeddedRootNamespaceRegistration) : typeof(RootNamespaceRegistration))]);
                }
                if ((int)tx["transaction"]["registrationType"] == 1)
                {
                    return JsonConvert.DeserializeObject<T>(tx.ToString(), [new TransactionConverter(embedded ? typeof(EmbeddedChildNamespaceRegistration) : typeof(ChildNamespaceRegistration))]);
                }
            }
            if (type == TransactionTypes.Types.MOSAIC_SUPPLY_CHANGE)
                return JsonConvert.DeserializeObject<T>(tx.ToString(), [new TransactionConverter(embedded ? typeof(EmbeddedMosaicSupplyChange) : typeof(MosaicSupplyChange))]);

            if (type == TransactionTypes.Types.MOSAIC_SUPPLY_REVOCATION)
                return JsonConvert.DeserializeObject<T>(tx.ToString(), [new TransactionConverter(embedded ? typeof(EmbeddedMosaicSupplyRevocation) : typeof(MosaicSupplyRevocation))]);

            if (type == TransactionTypes.Types.HASH_LOCK)
                return JsonConvert.DeserializeObject<T>(tx.ToString(), [new TransactionConverter(embedded ? typeof(EmbeddedHashLockTransaction) : typeof(HashLockT))]);

            if (type == TransactionTypes.Types.SECRET_LOCK)
                return JsonConvert.DeserializeObject<T>(tx.ToString(), [new TransactionConverter(embedded ? typeof(EmbeddedSecretLockT) : typeof(SecretLockT))]);

            if (type == TransactionTypes.Types.SECRET_PROOF)
                return JsonConvert.DeserializeObject<T>(tx.ToString(), [new TransactionConverter(embedded ? typeof(EmbeddedSecretProofT) : typeof(SecretProofT))]);

            if (type == TransactionTypes.Types.ADDRESS_ALIAS)
                return JsonConvert.DeserializeObject<T>(tx.ToString(), [new TransactionConverter(embedded ? typeof(EmbeddedAddressAlias) : typeof(AddressAlias))]);

            if (type == TransactionTypes.Types.MOSAIC_ALIAS)
                return JsonConvert.DeserializeObject<T>(tx.ToString(), [new TransactionConverter(embedded ? typeof(EmbeddedMosaicAlias) : typeof(MosaicAlias))]);

            if (type == TransactionTypes.Types.ACCOUNT_ADDRESS_RESTRICTION)
                return JsonConvert.DeserializeObject<T>(tx.ToString(), [new TransactionConverter(embedded ? typeof(EmbeddedAccountAddressRestriction) : typeof(AccountAddressRestriction))]);

            if (type == TransactionTypes.Types.MOSAIC_ADDRESS_RESTRICTION)
                return JsonConvert.DeserializeObject<T>(tx.ToString(), [new TransactionConverter(embedded ? typeof(EmbeddedAccountAddressRestriction) : typeof(MosaicAddressRestriction))]);

            if (type == TransactionTypes.Types.ACCOUNT_MOSAIC_RESTRICTION)
                return JsonConvert.DeserializeObject<T>(tx.ToString(), [new TransactionConverter(embedded ? typeof(EmbeddedAccountMosaicRestriction) : typeof(AccountMosaicRestriction))]);

            if (type == TransactionTypes.Types.ACCOUNT_KEY_LINK
             || type == TransactionTypes.Types.NODE_KEY_LINK
             || type == TransactionTypes.Types.VRF_KEY_LINK)
                return JsonConvert.DeserializeObject<T>(tx.ToString(), [new TransactionConverter(embedded ? typeof(EmbeddedKeyLink) : typeof(KeyLink))]);

            if (type == TransactionTypes.Types.VOTING_KEY_LINK)
                return JsonConvert.DeserializeObject<T>(tx.ToString(), [new TransactionConverter(embedded ? typeof(EmbeddedVotingKeyLink) : typeof(VotingKeyLink))]);

            if (type == TransactionTypes.Types.AGGREGATE_COMPLETE || type == TransactionTypes.Types.AGGREGATE_BONDED)
                return JsonConvert.DeserializeObject<T>(tx.ToString(), [new TransactionConverter(typeof(Aggregate))]);

            else throw new NotImplementedException();   
        }
        internal static List<T> Filter(string data, bool embedded = false)
        {
            var tx = JObject.Parse(data)["data"].ToList();

            List<T> txs = new List<T>();

            foreach (var t in tx)
            {
                txs.Add(FilterSingle(t.ToString(), embedded));
            }

            return txs;
        }
    }
}
