using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Mosaics;
using io.nem2.sdk.Model.Namespace;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.Model2.Transactions;
using io.nem2.sdk.Model2.Transactions.AccountRestrictions;
using io.nem2.sdk.Model2.Transactions.CrossChainTransactions;
using io.nem2.sdk.Model2.Transactions.KeyLinkTransactions;
using io.nem2.sdk.Model2.Transactions.MetadataTransactions;
using io.nem2.sdk.Model2.Transactions.MosaicPropertiesTransactions;
using io.nem2.sdk.Model2.Transactions.MosaicRestrictions;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Model.Network;

namespace io.nem2.sdk.Model2
{
    public class TransactionFactory
    {
        internal NetworkType.Types Type { get; set; }
        internal string Node { get; set; }
        internal int Port { get; set; }

        public TransactionFactory(NetworkType.Types type, string node, int port)
        {
            Type = type;
            Node = node;
            Port = port;
        }

        public MosaicSupplyChangeTransaction1 CreateMosaicSupplyChangeTransaction(ulong delta, Tuple<string, ulong> mosaic, MosaicSupplyType.Type supplyType)
        {
            return new MosaicSupplyChangeTransaction1(delta, mosaic, supplyType)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Version = 0x01,
                    Network = Type
                },
                Type = TransactionTypes.Types.MOSAIC_SUPPLY_CHANGE,
                Deadline = Deadline.AutoDeadline(Node, Port),
                Fee = 100
            };
        }

        public NamespaceMetadataTransaction1 CreateNamespaceMetadataTransaction(string targetAddress, string scopedKey, string targetNamespaceId, short valueSizeDelta, short valueSize, byte[] value)
        {
            return new NamespaceMetadataTransaction1(targetAddress, scopedKey, targetNamespaceId, valueSizeDelta, valueSize, value)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Version = 0x01,
                    Network = Type
                },
                Type = TransactionTypes.Types.MOSAIC_METADATA,
                Deadline = Deadline.AutoDeadline(Node, Port),
                Fee = 100
            };
        }

        public AccountMetadataTransaction1 CreateAccountMetadataTransaction(string targetAddress, string scopedKey, short valueSizeDelta, short valueSize, byte[] value)
        {
            return new AccountMetadataTransaction1(targetAddress, scopedKey, valueSizeDelta, valueSize, value)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Version = 0x01,
                    Network = Type
                },
                Type = TransactionTypes.Types.MOSAIC_METADATA,
                Deadline = Deadline.AutoDeadline(Node, Port),
                Fee = 100
            };
        }

        public MosaicMetadataTransaction1 CreateMosaicMetadataTransaction(string targetAddress, string scopedKey, string targetMosaicId, short valueSizeDelta, short valueSize, byte[] value)
        {
            return new MosaicMetadataTransaction1(targetAddress, scopedKey, targetMosaicId, valueSizeDelta, valueSize, value)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Version = 0x01,
                    Network = Type
                },
                Type = TransactionTypes.Types.MOSAIC_METADATA,
                Deadline = Deadline.AutoDeadline(Node, Port),
                Fee = 100
            };
        }

        public MosaicDefinitionTransaction1 CreateMosaicDefinitionTransaction(string mosaicName, NamespaceId namespaceId, MosaicId mosaicId, MosaicProperties properties)
        {
            return new MosaicDefinitionTransaction1(mosaicName, namespaceId, mosaicId, properties)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Version = 0x01,
                    Network = Type
                },
                Type = TransactionTypes.Types.MOSAIC_DEFINITION,
                Deadline = Deadline.AutoDeadline(Node, Port),
                Fee = 100
            };
        }

        public LockFundsTransaction1 CreateLockFundsTransaction(Tuple<string, ulong> mosaic, ulong duration, string transactionHash)
        {
            return new LockFundsTransaction1(mosaic, duration, transactionHash)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Version = 0x01,
                    Network = Type
                },
                Type = TransactionTypes.Types.HASH_LOCK,
                Deadline = Deadline.AutoDeadline(Node, Port),
                Fee = 100
            };
        }

        public SecretLockTransaction1 CreateSecretLockTransaction(Tuple<string, ulong> mosaic, ulong duration, string secret, HashType.Types hashAlgo, string recipient)
        {
            return new SecretLockTransaction1(mosaic, duration, secret, hashAlgo, recipient)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Version = 0x01,
                    Network = Type
                },
                Type = TransactionTypes.Types.SECRET_LOCK,
                Deadline = Deadline.AutoDeadline(Node, Port),
                Fee = 100
            };

        }

        public SecretProofTransaction1 CreateSecretProofTransaction(string recipientAddress, string secret, HashType.Types hashAlgo, string proof)
        {
            return new SecretProofTransaction1(recipientAddress, secret, hashAlgo, proof)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Version = 0x01,
                    Network = Type
                },
                Type = TransactionTypes.Types.SECRET_PROOF,
                Deadline = Deadline.AutoDeadline(Node, Port),
                Fee = 100           
            };
        }

        public RegisterNamespace CreateNamespaceRegistrationTransaction(ulong duration, NamespaceId parentId, NamespaceId id, NamespaceTypes.Types type, string name)
        {
            return new RegisterNamespace(duration, parentId, id, type, name)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Version = 0x01,
                    Network = Type
                },
                Type = TransactionTypes.Types.NAMESPACE_REGISTRATION,
                RegistrationType = parentId == null ? NamespaceTypes.Types.RootNamespace : NamespaceTypes.Types.SubNamespace,
                Deadline = Deadline.AutoDeadline(Node, Port),
                Fee = 100
            };
        }

        public KeyLinkTransaction1 CreateVotingKeyLinkTransaction(TransactionTypes.Types type, ulong startEpoch, ulong endEpoch, string linkedPublicKey, int linkAction, byte linkType)
        {
            return new VotingKeyLinkTransaction1(startEpoch, endEpoch, linkedPublicKey, linkAction)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Version = 0x01,
                    Network = Type
                },
                Type = TransactionTypes.Types.VOTING_KEY_LINK,
                Deadline = Deadline.AutoDeadline(Node, Port),
                Fee = 100
            };
        }

        public KeyLinkTransaction1 CreateKeyLinkTransaction(TransactionTypes.Types type, string linkedPublicKey, int linkAction, byte linkType)
        {
            return new KeyLinkTransaction1(linkedPublicKey, linkAction)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Version = 0x01,
                    Network = Type
                },
                Type = type,
                Deadline = Deadline.AutoDeadline(Node, Port),
                Fee = 100
            };
        }

        public TransferTransaction_V1 CreateTransferTransaction(string address, string messege, Tuple<string, ulong> mosaic)
        {
            return new TransferTransaction_V1(address, messege, mosaic)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Version = 0x01,
                    Network = Type
                },
                Type = TransactionTypes.Types.TRANSFER,
                Deadline = Deadline.AutoDeadline(Node, Port),
                Fee = 100
            };
        }

        public AccountRestrictionsTransaction1 CreateAccountRestrictionTransaction(TransactionTypes.Types type, int restrictionFlags, string[] additions, string[] deletions)
        {
            return new AccountRestrictionsTransaction1(type, restrictionFlags, additions, deletions)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Version = 0x01,
                    Network = Type
                },
                Type = type,
                Deadline = Deadline.AutoDeadline(Node, Port),
                Fee = 100
            };
        }

        public MosaicAddressRestrictionTransaction CreateMosaicAddressRestrictionTransaction(string targetAddress, string mosaicID, string restrictionKey, string previousRestrictionValue, string newRestrictionValue)
        {
            return new MosaicAddressRestrictionTransaction(targetAddress, mosaicID, restrictionKey, previousRestrictionValue, newRestrictionValue)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Version = 0x01,
                    Network = Type
                },
                Type = TransactionTypes.Types.MOSAIC_ADDRESS_RESTRICTION,
                Deadline = Deadline.AutoDeadline(Node, Port),
                Fee = 100
            };
        }

        public MosaicGlobalRestrictionTransaction CreateMosaicGlobalRestrictionTransaction(string referenceMosaicId, string mosaicID, string restrictionKey, string previousRestrictionValue, string newRestrictionValue)
        {
            return new MosaicGlobalRestrictionTransaction(referenceMosaicId, mosaicID, restrictionKey, previousRestrictionValue, newRestrictionValue)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Version = 0x01,
                    Network = Type
                },
                Type = TransactionTypes.Types.MOSAIC_GLOBAL_RESTRICTION,
                Deadline = Deadline.AutoDeadline(Node, Port),
                Fee = 100
            };
        }

        public AddressAliasTransaction1 CreateAddressAliasTransaction(string address, string namepaceId, byte aliasAction)
        {
            return new AddressAliasTransaction1(address, namepaceId, aliasAction)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Version = 0x01,
                    Network = Type
                },
                Type = TransactionTypes.Types.ADDRESS_ALIAS,
                Deadline = Deadline.AutoDeadline(Node, Port),
                Fee = 100
            };
        }

        public MosaicAliasTransaction1 CreateMosaicAliasTransaction(string mosaicId, string namepaceId, byte aliasAction)
        {
            return new MosaicAliasTransaction1(mosaicId, namepaceId, aliasAction)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Version = 0x01,
                    Network = Type
                },
                Type = TransactionTypes.Types.MOSAIC_ALIAS,
                Deadline = Deadline.AutoDeadline(Node, Port),
                Fee = 100
            };
        }

        public MultisigAccountModificationTransaction1 CreateMultisigAccountTransaction(byte minApproval, byte minRemoval, string[] addressAdditions, string[] addressDeletions)
        {
            return new MultisigAccountModificationTransaction1(minApproval, minRemoval, addressAdditions, addressDeletions)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Version = 0x01,
                    Network = Type
                },
                Type = TransactionTypes.Types.MULTISIG_ACCOUNT_MODIFICATION,
                Deadline = Deadline.AutoDeadline(Node, Port),
                Fee = 100
            };
        }

        public AggregateTransaction1 CreateAggregateComplete(string txsHash, byte[] embeddedTransactions, byte[] cosignatures)
        {
            return new AggregateTransaction1(txsHash, embeddedTransactions, cosignatures, TransactionTypes.Types.AGGREGATE_COMPLETE)
            {
                EntityBody = new EntityBody()
                {
                    Signer = null,
                    Entity_body_reserved_1 = 0,
                    Version = 0x01,
                    Network = Type
                },
                Type = TransactionTypes.Types.AGGREGATE_COMPLETE,
                Deadline = Deadline.AutoDeadline(Node, Port),
                Fee = 100
            };
        }
    }  
}
