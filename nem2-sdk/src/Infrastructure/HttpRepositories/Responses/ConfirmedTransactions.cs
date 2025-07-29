using io.nem2.sdk.src.Model2.Accounts;
using System.ComponentModel;


namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses
{
    public class Aggregate : TransactionData.BaseTransaction
    {
        public string TransactionsHash { get; set; }

        public List<Cosignature> Cosignatures { get; set; }

        public List<EmbeddedTransactionData> Transactions { get; set; }
    }

    public class EmbeddedMultisigModification : EmbeddedTransactionData.EmbeddedBaseTransaction // Multisig modification must be embedded
    {
        public int minRemovalDelta { get; set; }

        public int minApprovalDelta { get; set; }

        public List<string> addressAdditions { get; set; }

        public List<string> addressDeletions { get; set; }
    }

    public class Cosignature
    {
        public int Version { get; set; }

        public string SignerPublicKey { get; set; }

        public string Signature { get; set; }
    }

    public class EmbeddedTransactionData
    {
        public Metadata Meta { get; set; }

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

        public class EmbeddedBaseTransaction
        {
            public string SignerPublicKey { get; set; }

            public byte Version { get; set; }

            public byte Network { get; set; }

            public ushort Type { get; set; }
        }
    }

    public class TransactionData
    {
        public Metadata Meta { get; set; }

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
        }
    }

    public class AccountMetadata : TransactionData.BaseTransaction
    {
        public string TargetAddress { get; set; }

        public string ScopedMetadataKey { get; set; }

        public int ValueSizeDelta { get; set; }

        public int ValueSize { get; set; }

        public string Value { get; set; }
    }

    public class EmbeddedAccountMetadata : EmbeddedTransactionData.EmbeddedBaseTransaction
    {
        public string TargetAddress { get; set; }

        public string ScopedMetadataKey { get; set; }

        public int ValueSizeDelta { get; set; }

        public int ValueSize { get; set; }

        public string Value { get; set; }
    }

    public class EmbeddedSimpleTransfer : EmbeddedTransactionData.EmbeddedBaseTransaction
    {
        public string RecipientAddress { get; set; }

        public List<MosaicTransfer> Mosaics { get; set; }

        public string Message { get; set; }
    }

    public class SimpleTransfer : TransactionData.BaseTransaction
    {
        public string RecipientAddress { get; set; }

        public List<MosaicTransfer> Mosaics { get; set; }

        public string Message { get; set; }
    }

    public class EmbeddedKeyLink : EmbeddedTransactionData.EmbeddedBaseTransaction // account key link + node key link + VRF key link
    {
        public string LinkedPublicKey { get; set; }

        public int LinkAction { get; set; }
    }

    [Description("Account, Node, VRF, Differentiate with field Type of type TransactionType.Types")]
    public class KeyLink : TransactionData.BaseTransaction // account key link + node key link + VRF key link
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
    public class EmbeddedEventMetadata : EmbeddedTransactionData.EmbeddedBaseTransaction
    {
        public string TargetAddress { get; set; }

        public string ScopedMetadataKey { get; set; }

        public int ValueSizeDelta { get; set; }

        public int ValueSize { get; set; }

        public string Value { get; set; }
    }

    public class EventMetadata : TransactionData.BaseTransaction
    {
        public string TargetAddress { get; set; }

        public string ScopedMetadataKey { get; set; }   
        
        public int ValueSizeDelta { get; set; }

        public int ValueSize { get; set; }

        public string Value { get; set; }
    }

    public class EmbeddedMosaicSupplyChange : EmbeddedTransactionData.EmbeddedBaseTransaction
    {
        public string MosaicId { get; set; }

        public int Action { get; set; }

        public ulong Delta { get; set; }
    }

    public class MosaicSupplyChange : TransactionData.BaseTransaction
    {
        public string MosaicId { get; set; }

        public int Action { get; set; }

        public ulong Delta { get; set; }     
    }

    public class EmbeddedMosaicSupplyRevocation : EmbeddedTransactionData.EmbeddedBaseTransaction
    {
        public string SourceAddress { get; set; }

        public string MosaicId { get; set; }

        public ulong Amount { get; set; }
    }

    public class MosaicSupplyRevocation : TransactionData.BaseTransaction
    {
        public string SourceAddress { get; set; }

        public string MosaicId { get; set; }

        public ulong Amount { get; set; }
    }

    public class EmbeddedNamespaceRegistration : EmbeddedTransactionData.EmbeddedBaseTransaction
    {
        public int RegistrationType { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }
    }

    public class NamespaceRegistration : TransactionData.BaseTransaction
    {
        public int RegistrationType { get; set; }

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

    public class EmbeddedSecretProofLock : EmbeddedTransactionData.EmbeddedBaseTransaction
    {
        public int HashAlgorithm { get; set; }

        public string Secret { get; set; }

        public string RecipientAddress { get; set; }
    }

    public class SecretProofLock : TransactionData.BaseTransaction
    {
        public int HashAlgorithm { get; set; }

        public string Secret { get; set; }

        public string RecipientAddress { get; set; }
    }

    public class EmbeddedHashLockT : EmbeddedTransactionData.EmbeddedBaseTransaction
    {
        public ulong Duration { get; set; }

        public string MosaicId { get; set; }

        public ulong Amount { get; set; }

        public string Hash { get; set; }
    }

    public class HashLockT : TransactionData.BaseTransaction
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

    public class EmbeddedAccountAddressRestriction : EmbeddedTransactionData.EmbeddedBaseTransaction // Address, Mosaic, Operation Restriction
    {
        public List<string> RestrictionAdditions { get; set; }

        public List<string> RestrictionDeletions { get; set; }

        public int RestrictionFlags { get; set; }
    }

    public class AccountRestriction : TransactionData.BaseTransaction
    {
        public List<string> RestrictionAdditions { get; set; }

        public List<string> RestrictionDeletions { get; set; }

        public int RestrictionFlags { get; set; }
    }

    public class EmbeddedAccountRestriction : EmbeddedTransactionData.EmbeddedBaseTransaction // Address, Mosaic, Operation Restriction
    {
        public List<string> RestrictionAdditions { get; set; }

        public List<string> RestrictionDeletions { get; set; }

        public int RestrictionFlags { get; set; }
    }

    public class AccountOperationRestriction : TransactionData.BaseTransaction
    {
        public List<ushort> RestrictionAdditions { get; set; }

        public List<ushort> RestrictionDeletions { get; set; }

        public int RestrictionFlags { get; set; }
    }

    public class EmbeddedAccountOperationRestriction : EmbeddedTransactionData.EmbeddedBaseTransaction
    {
        public List<ushort> RestrictionAdditions { get; set; }

        public List<ushort> RestrictionDeletions { get; set; }

        public int RestrictionFlags { get; set; }
    }

    public class EmbeddedMosaicAddressRestriction : EmbeddedTransactionData.EmbeddedBaseTransaction
    {
        public string MosaicId { get; set; }

        public string RestrictionKey { get; set; }

        public Address TargetAddress { get; set; }

        public string PreviousRestrictionValue { get; set; }

        public string NewRestrictionValue { get; set; }
    }

    public class MosaicAddressRestriction : TransactionData.BaseTransaction
    {
        public string MosaicId { get; set; }

        public string RestrictionKey { get; set; }

        public string TargetAddress { get; set; }

        public ulong PreviousRestrictionValue { get; set; }

        public ulong NewRestrictionValue { get; set; }      
    }

    public class EmbeddedAliasTransaction : EmbeddedTransactionData.EmbeddedBaseTransaction
    {
        public string NamespaceId { get; set; }

        public int AliasAction { get; set; }
    }

    public class AliasTransaction : TransactionData.BaseTransaction
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

    public class EmbeddedMosaicDefinition : EmbeddedTransactionData.EmbeddedBaseTransaction
    {
        public ulong Nonce { get; set; }

        public string Id { get; set; }

        public int Flags { get; set; }

        public int Divisibility { get; set; }

        public ulong Duration { get; set; }

    }

    public class MosaicDefinition : TransactionData.BaseTransaction
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
