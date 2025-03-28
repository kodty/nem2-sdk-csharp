using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Model.Network;
using System.Reactive.Linq;


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

            Assert.That(response.Count, Is.GreaterThan(0));

            response.ForEach(i =>
            {

                Assert.That(i.Block.Height, Is.GreaterThan(0));
                Assert.That(i.Block.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
                Assert.That(i.Block.Type, Is.GreaterThan(1));
                Assert.That(i.Block.Height, Is.GreaterThanOrEqualTo(0));
                Assert.That(i.Block.Size, Is.GreaterThan(1));
                Assert.That(i.Block.SignerPublicKey.Length, Is.EqualTo(64));
                Assert.That(i.Block.Version, Is.EqualTo(1));
                Assert.That(i.Block.Signature.Length, Is.EqualTo(128));
                Assert.That(i.Block.ProofGamma.Length, Is.EqualTo(64));
                Assert.That(i.Block.ProofScalar.Length, Is.EqualTo(64));
                Assert.That(i.Block.ProofVerificationHash.Length, Is.EqualTo(32)); // check why 32
                Assert.That(i.Block.Timestamp, Is.GreaterThanOrEqualTo(0));
                Assert.That(i.Block.Difficulty, Is.GreaterThanOrEqualTo(1));
                Assert.That(i.Block.PreviousBlockHash.Length, Is.EqualTo(64));
                Assert.That(i.Block.BeneficiaryAddress.Length, Is.EqualTo(48));
                Assert.That(i.Block.TransactionsHash.Length, Is.EqualTo(64));
                Assert.That(i.Block.ReceiptsHash.Length, Is.EqualTo(64));
                Assert.That(i.Block.StateHash.Length, Is.EqualTo(64));
                Assert.That(i.Block.FeeMultiplier, Is.GreaterThanOrEqualTo(0));

                Assert.That(i.Meta.GenerationHash.Length, Is.EqualTo(64));
                Assert.That(i.Meta.TransactionsCount, Is.GreaterThanOrEqualTo(0));
                Assert.That(i.Meta.TotalTransactionsCount, Is.GreaterThanOrEqualTo(0));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));
                Assert.That(i.Meta.StateHashSubCacheMerkleRoots.Count, Is.GreaterThanOrEqualTo(0));
                Assert.That(i.Meta.StatementsCount, Is.GreaterThanOrEqualTo(0));
                Assert.That(i.Meta.TotalFee, Is.GreaterThanOrEqualTo(0));

                Assert.That(i.Id.Length, Is.EqualTo(24));
            });
        }

        [Test, Timeout(20000)]
        public async Task GetblocksByHeight()
        {
            var client = new BlockchainHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetBlock(1);

            Assert.That(response.Block.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.That(response.Meta.Hash.Length, Is.EqualTo(64));
            Assert.That(response.Meta.StatementsCount, Is.EqualTo(259));
            Assert.That(response.Meta.TotalFee, Is.EqualTo(0));
            Assert.That(response.Id, Is.EqualTo("66641FAF9E09C6EE9601A233"));
            Assert.That(response.Block.Height, Is.GreaterThan(0));
        }

        [Test, Timeout(20000)]
        public async Task GetBlockTransactionMerkle()
        {
            var client = new BlockchainHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetBlockTransactionMerkle(1, "B3FAD63E287D08209AA9CBE5E2E48CC1BEB1DA57993CBE7BE17E39C089186302");

            Assert.That(response[0].Hash, Is.EqualTo("035B8D7AA90D41724506E2DD1A9A8D5B47B9AF5BB627904B591F9221D08CF335"));
            Assert.That(response[0].Position, Is.EqualTo("right"));

        }

        [Test, Timeout(20000)]
        public async Task GetChainInfo()
        {
            var client = new BlockchainHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetBlockchainInfo();

            Assert.That(response.Height, Is.GreaterThan(0));
            Assert.That(response.ScoreHigh, Is.GreaterThan(0));
            Assert.That(response.ScoreLow, Is.GreaterThan(0));
            Assert.That(response.LatestFinalizedBlock.Hash.Length, Is.EqualTo(64));
            Assert.That(response.LatestFinalizedBlock.Height, Is.GreaterThan(0));
            Assert.That(response.LatestFinalizedBlock.FinalizationEpoch, Is.GreaterThan(0));
        }
    }
}
