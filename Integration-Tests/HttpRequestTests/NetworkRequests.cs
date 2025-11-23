using io.nem2.sdk.src;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using System.Reactive.Linq;

namespace Integration_Tests.HttpRequests
{
    internal class NetworkRequests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test, Timeout(20000)]
        public async Task GetNetwork()
        {
            var client = new NetworkHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetNetwork();

            Assert.That(response.ComposedResponse.Name, Is.EqualTo("mainnet"));
            Assert.That(response.ComposedResponse.Description, Is.EqualTo("Symbol Mainnet"));

        }

        [Test, Timeout(20000)]
        public async Task GetNetworkRentalFees()
        {
            var client = new NetworkHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetRentalFees();

            Assert.That(response.ComposedResponse.EffectiveMosaicRentalFee, Is.EqualTo(50000000));
            Assert.That(response.ComposedResponse.EffectiveRootNamespaceRentalFeePerBlock, Is.EqualTo(200));
            Assert.That(response.ComposedResponse.EffectiveChildNamespaceRentalFee, Is.EqualTo(10000000));
        }

        //[Test, Timeout(20000)]
        public async Task GetNetworkTransactionFees()
        {
            var client = new NetworkHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetTransactionFees();

            Assert.That(response.ComposedResponse.LowestFeeMultiplier, Is.GreaterThanOrEqualTo(0));
            Assert.That(response.ComposedResponse.MedianFeeMultiplier, Is.GreaterThanOrEqualTo(0));
            Assert.That(response.ComposedResponse.HighestFeeMultiplier, Is.LessThan(10000));
            Assert.That(response.ComposedResponse.MinFeeMultiplier, Is.GreaterThanOrEqualTo(0));
            Assert.That(response.ComposedResponse.AverageFeeMultiplier, Is.AtLeast(90));
            Assert.That(response.ComposedResponse.AverageFeeMultiplier, Is.AtMost(300));
        }

        [Test, Timeout(20000)]
        public async Task GetNetworkProperties()
        {
            var client = new NetworkHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetNetworkProperties();

            Assert.That(response.ComposedResponse.TreasuryReissuanceTransactionSignatures.Count, Is.GreaterThan(0));

            Assert.That(response.ComposedResponse.Network.NemesisSignerPublicKey, Is.EqualTo("BE0B4CF546B7B4F4BBFCFF9F574FDA527C07A53D3FC76F8BB7DB746F8E8E0A9F"));
            Assert.That(response.ComposedResponse.Network.NodeEqualityStrategy, Is.EqualTo("host"));
            Assert.That(response.ComposedResponse.Network.GenerationHashSeed, Is.EqualTo("57F7DA205008026C776CB6AED843393F04CD458E0AA2D9F1D5F31A402072B2D6"));
            Assert.That(response.ComposedResponse.Network.EpochAdjustment, Is.GreaterThanOrEqualTo("1615853185s"));
            Assert.That(response.ComposedResponse.Chain.EnableVerifiableReceipts, Is.True);
            Assert.That(response.ComposedResponse.Chain.EnableVerifiableState, Is.True);
            Assert.That(response.ComposedResponse.ForkHeights.TreasuryReissuance, Is.EqualTo("689'761"));

            Assert.That(response.ComposedResponse.Plugins.Accountlink.Dummy, !Is.Null);
            Assert.That(response.ComposedResponse.Plugins.Restrictionmosaic.MaxMosaicRestrictionValues, !Is.Null);
            Assert.That(response.ComposedResponse.Plugins.Mosaic.MosaicRentalFee, !Is.Null);
            Assert.That(response.ComposedResponse.Plugins.Namespace.MaxNameSize, Is.EqualTo(64));
            Assert.That(response.ComposedResponse.Plugins.Accountlink.Dummy, Is.EqualTo("to trigger plugin load"));
            Assert.That(response.ComposedResponse.Plugins.Aggregate.EnableStrictCosignatureCheck, Is.EqualTo(false));
            Assert.That(response.ComposedResponse.Plugins.Lockhash.LockedFundsPerAggregate, Is.EqualTo("10'000'000"));
            Assert.That(response.ComposedResponse.Plugins.Locksecret.MinProofSize, Is.EqualTo("0"));
            Assert.That(response.ComposedResponse.Plugins.Metadata.MaxValueSize, Is.EqualTo("1024"));
            Assert.That(response.ComposedResponse.Plugins.Restrictionaccount.MaxAccountRestrictionValues, Is.EqualTo("100"));
            Assert.That(response.ComposedResponse.Plugins.Restrictionmosaic.MaxMosaicRestrictionValues, Is.EqualTo("20"));
            Assert.That(response.ComposedResponse.Plugins.Transfer.MaxMessageSize, Is.GreaterThanOrEqualTo("1024"));
            Assert.That(response.ComposedResponse.Plugins.Mosaic.MaxMosaicDivisibility, Is.EqualTo("6"));

            Assert.That(response.ComposedResponse.ForkHeights.TreasuryReissuance, Is.EqualTo("689'761"));
            Assert.That(response.ComposedResponse.CorruptAggregateTransactionHashes[0], Is.EqualTo("26FF5E7174DEEF3147DB25C37C7AE9905157ACBA2D233D40D1F77A65B60D59BC = 3A2F78C2E7B10FF84EB33BF1DD0FD61951F8E3EA3303614D1B20060CD88F6E14"));
            Assert.That(response.ComposedResponse.TreasuryReissuanceTransactionSignatures.Count, Is.GreaterThan(0));
        }
    }
}
