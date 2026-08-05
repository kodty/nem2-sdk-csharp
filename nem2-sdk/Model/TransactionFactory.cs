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

        public SimpleTransaction<T> CreateTransaction<T>(T transaction, ulong fee) where T : TransactionExtension
        {
            return new SimpleTransaction<T>(transaction, NetworkType, fee, Deadline.AddHours(1, NetworkType))
            {
                Signer = null,
                Network = NetworkType.GetNetworkByte()
            };
        }

        public SimpleTransaction<TransferTransaction_V1> CreateTransferTransaction(Address address, IMessage messege, Mosaic mosaic, ulong fee)
        {
            return CreateTransaction(new TransferTransaction_V1(address, messege, mosaic), fee);
        }

        public SimpleTransaction<LockFundsTransaction> CreateHashLockTransaction(string mosaic, ulong amount, ulong duration, string transactionHash, ulong fee)
        {
            return CreateTransaction(new LockFundsTransaction(mosaic, amount, duration, transactionHash), fee);
        }

        public VerifiableTransaction CreateAggregateBonded(AggregatePayload payload, NetworkType.Types networkType, byte[] signer, ulong fee, Deadline deadline)
        {
            return VerifiableTransaction.Create(payload, networkType, fee, deadline);
        }

        public SimpleTransaction<AggregatePayload> CreateAggregateComplete(AggregatePayload payload, NetworkType.Types networkType, byte[] signer, ulong fee, Deadline deadline)
        {
            return VerifiableTransaction.Create(payload, networkType, fee, deadline);
        }

        public SimpleTransaction<MultisigAccountModificationTransaction> CreateMultisigAccountTransaction(byte minApproval, byte minRemoval, string[] addressAdditions, string[] addressDeletions, ulong fee)
        {
            return CreateTransaction(new MultisigAccountModificationTransaction(minApproval, minRemoval, addressAdditions, addressDeletions), fee);
        }

        public SimpleTransaction<AccountRestrictionsTransaction> CreateAccountRestrictionTransaction(TransactionTypes.Types type, ushort restrictionFlags, string[] additions, string[] deletions, ulong fee)
        { // covers account mosaic, account address, account operation restrictions

            return CreateTransaction(new AccountRestrictionsTransaction(type, restrictionFlags, additions, deletions), fee);
        }

        public SimpleTransaction<KeyLinkTransaction> CreateKeyLinkTransaction(TransactionTypes.Types type, string linkedPublicKey, byte linkAction, ulong fee)
        {
            return CreateTransaction(new KeyLinkTransaction(type, linkedPublicKey, linkAction), fee);
        }

        public SimpleTransaction<VotingKeyLinkTransaction> CreateVotingKeyLinkTransaction(uint startEpoch, uint endEpoch, string linkedPublicKey, byte linkAction, ulong fee)
        {
            return CreateTransaction(new VotingKeyLinkTransaction(startEpoch, endEpoch, linkedPublicKey, linkAction), fee);
        }

        public SimpleTransaction<AddressAliasTransaction> CreateAddressAliasTransaction(string address, ulong namepaceId, byte aliasAction, ulong fee)
        {
            return CreateTransaction(new AddressAliasTransaction(address, namepaceId, aliasAction), fee);
        }

        public SimpleTransaction<AccountMetadataTransaction> CreateAccountMetadataTransaction(string targetAddress, string scopedKey, ushort valueSizeDelta, ushort valueSize, byte[] value, ulong fee)
        {
            return CreateTransaction(new AccountMetadataTransaction(targetAddress, scopedKey, valueSizeDelta, valueSize, value), fee);
        }

        public SimpleTransaction<MosaicAliasTransaction> CreateMosaicAliasTransaction(ulong mosaicId, ulong namepaceId, byte aliasAction, ulong fee)
        {
            return CreateTransaction(new MosaicAliasTransaction(mosaicId, namepaceId, aliasAction), fee);
        }

        public SimpleTransaction<MosaicMetadataTransaction> CreateMosaicMetadataTransaction(string targetAddress, string scopedKey, string targetMosaicId, ushort valueSizeDelta, ushort valueSize, byte[] value, ulong fee)
        {
            return CreateTransaction(new MosaicMetadataTransaction(targetAddress, scopedKey, targetMosaicId, valueSizeDelta, valueSize, value), fee);
        }

        public SimpleTransaction<RegisterNamespace> CreateNamespaceRegistrationTransaction(ulong duration, ulong parentId, ulong id, NamespaceTypes.Types type, string name, ulong fee)
        {
            return CreateTransaction(new RegisterNamespace(duration, parentId, id, type, name), fee);
        }

        public SimpleTransaction<NamespaceMetadataTransaction> CreateNamespaceMetadataTransaction(string targetAddress, string scopedKey, string targetNamespaceId, ushort valueSizeDelta, ushort valueSize, byte[] value, ulong fee)
        {
            return CreateTransaction(new NamespaceMetadataTransaction(targetAddress, scopedKey, targetNamespaceId, valueSizeDelta, valueSize, value), fee);
        }

        public SimpleTransaction<MosaicDefinitionTransaction> CreateMosaicDefinitionTransaction(ulong id, uint nonce, MosaicProperties properties, ulong fee)
        {
            return CreateTransaction(new MosaicDefinitionTransaction(id, nonce, properties), fee);
        }

        public SimpleTransaction<MosaicAddressRestrictionTransaction> CreateMosaicAddressRestrictionTransaction(string targetAddress, string mosaicID, string restrictionKey, string previousRestrictionValue, string newRestrictionValue, ulong fee)
        {
            return CreateTransaction(new MosaicAddressRestrictionTransaction(targetAddress, mosaicID, restrictionKey, previousRestrictionValue, newRestrictionValue), fee);
        }

        public SimpleTransaction<MosaicGlobalRestrictionTransaction> CreateMosaicGlobalRestrictionTransaction(string referenceMosaicId, string mosaicID, string restrictionKey, string previousRestrictionValue, string newRestrictionValue, byte previousRestrictionType, byte newRestrictionType, ulong fee)
        {
            return CreateTransaction(new MosaicGlobalRestrictionTransaction(referenceMosaicId, mosaicID, restrictionKey, previousRestrictionValue, newRestrictionValue, previousRestrictionType, newRestrictionType), fee);
        }

        public SimpleTransaction<MosaicSupplyChangeTransaction> CreateMosaicSupplyChangeTransaction(ulong delta, ulong mosaicId, MosaicSupplyType.Type supplyType, ulong fee)
        {
            return CreateTransaction(new MosaicSupplyChangeTransaction(delta, mosaicId, supplyType), fee);
        }

        public SimpleTransaction<MosaicReclamationTransaction> CreateMosaicReclamationTransaction(Address debtorImposed, ulong mosaicId, ulong amount, ulong fee)
        {
            return CreateTransaction(new MosaicReclamationTransaction(debtorImposed, mosaicId, amount), fee);
        }

        public SimpleTransaction<SecretLockTransaction> CreateSecretLockTransaction(string mosaic, ulong amount, ulong duration, string secret, HashType.Types hashAlgo, string recipient, ulong fee)
        {
            return CreateTransaction(new SecretLockTransaction(mosaic, amount, duration, secret, hashAlgo, recipient), fee);
        }

        public SimpleTransaction<SecretProofTransaction> CreateSecretProofTransaction(string recipientAddress, string secret, HashType.Types hashAlgo, string proof, ulong fee)
        {
            return CreateTransaction(new SecretProofTransaction(recipientAddress, secret, hashAlgo, proof), fee);
        }
    }  
}
