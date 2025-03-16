using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Model.Network;
using Newtonsoft.Json;
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

            public ushort Version { get; set; }

            public NetworkType.Types Network { get; set; }

            public TransactionTypes.Types Type { get; set; }
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
            public string SignerPublicKey { get; set; }

            public int Version { get; set; }

            public NetworkType.Types Network { get; set; }

            public TransactionTypes.Types Type { get; set; }

            public int Size { get; set; }

            public string Signature { get; set; }

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

        public string Messege { get; set; }
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

        [JsonProperty("hashAlgorithm")]
        public int HashAlgorithm { get; set; }

        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("recipientAddress")]
        public string RecipientAddress { get; set; }
    }

    public class EmbeddedHashLockT : EmbeddedTransactionData.EmbeddedBaseTransaction
    {

        [JsonProperty("duration")]
        public ulong Duration { get; set; }

        [JsonProperty("mosaicId")]
        public string MosaicId { get; set; }

        [JsonProperty("amount")]
        public ulong Amount { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

    }

    public class HashLockT : TransactionData.BaseTransaction
    {

        [JsonProperty("duration")]
       
        public ulong Duration { get; set; }

        [JsonProperty("mosaicId")]
        public string MosaicId { get; set; }

        [JsonProperty("amount")]
        public ulong Amount { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

    }

    public class EmbeddedSecretLockT : EmbeddedSecretProofLock
    {
        [JsonProperty("duration")]
        public ulong Duration { get; set; }

        [JsonProperty("mosaicId")]
        public string MosaicId { get; set; }

        [JsonProperty("amount")]
        public ulong Amount { get; set; }

    }

    public class SecretLockT : SecretProofLock
    {
        [JsonProperty("duration")]
        public ulong Duration { get; set; }

        [JsonProperty("mosaicId")]
        public string MosaicId { get; set; }

        [JsonProperty("amount")]
        public ulong Amount { get; set; }
       
    }

    public class EmbeddedSecretProofT : EmbeddedSecretProofLock
    {
        [JsonProperty("proof")]
        public string Proof { get; set; }

    }

    public class SecretProofT : SecretProofLock
    {
        [JsonProperty("proof")]
        public string Proof { get; set; }
        
    }

    public class EmbeddedAccountAddressRestriction : EmbeddedTransactionData.EmbeddedBaseTransaction // Address, Mosaic, Operation Restriction
    {
        [JsonProperty("restrictionAdditions")]
        public List<string> RestrictionAdditions { get; set; }

        [JsonProperty("restrictionDeletions")]
        public List<string> RestrictionDeletions { get; set; }

        [JsonProperty("restrictionFlags")]
        public int RestrictionFlags { get; set; }
    }

    public class AccountAddressRestriction : TransactionData.BaseTransaction // Address, Mosaic // check tests, absent
    {
        [JsonProperty("restrictionAdditions")]
        public List<string> RestrictionAdditions { get; set; }

        [JsonProperty("restrictionDeletions")]
        public List<string> RestrictionDeletions { get; set; }

        [JsonProperty("restrictionFlags")]
        public int RestrictionFlags { get; set; }
    }

    public class EmbeddedAccountMosaicRestriction : EmbeddedTransactionData.EmbeddedBaseTransaction // Address, Mosaic, Operation Restriction
    {
        public List<string> RestrictionAdditions { get; set; }

        public List<string> RestrictionDeletions { get; set; }

        public int RestrictionFlags { get; set; }
    }

    public class AccountMosaicRestriction : TransactionData.BaseTransaction // Address, Mosaic
    {
        [JsonProperty("restrictionAdditions")]
        public List<string> RestrictionAdditions { get; set; }

        [JsonProperty("restrictionDeletions")]
        public List<string> RestrictionDeletions { get; set; }

        [JsonProperty("restrictionFlags")]
        public int RestrictionFlags { get; set; }
    }

    public class AccountOpperationRestriction : TransactionData.BaseTransaction
    {
        [JsonProperty("restrictionAdditions")]
        public List<int> RestrictionAdditions { get; set; }

        [JsonProperty("restrictionDeletions")]
        public List<int> RestrictionDeletions { get; set; }

        [JsonProperty("restrictionFlags")]
        public int RestrictionFlags { get; set; }
    }

    public class EmbeddedAccountOpperationRestriction : EmbeddedTransactionData.EmbeddedBaseTransaction
    {
        [JsonProperty("restrictionAdditions")]
        public List<int> RestrictionAdditions { get; set; }

        [JsonProperty("restrictionDeletions")]
        public List<int> RestrictionDeletions { get; set; }

        [JsonProperty("restrictionFlags")]
        public int RestrictionFlags { get; set; }
    }

    public class EmbeddedMosaicAddressRestriction : EmbeddedTransactionData.EmbeddedBaseTransaction
    {
        [JsonProperty("mosaicId")]
        public string MosaicId { get; set; }

        [JsonProperty("restrictionKey")]
        public string RestrictionKey { get; set; }

        [JsonProperty("targetAddress")]
        public Address TargetAddress { get; set; }

        [JsonProperty("previousRestrictionValue")]
        public string PreviousRestrictionValue { get; set; }

        [JsonProperty("newRestrictionValue")]
        public string NewRestrictionValue { get; set; }
    }

    public class MosaicAddressRestriction : TransactionData.BaseTransaction
    {
        [JsonProperty("mosaicId")]
        public string MosaicId { get; set; }

        [JsonProperty("restrictionKey")]
        public string RestrictionKey { get; set; }

        [JsonProperty("targetAddress")]
        public Address TargetAddress { get; set; }

        [JsonProperty("previousRestrictionValue")]
        public string PreviousRestrictionValue { get; set; }

        [JsonProperty("newRestrictionValue")]
        public string NewRestrictionValue { get; set; }      
    }

    public class EmbeddedAliasTransaction : EmbeddedTransactionData.EmbeddedBaseTransaction
    {
        [JsonProperty("namespaceId")]
        public string NamespaceId { get; set; }

        [JsonProperty("aliasAction")]
        public int AliasAction { get; set; }
    }

    public class AliasTransaction : TransactionData.BaseTransaction
    {
        [JsonProperty("namespaceId")] // address
        public string NamespaceId { get; set; }

        [JsonProperty("aliasAction")]
        public int AliasAction { get; set; }
    }


    public class EmbeddedAddressAlias : EmbeddedAliasTransaction
    {
        [JsonProperty("address")]
        public string Address { get; set; }
    }

    public class AddressAlias : AliasTransaction
    {
        [JsonProperty("address")]
        public string Address { get; set; }    
    }

    public class EmbeddedMosaicAlias : EmbeddedAliasTransaction
    {
        [JsonProperty("mosaicId")]
        public string MosaicId { get; set; }
    }

    public class MosaicAlias : AliasTransaction
    {
        [JsonProperty("mosaicId")]
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

        public string MosaicId { get; set; }

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
        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("ownerAddress")]
        public string OwnerAddress { get; set; }

        [JsonProperty("mosaicId")]
        public string MosaicId { get; set; }

        [JsonProperty("amount")]
        public ulong Amount { get; set; }

        [JsonProperty("endHeight")]
        public ulong EndHeight { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("hashAlgorithm")]
        public int HashAlgorithm { get; set; }

        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("recipientAddress")]
        public string RecipientAddress { get; set; }

        [JsonProperty("compositeHash")]
        public string CompositeHash { get; set; }
    }

    public class HashLockEvent
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("ownerAddress")]
        public string OwnerAddress { get; set; }

        [JsonProperty("mosaicId")]
        public string MosaicId { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("endHeight")]
        public string EndHeight { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }
        public string Hash { get; set; }
    }
}
