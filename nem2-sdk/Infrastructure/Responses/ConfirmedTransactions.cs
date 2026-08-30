using Coppery;
using io.nem2.sdk.Model.Accounts;
using System.ComponentModel;
using System.Text.Json.Serialization;


namespace io.nem2.sdk.Infrastructure.Responses
{
    public class Aggregate : BaseTransaction
    {
        public string TransactionsHash { get; set; }

        public List<Cosignature> Cosignatures { get; set; }

        public List<EmbeddedTransactionData> Transactions { get; set; }
    }

    public class EmbeddedMultisigModification : EmbeddedBaseTransaction // Multisig modification must be embedded
    {
        public int MinRemovalDelta { get; set; }

        public int MinApprovalDelta { get; set; }

        public List<string> AddressAdditions { get; set; }

        public List<string> AddressDeletions { get; set; }
    }

    public class Cosignature
    {
        public ulong Version { get; set; }

        public string SignerPublicKey { get; set; }

        public string Signature { get; set; }
    }

    public class EmbeddedTransactionData
    {
        public Metadata Meta { get; set; }

        [JsonConverter(typeof(ExtendEmbeddedBaseTransactionConverter))]
        public EmbeddedBaseTransaction Transaction { get; set; }

        public string Id { get; set; }

        public class Metadata
        {
            public ulong Height { get; set; }

            public string AggregateHash { get; set; }

            public string AggregateId { get; set; }

            public int Index { get; set; }

            public ulong Timestamp { get; set; }

            public int FeeMultiplier { get; set; }
        }

       
    }

    public class EmbeddedBaseTransaction
    {
        public string SignerPublicKey { get; set; }

        public byte Version { get; set; }

        public byte Network { get; set; }

        public ushort Type { get; set; }

        public T AsExtendedType<T>() where T : EmbeddedBaseTransaction
        {
            return (T)this;
        }
    }

    public class TransactionData<T> where T : BaseTransaction
    {
        public Metadata Meta { get; set; }

        [JsonConverter(typeof(ExtendBaseTransactionConverter))]
        public T Transaction { get; set; }

        public string Id { get; set; }

        public class Metadata
        {
            public ulong Height { get; set; }

            public string Hash { get; set; }

            public string MerkleComponentHash { get; set; }

            public int Index { get; set; }

            public ulong Timestamp { get; set; }

            public int FeeMultiplier { get; set; }
        }     
    }

    public class BaseTransaction
    {
        public int Size { get; set; }

        public string Signature { get; set; }

        public string SignerPublicKey { get; set; }

        public byte Version { get; set; }

        public byte Network { get; set; }

        public ushort Type { get; set; }

        public ulong MaxFee { get; set; }

        public ulong Deadline { get; set; }

        public T AsExtendedType<T>() where T : BaseTransaction
        {
            return (T)this;
        }
    }

    public class TransactionData
    {      
        public Metadata Meta { get; set; }

        [JsonConverter(typeof(ExtendBaseTransactionConverter))]
        public BaseTransaction Transaction { get; set; }
    
        public string Id { get; set; }

        public class Metadata
        {
            public ulong Height { get; set; }

            public string Hash { get; set; }

            public string MerkleComponentHash { get; set; }

            public int Index { get; set; }

            public ulong Timestamp { get; set; }

            public int FeeMultiplier { get; set; }
        }
    }

    public class AccountMetadata : BaseTransaction
    {
        public string TargetAddress { get; set; }

        public string ScopedMetadataKey { get; set; }

        public int ValueSizeDelta { get; set; }

        public int ValueSize { get; set; }

        public string Value { get; set; }
    }

    public class EmbeddedAccountMetadata : EmbeddedBaseTransaction
    {
        public string TargetAddress { get; set; }

        public string ScopedMetadataKey { get; set; }

        public int ValueSizeDelta { get; set; }

        public int ValueSize { get; set; }

        public string Value { get; set; }
    }

    public class EmbeddedSimpleTransfer : EmbeddedBaseTransaction
    {
        public string RecipientAddress { get; set; }

        public List<MosaicTransfer> Mosaics { get; set; }

        public string Message { get; set; }
    }

    public class SimpleTransfer : BaseTransaction
    {
        public string RecipientAddress { get; set; }

        public List<MosaicTransfer> Mosaics { get; set; }

        public string Message { get; set; }
    }

    public class EmbeddedKeyLink : EmbeddedBaseTransaction // account key link + node key link + VRF key link
    {
        public string LinkedPublicKey { get; set; }

        public int LinkAction { get; set; }
    }

