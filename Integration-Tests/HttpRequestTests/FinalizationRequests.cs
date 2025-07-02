using io.nem2.sdk.src.Export;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using System.Reactive.Linq;

namespace Integration_Tests.HttpRequests
{
    internal class FinalizationRequests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test, Timeout(20000)]
        public async Task GetFinalizationProof()
        {
            var client = new FinalizationHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetFinalizationProofByHeight(1);

            Assert.IsTrue(response.Hash.IsHex(64));
            Assert.That(response.FinalizationPoint, Is.EqualTo(1));
            Assert.That(response.Height, Is.EqualTo(1));
            Assert.That(response.Version, Is.EqualTo(1));
            Assert.That(response.FinalizationEpoch, Is.EqualTo(1));
            Assert.That(response.MessageGroups, Is.Empty);
        }

        [Test, Timeout(20000)]
        public async Task GetFinalizationProofByEpoch()
        {
            var client = new FinalizationHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetFinalizationProofByEpoch(10);

            Assert.IsTrue(response.Hash.IsHex(64));
            Assert.That(response.FinalizationPoint, Is.EqualTo(71));
            Assert.That(response.Height, Is.EqualTo(12960));
            Assert.That(response.FinalizationEpoch, Is.EqualTo(10));
            Assert.That(response.MessageGroups[0].Stage, Is.EqualTo(1));
            Assert.That(response.MessageGroups[0].Height, Is.EqualTo(12960));
            Assert.IsTrue(response.MessageGroups[0].Hashes[0].IsHex(64));
            Assert.IsTrue(response.MessageGroups[0].Signatures[0].Root.Signature.IsHex(128));
            Assert.IsTrue(response.MessageGroups[0].Signatures[0].Bottom.ParentPublicKey.IsHex(64));
        }
    }
}
