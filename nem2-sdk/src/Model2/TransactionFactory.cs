using io.nem2.sdk.Model.Mosaics;
using io.nem2.sdk.Model.Namespace;
using io.nem2.sdk.src.Model2.Transactions;
using io.nem2.sdk.src.Model2.Transactions.AccountRestrictions;
using io.nem2.sdk.src.Model2.Transactions.CrossChainTransactions;
using io.nem2.sdk.src.Model2.Transactions.KeyLinkTransactions;
using io.nem2.sdk.src.Model2.Transactions.MetadataTransactions;
using io.nem2.sdk.src.Model2.Transactions.MosaicPropertiesTransactions;
using io.nem2.sdk.src.Model2.Transactions.MosaicRestrictions;

namespace io.nem2.sdk.src.Model2
{
    public class TransactionFactory
    {
        internal NetworkType.Types NetworkType { get; set; }

        internal EntityBody DefaultEntityBody { get; set;}

        internal TransactionTypes.Types TransactionType { get; set; }

        internal byte Version { get; set; }

        internal ulong DefaultDeadline { get; set; }

        internal ulong Fee { get; set; }
        
        internal string Node { get; set; }
        internal int Port { get; set; }

        public TransactionFactory(NetworkType.Types type, string node, int port)
        {
            NetworkType = type;
            Node = node;
            Port = port;

            DefaultEntityBody = new EntityBody()
            {
                Signer = null,
                Entity_body_reserved_1 = 0,
                Version = 0x01,
                Network = NetworkType.GetNetworkByte()
            };
        }

        public TransactionFactory(NetworkType.Types networkType, TransactionTypes.Types transactionType, byte version, ulong deadline, ulong fee, string node, int port)
        {
            DefaultEntityBody = new EntityBody()
            {
                Signer = null,
                Entity_body_reserved_1 = 0,
                Version = 0x01,
                Network = NetworkType.GetNetworkByte()
            };

            NetworkType = networkType;
            TransactionType = transactionType;
            Fee = fee;
            DefaultDeadline = deadline;
            Node = node;
            Port = port;
        }

        public MosaicSupplyChangeTransaction1 CreateMosaicSupplyChangeTransaction(ulong delta, Tuple<string, ulong> mosaic, MosaicSupplyType.Type supplyType)
        {
            return new MosaicSupplyChangeTransaction1(delta, mosaic, supplyType)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.MOSAIC_SUPPLY_CHANGE.GetValue(),
                Deadline = Deadline.AutoDeadline(Node, Port).Ticks,
                Fee = 100
            };
        }