    [Description("Account, Node, VRF, Differentiate with field Type of type TransactionType.Types")]
    public class KeyLink : BaseTransaction // account key link + node key link + VRF key link
    {
        public string LinkedPublicKey { get; set; }

        public int LinkAction { get; set; }
    }

    public class EmbeddedVotingKeyLink : EmbeddedKeyLink
    {
        public ulong StartEpoch { get; set; }

        public ulong EndEpoch { get; set; }
    }

    public class VotingKeyLink : KeyLink
    {
        public ulong StartEpoch { get; set; }

        public ulong EndEpoch { get; set; }
    }

    public class Public_Keys
    {
        public List<string> publicKeys { get; set; }

       //[JsonProperty("addresses")]
       //public List<string> Addresses { get; set; }    
    }

    public class MosaicMetadata : EventMetadata
    {
        public string TargetMosaicId { get; set; }
    }

    public class EmbeddedMosaicMetadata : EmbeddedEventMetadata
    {
        public string TargetMosaicId { get; set; }
    }

    public class NamespaceMetadata : EventMetadata
    {
        public string TargetNamespaceId { get; set; }
    }

    public class EmbeddedNamespaceMetadata : EmbeddedEventMetadata
    {
        public string TargetNamespaceId { get; set; }
    }
    public class EmbeddedEventMetadata : EmbeddedBaseTransaction
    {
        public string TargetAddress { get; set; }

        public string ScopedMetadataKey { get; set; }

        public ushort ValueSizeDelta { get; set; }

        public ushort ValueSize { get; set; }

        public string Value { get; set; }
    }

    public class EventMetadata : BaseTransaction
    {
        public string TargetAddress { get; set; }

        public string ScopedMetadataKey { get; set; }   
        
        public int ValueSizeDelta { get; set; }

        public int ValueSize { get; set; }

        public string Value { get; set; }
    }

    public class EmbeddedMosaicSupplyChange : EmbeddedBaseTransaction
    {
        public string MosaicId { get; set; }

        public int Action { get; set; }

        public ulong Delta { get; set; }
    }

    public class MosaicSupplyChange : BaseTransaction
    {
        public string MosaicId { get; set; }

        public int Action { get; set; }

        public ulong Delta { get; set; }     
    }

    public class EmbeddedMosaicSupplyRevocation : EmbeddedBaseTransaction
    {
        public string SourceAddress { get; set; }

        public string MosaicId { get; set; }

        public ulong Amount { get; set; }
    }

    public class MosaicSupplyRevocation : BaseTransaction
    {
        public string SourceAddress { get; set; }

        public string MosaicId { get; set; }

        public ulong Amount { get; set; }
    }

    public class EmbeddedNamespaceRegistration : EmbeddedBaseTransaction
    {
        public ushort RegistrationType { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }
    }

    public class NamespaceRegistration : BaseTransaction
    {
        public ushort RegistrationType { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }
    }

    public class EmbeddedRootNamespaceRegistration : EmbeddedNamespaceRegistration
    {
        public string ParentId { get; set; }
    }

    public class EmbeddedChildNamespaceRegistration : EmbeddedNamespaceRegistration
    {
        public ulong Duration { get; set; }
    }

    public class RootNamespaceRegistration : NamespaceRegistration 
    {
        public ulong Duration { get; set; }
    }

    public class ChildNamespaceRegistration : NamespaceRegistration
    {
        public string ParentId { get; set; }
    }

    public class EmbeddedSecretProofLock : EmbeddedBaseTransaction
    {
        public int HashAlgorithm { get; set; }

        public string Secret { get; set; }

        public string RecipientAddress { get; set; }
    }

    public class SecretProofLock : BaseTransaction
    {
        public int HashAlgorithm { get; set; }

        public string Secret { get; set; }

        public string RecipientAddress { get; set; }
    }

    public class EmbeddedHashLockT : EmbeddedBaseTransaction
    {
        public ulong Duration { get; set; }

        public string MosaicId { get; set; }

        public ulong Amount { get; set; }

        public string Hash { get; set; }
    }

    public class HashLockT : BaseTransaction
    {     
        public ulong Duration { get; set; }

        public string MosaicId { get; set; }

        public ulong Amount { get; set; }

        public string Hash { get; set; }

    }

    public class EmbeddedSecretLockT : EmbeddedSecretProofLock
    {
        public ulong Duration { get; set; }

        public string MosaicId { get; set; }

        public ulong Amount { get; set; }

    }

    public class SecretLockT : SecretProofLock
    {
        public ulong Duration { get; set; }

