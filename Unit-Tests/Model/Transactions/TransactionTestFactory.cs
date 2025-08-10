using CopperCurve;
using io.nem2.sdk.src.Model;
using io.nem2.sdk.src.Model.Articles;
using io.nem2.sdk.src.Model.Transactions;
using io.nem2.sdk.src.Model.Transactions.AccountRestrictions;
using io.nem2.sdk.src.Model.Transactions.CrossChainTransactions;
using io.nem2.sdk.src.Model.Transactions.KeyLinkTransactions;
using io.nem2.sdk.src.Model.Transactions.MetadataTransactions;
using io.nem2.sdk.src.Model.Transactions.MosaicPropertiesTransactions;
using io.nem2.sdk.src.Model.Transactions.MosaicRestrictions;
using System.Diagnostics;
using Unit_Tests.Model.Transactions.Verified;

namespace Unit_Tests.Model.Transactions
{
    public class TransactionTestFactory
    {
        internal NetworkType.Types NetworkType { get; set; }

        internal EntityBody DefaultEntityBody { get; set; }

        internal TransactionTypes.Types TransactionType { get; set; }

        internal byte Version { get; set; }

        internal ulong DefaultDeadline { get; set; }

        internal ulong Fee { get; set; }

        internal string Node { get; set; }
        internal int Port { get; set; }

        public TransactionTestFactory(NetworkType.Types type, string node, int port)
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

        public TransactionTestFactory(NetworkType.Types networkType, TransactionTypes.Types transactionType, byte version, ulong deadline, ulong fee, string node, int port)
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

        public MosaicSupplyChangeTransaction CreateMosaicSupplyChangeTransaction(ulong delta, string mosaicId, MosaicSupplyType.Type supplyType, bool embedded)
        {
            return new MosaicSupplyChangeTransaction(delta, mosaicId, supplyType, embedded)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.MOSAIC_SUPPLY_CHANGE.GetValue(),
                Deadline = ((ulong)10101010101).ConvertFromUInt64(),
                Fee = ((ulong)20202020202).ConvertFromUInt64()
            };
        }

