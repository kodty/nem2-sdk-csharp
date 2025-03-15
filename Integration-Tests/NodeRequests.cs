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
            Assert.That(response.Status.ApiNode, Is.EqualTo("up"));
           
        }

        [Test, Timeout(20000)]
        public async Task GetNodeInformation()
        {
            var nodeClient = new NodeHttp("75.119.150.108", 3000);

            var response = await nodeClient.GetNodeInformation();

            Assert.That(response.FriendlyName, Is.EqualTo("!King.radicasse.jp"));
            Assert.That(response.Version, Is.EqualTo(16777991));
            Assert.That(response.Host, Is.EqualTo("75.119.150.108"));
            Assert.That(response.Roles, Is.EqualTo(3));
            Assert.That(response.Port, Is.EqualTo(7900));
            Assert.That(response.NetworkIdentifier, Is.EqualTo(104));
            Assert.That(response.NetworkGenerationHashSeed, Is.EqualTo("57F7DA205008026C776CB6AED843393F04CD458E0AA2D9F1D5F31A402072B2D6"));
            Assert.That(response.FriendlyName, Is.EqualTo("!King.radicasse.jp"));
            Assert.That(response.PublicKey, Is.EqualTo("15F19765910141987A0F9AB925352A2F4060E0EDF7CA34F2EBC39FC012A1196B"));
            Assert.That(response.NodePublicKey, Is.EqualTo("B23BA5E2529DA132D9B6BA911BE06D9991DA3E7270310028B4D620314A366C05"));
        }

        [Test, Timeout(20000)]
        public async Task GetNodePeers()
        {
            var nodeClient = new NodeHttp("75.119.150.108", 3000);

            var response = await nodeClient.GetNodePeers();

            Assert.That(response[0].Version, Is.EqualTo(16777991));
            Assert.That(response[0].Host, !Is.Null);
            Assert.That(response[0].Port, Is.EqualTo(7900));
            Assert.That(response[0].NetworkIdentifier, Is.EqualTo(104));
            Assert.That(response[0].Roles, Is.GreaterThan(0));
            Assert.That(response[0].NetworkGenerationHashSeed, Is.EqualTo("57F7DA205008026C776CB6AED843393F04CD458E0AA2D9F1D5F31A402072B2D6"));
            Assert.That(response[0].FriendlyName.Length, Is.GreaterThan(0));
            Assert.That(response[0].PublicKey.Length, Is.EqualTo(64));
            
        }

        [Test, Timeout(20000)]
        public async Task GetNodeStorage()
        {
            var nodeClient = new NodeHttp("75.119.150.108", 3000);

            var response = await nodeClient.GetNodeStorageInfo();

            Assert.That(response.NumBlocks, Is.GreaterThan(1));
            Assert.That(response.NumTransactions, Is.GreaterThan(1));
            Assert.That(response.NumAccounts, Is.GreaterThan(1));
        }

        [Test, Timeout(20000)]
        public async Task GetNodeTime()
        {
            var nodeClient = new NodeHttp("75.119.150.108", 3000);

            var response = await nodeClient.GetNodeTime();

            Assert.That(response.CommunicationTimestamps.ReceiveTimestamp, Is.GreaterThan(0));
            Assert.That(response.CommunicationTimestamps.SendTimestamp, Is.GreaterThan(1));

        }

        [Test, Timeout(20000)]
        public async Task GetNodeRESTVersion()
        {
            var nodeClient = new NodeHttp("75.119.150.108", 3000);

            var response = await nodeClient.GetNodeRESTVersion();

            Assert.That(response.ServerInfo.Deployment.LastUpdatedDate, Is.EqualTo("2024-11-17"));
            Assert.That(response.ServerInfo.Deployment.DeploymentToolVersion, Is.EqualTo("1.1.11"));
            Assert.That(response.ServerInfo.Deployment.DeploymentTool, Is.EqualTo("symbol-bootstrap"));
            Assert.That(response.ServerInfo.RestVersion, Is.EqualTo("2.4.4"));
            Assert.That(response.ServerInfo.SdkVersion, Is.Null);
        }
    }
}
