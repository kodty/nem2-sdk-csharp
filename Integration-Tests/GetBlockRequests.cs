using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Model.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration_Tests
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
            string pubKey = "BE0B4CF546B7B4F4BBFCFF9F574FDA527C07A53D3FC76F8BB7DB746F8E8E0A9F";

            var blockHttp = new BlockchainHttp("75.119.150.108", 3000);

            var queryModel = new QueryModel(QueryModel.DefineRequest.SearchBlocks);
            queryModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);

            var response = await blockHttp.SearchBlocks(queryModel);

            response.ForEach(i => {
                Assert.That(i.Block.NetworkType, Is.EqualTo(NetworkType.Types.MAIN_NET.GetNetworkByte()));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));
                Assert.That(i.Meta.StatementsCount, Is.EqualTo(259));
                Assert.That(i.Meta.TotalFee, Is.EqualTo(0));
                Assert.That(i.BlockID, Is.EqualTo("6644D77CED4FBE21460A2223"));
                Assert.That(i.Block.Height, Is.GreaterThan(0));
               
            });
        }

        [Test, Timeout(20000)]
        public async Task GetblocksByHeight()
        {
            var blockHttp = new BlockchainHttp("75.119.150.108", 3000);

            var response = await blockHttp.GetBlock(1);
      
            Assert.That(response.Block.NetworkType, Is.EqualTo(NetworkType.Types.MAIN_NET.GetNetworkByte()));
            Assert.That(response.Meta.Hash.Length, Is.EqualTo(64));
            Assert.That(response.Meta.StatementsCount, Is.EqualTo(259));
            Assert.That(response.Meta.TotalFee, Is.EqualTo(0));
            Assert.That(response.BlockID, Is.EqualTo("6644D77CED4FBE21460A2223"));
            Assert.That(response.Block.Height, Is.GreaterThan(0));
        }

        //[Test, Timeout(20000)]
        public async Task GetblockMerkle()
        {
            var blockHttp = new BlockchainHttp("75.119.150.108", 3000);

            var response = await blockHttp.GetBlockTransactionMerkle(1, "31F7F7C54D268BB1DE6FA53C3EC5CCD0F2CEFBB9BFD2AADFEDDD36CD8FEDF444");

           // return 
        }

        [Test, Timeout(20000)]
        public async Task GetChainInfo()
        {
            var blockHttp = new BlockchainHttp("75.119.150.108", 3000);

            var response = await blockHttp.GetBlockchainInfo();

            Assert.That(response.Height, Is.GreaterThan(0));
            Assert.That(response.ScoreHigh, Is.GreaterThan(0));
            Assert.That(response.LatestFinalizedBlock.Hash, Is.EqualTo("4AF66C45DF8DC73501F4ADAB16B737F8C67486B9F06E3C03ED80D0B5C6B9B71B"));
        }
    }
}
