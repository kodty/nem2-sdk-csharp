using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Model.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration_Tests
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
            var client = new FinalizationHttp("75.119.150.108", 3000);

            var response = await client.GetFinalizationProofByHeight(1);

            Assert.That(response.Hash, Is.EqualTo("BEED005B82B22FC32DA6FDF4DFEB7C11BA6A8C5C504EB7B9CCF91B9B2A09E020"));
            Assert.That(response.FinalizationPoint, Is.EqualTo(1));
            Assert.That(response.Height, Is.EqualTo(1));
            Assert.That(response.Version, Is.EqualTo(1));
            Assert.That(response.FinalizationEpoch, Is.EqualTo(1));
            Assert.That(response.MessageGroups, Is.Empty);          
        }

        [Test, Timeout(20000)]
        public async Task GetFinalizationProofByEpoch()
        {
            var client = new FinalizationHttp("75.119.150.108", 3000);

            var response = await client.GetFinalizationProofByEpoch(10);

            Assert.That(response.Hash, Is.EqualTo("F293A16AD9D640A9640F5ABA1C2E7241A8473C1FF277888DECDED9EA3F25D6DE"));
            Assert.That(response.FinalizationPoint, Is.EqualTo(71));
            Assert.That(response.Height, Is.EqualTo(12960));
            Assert.That(response.FinalizationEpoch, Is.EqualTo(10));
            Assert.That(response.MessageGroups[0].Stage, Is.EqualTo(1));
            Assert.That(response.MessageGroups[0].Height, Is.EqualTo(12960));
            Assert.That(response.MessageGroups[0].Hashes[0].Length, Is.EqualTo(64));
            Assert.That(response.MessageGroups[0].Signatures[0].Root.ParentPublicKey.Length, Is.EqualTo(64));
            Assert.That(response.MessageGroups[0].Signatures[0].Root.Signature.Length, Is.EqualTo(128));
            Assert.That(response.MessageGroups[0].Signatures[0].Bottom.ParentPublicKey.Length, Is.EqualTo(64));
            Assert.That(response.MessageGroups[0].Signatures[0].Bottom.Signature.Length, Is.EqualTo(128));
        }
    }
}
