using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Infrastructure.Buffers.Model.JsonConverters;
using io.nem2.sdk.src.Model.Network;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses
{   
    public class Aggregate : TransactionData
    {      
        public string TransactionsHash { get; set; }
        public List<Cosignature> Cosignatures { get; set; }
        public List<EmbeddedTransactionData> Embedded { get; set; }
    }

    public class Cosignature
    {
        public int Version { get; set; }
        public string SignerPublicKey { get; set; }
        public string Signature { get; set; }
    }

    public class EmbeddedTransactionData
    {
        public class Metadata
        {
            [JsonProperty("height")]
            public ulong Height { get; set; }

            [JsonProperty("aggregateHash")]
            public string AggregateHash { get; set; }

            [JsonProperty("aggregateId")]
            public string AggregateId { get; set; }

            [JsonProperty("index")]
            public int Index { get; set; }

            [JsonProperty("timestamp")]
            public ulong Timestamp { get; set; }

            [JsonProperty("feeMultiplier")]
            public int FeeMultiplier { get; set; }
        }

        [JsonProperty("meta")]
        public Metadata Meta { get; set; }

        [JsonProperty("signerPublicKey")]
        public string SignerPublicKey { get; set; }

        [JsonProperty("version")]
        public int Version;

        [JsonProperty("network")]
        public NetworkType.Types Network { get; set; }

        [JsonProperty("type")]
        public TransactionTypes.Types Type { get; set; }      
    }

    public class TransactionData
    {
        public class Metadata
        {
            [JsonProperty("height")]
            public ulong Height { get; set; }

            [JsonProperty("hash")]
            public string Hash { get; set; }

            [JsonProperty("merkleComponentHash")]
            public string MerkleComponentHash { get; set; }

            [JsonProperty("index")]
            public int Index { get; set; }

            [JsonProperty("timestamp")]
            public ulong Timestamp { get; set; }

            [JsonProperty("feeMultiplier")]
            public int FeeMultiplier { get; set; }
        }

        [JsonProperty("meta")]
        public Metadata Meta { get; set; }

        [JsonProperty("signerPublicKey")]
        public string SignerPublicKey { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("network")]
        public NetworkType.Types Network { get; set; }

        [JsonProperty("type")]
        public TransactionTypes.Types Type { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }

        [JsonProperty("maxFee")]
        public ulong MaxFee { get; set; }

        [JsonProperty("deadline")]
        public ulong Deadline { get; set; }
    }

    public class EmbeddedSimpleTransfer : EmbeddedTransactionData
    {
        [JsonProperty("recipientAddress")]
        public string RecipientAddress { get; set; }

        [JsonProperty("mosaics")]
        public List<MosaicTransfer> Mosaics { get; set; }

        [JsonProperty("Message")]
        public string Messege { get; set; }
    }
    public class SimpleTransfer : TransactionData
    {
        [JsonProperty("recipientAddress")]
        public string RecipientAddress { get; set; }

        [JsonProperty("mosaics")]
        public List<MosaicTransfer> Mosaics { get; set; }

        [JsonProperty("Message")]
        public string Messege { get; set; }
    }

    public class MosaicTransfer
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("amount")]
        public ulong Amount { get; set; }
    }

    public class EmbeddedKeyLink : EmbeddedTransactionData // account key link + node key link + VRF key link
    {

        [JsonProperty("linkedPublicKey")]
        public string LinkedPublicKey { get; set; }

        [JsonProperty("linkAction")]
        public int LinkAction { get; set; }
    }

    [Description("Account, Node, VRF, Differentiate with field Type of type TransactionType.Types")]
    public class KeyLink : TransactionData // account key link + node key link + VRF key link
    {

        [JsonProperty("linkedPublicKey")]
        public string LinkedPublicKey { get; set; }

        [JsonProperty("linkAction")]
        public int LinkAction { get; set; }
    }

    public class EmbeddedVotingKeyLink : EmbeddedKeyLink
    {

        [JsonProperty("startEpoch")]
        public ulong StartEpoch { get; set; }

        [JsonProperty("endEpoch")]
        public ulong EndEpoch { get; set; }
    }

    public class VotingKeyLink : KeyLink
    {
        [JsonProperty("startEpoch")]
        public ulong StartEpoch { get; set; }

        [JsonProperty("endEpoch")]
        public ulong EndEpoch { get; set; }
    }

    public class PublicKeys
    {
        [JsonProperty("publicKeys")]
        public List<string> Public_Keys { get; set; }

       //[JsonProperty("addresses")]
       //public List<string> Addresses { get; set; }    
    }

    public class EmbeddedMosaicSupplyChange : EmbeddedTransactionData
    {

        [JsonProperty("mosaicId")]
        public string MosaicId { get; set; }

        [JsonProperty("action")]
        public int Action { get; set; }

        [JsonProperty("delta")]
        public ulong Delta { get; set; }


    }

    public class MosaicSupplyChange : TransactionData
    {

        [JsonProperty("mosaicId")]
        public string MosaicId { get; set; }

        [JsonProperty("action")]
        public int Action { get; set; }

        [JsonProperty("delta")]
        public ulong Delta { get; set; }

        
    }

    public class EmbeddedMosaicSupplyRevocation : EmbeddedTransactionData
    {

        [JsonProperty("sourceAddress")]
        public string SourceAddress { get; set; }

        [JsonProperty("mosaicId")]
        public string MosaicId { get; set; }

        [JsonProperty("amount")]
        public ulong Amount { get; set; }
    }

    public class MosaicSupplyRevocation : TransactionData
    {

        [JsonProperty("sourceAddress")]
        public string SourceAddress { get; set; }

        [JsonProperty("mosaicId")]
        public string MosaicId { get; set; }

        [JsonProperty("amount")]
        public ulong Amount { get; set; }
    }

    public class EmbeddedNamespaceRegistration : EmbeddedTransactionData
    {

        [JsonProperty("registrationType")]
        public int RegistrationType { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class NamespaceRegistration : TransactionData
    {

        [JsonProperty("registrationType")]
        public int RegistrationType { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class EmbeddedRootNamespaceRegistration : EmbeddedNamespaceRegistration
    {
        [JsonProperty("parentId")]
        public string ParentId { get; set; }
    }

    public class EmbeddedChildNamespaceRegistration : EmbeddedNamespaceRegistration
    {
        [JsonProperty("duration")]
        public ulong Duraation { get; set; }
    }

    public class RootNamespaceRegistration : NamespaceRegistration 
    {
        [JsonProperty("duration")]
        public ulong Duration { get; set; }
    }

    public class ChildNamespaceRegistration : NamespaceRegistration
    {
        [JsonProperty("parentId")]
        public string ParentId { get; set; }
    }

    public class EmbeddedSecretProofLock : EmbeddedTransactionData
    {

        [JsonProperty("hashAlgorithm")]
        public int HashAlgorithm { get; set; }

        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("recipientAddress")]
        public string RecipientAddress { get; set; }
    }

    public class SecretProofLock : TransactionData
    {

        [JsonProperty("hashAlgorithm")]
        public int HashAlgorithm { get; set; }

        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("recipientAddress")]
        public string RecipientAddress { get; set; }
    }

    public class EmbeddedHashLockTransaction : EmbeddedTransactionData
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

    public class HashLockT : TransactionData
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

    public class EmbeddedAccountAddressRestriction : EmbeddedTransactionData // Address, Mosaic, Operation Restriction
    {
        [JsonProperty("restrictionAdditions")]
        public List<string> RestrictionAdditions { get; set; }

        [JsonProperty("restrictionDeletions")]
        public List<string> RestrictionDeletions { get; set; }

        [JsonProperty("restrictionFlags")]
        public int Flags { get; set; }
    }

    public class AccountAddressRestriction : TransactionData // Address, Mosaic
    {
        [JsonProperty("restrictionAdditions")]
        public List<string> RestrictionAdditions { get; set; }

        [JsonProperty("restrictionDeletions")]
        public List<string> RestrictionDeletions { get; set; }

        [JsonProperty("restrictionFlags")]
        public int Flags { get; set; }
    }

    public class EmbeddedAccountMosaicRestriction : EmbeddedTransactionData // Address, Mosaic, Operation Restriction
    {
        [JsonProperty("restrictionAdditions")]
        public List<string> RestrictionAdditions { get; set; }

        [JsonProperty("restrictionDeletions")]
        public List<string> RestrictionDeletions { get; set; }

        [JsonProperty("restrictionFlags")]
        public int Flags { get; set; }
    }

    public class AccountMosaicRestriction : TransactionData // Address, Mosaic
    {
        [JsonProperty("restrictionAdditions")]
        public List<string> RestrictionAdditions { get; set; }

        [JsonProperty("restrictionDeletions")]
        public List<string> RestrictionDeletions { get; set; }

        [JsonProperty("restrictionFlags")]
        public int Flags { get; set; }
    }

    public class AccountOpperationRestriction : TransactionData
    {
        [JsonProperty("restrictionAdditions")]
        public List<int> RestrictionAdditions { get; set; }

        [JsonProperty("restrictionDeletions")]
        public List<int> RestrictionDeletions { get; set; }

        [JsonProperty("restrictionFlags")]
        public int Flags { get; set; }
    }

    public class EmbeddedAccountOpperationRestriction : EmbeddedTransactionData
    {
        [JsonProperty("restrictionAdditions")]
        public List<int> RestrictionAdditions { get; set; }

        [JsonProperty("restrictionDeletions")]
        public List<int> RestrictionDeletions { get; set; }

        [JsonProperty("restrictionFlags")]
        public int Flags { get; set; }
    }

    public class EmbeddedMosaicAddressRestriction : EmbeddedTransactionData
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

    public class MosaicAddressRestriction : TransactionData
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

    public class EmbeddedAliasTransaction : EmbeddedTransactionData
    {
        [JsonProperty("namespaceId")]
        public string NamespaceId { get; set; }

        [JsonProperty("aliasAction")]
        public int AliasAction { get; set; }
    }

    public class AliasTransaction : TransactionData
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

    public class EmbeddedMosaicDefinition : EmbeddedTransactionData
    {
        [JsonProperty("nonce")]
        public int Nonce { get; set; }

        [JsonProperty("id")]
        public string MosaicId { get; set; }

        [JsonProperty("flags")]
        public int Flags { get; set; }

        [JsonProperty("divisibility")]
        public int Divisibility { get; set; }

        [JsonProperty("duration")]
        public ulong Duration { get; set; }

    }

    public class MosaicDefinition : TransactionData
    {
        [JsonProperty("nonce")]
        public int Nonce { get; set; }

        [JsonProperty("id")]
        public string MosaicId { get; set; }

        [JsonProperty("flags")]
        public int Flags { get; set; }

        [JsonProperty("divisibility")]
        public int Divisibility { get; set; }

        [JsonProperty("duration")]
        public ulong Duration { get; set; }

    }


    public class SecretLockEvent
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

        [JsonProperty("hash")]
        public string Hash { get; set; }
    }
}