        public NamespaceMetadataTransaction1 CreateNamespaceMetadataTransaction(string targetAddress, string scopedKey, string targetNamespaceId, short valueSizeDelta, short valueSize, byte[] value)
        {
            return new NamespaceMetadataTransaction1(targetAddress, scopedKey, targetNamespaceId, valueSizeDelta, valueSize, value)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.MOSAIC_METADATA.GetValue(),
                Deadline = Deadline.AutoDeadline(Node, Port).Ticks,
                Fee = 100
            };
        }

        public AccountMetadataTransaction1 CreateAccountMetadataTransaction(string targetAddress, string scopedKey, short valueSizeDelta, short valueSize, byte[] value)
        {
            return new AccountMetadataTransaction1(targetAddress, scopedKey, valueSizeDelta, valueSize, value)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.MOSAIC_METADATA.GetValue(),
                Deadline = Deadline.AutoDeadline(Node, Port).Ticks,
                Fee = 100
            };
        }

        public MosaicMetadataTransaction1 CreateMosaicMetadataTransaction(string targetAddress, string scopedKey, string targetMosaicId, short valueSizeDelta, short valueSize, byte[] value)
        {
            return new MosaicMetadataTransaction1(targetAddress, scopedKey, targetMosaicId, valueSizeDelta, valueSize, value)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.MOSAIC_METADATA.GetValue(),
                Deadline = Deadline.AutoDeadline(Node, Port).Ticks,
                Fee = 100
            };
        }

        public MosaicDefinitionTransaction1 CreateMosaicDefinitionTransaction(string mosaicName, NamespaceId namespaceId, MosaicId mosaicId, MosaicProperties properties)
        {
            return new MosaicDefinitionTransaction1(mosaicName, namespaceId, mosaicId, properties)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.MOSAIC_DEFINITION.GetValue(),
                Deadline = Deadline.AutoDeadline(Node, Port).Ticks,
                Fee = 100
            };
        }

        public LockFundsTransaction1 CreateLockFundsTransaction(Tuple<string, ulong> mosaic, ulong duration, string transactionHash)
        {
            return new LockFundsTransaction1(mosaic, duration, transactionHash)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.HASH_LOCK.GetValue(),
                Deadline = Deadline.AutoDeadline(Node, Port).Ticks,
                Fee = 100
            };
        }

        public SecretLockTransaction1 CreateSecretLockTransaction(Tuple<string, ulong> mosaic, ulong duration, string secret, HashType.Types hashAlgo, string recipient)
        {
            return new SecretLockTransaction1(mosaic, duration, secret, hashAlgo, recipient)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.SECRET_LOCK.GetValue(),
                Deadline = Deadline.AutoDeadline(Node, Port).Ticks,
                Fee = 100
            };

        }

        public SecretProofTransaction1 CreateSecretProofTransaction(string recipientAddress, string secret, HashType.Types hashAlgo, string proof)
        {
            return new SecretProofTransaction1(recipientAddress, secret, hashAlgo, proof)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.SECRET_PROOF.GetValue(),
                Deadline = Deadline.AutoDeadline(Node, Port).Ticks,
                Fee = 100           
            };
        }

        public RegisterNamespace CreateNamespaceRegistrationTransaction(ulong duration, NamespaceId parentId, NamespaceId id, NamespaceTypes.Types type, string name)
        {
            return new RegisterNamespace(duration, parentId, id, type, name)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.NAMESPACE_REGISTRATION.GetValue(),
                RegistrationType = type,
                Deadline = Deadline.AutoDeadline(Node, Port).Ticks,
                Fee = 100
            };
        }

        public KeyLinkTransaction1 CreateVotingKeyLinkTransaction(TransactionTypes.Types type, ulong startEpoch, ulong endEpoch, string linkedPublicKey, int linkAction, byte linkType)
        {
            return new VotingKeyLinkTransaction1(startEpoch, endEpoch, linkedPublicKey, linkAction)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.VOTING_KEY_LINK.GetValue(),
                Deadline = Deadline.AutoDeadline(Node, Port).Ticks,
                Fee = 100
            };
        }

        public KeyLinkTransaction1 CreateKeyLinkTransaction(TransactionTypes.Types type, string linkedPublicKey, int linkAction, byte linkType)
        {
            return new KeyLinkTransaction1(linkedPublicKey, linkAction)
            {
                EntityBody = DefaultEntityBody,
                Type = type.GetValue(),
                Deadline = Deadline.AutoDeadline(Node, Port).Ticks,
                Fee = 100
            };
        }

        public TransferTransaction_V1 CreateTransferTransaction(string address, string messege, Tuple<string, ulong> mosaic)
        {
            return new TransferTransaction_V1(address, messege, mosaic)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.TRANSFER.GetValue(),
                Deadline = Deadline.AutoDeadline(Node, Port).Ticks,
                Fee = 100
            };
        }

        public AccountRestrictionsTransaction1 CreateAccountRestrictionTransaction(TransactionTypes.Types type, int restrictionFlags, string[] additions, string[] deletions)
        {
            return new AccountRestrictionsTransaction1(type, restrictionFlags, additions, deletions)
            {
                EntityBody = DefaultEntityBody,
                Type = type.GetValue(),
                Deadline = Deadline.AutoDeadline(Node, Port).Ticks,
                Fee = 100
            };
        }

        public MosaicAddressRestrictionTransaction CreateMosaicAddressRestrictionTransaction(string targetAddress, string mosaicID, string restrictionKey, string previousRestrictionValue, string newRestrictionValue)
        {
            return new MosaicAddressRestrictionTransaction(targetAddress, mosaicID, restrictionKey, previousRestrictionValue, newRestrictionValue)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.MOSAIC_ADDRESS_RESTRICTION.GetValue(),
                Deadline = Deadline.AutoDeadline(Node, Port).Ticks,
                Fee = 100
            };
        }

        public MosaicGlobalRestrictionTransaction CreateMosaicGlobalRestrictionTransaction(string referenceMosaicId, string mosaicID, string restrictionKey, string previousRestrictionValue, string newRestrictionValue)
        {
            return new MosaicGlobalRestrictionTransaction(referenceMosaicId, mosaicID, restrictionKey, previousRestrictionValue, newRestrictionValue)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.MOSAIC_GLOBAL_RESTRICTION.GetValue(),
                Deadline = Deadline.AutoDeadline(Node, Port).Ticks,
                Fee = 100
            };
        }

        public AddressAliasTransaction1 CreateAddressAliasTransaction(string address, string namepaceId, byte aliasAction)
        {
            return new AddressAliasTransaction1(address, namepaceId, aliasAction)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.ADDRESS_ALIAS.GetValue(),
                Deadline = Deadline.AutoDeadline(Node, Port).Ticks,
                Fee = 100
            };
        }

        public MosaicAliasTransaction1 CreateMosaicAliasTransaction(string mosaicId, string namepaceId, byte aliasAction)
        {
            return new MosaicAliasTransaction1(mosaicId, namepaceId, aliasAction)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.MOSAIC_ALIAS.GetValue(),
                Deadline = Deadline.AutoDeadline(Node, Port).Ticks,
                Fee = 100
            };
        }

        public MultisigAccountModificationTransaction1 CreateMultisigAccountTransaction(byte minApproval, byte minRemoval, string[] addressAdditions, string[] addressDeletions)
        {
            return new MultisigAccountModificationTransaction1(minApproval, minRemoval, addressAdditions, addressDeletions)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.MULTISIG_ACCOUNT_MODIFICATION.GetValue(),
                Deadline = Deadline.AutoDeadline(Node, Port).Ticks,
                Fee = 100
            };
        }

        public AggregateTransaction1 CreateAggregateComplete(string txsHash, byte[] embeddedTransactions, byte[] cosignatures)
        {
            return new AggregateTransaction1(txsHash, embeddedTransactions, cosignatures, TransactionTypes.Types.AGGREGATE_COMPLETE)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.AGGREGATE_COMPLETE.GetValue(),
                Deadline = Deadline.AutoDeadline(Node, Port).Ticks,
                Fee = 100
            };
        }
    }  
}
