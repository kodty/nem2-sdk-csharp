using Newtonsoft.Json;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses
{
    public class NetworkInfo
    {
        public string Name { get; set; }

        public string Description { get; set; }

    }

    public class NetworkRentalFees
    {
        public int EffectiveRootNamespaceRentalFeePerBlock { get; set; }

        public int EffectiveChildNamespaceRentalFee { get; set; }

        public int EffectiveMosaicRentalFee { get; set; }
    }

    public class NetworkTransactionFees
    {
        public int AverageFeeMultiplier { get; set; }

        public int MedianFeeMultiplier { get; set; }

        public int HighestFeeMultiplier { get; set; }

        public int LowestFeeMultiplier { get; set; }

        public int MinFeeMultiplier { get; set; }
    }

    public class Accountlink
    {
        public string Dummy { get; set; }
    }

    public class AggregateProperties
    {
        public string MaxTransactionsPerAggregate { get; set; }

        public string MaxCosignaturesPerAggregate { get; set; }

        public bool EnableStrictCosignatureCheck { get; set; }

        public bool EnableBondedAggregateSupport { get; set; }

        public string MaxBondedTransactionLifetime { get; set; }
    }

    public class Chain
    {
        public bool EnableVerifiableState { get; set; }

        public bool EnableVerifiableReceipts { get; set; }

        public string CurrencyMosaicId { get; set; }

        public string HarvestingMosaicId { get; set; }

        public string BlockGenerationTargetTime { get; set; }

        public string BlockTimeSmoothingFactor { get; set; }

        public string ImportanceGrouping { get; set; }

        public string ImportanceActivityPercentage { get; set; }

        public string MaxRollbackBlocks { get; set; }

        public string MaxDifficultyBlocks { get; set; }

        public string DefaultDynamicFeeMultiplier { get; set; }

        public string MaxTransactionLifetime { get; set; }

        public string MaxBlockFutureTime { get; set; }

        public string InitialCurrencyAtomicUnits { get; set; }

        public string MaxMosaicAtomicUnits { get; set; }

        public string TotalChainImportance { get; set; }

        public string MinHarvesterBalance { get; set; }

        public string MaxHarvesterBalance { get; set; }

        public string MinVoterBalance { get; set; }

        public string MaxVotingKeysPerAccount { get; set; }

        public string MinVotingKeyLifetime { get; set; }

        public string MaxVotingKeyLifetime { get; set; }

        public string HarvestBeneficiaryPercentage { get; set; }

        public string HarvestNetworkPercentage { get; set; }

        public string HarvestNetworkFeeSinkAddress { get; set; }

        public string MaxTransactionsPerBlock { get; set; }
    }

    public class Lockhash
    {
        public string LockedFundsPerAggregate { get; set; }

        public string MaxHashLockDuration { get; set; }
    }

    public class Locksecret
    {
        public string MaxSecretLockDuration { get; set; }

        public string MinProofSize { get; set; }

        public string MaxProofSize { get; set; }
    }

    public class MetadataMax
    {
        public string MaxValueSize { get; set; }
    }

    public class MosaicNetworkProperties
    {
        public string MaxMosaicsPerAccount { get; set; }

        public string MaxMosaicDuration { get; set; }

        public string MaxMosaicDivisibility { get; set; }

        public string MosaicRentalFeeSinkAddress { get; set; }

        public string MosaicRentalFee { get; set; }
    }

    public class Multisig
    {
        public int MaxMultisigDepth { get; set; }

        public int MaxCosignatoriesPerAccount { get; set; }

        public int MaxCosignedAccountsPerAccount { get; set; }
    }

    public class Namespace
    {
        public int MaxNameSize { get; set; }

        public int MaxChildNamespaces { get; set; }

        public int MaxNamespaceDepth { get; set; }

        public string MinNamespaceDuration { get; set; }

        public string MaxNamespaceDuration { get; set; }

        public string NamespaceGracePeriodDuration { get; set; }

        public string ReservedRootNamespaceNames { get; set; }

        public string NamespaceRentalFeeSinkAddress { get; set; }

        public int RootNamespaceRentalFeePerBlock { get; set; }

        public int ChildNamespaceRentalFee { get; set; }
    }

    public class Network
    {
        public string Identifier { get; set; }

        public string NodeEqualityStrategy { get; set; }

        public string NemesisSignerPublicKey { get; set; }

        public string GenerationHashSeed { get; set; }

        public string EpochAdjustment { get; set; }
    }

    public class Plugins
    {
        public Accountlink Accountlink { get; set; }

        public AggregateProperties Aggregate { get; set; }

        public Lockhash Lockhash { get; set; }

        public Locksecret Locksecret { get; set; }

        public MetadataMax Metadata { get; set; }

        public MosaicNetworkProperties Mosaic { get; set; }

        public Multisig Multisig { get; set; }

        public Namespace Namespace { get; set; }

        public Restrictionaccount Restrictionaccount { get; set; }

        public Restrictionmosaic Restrictionmosaic { get; set; }

        public Transfer Transfer { get; set; }
    }

    public class Restrictionaccount
    {
        public string MaxAccountRestrictionValues { get; set; }
    }

    public class Restrictionmosaic
    {
        public string MaxMosaicRestrictionValues { get; set; }
    }

    public class NetworkProperties
    {
        public Network Network { get; set; }

        public Chain Chain { get; set; }

        public Plugins Plugins { get; set; }

        public ForkHeights ForkHeights { get; set; }

        public List<string> TreasuryReissuanceTransactionSignatures { get; set; }

        public List<string> CorruptAggregateTransactionHashes { get; set; }

    }
    public class ForkHeights
    {
        public string TotalVotingBalanceCalculationFix { get; set; }

        public string TreasuryReissuance { get; set; }

        public string StrictAggregateTransactionHash { get; set; }

        public string SkipSecretLockUniquenessChecks { get; set; }

        public string SkipSecretLockExpirations { get; set; }

        public string ForceSecretLockExpirations { get; set; }
    }

    public class Transfer
    {
        public string MaxMessageSize { get; set; }
    }
}
