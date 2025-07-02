using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.Model2;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Text.Json.Nodes;
using CopperCurve;

namespace io.nem2.sdk.src.Export
{
    internal static class ResponseFilters<T> where T : class
    {
        internal static List<T> FilterEvents(string data, string path = null)
        {
            var evs = path == null ? JsonNode.Parse(data) : JsonNode.Parse(data)[path];

            List<T> events = new List<T>();

            foreach (var e in evs.AsArray())
            {
                events.Add((T)new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject(typeof(T), e.AsObject()));
            }

            return events;
        }

        internal static List<T> FilterTransactions(string data, string path = null)
        {
            var tx = path == null ? JsonNode.Parse(data).AsArray() : JsonNode.Parse(data)[path].AsArray();

            List<T> txs = new List<T>();

            foreach (var t in tx)
            {
                txs.Add(FilterSingle(t.ToString()));
            }

            return txs;
        }

        internal static T GetBaseTransaction(JsonObject ob)
        {
            return new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<T>(ob.ToString());
        }

        internal static string GetSpecifiedTx(JsonObject ob)
        {
            return ob["transaction"].ToString();
        }

        internal static Type GetAssocations(JsonObject tx)
        {
            return ((ushort)tx["transaction"]["type"]).GetObjectTypeAssocations();
        }
        internal static TransactionTypes.Types GetTxType(JsonObject tx)
        {
            return ((ushort)tx["transaction"]["type"]).GetRawValue();
        }

        internal static T FilterSingle1(string data)
        {
            var tx = JsonObject.Parse(data).AsObject();

            dynamic shell = GetBaseTransaction(tx.AsObject());

            var type = GetTxType(tx.AsObject());

            var associations = (Tuple<Type, Type>)Activator.CreateInstance(GetAssocations(tx.AsObject()));

            if (typeof(T) == typeof(TransactionData))
                shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject(associations.Item1.GetType(), GetSpecifiedTx(tx));
            if (typeof(T) == typeof(EmbeddedTransactionData))
                shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject(associations.Item2.GetType(), GetSpecifiedTx(tx));

            return shell;
        }

        internal static T FilterBaseTransactionType(string data)
        {
            var tx = JsonObject.Parse(data).AsObject();

            dynamic shell = GetBaseTransaction(tx.AsObject());

            var type = GetTxType(tx.AsObject());
   
            var associations = (Tuple<Type, Type>)Activator.CreateInstance(GetAssocations(tx.AsObject()));

            if (typeof(T) == typeof(TransactionData))
                shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject(associations.Item1.GetType(), GetSpecifiedTx(tx));
            if (typeof(T) == typeof(EmbeddedTransactionData))
                shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject(associations.Item2.GetType(), GetSpecifiedTx(tx));

            return shell;
        }


