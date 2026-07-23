using Coppery;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Articles;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.Model.Transactions.AccountRestrictions;
using io.nem2.sdk.Model.Transactions.CrossChainTransactions;
using io.nem2.sdk.Model.Transactions.KeyLinkTransactions;
using io.nem2.sdk.Model.Transactions.Messages;
using io.nem2.sdk.Model.Transactions.MetadataTransactions;
using io.nem2.sdk.Model.Transactions.MosaicPropertiesTransactions;
using io.nem2.sdk.Model.Transactions.MosaicRestrictions;

namespace io.nem2.sdk.Model
{
    public class TransactionFactory
    {
        internal NetworkType.Types NetworkType { get; set; }

        internal ulong DefaultDeadline { get; set; }

        internal ulong Fee { get; set; }
        
        internal string Node { get; set; }

        internal int Port { get; set; }

        public TransactionFactory(NetworkType.Types networkType, string node, int port)
        {
            NetworkType = networkType;
            Node = node;
            Port = port;
        }

        public TransactionFactory(NetworkType.Types networkType, ulong deadline, ulong fee, string node, int port)
        {
            NetworkType = networkType;
            Fee = fee;
            DefaultDeadline = deadline;
            Node = node;
            Port = port;
        }

        public TransferTransaction_V1 CreateTransferTransaction(Address address, IMessage messege, Mosaic mosaic, ulong fee, bool embedded)
        {
            return new TransferTransaction_V1(address, messege, mosaic, embedded)
            {
                Signer = null,
                Network = NetworkType.GetNetworkByte(),
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public LockFundsTransaction CreateHashLockTransaction(string mosaic, ulong amount, ulong duration, string transactionHash, ulong fee, bool embedded)
        {
            return new LockFundsTransaction(mosaic, amount, duration, transactionHash, embedded)
            {
                Signer = null,
                Network = NetworkType.GetNetworkByte(),
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public AggregateTransaction CreateAggregateBonded(SignedTransaction[] embeddedTransactions, byte[] signer, byte[] cosignatures, ulong fee)
        {
            return new AggregateTransaction(embeddedTransactions, cosignatures, TransactionTypes.Types.AGGREGATE_BONDED)
            {
                Signer = signer,
                Network = NetworkType.GetNetworkByte(),
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public AggregateTransaction CreateAggregateComplete(SignedTransaction[] embeddedTransactions, byte[] signer, ulong fee)
        {
            return new AggregateTransaction(embeddedTransactions, null, TransactionTypes.Types.AGGREGATE_COMPLETE)
            {
                Signer = signer,
                Network = NetworkType.GetNetworkByte(),
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public MultisigAccountModificationTransaction CreateMultisigAccountTransaction(byte minApproval, byte minRemoval, string[] addressAdditions, string[] addressDeletions, ulong fee)
        {
            return new MultisigAccountModificationTransaction(minApproval, minRemoval, addressAdditions, addressDeletions)
            {
                Signer = null,
                Network = NetworkType.GetNetworkByte(),
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public AccountRestrictionsTransaction CreateAccountRestrictionTransaction(TransactionTypes.Types type, ushort restrictionFlags, string[] additions, string[] deletions, ulong fee, bool embedded)
        { // covers account mosaic, account address, account operation restrictions
            return new AccountRestrictionsTransaction(type, restrictionFlags, additions, deletions, embedded)
            {
                Signer = null,
                Network = NetworkType.GetNetworkByte(),
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public KeyLinkTransaction CreateKeyLinkTransaction(TransactionTypes.Types type, string linkedPublicKey, byte linkAction, byte linkType, ulong fee, bool embedded)
        {
            return new KeyLinkTransaction(type, linkedPublicKey, linkAction, embedded)
            {
                Signer = null,
                Network = NetworkType.GetNetworkByte(),
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public VotingKeyLinkTransaction CreateVotingKeyLinkTransaction(TransactionTypes.Types type, ulong startEpoch, ulong endEpoch, string linkedPublicKey, byte linkAction, byte linkType, ulong fee, bool embedded)
        {
            return new VotingKeyLinkTransaction(startEpoch, endEpoch, linkedPublicKey, linkAction, embedded)
            {
                Signer = null,
                Network = NetworkType.GetNetworkByte(),
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public AddressAliasTransaction CreateAddressAliasTransaction(string address, string namepaceId, byte aliasAction, ulong fee, bool embedded)
        {
            return new AddressAliasTransaction(address, namepaceId, aliasAction, embedded)
            {
                Signer = null,
                Network = NetworkType.GetNetworkByte(),
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public AccountMetadataTransaction CreateAccountMetadataTransaction(string targetAddress, string scopedKey, ushort valueSizeDelta, ushort valueSize, byte[] value, ulong fee)
        {
            return new AccountMetadataTransaction(targetAddress, scopedKey, valueSizeDelta, valueSize, value)
            {
                Signer = null,
                Network = NetworkType.GetNetworkByte(),
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public MosaicAliasTransaction CreateMosaicAliasTransaction(string mosaicId, string namepaceId, byte aliasAction, ulong fee, bool embedded)
        {
            return new MosaicAliasTransaction(mosaicId, namepaceId, aliasAction, embedded)
            {
                Signer = null,
                Network = NetworkType.GetNetworkByte(),
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public MosaicMetadataTransaction CreateMosaicMetadataTransaction(string targetAddress, string scopedKey, string targetMosaicId, ushort valueSizeDelta, ushort valueSize, byte[] value, ulong fee)
        {
            return new MosaicMetadataTransaction(targetAddress, scopedKey, targetMosaicId, valueSizeDelta, valueSize, value)
            {
                Signer = null,
                Network = NetworkType.GetNetworkByte(),
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public RegisterNamespace CreateNamespaceRegistrationTransaction(ulong duration, ulong parentId, ulong id, NamespaceTypes.Types type, string name, ulong fee, bool embedded)
        {
            return new RegisterNamespace(duration, parentId, id, type, name, embedded)
            {
                Signer = null,
                Network = NetworkType.GetNetworkByte(),
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public NamespaceMetadataTransaction CreateNamespaceMetadataTransaction(string targetAddress, string scopedKey, string targetNamespaceId, ushort valueSizeDelta, ushort valueSize, byte[] value, ulong fee)
        {
            return new NamespaceMetadataTransaction(targetAddress, scopedKey, targetNamespaceId, valueSizeDelta, valueSize, value)
            {
                Signer = null,
                Network = NetworkType.GetNetworkByte(),
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public MosaicDefinitionTransaction CreateMosaicDefinitionTransaction(string id, uint nonce, MosaicProperties properties, ulong fee, bool embedded)
        {
            return new MosaicDefinitionTransaction(id, nonce, properties, embedded)
            {
                Signer = null,
                Network = NetworkType.GetNetworkByte(),
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public MosaicAddressRestrictionTransaction CreateMosaicAddressRestrictionTransaction(string targetAddress, string mosaicID, string restrictionKey, string previousRestrictionValue, string newRestrictionValue, ulong fee, bool embedded)
        {
            return new MosaicAddressRestrictionTransaction(targetAddress, mosaicID, restrictionKey, previousRestrictionValue, newRestrictionValue, embedded)
            {
                Signer = null,
                Network = NetworkType.GetNetworkByte(),
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public MosaicGlobalRestrictionTransaction CreateMosaicGlobalRestrictionTransaction(string referenceMosaicId, string mosaicID, string restrictionKey, string previousRestrictionValue, string newRestrictionValue, byte previousRestrictionType, byte newRestrictionType, ulong fee, bool embedded)
        {
            return new MosaicGlobalRestrictionTransaction(referenceMosaicId, mosaicID, restrictionKey, previousRestrictionValue, newRestrictionValue, previousRestrictionType, newRestrictionType, embedded)
            {
                Signer = null,
                Network = NetworkType.GetNetworkByte(),
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public MosaicSupplyChangeTransaction CreateMosaicSupplyChangeTransaction(ulong delta, string mosaicId, MosaicSupplyType.Type supplyType, ulong fee, bool embedded)
        {
            return new MosaicSupplyChangeTransaction(delta, mosaicId, supplyType, embedded)
            {
                Signer = null,
                Network = NetworkType.GetNetworkByte(),
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public SecretLockTransaction CreateSecretLockTransaction(string mosaic, ulong duration, string secret, HashType.Types hashAlgo, string recipient, ulong fee, bool embedded)
        {
            return new SecretLockTransaction(mosaic, duration, secret, hashAlgo, recipient, embedded)
            {
                Signer = null,
                Network = NetworkType.GetNetworkByte(),
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee =  DataConverter.ConvertFrom(fee)
            };

        }

        public SecretProofTransaction CreateSecretProofTransaction(string recipientAddress, string secret, HashType.Types hashAlgo, string proof, ulong fee, bool embedded)
        {
            return new SecretProofTransaction(recipientAddress, secret, hashAlgo, proof, embedded)
            {
                Signer = null,
                Network = NetworkType.GetNetworkByte(),
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee =  DataConverter.ConvertFrom(fee)           
            };
        }
    }  
}
