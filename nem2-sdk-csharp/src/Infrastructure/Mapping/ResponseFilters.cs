using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using Newtonsoft.Json.Linq;

namespace io.nem2.sdk.src.Infrastructure.Mapping
{
    internal static class ResponseFilters<T> where T : class
    {
        internal static List<T> FilterEvents(string data, string path = null)
        {
            var evs = path == null ?  JToken.Parse(data) : JToken.Parse(data)[path];

            List<T> events = new List<T>();

            foreach (var e in evs)
            {
                events.Add((T)ObjectComposer.GenerateObject(typeof(T), e));
            }
           
            return events;
        }

        internal static List<T> FilterTransactions(string data, string path = null)
        {
            var tx = path == null ? JToken.Parse(data).ToList() : JToken.Parse(data)[path].ToList();

            List<T> txs = new List<T>();

            foreach (var t in tx)
            {
                txs.Add(FilterSingle(t.ToString()));
            }

            return txs;
        }

        internal static T GetBaseTransaction(JObject ob)
        {
            return ObjectComposer.GenerateObject<T>(ob.ToString());
        }

        internal static string GetSpecifiedTx(JObject ob)
        {
            return ob["transaction"].ToString();
        }

        internal static TransactionTypes.Types GetTxType(JObject tx)
        {
            return TransactionTypes.GetRawValue((ushort)tx["transaction"]["type"]);
        }

        internal static T FilterSingle(string data)
        {
            var tx = JObject.Parse(data);

            dynamic shell = GetBaseTransaction(tx);

            var type = GetTxType(tx);

            if (type == TransactionTypes.Types.TRANSFER)
            { 
                if(typeof(T) == typeof(TransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<SimpleTransfer>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<EmbeddedSimpleTransfer>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.NAMESPACE_REGISTRATION)
            {
                if ((int)tx["transaction"]["registrationType"] == 0)
                {
                    if (typeof(T) == typeof(TransactionData))
                        shell.Transaction = ObjectComposer.GenerateObject<RootNamespaceRegistration>(GetSpecifiedTx(tx));
                    if (typeof(T) == typeof(EmbeddedTransactionData))
                        shell.Transaction = ObjectComposer.GenerateObject<EmbeddedRootNamespaceRegistration>(GetSpecifiedTx(tx));

                    return shell;
                }
                if ((int)tx["transaction"]["registrationType"] == 1)
                {
                    if (typeof(T) == typeof(TransactionData))
                        shell.Transaction = ObjectComposer.GenerateObject<ChildNamespaceRegistration>(GetSpecifiedTx(tx));
                    if (typeof(T) == typeof(EmbeddedTransactionData))
                        shell.Transaction = ObjectComposer.GenerateObject<EmbeddedChildNamespaceRegistration>(GetSpecifiedTx(tx));

                    return shell;
                }
            }
            if (type == TransactionTypes.Types.MOSAIC_DEFINITION)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<MosaicDefinition>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<EmbeddedMosaicDefinition>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.MOSAIC_SUPPLY_CHANGE)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<MosaicSupplyChange>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<EmbeddedMosaicSupplyChange>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.MOSAIC_SUPPLY_REVOCATION)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<MosaicSupplyRevocation>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<EmbeddedMosaicSupplyRevocation>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.HASH_LOCK)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<HashLockT>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<EmbeddedHashLockT>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.SECRET_LOCK)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<SecretLockT>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<EmbeddedSecretLockT>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.SECRET_PROOF)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<SecretProofT>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<EmbeddedSecretProofT>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.ADDRESS_ALIAS)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<AddressAlias>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<EmbeddedAddressAlias>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.MOSAIC_ALIAS)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<MosaicAlias>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<EmbeddedMosaicAlias>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.ACCOUNT_ADDRESS_RESTRICTION)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<AccountRestriction>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<EmbeddedAccountAddressRestriction>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.ACCOUNT_OPERATION_RESTRICTION)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<AccountOpperationRestriction>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<EmbeddedAccountOpperationRestriction>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.MOSAIC_ADDRESS_RESTRICTION)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<MosaicAddressRestriction>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<EmbeddedMosaicAddressRestriction>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.ACCOUNT_MOSAIC_RESTRICTION)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<AccountRestriction>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<EmbeddedAccountMosaicRestriction>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.ACCOUNT_KEY_LINK
             || type == TransactionTypes.Types.NODE_KEY_LINK
             || type == TransactionTypes.Types.VRF_KEY_LINK)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<KeyLink>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<EmbeddedKeyLink>(GetSpecifiedTx(tx));

                return shell;          
            }
            if (type == TransactionTypes.Types.VOTING_KEY_LINK)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<VotingKeyLink>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<EmbeddedVotingKeyLink>(GetSpecifiedTx(tx));

                return shell;             
            }
            if (type == TransactionTypes.Types.MOSAIC_METADATA)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<MosaicMetadata>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<EmbeddedMosaicMetadata>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.NAMESPACE_METADATA)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<NamespaceMetadata>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<EmbeddedNamespaceMetadata>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.ACCOUNT_METADATA)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<AccountMetadata>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = ObjectComposer.GenerateObject<EmbeddedAccountMetadata>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.AGGREGATE_COMPLETE || type == TransactionTypes.Types.AGGREGATE_BONDED)
            {
                shell.Transaction = ObjectComposer.GenerateObject<Aggregate>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.MULTISIG_ACCOUNT_MODIFICATION)
            {
                shell.Transaction = ObjectComposer.GenerateObject<EmbeddedMultisigModification>(GetSpecifiedTx(tx));

                return shell;
            }
            else throw new NotImplementedException("TransactionTypes.Type not implemented");   
        }
    }
}
