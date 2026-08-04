using Coppery;
using io.nem2.sdk.Model;
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

namespace Unit_Tests.Model.Transactions
{
    public class TransactionTestFactory
    {
        internal NetworkType.Types NetworkType { get; set; }
        internal ulong DefaultDeadline { get; set; }
        internal ulong Fee { get; set; }
        internal string Node { get; set; }
        internal int Port { get; set; }

        public TransactionTestFactory(NetworkType.Types type, string node, int port)
        {
            NetworkType = type;
            Node = node;
            Port = port;
        }

        public TransactionTestFactory(NetworkType.Types networkType, byte version, ulong deadline, ulong fee, string node, int port)
        {
            NetworkType = networkType;
            Fee = fee;
            DefaultDeadline = deadline;
            Node = node;
            Port = port;

            
        }

        public SimpleTransaction CreateTransaction(TransactionExtension transaction, ulong fee)
        {
            return new SimpleTransaction(transaction, NetworkType, fee, Deadline.AddHours(1))
            {
                Signer = null,
                Network = NetworkType.GetNetworkByte(),
                Deadline = DataConverter.ConvertFrom(10101010101),
                Fee = DataConverter.ConvertFrom(20202020202)
            };
        }

        public SimpleTransaction CreateTransferTransaction(Address address, IMessage messege, Mosaic mosaic, ulong fee)
        {
            return CreateTransaction(new TransferTransaction_V1(address, messege, mosaic), fee);
        }

        public SimpleTransaction CreateHashLockTransaction(string mosaic, ulong amount, ulong duration, string transactionHash, ulong fee)
        {
            return CreateTransaction(new LockFundsTransaction(mosaic, amount, duration, transactionHash), fee);
        }

        public SimpleTransaction CreateAggregateBonded(AggregatePayload payload, NetworkType.Types networkType, ulong fee)
        {
            return CreateTransaction(payload, fee);
        }

        public SimpleTransaction CreateAggregateComplete(AggregatePayload payload, NetworkType.Types networkType, ulong fee)
        {
            return CreateTransaction(payload, fee);
        }

        public SimpleTransaction CreateMultisigAccountTransaction(byte minApproval, byte minRemoval, string[] addressAdditions, string[] addressDeletions, ulong fee)
        {
            return CreateTransaction(new MultisigAccountModificationTransaction(minApproval, minRemoval, addressAdditions, addressDeletions), fee);
        }

        public SimpleTransaction CreateAccountRestrictionTransaction(TransactionTypes.Types type, ushort restrictionFlags, string[] additions, string[] deletions, ulong fee)
        { // covers account mosaic, account address, account operation restrictions

            return CreateTransaction(new AccountRestrictionsTransaction(type, restrictionFlags, additions, deletions), fee);
        }

        public SimpleTransaction CreateKeyLinkTransaction(TransactionTypes.Types type, string linkedPublicKey, byte linkAction, ulong fee)
        {
            return CreateTransaction(new KeyLinkTransaction(type, linkedPublicKey, linkAction), fee);
        }

        public SimpleTransaction CreateVotingKeyLinkTransaction(uint startEpoch, uint endEpoch, string linkedPublicKey, byte linkAction, ulong fee)
        {
            return CreateTransaction(new VotingKeyLinkTransaction(startEpoch, endEpoch, linkedPublicKey, linkAction), fee);
        }

        public SimpleTransaction CreateAddressAliasTransaction(string address, string namepaceId, byte aliasAction, ulong fee)
        {
            return CreateTransaction(new AddressAliasTransaction(address, namepaceId, aliasAction), fee);
        }

        public SimpleTransaction CreateAccountMetadataTransaction(string targetAddress, string scopedKey, ushort valueSizeDelta, ushort valueSize, byte[] value, ulong fee)
        {
            return CreateTransaction(new AccountMetadataTransaction(targetAddress, scopedKey, valueSizeDelta, valueSize, value), fee);
        }

        public SimpleTransaction CreateMosaicAliasTransaction(string mosaicId, string namepaceId, byte aliasAction, ulong fee)
        {
            return CreateTransaction(new MosaicAliasTransaction(mosaicId, namepaceId, aliasAction), fee);
        }

        public SimpleTransaction CreateMosaicMetadataTransaction(string targetAddress, string scopedKey, string targetMosaicId, ushort valueSizeDelta, ushort valueSize, byte[] value, ulong fee)
        {
            return CreateTransaction(new MosaicMetadataTransaction(targetAddress, scopedKey, targetMosaicId, valueSizeDelta, valueSize, value), fee);
        }

        public SimpleTransaction CreateNamespaceRegistrationTransaction(ulong duration, ulong parentId, ulong id, NamespaceTypes.Types type, string name, ulong fee)
        {
            return CreateTransaction(new RegisterNamespace(duration, parentId, id, type, name), fee);
        }

        public SimpleTransaction CreateNamespaceMetadataTransaction(string targetAddress, string scopedKey, string targetNamespaceId, ushort valueSizeDelta, ushort valueSize, byte[] value, ulong fee)
        {
            return CreateTransaction(new NamespaceMetadataTransaction(targetAddress, scopedKey, targetNamespaceId, valueSizeDelta, valueSize, value), fee);
        }

        public SimpleTransaction CreateMosaicDefinitionTransaction(string id, uint nonce, MosaicProperties properties, ulong fee)
        {
            return CreateTransaction(new MosaicDefinitionTransaction(id, nonce, properties), fee);
        }

        public SimpleTransaction CreateMosaicAddressRestrictionTransaction(string targetAddress, string mosaicID, string restrictionKey, string previousRestrictionValue, string newRestrictionValue, ulong fee)
        {
            return CreateTransaction(new MosaicAddressRestrictionTransaction(targetAddress, mosaicID, restrictionKey, previousRestrictionValue, newRestrictionValue), fee);
        }

        public SimpleTransaction CreateMosaicGlobalRestrictionTransaction(string referenceMosaicId, string mosaicID, string restrictionKey, string previousRestrictionValue, string newRestrictionValue, byte previousRestrictionType, byte newRestrictionType, ulong fee)
        {
            return CreateTransaction(new MosaicGlobalRestrictionTransaction(referenceMosaicId, mosaicID, restrictionKey, previousRestrictionValue, newRestrictionValue, previousRestrictionType, newRestrictionType), fee);
        }

        public SimpleTransaction CreateMosaicSupplyChangeTransaction(ulong delta, string mosaicId, MosaicSupplyType.Type supplyType, ulong fee)
        {
            return CreateTransaction(new MosaicSupplyChangeTransaction(delta, mosaicId, supplyType), fee);
        }

        public SimpleTransaction CreateMosaicReclamationTransaction(Address debtorImposed, string mosaicId, ulong amount, ulong fee)
        {
            return CreateTransaction(new MosaicReclamationTransaction(debtorImposed, mosaicId, amount), fee);
        }

        public SimpleTransaction CreateSecretLockTransaction(string mosaic, ulong amount, ulong duration, string secret, HashType.Types hashAlgo, string recipient, ulong fee)
        {
            return CreateTransaction(new SecretLockTransaction(mosaic, amount, duration, secret, hashAlgo, recipient), fee);
        }

        public SimpleTransaction CreateSecretProofTransaction(string recipientAddress, string secret, HashType.Types hashAlgo, string proof, ulong fee)
        {
            return CreateTransaction(new SecretProofTransaction(recipientAddress, secret, hashAlgo, proof), fee);
        }
    }
}
