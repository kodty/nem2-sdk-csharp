using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration_Tests
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
            var metadataHttp = new NetworkHttp("75.119.150.108", 3000);

            var response = await metadataHttp.GetNetwork();

            Assert.That(response.Name, Is.EqualTo("mainnet"));
            Assert.That(response.Description, Is.EqualTo("Symbol Mainnet"));
           
        }

        [Test, Timeout(20000)]
        public async Task GetNetworkRentalFees()
        {
            var metadataHttp = new NetworkHttp("75.119.150.108", 3000);

            var response = await metadataHttp.GetRentalFees();

            Assert.That(response.EffectiveMosaicRentalFee, Is.EqualTo(50000000));
            Assert.That(response.EffectiveRootNamespaceRentalFeePerBlock, Is.EqualTo(200));
            Assert.That(response.EffectiveChildNamespaceRentalFee, Is.EqualTo(10000000));
        }

        [Test, Timeout(20000)]
        public async Task GetNetworkTransactionFees()
        {
            var metadataHttp = new NetworkHttp("75.119.150.108", 3000);

            var response = await metadataHttp.GetTransactionFees();

            Assert.That(response.lowestFeeMultiplier, Is.GreaterThanOrEqualTo(0));
            Assert.That(response.medianFeeMultiplier, Is.GreaterThanOrEqualTo(0));
            Assert.That(response.highestFeeMultiplier, Is.LessThan(10000));
            Assert.That(response.minFeeMultiplier, Is.GreaterThanOrEqualTo(0));
            Assert.That(response.averageFeeMultiplier, Is.AtLeast(200) );
            Assert.That(response.averageFeeMultiplier, Is.AtMost(300));
        }

        [Test, Timeout(20000)]
        public async Task GetNetworkProperties()
        {
            var metadataHttp = new NetworkHttp("75.119.150.108", 3000);

            var response = await metadataHttp.GetNetworkProperties();

            Assert.That(response.TreasuryReissuanceTransactionSignatures.Count, Is.GreaterThan(0));
               
            Assert.That(response.Network.NemesisSignerPublicKey, Is.EqualTo("BE0B4CF546B7B4F4BBFCFF9F574FDA527C07A53D3FC76F8BB7DB746F8E8E0A9F"));
            Assert.That(response.Network.NodeEqualityStrategy, Is.EqualTo("host"));
            Assert.That(response.Network.GenerationHashSeed, Is.EqualTo("57F7DA205008026C776CB6AED843393F04CD458E0AA2D9F1D5F31A402072B2D6"));
            Assert.That(response.Network.EpochAdjustment, Is.GreaterThanOrEqualTo("1615853185s"));
            Assert.That(response.Chain.EnableVerifiableReceipts, Is.True);
            Assert.That(response.Chain.EnableVerifiableState, Is.True);
            Assert.That(response.ForkHeights.TreasuryReissuance, Is.EqualTo("689'761"));
            
            Assert.That(response.Plugins.Accountlink.Dummy, !Is.Null);
            Assert.That(response.Plugins.Restrictionmosaic.MaxMosaicRestrictionValues, !Is.Null);
            Assert.That(response.Plugins.Mosaic.MosaicRentalFee, !Is.Null);
            Assert.That(response.Plugins.Namespace.MaxNameSize, Is.EqualTo(64));
            Assert.That(response.Plugins.Accountlink.Dummy, Is.EqualTo("to trigger plugin load"));
            Assert.That(response.Plugins.Aggregate.EnableStrictCosignatureCheck, Is.EqualTo(false));
            Assert.That(response.Plugins.Lockhash.LockedFundsPerAggregate, Is.EqualTo("10'000'000"));
            Assert.That(response.Plugins.Locksecret.MinProofSize, Is.EqualTo("0"));
            Assert.That(response.Plugins.Metadata.MaxValueSize, Is.EqualTo("1024"));
            Assert.That(response.Plugins.Restrictionaccount.MaxAccountRestrictionValues, Is.EqualTo("100"));
            Assert.That(response.Plugins.Restrictionmosaic.MaxMosaicRestrictionValues, Is.EqualTo("20"));
            Assert.That(response.Plugins.Transfer.MaxMessageSize, Is.GreaterThanOrEqualTo("1024"));
            Assert.That(response.Plugins.Mosaic.MaxMosaicDivisibility, Is.EqualTo("6"));


            Assert.That(response.ForkHeights.TreasuryReissuance, Is.EqualTo("689'761"));
            Assert.That(response.CorruptAggregateTransactionHashes[0], Is.EqualTo("26FF5E7174DEEF3147DB25C37C7AE9905157ACBA2D233D40D1F77A65B60D59BC = 3A2F78C2E7B10FF84EB33BF1DD0FD61951F8E3EA3303614D1B20060CD88F6E14"));
            Assert.That(response.TreasuryReissuanceTransactionSignatures.Count, Is.GreaterThan(0));

        }
    }
}
