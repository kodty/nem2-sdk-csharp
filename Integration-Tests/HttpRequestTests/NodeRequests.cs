using io.nem2.sdk.Infrastructure.Responses;
using io.nem2.sdk.Infrastructure.HttpClients;
using System.Reactive.Linq;
using io.nem2.sdk.Model;
using Coppery;

namespace Integration_Tests
{
    internal partial class NodeRequests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test, Timeout(20000)]
        public async Task GetNodeHealth()
        {
            var client = new NetworkNodeHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            var response = await client.GetNodeHealth();
            Assert.That(response, Is.Not.Null);
            Assert.That(response.ComposedResponse, Is.Not.Null);
            Assert.That(response.ComposedResponse.GetType(), Is.EqualTo(typeof(NodeHealth)));
            Assert.That(response.Response, Is.Not.Null);
            Assert.That(response.ComposedResponse.Status, Is.Not.Null);
            Assert.That(response.ComposedResponse.Status.Db, Is.Not.Null);
            Assert.That(response.ComposedResponse.Status.Db, Is.EqualTo("up"));
            Assert.That(response.ComposedResponse.Status.ApiNode, Is.EqualTo("up"));          
        }

        
        [Test, Timeout(20000)]
        public async Task GetNodePeers()
        {
            var client = new NetworkNodeHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            var response = await client.GetNodePeers();

            Assert.That(response.ComposedResponse[1].Version == 16777993 || response.ComposedResponse[1].Version == 0);
            Assert.That(response.ComposedResponse[1].Host, !Is.Null);
            Assert.That(response.ComposedResponse[1].Port == 7900);
            Assert.That(response.ComposedResponse[1].NetworkIdentifier.GetNetworkValue(), Is.EqualTo(NetworkType.Types.TEST_NET));
            Assert.That(response.ComposedResponse[1].Roles, Is.GreaterThan(0));
            Assert.That(response.ComposedResponse[1].NetworkGenerationHashSeed, Is.EqualTo(HttpSetUp.genHash));
            Assert.That(response.ComposedResponse[1].FriendlyName.Length, Is.GreaterThan(0));
            Assert.That(response.ComposedResponse[1].PublicKey.Length, Is.EqualTo(64));
            Assert.That(response.ComposedResponse[1].PublicKey.IsHex());
        }

        [Test, Timeout(20000)]
        public async Task GetNodeInfo()
        {
            var client = new NetworkNodeHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            var response = await client.GetNodeInformation();

            Assert.That(response.ComposedResponse.Version == 16777993 || response.ComposedResponse.Version == 0);
            Assert.That(response.ComposedResponse.Host, !Is.Null);
            Assert.That(response.ComposedResponse.Port == 7900);
            Assert.That(response.ComposedResponse.NetworkIdentifier.GetNetworkValue(), Is.EqualTo(NetworkType.Types.TEST_NET));
            Assert.That(response.ComposedResponse.Roles, Is.GreaterThan(0));
            Assert.That(response.ComposedResponse.NetworkGenerationHashSeed, Is.EqualTo(HttpSetUp.genHash));
            Assert.That(response.ComposedResponse.FriendlyName.Length, Is.GreaterThan(0));
            Assert.That(response.ComposedResponse.PublicKey.Length, Is.EqualTo(64));
            Assert.That(response.ComposedResponse.PublicKey.IsHex());
            Assert.That(response.ComposedResponse.PublicKey.Length, Is.EqualTo(64));
            Assert.That(response.ComposedResponse.NodePublicKey.IsHex());
        }


        [Test, Timeout(20000)]
        public async Task GetNodeStorage()
        {
            var client = new NetworkNodeHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            var response = await client.GetNodeStorageInfo();

            Assert.That(response.ComposedResponse.NumBlocks, Is.GreaterThan(1));
            Assert.That(response.ComposedResponse.NumTransactions, Is.GreaterThan(1));
            Assert.That(response.ComposedResponse.NumAccounts, Is.GreaterThan(1));
            Assert.That(response.ComposedResponse.Database, !Is.Null);
        }

        [Test, Timeout(20000)]
        public async Task GetNodeTime()
        {
            var client = new NetworkNodeHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            var response = await client.GetNodeTime();

            Assert.That(response.ComposedResponse.CommunicationTimestamps.ReceiveTimestamp, Is.GreaterThan(0));
            Assert.That(response.ComposedResponse.CommunicationTimestamps.SendTimestamp, Is.GreaterThan(1));

        }

        [Test, Timeout(20000)]
        public async Task GetNodeRESTVersion()
        {
            var client = new NetworkNodeHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            var response = await client.GetNodeRESTVersion();

            Assert.That(response.ComposedResponse.ServerInfo.Deployment.LastUpdatedDate, Is.EqualTo("n/a"));
            Assert.That(response.ComposedResponse.ServerInfo.Deployment.DeploymentToolVersion, Is.EqualTo("alpha"));
            Assert.That(response.ComposedResponse.ServerInfo.Deployment.DeploymentTool == "symbol-bootstrap" || response.ComposedResponse.ServerInfo.Deployment.DeploymentTool == "shoestring");
            Assert.That(response.ComposedResponse.ServerInfo.RestVersion, !Is.Null);
        }

        [Test, Timeout(20000)]
        public async Task GetNodeHarvestAccount()
        {
            var client = new NetworkNodeHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            var response = await client.GetNodeHarvestingAccountInfo();

            Assert.That(response.ComposedResponse.UnlockedAccount[0].IsHex());         
        }
    }
}
