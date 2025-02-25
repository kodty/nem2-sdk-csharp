using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration_Tests
{
    internal class NodeRequests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test, Timeout(20000)]
        public async Task GetNodeHealth()
        {
            var nodeClient = new NodeHttp("75.119.150.108", 3000);

            var response = await nodeClient.GetNodeHealth();

            Assert.That(response.Status.Db, Is.EqualTo("up"));
        }

        [Test, Timeout(20000)]
        public async Task GetNodeInformation()
        {
            var nodeClient = new NodeHttp("75.119.150.108", 3000);

            var response = await nodeClient.GetNodeInformation();

            Assert.That(response.FriendlyName, Is.EqualTo("!King.radicasse.jp"));

        }

        [Test, Timeout(20000)]
        public async Task GetNodePeers()
        {
            var nodeClient = new NodeHttp("75.119.150.108", 3000);

            var response = await nodeClient.GetNodePeers();

            Assert.That(response[0].Version, Is.EqualTo(16777991));

        }

        [Test, Timeout(20000)]
        public async Task GetNodeStorage()
        {
            var nodeClient = new NodeHttp("75.119.150.108", 3000);

            var response = await nodeClient.GetNodeStorageInfo();

            Assert.That(response.NumBlocks, Is.GreaterThan(1));
        }

        [Test, Timeout(20000)]
        public async Task GetNodeTime()
        {
            var nodeClient = new NodeHttp("75.119.150.108", 3000);

            var response = await nodeClient.GetNodeTime();

            Assert.That(response.CommunicationTimestamps.ReceiveTimestamp, Is.GreaterThan(1));
        }

        [Test, Timeout(20000)]
        public async Task GetNodeRESTVersion()
        {
            var nodeClient = new NodeHttp("75.119.150.108", 3000);

            var response = await nodeClient.GetNodeRESTVersion();

            Assert.That(response.ServerInfo.RestVersion, Is.EqualTo("2.4.4"));
        }
    }
}