        public string MosaicId { get; set; }

        public ulong Amount { get; set; }
       
    }

    public class EmbeddedSecretProofT : EmbeddedSecretProofLock
    {
        public string Proof { get; set; }

    }

    public class SecretProofT : SecretProofLock
    {
        public string Proof { get; set; }
        
    }

    public class EmbeddedAccountAddressRestriction : EmbeddedBaseTransaction // Address, Mosaic, Operation Restriction
    {
        public List<string> RestrictionAdditions { get; set; }

        public List<string> RestrictionDeletions { get; set; }

        public int RestrictionFlags { get; set; }
    }

    public class AccountRestriction : BaseTransaction
    {
        public List<string> RestrictionAdditions { get; set; }

        public List<string> RestrictionDeletions { get; set; }

        public int RestrictionFlags { get; set; }
    }

    public class EmbeddedAccountRestriction : EmbeddedBaseTransaction // Address, Mosaic, Operation Restriction
    {
        public List<string> RestrictionAdditions { get; set; }

        public List<string> RestrictionDeletions { get; set; }

        public int RestrictionFlags { get; set; }
    }

    public class AccountOperationRestriction : BaseTransaction
    {
        public List<ushort> RestrictionAdditions { get; set; }

        public List<ushort> RestrictionDeletions { get; set; }

        public int RestrictionFlags { get; set; }
    }

    public class EmbeddedAccountOperationRestriction : EmbeddedBaseTransaction
    {
        public List<ushort> RestrictionAdditions { get; set; }

        public List<ushort> RestrictionDeletions { get; set; }

        public int RestrictionFlags { get; set; }
    }

    public class EmbeddedMosaicAddressRestriction : EmbeddedBaseTransaction
    {
        public string MosaicId { get; set; }

        public string RestrictionKey { get; set; }

        public Address TargetAddress { get; set; }

        public string PreviousRestrictionValue { get; set; }

        public string NewRestrictionValue { get; set; }
    }

    public class MosaicAddressRestriction : BaseTransaction
    {
        public string MosaicId { get; set; }

        public string RestrictionKey { get; set; }

        public string TargetAddress { get; set; }

        public ulong PreviousRestrictionValue { get; set; }

        public ulong NewRestrictionValue { get; set; }      
    }

    public class EmbeddedAliasTransaction : EmbeddedBaseTransaction
    {
        public string NamespaceId { get; set; }

        public int AliasAction { get; set; }
    }

    public class AliasTransaction : BaseTransaction
    {
        public string NamespaceId { get; set; }

        public int AliasAction { get; set; }
    }


    public class EmbeddedAddressAlias : EmbeddedAliasTransaction
    {
        public string Address { get; set; }
    }

    public class AddressAlias : AliasTransaction
    {
        public string Address { get; set; }    
    }

    public class EmbeddedMosaicAlias : EmbeddedAliasTransaction
    {
        public string MosaicId { get; set; }
    }

    public class MosaicAlias : AliasTransaction
    {
        public string MosaicId { get; set; }
    }

    public class EmbeddedMosaicDefinition : EmbeddedBaseTransaction
    {
        public ulong Nonce { get; set; }

        public string Id { get; set; }

        public int Flags { get; set; }

        public int Divisibility { get; set; }

        public ulong Duration { get; set; }

    }

    public class MosaicDefinition : BaseTransaction
    {
        public ulong Nonce { get; set; }

        public string Id { get; set; }

        public int Flags { get; set; }

        public int Divisibility { get; set; }

        public ulong Duration { get; set; }

    }

    public class SecretLockEvent
    {
        public SecretLock Lock { get; set; }

        public string Id { get; set; }
    }
    public class SecretLock
    {
        public int Version { get; set; }

        public string OwnerAddress { get; set; }

        public string MosaicId { get; set; }

        public ulong Amount { get; set; }

        public ulong EndHeight { get; set; }

        public int Status { get; set; }

        public int HashAlgorithm { get; set; }

        public string Secret { get; set; }

        public string RecipientAddress { get; set; }

        public string CompositeHash { get; set; }
    }

    public class HashLock
    {
        public string Id { get; set; }

        public int Version { get; set; }

        public string OwnerAddress { get; set; }

        public string MosaicId { get; set; }

        public string Amount { get; set; }

        public string EndHeight { get; set; }

        public int Status { get; set; }

        public string Hash { get; set; }
    }

    public class HashLockEvent
    {
        public string Id { get; set; }
        
        public HashLock Lock { get; set; }
    }
}
