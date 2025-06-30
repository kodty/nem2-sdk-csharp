using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Text.Json.Nodes;

namespace io.nem2.sdk.src.Export
{
    internal class ResponseFilters<T> where T : class
    {
        internal object[] Args { get; set; }

        public ResponseFilters(object[] args) {
            Args = args;
        }

        internal List<T> FilterEvents(string data, string path = null)
        {
            var evs = path == null ? JsonNode.Parse(data) : JsonNode.Parse(data)[path];

            List<T> events = new List<T>();

            foreach (var e in evs.AsArray())
            {
                events.Add((T)new ObjectComposer(Args).GenerateObject(typeof(T), e.AsObject()));
            }

            return events;
        }

        internal List<T> FilterTransactions(Func<string, Type> GetTransactionType, string data, string path = null)
        {
            var tx = path == null ? JsonNode.Parse(data).AsArray() : JsonNode.Parse(data)[path];

            List<T> txs = new List<T>();

            foreach (var t in tx.AsArray())
            {
                txs.Add(FilterSingle2(GetTransactionType(t.ToString()), t.ToString()));
            }

            return txs;
        }

        internal string GetSpecifiedTx(JsonObject ob)
        {
            return ob["transaction"].ToString();
        }

        internal T FilterSingle2(Type type, string data)
        {
            var tx = JsonObject.Parse(data).AsObject();

            dynamic shell = new ObjectComposer(Args).GenerateObject<T>(tx.ToString());
         
            shell.Transaction = new ObjectComposer(Args).GenerateObject(type, GetSpecifiedTx(tx));

            return shell;         
        }

        internal T FilterSingle(string data)
        {
            var tx = JsonObject.Parse(data).AsObject();

            dynamic shell = new ObjectComposer(Args).GenerateObject<T>(tx.ToString());

            var type = ((ushort)tx["transaction"]["type"]).GetRawValue();

            if (type == TransactionTypes.Types.TRANSFER)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<SimpleTransfer>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<EmbeddedSimpleTransfer>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.NAMESPACE_REGISTRATION)
            {
                if ((int)tx["transaction"]["registrationType"] == 0)
                {
                    if (typeof(T) == typeof(TransactionData))
                        shell.Transaction = new ObjectComposer(Args).GenerateObject<RootNamespaceRegistration>(GetSpecifiedTx(tx));
                    if (typeof(T) == typeof(EmbeddedTransactionData))
                        shell.Transaction = new ObjectComposer(Args).GenerateObject<EmbeddedRootNamespaceRegistration>(GetSpecifiedTx(tx));

                    return shell;
                }
                if ((int)tx["transaction"]["registrationType"] == 1)
                {
                    if (typeof(T) == typeof(TransactionData))
                        shell.Transaction = new ObjectComposer(Args).GenerateObject<ChildNamespaceRegistration>(GetSpecifiedTx(tx));
                    if (typeof(T) == typeof(EmbeddedTransactionData))
                        shell.Transaction = new ObjectComposer(Args).GenerateObject<EmbeddedChildNamespaceRegistration>(GetSpecifiedTx(tx));

                    return shell;
                }
            }
            if (type == TransactionTypes.Types.MOSAIC_DEFINITION)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<MosaicDefinition>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<EmbeddedMosaicDefinition>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.MOSAIC_SUPPLY_CHANGE)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<MosaicSupplyChange>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<EmbeddedMosaicSupplyChange>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.MOSAIC_SUPPLY_REVOCATION)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<MosaicSupplyRevocation>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<EmbeddedMosaicSupplyRevocation>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.HASH_LOCK)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<HashLockT>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<EmbeddedHashLockT>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.SECRET_LOCK)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<SecretLockT>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<EmbeddedSecretLockT>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.SECRET_PROOF)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<SecretProofT>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<EmbeddedSecretProofT>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.ADDRESS_ALIAS)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<AddressAlias>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<EmbeddedAddressAlias>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.MOSAIC_ALIAS)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<MosaicAlias>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<EmbeddedMosaicAlias>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.ACCOUNT_ADDRESS_RESTRICTION)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<AccountRestriction>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<EmbeddedAccountAddressRestriction>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.ACCOUNT_OPERATION_RESTRICTION)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<AccountOperationRestriction>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<EmbeddedAccountOperationRestriction>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.MOSAIC_ADDRESS_RESTRICTION)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<MosaicAddressRestriction>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<EmbeddedMosaicAddressRestriction>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.ACCOUNT_MOSAIC_RESTRICTION)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<AccountRestriction>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<EmbeddedAccountRestriction>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.ACCOUNT_KEY_LINK
             || type == TransactionTypes.Types.NODE_KEY_LINK
             || type == TransactionTypes.Types.VRF_KEY_LINK)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<KeyLink>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<EmbeddedKeyLink>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.VOTING_KEY_LINK)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<VotingKeyLink>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<EmbeddedVotingKeyLink>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.MOSAIC_METADATA)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<MosaicMetadata>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<EmbeddedMosaicMetadata>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.NAMESPACE_METADATA)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<NamespaceMetadata>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<EmbeddedNamespaceMetadata>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.ACCOUNT_METADATA)
            {
                if (typeof(T) == typeof(TransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<AccountMetadata>(GetSpecifiedTx(tx));
                if (typeof(T) == typeof(EmbeddedTransactionData))
                    shell.Transaction = new ObjectComposer(Args).GenerateObject<EmbeddedAccountMetadata>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.AGGREGATE_COMPLETE || type == TransactionTypes.Types.AGGREGATE_BONDED)
            {
                shell.Transaction = new ObjectComposer(Args).GenerateObject<Aggregate>(GetSpecifiedTx(tx));

                return shell;
            }
            if (type == TransactionTypes.Types.MULTISIG_ACCOUNT_MODIFICATION)
            {
                shell.Transaction = new ObjectComposer(Args).GenerateObject<EmbeddedMultisigModification>(GetSpecifiedTx(tx));

                return shell;
            }
            else throw new NotImplementedException("TransactionTypes.Type not implemented");
        }
    } 
}

