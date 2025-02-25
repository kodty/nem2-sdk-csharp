using Newtonsoft.Json;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses
{
    public class NetworkInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

    }

    public class NetworkRentalFees
    {
        [JsonProperty("effectiveRootNamespaceRentalFeePerBlock")]
        public int EffectiveRootNamespaceRentalFeePerBlock { get; set; }

        [JsonProperty("effectiveChildNamespaceRentalFee")]
        public int EffectiveChildNamespaceRentalFee { get; set; }

        [JsonProperty("effectiveMosaicRentalFee")]
        public int EffectiveMosaicRentalFee { get; set; }
    }

    public class NetworkTransactionFees
    {
        [JsonProperty("averageFeeMultiplier")]
        public int averageFeeMultiplier { get; set; }

        [JsonProperty("medianFeeMultiplier")]
        public int medianFeeMultiplier { get; set; }

        [JsonProperty("highestFeeMultiplier")]
        public int highestFeeMultiplier { get; set; }

        [JsonProperty("lowestFeeMultiplier")]
        public int lowestFeeMultiplier { get; set; }

        [JsonProperty("minFeeMultiplier")]
        public int minFeeMultiplier { get; set; }
    }

    public class Accountlink
    {
        [JsonProperty("dummy")]
        public string Dummy { get; set; }
    }

    public class AggregateProperties
    {
        [JsonProperty("maxTransactionsPerAggregate")]
        public string MaxTransactionsPerAggregate { get; set; }

        [JsonProperty("maxCosignaturesPerAggregate")]
        public string MaxCosignaturesPerAggregate { get; set; }

        [JsonProperty("enableStrictCosignatureCheck")]
        public bool EnableStrictCosignatureCheck { get; set; }

        [JsonProperty("enableBondedAggregateSupport")]
        public bool EnableBondedAggregateSupport { get; set; }

        [JsonProperty("maxBondedTransactionLifetime")]
        public string MaxBondedTransactionLifetime { get; set; }
    }

    public class Chain
    {
        [JsonProperty("enableVerifiableState")]
        public bool EnableVerifiableState { get; set; }

        [JsonProperty("enableVerifiableReceipts")]
        public bool EnableVerifiableReceipts { get; set; }

        [JsonProperty("currencyMosaicId")]
        public string CurrencyMosaicId { get; set; }

        [JsonProperty("harvestingMosaicId")]
        public string HarvestingMosaicId { get; set; }

        [JsonProperty("blockGenerationTargetTime")]
        public string BlockGenerationTargetTime { get; set; }

        [JsonProperty("blockTimeSmoothingFactor")]
        public string BlockTimeSmoothingFactor { get; set; }

        [JsonProperty("importanceGrouping")]
        public string ImportanceGrouping { get; set; }

        [JsonProperty("importanceActivityPercentage")]
        public string ImportanceActivityPercentage { get; set; }

        [JsonProperty("maxRollbackBlocks")]
        public string MaxRollbackBlocks { get; set; }

        [JsonProperty("maxDifficultyBlocks")]
        public string MaxDifficultyBlocks { get; set; }

        [JsonProperty("defaultDynamicFeeMultiplier")]
        public string DefaultDynamicFeeMultiplier { get; set; }

        [JsonProperty("maxTransactionLifetime")]
        public string MaxTransactionLifetime { get; set; }

        [JsonProperty("maxBlockFutureTime")]
        public string MaxBlockFutureTime { get; set; }

        [JsonProperty("initialCurrencyAtomicUnits")]
        public string InitialCurrencyAtomicUnits { get; set; }

        [JsonProperty("maxMosaicAtomicUnits")]
        public string MaxMosaicAtomicUnits { get; set; }

        [JsonProperty("totalChainImportance")]
        public string TotalChainImportance { get; set; }

        [JsonProperty("minHarvesterBalance")]
        public string MinHarvesterBalance { get; set; }

        [JsonProperty("maxHarvesterBalance")]
        public string MaxHarvesterBalance { get; set; }

        [JsonProperty("minVoterBalance")]
        public string MinVoterBalance { get; set; }

        [JsonProperty("maxVotingKeysPerAccount")]
        public string MaxVotingKeysPerAccount { get; set; }

        [JsonProperty("minVotingKeyLifetime")]
        public string MinVotingKeyLifetime { get; set; }

        [JsonProperty("maxVotingKeyLifetime")]
        public string MaxVotingKeyLifetime { get; set; }

        [JsonProperty("harvestBeneficiaryPercentage")]
        public string HarvestBeneficiaryPercentage { get; set; }

        [JsonProperty("harvestNetworkPercentage")]
        public string HarvestNetworkPercentage { get; set; }

        [JsonProperty("harvestNetworkFeeSinkAddress")]
        public string HarvestNetworkFeeSinkAddress { get; set; }

        [JsonProperty("maxTransactionsPerBlock")]
        public string MaxTransactionsPerBlock { get; set; }
    }

    public class Lockhash
    {
        [JsonProperty("lockedFundsPerAggregate")]
        public string LockedFundsPerAggregate { get; set; }

        [JsonProperty("maxHashLockDuration")]
        public string MaxHashLockDuration { get; set; }
    }

    public class Locksecret
    {
        [JsonProperty("maxSecretLockDuration")]
        public string MaxSecretLockDuration { get; set; }

        [JsonProperty("minProofSize")]
        public string MinProofSize { get; set; }

        [JsonProperty("maxProofSize")]
        public string MaxProofSize { get; set; }
    }

    public class MetadataMax
    {
        [JsonProperty("maxValueSize")]
        public string MaxValueSize { get; set; }
    }

    public class MosaicNetworkProperties
    {
        [JsonProperty("maxMosaicsPerAccount")]
        public string MaxMosaicsPerAccount { get; set; }

        [JsonProperty("maxMosaicDuration")]
        public string MaxMosaicDuration { get; set; }

        [JsonProperty("maxMosaicDivisibility")]
        public string MaxMosaicDivisibility { get; set; }

        [JsonProperty("mosaicRentalFeeSinkAddress")]
        public string MosaicRentalFeeSinkAddress { get; set; }

        [JsonProperty("mosaicRentalFee")]
        public string MosaicRentalFee { get; set; }
    }

    public class Multisig
    {
        [JsonProperty("maxMultisigDepth")]
        public int MaxMultisigDepth { get; set; }

        [JsonProperty("maxCosignatoriesPerAccount")]
        public int MaxCosignatoriesPerAccount { get; set; }

        [JsonProperty("maxCosignedAccountsPerAccount")]
        public int MaxCosignedAccountsPerAccount { get; set; }
    }

    public class Namespace
    {
        [JsonProperty("maxNameSize")]
        public int MaxNameSize { get; set; }

        [JsonProperty("maxChildNamespaces")]
        public int MaxChildNamespaces { get; set; }

        [JsonProperty("maxNamespaceDepth")]
        public int MaxNamespaceDepth { get; set; }

        [JsonProperty("minNamespaceDuration")]
        public string MinNamespaceDuration { get; set; }

        [JsonProperty("maxNamespaceDuration")]
        public string MaxNamespaceDuration { get; set; }

        [JsonProperty("namespaceGracePeriodDuration")]
        public string NamespaceGracePeriodDuration { get; set; }

        [JsonProperty("reservedRootNamespaceNames")]
        public string ReservedRootNamespaceNames { get; set; }

        [JsonProperty("namespaceRentalFeeSinkAddress")]
        public string NamespaceRentalFeeSinkAddress { get; set; }

        [JsonProperty("rootNamespaceRentalFeePerBlock")]
        public int RootNamespaceRentalFeePerBlock { get; set; }

        [JsonProperty("childNamespaceRentalFee")]
        public int ChildNamespaceRentalFee { get; set; }
    }

    public class Network
    {
        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        [JsonProperty("nodeEqualityStrategy")]
        public string NodeEqualityStrategy { get; set; }

        [JsonProperty("nemesisSignerPublicKey")]
        public string NemesisSignerPublicKey { get; set; }

        [JsonProperty("generationHashSeed")]
        public string GenerationHashSeed { get; set; }

        [JsonProperty("epochAdjustment")]
        public string EpochAdjustment { get; set; }
    }

    public class Plugins
    {
        [JsonProperty("accountlink")]
        public Accountlink Accountlink { get; set; }

        [JsonProperty("aggregate")]
        public AggregateProperties Aggregate { get; set; }

        [JsonProperty("lockhash")]
        public Lockhash Lockhash { get; set; }

        [JsonProperty("locksecret")]
        public Locksecret Locksecret { get; set; }

        [JsonProperty("metadata")]
        public MetadataMax Metadata { get; set; }

        [JsonProperty("mosaic")]
        public MosaicNetworkProperties Mosaic { get; set; }

        [JsonProperty("multisig")]
        public Multisig Multisig { get; set; }

        [JsonProperty("namespace")]
        public Namespace Namespace { get; set; }

        [JsonProperty("restrictionaccount")]
        public Restrictionaccount Restrictionaccount { get; set; }

        [JsonProperty("restrictionmosaic")]
        public Restrictionmosaic Restrictionmosaic { get; set; }

        [JsonProperty("transfer")]
        public Transfer Transfer { get; set; }
    }

    public class Restrictionaccount
    {
        [JsonProperty("maxAccountRestrictionValues")]
        public string MaxAccountRestrictionValues { get; set; }
    }

    public class Restrictionmosaic
    {
        [JsonProperty("maxMosaicRestrictionValues")]
        public string MaxMosaicRestrictionValues { get; set; }
    }

    public class NetworkProperties
    {
        [JsonProperty("network")]
        public Network Network { get; set; }

        [JsonProperty("chain")]
        public Chain Chain { get; set; }

        [JsonProperty("plugins")]
        public Plugins Plugins { get; set; }

        [JsonProperty("forkHeights")]
        public ForkHeights ForkHeights { get; set; }

        [JsonProperty("treasuryReissuanceTransactionSignatures")]
        public List<string> TreasuryReissuanceTransactionSignatures { get; set; }

        [JsonProperty("corruptAggregateTransactionHashes")]
        public List<string> CorruptAggregateTransactionHashes { get; set; }

    }
    public class ForkHeights
    {
        [JsonProperty("totalVotingBalanceCalculationFix")]
        public string TotalVotingBalanceCalculationFix { get; set; }

        [JsonProperty("treasuryReissuance")]
        public string TreasuryReissuance { get; set; }

        [JsonProperty("strictAggregateTransactionHash")]
        public string StrictAggregateTransactionHash { get; set; }

        [JsonProperty("skipSecretLockUniquenessChecks")]
        public string SkipSecretLockUniquenessChecks { get; set; }

        [JsonProperty("skipSecretLockExpirations")]
        public string SkipSecretLockExpirations { get; set; }

        [JsonProperty("forceSecretLockExpirations")]
        public string ForceSecretLockExpirations { get; set; }
    }

    public class Transfer
    {
        [JsonProperty("maxMessageSize")]
        public string MaxMessageSize { get; set; }
    }
}
