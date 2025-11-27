using io.nem2.sdk.Infrastructure.HttpRepositories;
using Coppery;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using System.Reactive.Linq;
using io.nem2.sdk.src.Model;


namespace Integration_Tests.HttpRequests
{
    public class BlockRequests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test, Timeout(20000)]
        public async Task Getblocks()
        {
            string pubKey = "3162D9127111F34DEC9C6488E8CB4BBF0BA5CBE195D787836208CFC3B2DC2EE1";

            var client = new BlockchainHttp(HttpSetUp.Node, HttpSetUp.Port);

            var queryModel = new QueryModel(QueryModel.DefineRequest.SearchBlocks);
            queryModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);

            var response = await client.SearchBlocks(queryModel);

            Assert.That(response.ComposedResponse.Data.Count, Is.GreaterThan(0));

            response.ComposedResponse.Data.ForEach(i =>
            {

                Assert.That(i.Block.Height, Is.GreaterThan(0));
                Assert.That(i.Block.Network.GetNetworkValue(), Is.EqualTo(NetworkType.Types.MAIN_NET));
                Assert.That(i.Block.Type, Is.GreaterThan(1));
                Assert.IsTrue(i.Block.SignerPublicKey.IsHex(64));
                Assert.That(i.Block.Version, Is.EqualTo(1));
                Assert.IsTrue(i.Block.Signature.IsHex(128));
                Assert.That(i.Block.ProofGamma.Length, Is.EqualTo(64));
                Assert.That(i.Block.ProofScalar.Length, Is.EqualTo(64));
                Assert.IsTrue(i.Block.ProofVerificationHash.IsHex(32)); // check why 32
                Assert.That(i.Block.Timestamp, Is.GreaterThanOrEqualTo(0));
                Assert.That(i.Block.Difficulty, Is.GreaterThanOrEqualTo(1));
                Assert.IsTrue(i.Block.PreviousBlockHash.IsHex(64));
                Assert.IsTrue(i.Block.BeneficiaryAddress.IsHex(48));
                Assert.IsTrue(i.Block.TransactionsHash.IsHex(64));
                Assert.IsTrue(i.Block.ReceiptsHash.IsHex(64));
                Assert.IsTrue(i.Block.StateHash.IsHex(64));
                Assert.That(i.Block.FeeMultiplier, Is.GreaterThanOrEqualTo(0));

                Assert.IsTrue(i.Meta.GenerationHash.IsHex(64));
                Assert.That(i.Meta.TransactionsCount, Is.GreaterThanOrEqualTo(0));
                Assert.That(i.Meta.TotalTransactionsCount, Is.GreaterThanOrEqualTo(0));
                Assert.IsTrue(i.Meta.Hash.IsHex(64));
                Assert.That(i.Meta.StateHashSubCacheMerkleRoots.Count, Is.GreaterThanOrEqualTo(0));
                Assert.That(i.Meta.StatementsCount, Is.GreaterThanOrEqualTo(0));
                Assert.That(i.Meta.TotalFee, Is.GreaterThanOrEqualTo(0));
                Assert.That(i.Id.IsHex(24));
            });
        }

        [Test, Timeout(20000)]
        public async Task GetblocksByHeight()
        {
            var client = new BlockchainHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetBlock(1);

            Assert.That(response.ComposedResponse.Block.Network.GetNetworkValue(), Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.IsTrue(response.ComposedResponse.Meta.Hash.IsHex(64));
            Assert.That(response.ComposedResponse.Meta.StatementsCount, Is.EqualTo(259));
            Assert.That(response.ComposedResponse.Meta.TotalFee, Is.EqualTo(0));
            Assert.That(response.ComposedResponse.Id.IsHex(24));
            Assert.That(response.ComposedResponse.Block.Height, Is.GreaterThan(0));
        }

        [Test, Timeout(20000)]
        public async Task GetBlockTransactionMerkle()
        {
            var client = new BlockchainHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetBlockTransactionMerkle(1, "B3FAD63E287D08209AA9CBE5E2E48CC1BEB1DA57993CBE7BE17E39C089186302");

            Assert.That(response.ComposedResponse[0].Hash.IsHex(64));
            Assert.That(response.ComposedResponse[0].Position, Is.EqualTo("right"));

        }

        [Test, Timeout(20000)]
        public async Task GetChainInfo()
        {
            var client = new BlockchainHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetBlockchainInfo();

            Assert.That(response.ComposedResponse.Height, Is.GreaterThan(0));
            Assert.That(response.ComposedResponse.ScoreHigh, Is.GreaterThan(0));
            Assert.That(response.ComposedResponse.ScoreLow, Is.GreaterThan(0));
            Assert.IsTrue(response.ComposedResponse.LatestFinalizedBlock.Hash.IsHex(64));
            Assert.That(response.ComposedResponse.LatestFinalizedBlock.Height, Is.GreaterThan(0));
            Assert.That(response.ComposedResponse.LatestFinalizedBlock.FinalizationEpoch, Is.GreaterThan(0));
        }
    }
}