        public NamespaceMetadataTransaction CreateNamespaceMetadataTransaction(string targetAddress, string scopedKey, string targetNamespaceId, ushort valueSizeDelta, ushort valueSize, byte[] value, bool embedded)
        {
            return new NamespaceMetadataTransaction(targetAddress, scopedKey, targetNamespaceId, valueSizeDelta, valueSize, value, embedded)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.MOSAIC_METADATA.GetValue(),
                Deadline = ((ulong)10101010101).ConvertFromUInt64(),
                Fee = ((ulong)20202020202).ConvertFromUInt64()
            };
        }

        public AccountMetadataTransaction CreateAccountMetadataTransaction(string targetAddress, string scopedKey, ushort valueSizeDelta, ushort valueSize, byte[] value, bool embedded)
        {
            return new AccountMetadataTransaction(targetAddress, scopedKey, valueSizeDelta, valueSize, value, embedded)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.MOSAIC_METADATA.GetValue(),
                Deadline = ((ulong)10101010101).ConvertFromUInt64(),
                Fee = ((ulong)20202020202).ConvertFromUInt64()
            };
        }

        public MosaicMetadataTransaction CreateMosaicMetadataTransaction(string targetAddress, string scopedKey, string targetMosaicId, ushort valueSizeDelta, ushort valueSize, byte[] value, bool embedded)
        {
            return new MosaicMetadataTransaction(targetAddress, scopedKey, targetMosaicId, valueSizeDelta, valueSize, value, embedded)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.MOSAIC_METADATA.GetValue(),
                Deadline = ((ulong)10101010101).ConvertFromUInt64(),
                Fee = ((ulong)20202020202).ConvertFromUInt64()
            };
        }

        public MosaicDefinitionTransaction CreateMosaicDefinitionTransaction(string mosaicName, string namespaceId, string mosaicId, MosaicProperties properties, bool embedded)
        {
            return new MosaicDefinitionTransaction(mosaicName, namespaceId, mosaicId, properties, embedded)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.MOSAIC_DEFINITION.GetValue(),
                Deadline = ((ulong)10101010101).ConvertFromUInt64(),
                Fee = ((ulong)20202020202).ConvertFromUInt64()
            };
        }

        public LockFundsTransaction CreateLockFundsTransaction(Tuple<string, ulong> mosaic, ulong duration, string transactionHash, bool embedded)
        {
            return new LockFundsTransaction(mosaic, duration, transactionHash, embedded)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.HASH_LOCK.GetValue(),
                Deadline = ((ulong)10101010101).ConvertFromUInt64(),
                Fee = ((ulong)20202020202).ConvertFromUInt64()
            };
        }

        public SecretLockTransaction CreateSecretLockTransaction(Tuple<string, ulong> mosaic, ulong duration, string secret, HashType.Types hashAlgo, string recipient, bool embedded)
        {
            return new SecretLockTransaction(mosaic, duration, secret, hashAlgo, recipient, embedded)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.SECRET_LOCK.GetValue(),
                Deadline = ((ulong)10101010101).ConvertFromUInt64(),
                Fee = ((ulong)20202020202).ConvertFromUInt64()
            };

        }

        public SecretProofTransaction CreateSecretProofTransaction(string recipientAddress, string secret, HashType.Types hashAlgo, string proof, bool embedded)
        {
            return new SecretProofTransaction(recipientAddress, secret, hashAlgo, proof, embedded)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.SECRET_PROOF.GetValue(),
                Deadline = ((ulong)10101010101).ConvertFromUInt64(),
                Fee = ((ulong)20202020202).ConvertFromUInt64()
            };
        }

        public RegisterNamespace CreateNamespaceRegistrationTransaction(ulong duration, ulong parentId, ulong id, NamespaceTypes.Types type, string name, bool embedded)
        {
            return new RegisterNamespace(duration, parentId, id, type, name, embedded)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.NAMESPACE_REGISTRATION.GetValue(),
                Deadline = ((ulong)10101010101).ConvertFromUInt64(),
                Fee = ((ulong)20202020202).ConvertFromUInt64()
            };
        }

        public KeyLinkTransaction CreateVotingKeyLinkTransaction(TransactionTypes.Types type, ulong startEpoch, ulong endEpoch, string linkedPublicKey, int linkAction, byte linkType, bool embedded)
        {
            return new VotingKeyLinkTransaction(startEpoch, endEpoch, linkedPublicKey, linkAction, embedded)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.VOTING_KEY_LINK.GetValue(),
                Deadline = ((ulong)10101010101).ConvertFromUInt64(),
                Fee = ((ulong)20202020202).ConvertFromUInt64()
            };
        }

        public KeyLinkTransaction CreateKeyLinkTransaction(TransactionTypes.Types type, string linkedPublicKey, int linkAction, byte linkType, bool embedded)
        {
            return new KeyLinkTransaction(linkedPublicKey, linkAction, embedded)
            {
                EntityBody = DefaultEntityBody,
                Type = type.GetValue(),
                Deadline = ((ulong)10101010101).ConvertFromUInt64(),
                Fee = ((ulong)20202020202).ConvertFromUInt64()
            };
        }

        public TransferTransaction_V1 CreateTransferTransaction(string address, string messege, Tuple<string, ulong> mosaic, bool embedded)
        {
            return new TransferTransaction_V1(address, messege, mosaic, embedded)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.TRANSFER.GetValue(),
                Deadline = ((ulong)10101010101).ConvertFromUInt64(),
                Fee = ((ulong)20202020202).ConvertFromUInt64()
            };
        }



        public AccountRestrictionsTransaction CreateAccountRestrictionTransaction(TransactionTypes.Types type, ushort restrictionFlags, string[] additions, string[] deletions, bool embedded)
        {
            return new AccountRestrictionsTransaction(type, restrictionFlags, additions, deletions, embedded)
            {
                EntityBody = DefaultEntityBody,
                Type = type.GetValue(),
                Deadline = ((ulong)10101010101).ConvertFromUInt64(),
                Fee = ((ulong)20202020202).ConvertFromUInt64()
            };
        }

        public MosaicAddressRestrictionTransaction CreateMosaicAddressRestrictionTransaction(string targetAddress, string mosaicID, string restrictionKey, string previousRestrictionValue, string newRestrictionValue, bool embedded)
        {
            return new MosaicAddressRestrictionTransaction(targetAddress, mosaicID, restrictionKey, previousRestrictionValue, newRestrictionValue, embedded)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.MOSAIC_ADDRESS_RESTRICTION.GetValue(),
                Deadline = ((ulong)10101010101).ConvertFromUInt64(),
                Fee = ((ulong)20202020202).ConvertFromUInt64()
            };
        }

        public MosaicGlobalRestrictionTransaction CreateMosaicGlobalRestrictionTransaction(string referenceMosaicId, string mosaicID, string restrictionKey, string previousRestrictionValue, string newRestrictionValue, bool embedded)
        {
            return new MosaicGlobalRestrictionTransaction(referenceMosaicId, mosaicID, restrictionKey, previousRestrictionValue, newRestrictionValue, embedded)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.MOSAIC_GLOBAL_RESTRICTION.GetValue(),
                Deadline = ((ulong)10101010101).ConvertFromUInt64(),
                Fee = ((ulong)20202020202).ConvertFromUInt64()
            };
        }

        public AddressAliasTransaction CreateAddressAliasTransaction(string address, ulong namepaceId, byte aliasAction, bool embedded)
        {
            return new AddressAliasTransaction(address, namepaceId, aliasAction, embedded)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.ADDRESS_ALIAS.GetValue(),
                Deadline = ((ulong)10101010101).ConvertFromUInt64(),
                Fee = ((ulong)20202020202).ConvertFromUInt64()
            };
        }

        public MosaicAliasTransaction CreateMosaicAliasTransaction(string mosaicId, ulong namepaceId, byte aliasAction, bool embedded)
        {
            return new MosaicAliasTransaction(mosaicId, namepaceId, aliasAction, embedded)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.MOSAIC_ALIAS.GetValue(),
                Deadline = ((ulong)10101010101).ConvertFromUInt64(),
                Fee = ((ulong)20202020202).ConvertFromUInt64()
            };
        }

        public MultisigAccountModificationTransaction CreateMultisigAccountTransaction(byte minApproval, byte minRemoval, string[] addressAdditions, string[] addressDeletions, bool embedded)
        {
            return new MultisigAccountModificationTransaction(minApproval, minRemoval, addressAdditions, addressDeletions, embedded)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.MULTISIG_ACCOUNT_MODIFICATION.GetValue(),
                Deadline = ((ulong)10101010101).ConvertFromUInt64(),
                Fee = ((ulong)20202020202).ConvertFromUInt64()
            };
        }

        public AggregateTransaction CreateAggregateComplete(string txsHash, UnsignedTransaction[] embeddedTransactions, byte[] cosignatures, bool embedded)
        {
            return new AggregateTransaction(txsHash, embeddedTransactions, cosignatures, TransactionTypes.Types.AGGREGATE_COMPLETE, embedded)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.AGGREGATE_COMPLETE.GetValue(),
                Deadline = ((ulong)10101010101).ConvertFromUInt64(),
                Fee = ((ulong)20202020202).ConvertFromUInt64()
            };
        }

        public AggregateTransaction CreateAggregateBonded(string txsHash, UnsignedTransaction[] embeddedTransactions, byte[] cosignatures, bool embedded)
        {
            return new AggregateTransaction(txsHash, embeddedTransactions, cosignatures, TransactionTypes.Types.AGGREGATE_BONDED, embedded)
            {
                EntityBody = DefaultEntityBody,
                Type = TransactionTypes.Types.AGGREGATE_COMPLETE.GetValue(),
                Deadline = ((ulong)10101010101).ConvertFromUInt64(),
                Fee = ((ulong)20202020202).ConvertFromUInt64()
            };
        }
    }
}
