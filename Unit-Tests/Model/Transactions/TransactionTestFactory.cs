using Coppery;
using io.nem2.sdk.src.Model;
using io.nem2.sdk.src.Model.Accounts;
using io.nem2.sdk.src.Model.Articles;
using io.nem2.sdk.src.Model.Transactions;
using io.nem2.sdk.src.Model.Transactions.AccountRestrictions;
using io.nem2.sdk.src.Model.Transactions.CrossChainTransactions;
using io.nem2.sdk.src.Model.Transactions.KeyLinkTransactions;
using io.nem2.sdk.src.Model.Transactions.Messages;
using io.nem2.sdk.src.Model.Transactions.MetadataTransactions;
using io.nem2.sdk.src.Model.Transactions.MosaicPropertiesTransactions;
using io.nem2.sdk.src.Model.Transactions.MosaicRestrictions;

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

        public MosaicSupplyChangeTransaction CreateMosaicSupplyChangeTransaction(ulong delta, string mosaicId, MosaicSupplyType.Type supplyType, bool embedded)
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
                Deadline = DataConverter.ConvertFrom(10101010101), // DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(20202020202)
            };
        }

        public NamespaceMetadataTransaction CreateNamespaceMetadataTransaction(string targetAddress, string scopedKey, string targetNamespaceId, ushort valueSizeDelta, ushort valueSize, byte[] value, bool embedded)
        {
            return new NamespaceMetadataTransaction(targetAddress, scopedKey, targetNamespaceId, valueSizeDelta, valueSize, value, embedded)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Network = NetworkType.GetNetworkByte(),
                    Version = 0x01
                },
                Deadline = DataConverter.ConvertFrom(10101010101), // DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(20202020202)
            };
        }

        public AccountMetadataTransaction CreateAccountMetadataTransaction(string targetAddress, string scopedKey, ushort valueSizeDelta, ushort valueSize, byte[] value, bool embedded)
        {
            return new AccountMetadataTransaction(targetAddress, scopedKey, valueSizeDelta, valueSize, value, embedded)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Network = NetworkType.GetNetworkByte(),
                    Version = 0x01
                },
                Deadline = DataConverter.ConvertFrom(10101010101), // DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(20202020202)
            };
        }

        public MosaicMetadataTransaction CreateMosaicMetadataTransaction(string targetAddress, string scopedKey, string targetMosaicId, ushort valueSizeDelta, ushort valueSize, byte[] value, bool embedded)
        {
            return new MosaicMetadataTransaction(targetAddress, scopedKey, targetMosaicId, valueSizeDelta, valueSize, value, embedded)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Network = NetworkType.GetNetworkByte(),
                    Version = 0x01
                },
                Deadline = DataConverter.ConvertFrom(10101010101), // DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(20202020202)
            };
        }

        public MosaicDefinitionTransaction CreateMosaicDefinitionTransaction(string mosaicName, string namespaceId, string mosaicId, MosaicProperties properties, bool embedded)
        {
            return new MosaicDefinitionTransaction(mosaicName, namespaceId, mosaicId, properties, embedded)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Network = NetworkType.GetNetworkByte(),
                    Version = 0x01
                },
                Deadline = DataConverter.ConvertFrom(10101010101), // DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(20202020202)
            };
        }

        public LockFundsTransaction CreateLockFundsTransaction(Tuple<string, ulong> mosaic, ulong duration, string transactionHash, bool embedded)
        {
            return new LockFundsTransaction(mosaic, duration, transactionHash, embedded)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Network = NetworkType.GetNetworkByte(),
                    Version = 0x01
                },
                Deadline = DataConverter.ConvertFrom(10101010101), // DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(20202020202)
            };
        }

        public SecretLockTransaction CreateSecretLockTransaction(Tuple<string, ulong> mosaic, ulong duration, string secret, HashType.Types hashAlgo, string recipient, bool embedded)
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
                Deadline = DataConverter.ConvertFrom(10101010101), // DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(20202020202)
            };

        }

        public SecretProofTransaction CreateSecretProofTransaction(string recipientAddress, string secret, HashType.Types hashAlgo, string proof, bool embedded)
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
                Deadline = DataConverter.ConvertFrom(10101010101), // DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(20202020202)
            };
        }

        public RegisterNamespace CreateNamespaceRegistrationTransaction(ulong duration, ulong parentId, ulong id, NamespaceTypes.Types type, string name, bool embedded)
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
                Deadline = DataConverter.ConvertFrom(10101010101), // DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(20202020202)
            };
        }

        public VotingKeyLinkTransaction CreateVotingKeyLinkTransaction(TransactionTypes.Types type, ulong startEpoch, ulong endEpoch, string linkedPublicKey, byte linkAction, bool embedded)
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
                Type = type.GetValue(),
                Deadline = DataConverter.ConvertFrom(10101010101), // DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(20202020202)
            };
        }

        public KeyLinkTransaction CreateKeyLinkTransaction(TransactionTypes.Types type, string linkedPublicKey, byte linkAction, bool embedded)
        {
            return new KeyLinkTransaction(linkedPublicKey, linkAction, embedded)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Network = NetworkType.GetNetworkByte(),
                    Version = 0x01
                },
                Type = type.GetValue(),
                Deadline = DataConverter.ConvertFrom(10101010101), // DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(20202020202)
            };
        }

        public TransferTransaction_V1 CreateTransferTransaction(Address address, IMessage messege, Mosaic mosaic, bool embedded)
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
                Deadline = DataConverter.ConvertFrom(10101010101),
                Fee = DataConverter.ConvertFrom(287428975982)
            };
        }



        public AccountRestrictionsTransaction CreateAccountRestrictionTransaction(TransactionTypes.Types type, ushort restrictionFlags, string[] additions, string[] deletions, bool embedded)
        {
            return new AccountRestrictionsTransaction(type, restrictionFlags, additions, deletions, embedded)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Network = NetworkType.GetNetworkByte(),
                    Version = 0x01
                },
                Deadline = DataConverter.ConvertFrom(10101010101),
                Fee = DataConverter.ConvertFrom(20202020202)
            };
        }

        public MosaicAddressRestrictionTransaction CreateMosaicAddressRestrictionTransaction(string targetAddress, string mosaicID, string restrictionKey, string previousRestrictionValue, string newRestrictionValue, bool embedded)
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
                Deadline = DataConverter.ConvertFrom(10101010101),//DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(20202020202)
            };
        }

        public MosaicGlobalRestrictionTransaction CreateMosaicGlobalRestrictionTransaction(string referenceMosaicId, string mosaicID, string restrictionKey, string previousRestrictionValue, string newRestrictionValue, bool embedded)
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
                Deadline = DataConverter.ConvertFrom(10101010101), // DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(20202020202)
            };
        }

        public AddressAliasTransaction CreateAddressAliasTransaction(string address, ulong namepaceId, byte aliasAction, bool embedded)
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
                Deadline = DataConverter.ConvertFrom(10101010101), // DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(20202020202)
            };
        }

        public MosaicAliasTransaction CreateMosaicAliasTransaction(string mosaicId, ulong namepaceId, byte aliasAction, bool embedded)
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
                Deadline = DataConverter.ConvertFrom(10101010101), // DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(20202020202)
            };
        }

        public MultisigAccountModificationTransaction CreateMultisigAccountTransaction(byte minApproval, byte minRemoval, string[] addressAdditions, string[] addressDeletions)
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
                Deadline = DataConverter.ConvertFrom(10101010101), // DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(20202020202)
            };
        }

        public AggregateTransaction CreateAggregateComplete(string txsHash, UnsignedTransaction[] embeddedTransactions, byte[] cosignatures)
        {
            return new AggregateTransaction(txsHash, embeddedTransactions, cosignatures, TransactionTypes.Types.AGGREGATE_COMPLETE)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Network = NetworkType.GetNetworkByte(),
                    Version = 0x01
                },
                Deadline = DataConverter.ConvertFrom(10101010101), // DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(20202020202)
            };
        }

        public AggregateTransaction CreateAggregateBonded(string txsHash, UnsignedTransaction[] embeddedTransactions, byte[] cosignatures)
        {
            return new AggregateTransaction(txsHash, embeddedTransactions, cosignatures, TransactionTypes.Types.AGGREGATE_BONDED)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Network = NetworkType.GetNetworkByte(),
                    Version = 0x01
                },
                Deadline = DataConverter.ConvertFrom(10101010101), // DataConverter.ConvertFrom(Deadline.AddHours(1, NetworkType).Ticks),
                Fee = DataConverter.ConvertFrom(20202020202)
            };
        }
    }
}
