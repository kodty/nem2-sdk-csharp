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
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Network = NetworkType.GetNetworkByte(),
                    Version = 0x01
                },
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public LockFundsTransaction CreateHashLockTransaction(string mosaic, ulong amount, ulong duration, string transactionHash, ulong fee, bool embedded)
        {
            return new LockFundsTransaction(mosaic, amount, duration, transactionHash, embedded)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Network = NetworkType.GetNetworkByte(),
                    Version = 0x01
                },
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public AggregateTransaction CreateAggregateBonded(UnsignedTransaction[] embeddedTransactions, byte[] signer, byte[] cosignatures, ulong fee)
        {
            var entityBody = new EntityBody()
            {
                Signer = signer,
                Entity_body_reserved_1 = 0,
                Network = NetworkType.GetNetworkByte(),
                Version = 0x03
            };

            return new AggregateTransaction(entityBody, embeddedTransactions, cosignatures, TransactionTypes.Types.AGGREGATE_BONDED)
            {
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public AggregateTransaction CreateAggregateComplete(UnsignedTransaction[] embeddedTransactions, byte[] signer, ulong fee)
        {
            var entityBody = new EntityBody()
            {
                Signer = signer,
                Entity_body_reserved_1 = 0,
                Network = NetworkType.GetNetworkByte(),
                Version = 0x03
            };

            return new AggregateTransaction(entityBody, embeddedTransactions, null, TransactionTypes.Types.AGGREGATE_COMPLETE)
            {       
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public MultisigAccountModificationTransaction CreateMultisigAccountTransaction(byte minApproval, byte minRemoval, string[] addressAdditions, string[] addressDeletions, ulong fee)
        {
            return new MultisigAccountModificationTransaction(minApproval, minRemoval, addressAdditions, addressDeletions)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Network = NetworkType.GetNetworkByte(),
                    Version = 0x01
                },
                Type = TransactionTypes.Types.MULTISIG_ACCOUNT_MODIFICATION.GetValue(),
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public AccountRestrictionsTransaction CreateAccountRestrictionTransaction(TransactionTypes.Types type, ushort restrictionFlags, string[] additions, string[] deletions, ulong fee, bool embedded)
        { // covers account mosaic, account address, account operation restrictions
            return new AccountRestrictionsTransaction(type, restrictionFlags, additions, deletions, embedded)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Network = NetworkType.GetNetworkByte()
                },
                Type = type.GetValue(),
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public KeyLinkTransaction CreateKeyLinkTransaction(TransactionTypes.Types type, string linkedPublicKey, byte linkAction, byte linkType, ulong fee, bool embedded)
        {
            return new KeyLinkTransaction(linkedPublicKey, linkAction, embedded)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Network = NetworkType.GetNetworkByte()
                },
                Type = type.GetValue(),
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public VotingKeyLinkTransaction CreateVotingKeyLinkTransaction(TransactionTypes.Types type, ulong startEpoch, ulong endEpoch, string linkedPublicKey, byte linkAction, byte linkType, ulong fee, bool embedded)
        {
            return new VotingKeyLinkTransaction(startEpoch, endEpoch, linkedPublicKey, linkAction, embedded)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Network = NetworkType.GetNetworkByte(),
                    Version = 0x01
                },
                Type = TransactionTypes.Types.VOTING_KEY_LINK.GetValue(),
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public AddressAliasTransaction CreateAddressAliasTransaction(string address, ulong namepaceId, byte aliasAction, ulong fee, bool embedded)
        {
            return new AddressAliasTransaction(address, namepaceId, aliasAction, embedded)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Network = NetworkType.GetNetworkByte(),
                    Version = 0x01
                },
                Type = TransactionTypes.Types.ADDRESS_ALIAS.GetValue(),
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public AccountMetadataTransaction CreateAccountMetadataTransaction(string targetAddress, string scopedKey, ushort valueSizeDelta, ushort valueSize, byte[] value, ulong fee)
        {
            return new AccountMetadataTransaction(targetAddress, scopedKey, valueSizeDelta, valueSize, value)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Network = NetworkType.GetNetworkByte(),
                    Version = 0x01
                },
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public MosaicAliasTransaction CreateMosaicAliasTransaction(string mosaicId, ulong namepaceId, byte aliasAction, ulong fee, bool embedded)
        {
            return new MosaicAliasTransaction(mosaicId, namepaceId, aliasAction, embedded)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Network = NetworkType.GetNetworkByte(),
                    Version = 0x01
                },
                Type = TransactionTypes.Types.MOSAIC_ALIAS.GetValue(),
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public MosaicMetadataTransaction CreateMosaicMetadataTransaction(string targetAddress, string scopedKey, string targetMosaicId, ushort valueSizeDelta, ushort valueSize, byte[] value, ulong fee)
        {
            return new MosaicMetadataTransaction(targetAddress, scopedKey, targetMosaicId, valueSizeDelta, valueSize, value)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Network = NetworkType.GetNetworkByte(),
                    Version = 0x01
                },
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public RegisterNamespace CreateNamespaceRegistrationTransaction(ulong duration, ulong parentId, ulong id, NamespaceTypes.Types type, string name, ulong fee, bool embedded)
        {
            return new RegisterNamespace(duration, parentId, id, type, name, embedded)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Network = NetworkType.GetNetworkByte(),
                    Version = 0x01
                },
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public NamespaceMetadataTransaction CreateNamespaceMetadataTransaction(string targetAddress, string scopedKey, string targetNamespaceId, ushort valueSizeDelta, ushort valueSize, byte[] value, ulong fee)
        {
            return new NamespaceMetadataTransaction(targetAddress, scopedKey, targetNamespaceId, valueSizeDelta, valueSize, value)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Network = NetworkType.GetNetworkByte(),
                    Version = 0x01
                },
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public MosaicDefinitionTransaction CreateMosaicDefinitionTransaction(string id, uint nonce, MosaicProperties properties, ulong fee, bool embedded)
        {
            return new MosaicDefinitionTransaction(id, nonce, properties, embedded)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Network = NetworkType.GetNetworkByte(),
                    Version = 0x01
                },
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public MosaicAddressRestrictionTransaction CreateMosaicAddressRestrictionTransaction(string targetAddress, string mosaicID, string restrictionKey, string previousRestrictionValue, string newRestrictionValue, ulong fee, bool embedded)
        {
            return new MosaicAddressRestrictionTransaction(targetAddress, mosaicID, restrictionKey, previousRestrictionValue, newRestrictionValue, embedded)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Network = NetworkType.GetNetworkByte(),
                    Version = 0x01
                },
                Type = TransactionTypes.Types.MOSAIC_ADDRESS_RESTRICTION.GetValue(),
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public MosaicGlobalRestrictionTransaction CreateMosaicGlobalRestrictionTransaction(string referenceMosaicId, string mosaicID, string restrictionKey, string previousRestrictionValue, string newRestrictionValue, ulong fee, bool embedded)
        {
            return new MosaicGlobalRestrictionTransaction(referenceMosaicId, mosaicID, restrictionKey, previousRestrictionValue, newRestrictionValue, embedded)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Network = NetworkType.GetNetworkByte(),
                    Version = 0x01
                },
                Type = TransactionTypes.Types.MOSAIC_GLOBAL_RESTRICTION.GetValue(),
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public MosaicSupplyChangeTransaction CreateMosaicSupplyChangeTransaction(ulong delta, string mosaicId, MosaicSupplyType.Type supplyType, ulong fee, bool embedded)
        {
            return new MosaicSupplyChangeTransaction(delta, mosaicId, supplyType, embedded)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Network = NetworkType.GetNetworkByte(),
                    Version = 0x01
                },
                Type = TransactionTypes.Types.MOSAIC_SUPPLY_CHANGE.GetValue(),
                SupplyType = supplyType.GetValue(),
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(fee)
            };
        }

        public SecretLockTransaction CreateSecretLockTransaction(string mosaic, ulong duration, string secret, HashType.Types hashAlgo, string recipient, ulong fee, bool embedded)
        {
            return new SecretLockTransaction(mosaic, duration, secret, hashAlgo, recipient, embedded)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Network = NetworkType.GetNetworkByte(),
                    Version = 0x01
                },
                Type = TransactionTypes.Types.SECRET_LOCK.GetValue(),
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee =  DataConverter.ConvertFrom(fee)
            };

        }

        public SecretProofTransaction CreateSecretProofTransaction(string recipientAddress, string secret, HashType.Types hashAlgo, string proof, ulong fee, bool embedded)
        {
            return new SecretProofTransaction(recipientAddress, secret, hashAlgo, proof, embedded)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Network = NetworkType.GetNetworkByte(),
                    Version = 0x01
                },
                Type = TransactionTypes.Types.SECRET_PROOF.GetValue(),
                Deadline = DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee =  DataConverter.ConvertFrom(fee)           
            };
        }
    }  
}
