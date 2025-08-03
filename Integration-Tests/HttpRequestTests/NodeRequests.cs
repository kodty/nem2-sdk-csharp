using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using System.Reactive.Linq;

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
            var client = new NodeHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            var response = await client.GetNodeHealth();

            Assert.That(response.Status.Db, Is.EqualTo("up"));
            Assert.That(response.Status.ApiNode, Is.EqualTo("up"));
           
        }

        [Test, Timeout(20000)]
        public async Task GetNodePeers()
        {
            var client = new NodeHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            var response = await client.GetNodePeers();

            Assert.That(response[0].Version, Is.EqualTo(16777991));
            Assert.That(response[0].Host, !Is.Null);
            Assert.That(response[0].Port, Is.EqualTo(7900));
            Assert.That(response[0].NetworkIdentifier, Is.EqualTo(152));
            Assert.That(response[0].Roles, Is.GreaterThan(0));
            Assert.That(response[0].NetworkGenerationHashSeed, Is.EqualTo("49D6E1CE276A85B70EAFE52349AACCA389302E7A9754BCF1221E79494FC665A4"));
            Assert.That(response[0].FriendlyName.Length, Is.GreaterThan(0));
            Assert.That(response[0].PublicKey.Length, Is.EqualTo(64));           
        }

        [Test, Timeout(20000)]
        public async Task GetNodeStorage()
        {
            var client = new NodeHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            var response = await client.GetNodeStorageInfo();

            Assert.That(response.NumBlocks, Is.GreaterThan(1));
            Assert.That(response.NumTransactions, Is.GreaterThan(1));
            Assert.That(response.NumAccounts, Is.GreaterThan(1));
        }

        [Test, Timeout(20000)]
        public async Task GetNodeTime()
        {
            var client = new NodeHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            var response = await client.GetNodeTime();

            Assert.That(response.CommunicationTimestamps.ReceiveTimestamp, Is.GreaterThan(0));
            Assert.That(response.CommunicationTimestamps.SendTimestamp, Is.GreaterThan(1));

        }

        [Test, Timeout(20000)]
        public async Task GetNodeRESTVersion()
        {
            var client = new NodeHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            var response = await client.GetNodeRESTVersion();

            Assert.That(response.ServerInfo.Deployment.LastUpdatedDate, Is.EqualTo("n/a"));
            Assert.That(response.ServerInfo.Deployment.DeploymentToolVersion, Is.EqualTo("alpha"));
            Assert.That(response.ServerInfo.Deployment.DeploymentTool, Is.EqualTo("shoestring"));
            Assert.That(response.ServerInfo.RestVersion, Is.EqualTo("2.5.0"));
            Assert.That(response.ServerInfo.SdkVersion, Is.Null);
        }
    }
}