        internal static T FilterSingle(string data)
        {
            var tx = JsonObject.Parse(data).AsObject();

            dynamic shell = GetBaseTransaction(tx.AsObject());

            var type = GetTxType(tx.AsObject());


            if (type == TransactionTypes.Types.TRANSFER)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<SimpleTransfer>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<EmbeddedSimpleTransfer>(GetSpecifiedTx(tx));
            
                return shell;
            }
            if (type == TransactionTypes.Types.NAMESPACE_REGISTRATION)
            {
                if ((int)tx["transaction"]["registrationType"] == 0)
                {
                    if (typeof(T) == typeof(TransactionData))
                        shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<RootNamespaceRegistration>(GetSpecifiedTx(tx));
                    if (typeof(T) == typeof(EmbeddedTransactionData))
                        shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<EmbeddedRootNamespaceRegistration>(GetSpecifiedTx(tx));
            
                    return shell;
                }
                if ((int)tx["transaction"]["registrationType"] == 1)
                {
                    if (typeof(T) == typeof(TransactionData))
                        shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<ChildNamespaceRegistration>(GetSpecifiedTx(tx));
                    if (typeof(T) == typeof(EmbeddedTransactionData))
                        shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<EmbeddedChildNamespaceRegistration>(GetSpecifiedTx(tx));
            
                     return shell;
                 }
             }
             if (type == TransactionTypes.Types.MOSAIC_DEFINITION)
             {
                 if (typeof(T) == typeof(TransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<MosaicDefinition>(GetSpecifiedTx(tx));
                 if (typeof(T) == typeof(EmbeddedTransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<EmbeddedMosaicDefinition>(GetSpecifiedTx(tx));
            
                 return shell;
             }
             if (type == TransactionTypes.Types.MOSAIC_SUPPLY_CHANGE)
             {
                 if (typeof(T) == typeof(TransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<MosaicSupplyChange>(GetSpecifiedTx(tx));
                 if (typeof(T) == typeof(EmbeddedTransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<EmbeddedMosaicSupplyChange>(GetSpecifiedTx(tx));
            
                 return shell;
             }
             if (type == TransactionTypes.Types.MOSAIC_SUPPLY_REVOCATION)
             {
                 if (typeof(T) == typeof(TransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<MosaicSupplyRevocation>(GetSpecifiedTx(tx));
                 if (typeof(T) == typeof(EmbeddedTransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<EmbeddedMosaicSupplyRevocation>(GetSpecifiedTx(tx));
            
                 return shell;
             }
             if (type == TransactionTypes.Types.HASH_LOCK)
             {
                 if (typeof(T) == typeof(TransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<HashLockT>(GetSpecifiedTx(tx));
                 if (typeof(T) == typeof(EmbeddedTransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<EmbeddedHashLockT>(GetSpecifiedTx(tx));
            
                 return shell;
             }
             if (type == TransactionTypes.Types.SECRET_LOCK)
             {
                 if (typeof(T) == typeof(TransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<SecretLockT>(GetSpecifiedTx(tx));
                 if (typeof(T) == typeof(EmbeddedTransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<EmbeddedSecretLockT>(GetSpecifiedTx(tx));
            
                 return shell;
             }
             if (type == TransactionTypes.Types.SECRET_PROOF)
             {
                 if (typeof(T) == typeof(TransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<SecretProofT>(GetSpecifiedTx(tx));
                 if (typeof(T) == typeof(EmbeddedTransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<EmbeddedSecretProofT>(GetSpecifiedTx(tx));
            
                 return shell;
             }
             if (type == TransactionTypes.Types.ADDRESS_ALIAS)
             {
                 if (typeof(T) == typeof(TransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<AddressAlias>(GetSpecifiedTx(tx));
                 if (typeof(T) == typeof(EmbeddedTransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<EmbeddedAddressAlias>(GetSpecifiedTx(tx));
             
                 return shell;
             }
             if (type == TransactionTypes.Types.MOSAIC_ALIAS)
             {
                 if (typeof(T) == typeof(TransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<MosaicAlias>(GetSpecifiedTx(tx));
                 if (typeof(T) == typeof(EmbeddedTransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<EmbeddedMosaicAlias>(GetSpecifiedTx(tx));
             
                 return shell;
             }
             if (type == TransactionTypes.Types.ACCOUNT_ADDRESS_RESTRICTION)
             {
                 if (typeof(T) == typeof(TransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<AccountRestriction>(GetSpecifiedTx(tx));
                 if (typeof(T) == typeof(EmbeddedTransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<EmbeddedAccountAddressRestriction>(GetSpecifiedTx(tx));
             
                 return shell;
             }
             if (type == TransactionTypes.Types.ACCOUNT_OPERATION_RESTRICTION)
             {
                 if (typeof(T) == typeof(TransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<AccountOperationRestriction>(GetSpecifiedTx(tx));
                 if (typeof(T) == typeof(EmbeddedTransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<EmbeddedAccountOperationRestriction>(GetSpecifiedTx(tx));
             
                 return shell;
             }
             if (type == TransactionTypes.Types.MOSAIC_ADDRESS_RESTRICTION)
             {
                 if (typeof(T) == typeof(TransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<MosaicAddressRestriction>(GetSpecifiedTx(tx));
                 if (typeof(T) == typeof(EmbeddedTransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<EmbeddedMosaicAddressRestriction>(GetSpecifiedTx(tx));
             
                 return shell;
             }
             if (type == TransactionTypes.Types.ACCOUNT_MOSAIC_RESTRICTION)
             {
                 if (typeof(T) == typeof(TransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<AccountRestriction>(GetSpecifiedTx(tx));
                 if (typeof(T) == typeof(EmbeddedTransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<EmbeddedAccountRestriction>(GetSpecifiedTx(tx));
            
                 return shell;
             }
             if (type == TransactionTypes.Types.ACCOUNT_KEY_LINK
              || type == TransactionTypes.Types.NODE_KEY_LINK
              || type == TransactionTypes.Types.VRF_KEY_LINK)
             {
                 if (typeof(T) == typeof(TransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<KeyLink>(GetSpecifiedTx(tx));
                 if (typeof(T) == typeof(EmbeddedTransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<EmbeddedKeyLink>(GetSpecifiedTx(tx));
            
                 return shell;
             }
             if (type == TransactionTypes.Types.VOTING_KEY_LINK)
             {
                 if (typeof(T) == typeof(TransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<VotingKeyLink>(GetSpecifiedTx(tx));
                 if (typeof(T) == typeof(EmbeddedTransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<EmbeddedVotingKeyLink>(GetSpecifiedTx(tx));
            
                 return shell;
             }
             if (type == TransactionTypes.Types.MOSAIC_METADATA)
             {
                 if (typeof(T) == typeof(TransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<MosaicMetadata>(GetSpecifiedTx(tx));
                 if (typeof(T) == typeof(EmbeddedTransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<EmbeddedMosaicMetadata>(GetSpecifiedTx(tx));
           
                 return shell;
             }
             if (type == TransactionTypes.Types.NAMESPACE_METADATA)
             {
                 if (typeof(T) == typeof(TransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<NamespaceMetadata>(GetSpecifiedTx(tx));
                 if (typeof(T) == typeof(EmbeddedTransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<EmbeddedNamespaceMetadata>(GetSpecifiedTx(tx));
           
                 return shell;
             }
             if (type == TransactionTypes.Types.ACCOUNT_METADATA)
             {
                 if (typeof(T) == typeof(TransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<AccountMetadata>(GetSpecifiedTx(tx));
                 if (typeof(T) == typeof(EmbeddedTransactionData))
                     shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<EmbeddedAccountMetadata>(GetSpecifiedTx(tx));
           
                 return shell;
             }
             if (type == TransactionTypes.Types.AGGREGATE_COMPLETE || type == TransactionTypes.Types.AGGREGATE_BONDED)
             {
                 shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<Aggregate>(GetSpecifiedTx(tx));
           
                 return shell;
             }
             if (type == TransactionTypes.Types.MULTISIG_ACCOUNT_MODIFICATION)
             {
                 shell.Transaction = new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<EmbeddedMultisigModification>(GetSpecifiedTx(tx));
           
                 return shell;
             }
             else throw new NotImplementedException("TransactionTypes.Type not implemented");
        }
    } 
}
