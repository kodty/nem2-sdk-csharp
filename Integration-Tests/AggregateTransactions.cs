using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Infrastructure.Buffers.Model.JsonConverters;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Model.Network;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reactive.Linq;

namespace Integration_Tests
{
    public class AggregateTransactions
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test, Timeout(20000)]
        public async Task SearchAggTransactions()
        {
            string pubKey = "BE0B4CF546B7B4F4BBFCFF9F574FDA527C07A53D3FC76F8BB7DB746F8E8E0A9F";
            PublicAccount acc = new PublicAccount(pubKey, NetworkType.Types.MAIN_NET);
            Assert.That(acc.Address.Plain, Is.EqualTo("NASYMBOLLK6FSL7GSEMQEAWN7VW55ZSZU25TBOA"));

            var client = new TransactionHttp("75.119.150.108", 3000);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

         
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.AGGREGATE_COMPLETE.GetValue());
            qModel.SetParam(QueryModel.DefinedParams.embedded, true);

            var response = await client.SearchConfirmedTransactions(qModel);

            response.ForEach(i => {
                ((Aggregate)i.Transaction).Transactions
                    .ForEach(m =>
                    {
                        Debug.WriteLine(m.Transaction.Type);

                    });
            });
        }
  
        [Test, Timeout(20000)]
        public async Task GetAggregatesComplete()
        {
            string pubKey = "6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D";

            var client = new TransactionHttp("75.119.150.108", 3000);

            var response = await client.GetConfirmedTransactions(new string[] { "6644D77CED4FBE214609F1C3", "6644D77CED4FBE214609F1C3" });

            response.ForEach(i => {

                var tx = ((Aggregate)i.Transaction);

                Assert.That(i.Id, Is.EqualTo("6644D77CED4FBE214609F1C3"));
                Assert.That(i.Meta.Hash, Is.EqualTo("E906272E7A715CD24D959A51CDFADC4CC8CA0E63097EA161C1DEBD31E9754A74"));
                Assert.That(i.Meta.Index, Is.EqualTo(25465));
                Assert.That(i.Meta.Timestamp, Is.EqualTo(0)); 
                Assert.That(i.Meta.MerkleComponentHash, Is.EqualTo("904E12F6F155A858C89568A63C23E1F5CDB8AC5220969BB59BD22879FF334F83"));
                Assert.That(i.Meta.FeeMultiplier, Is.EqualTo(0));
                Assert.That(tx.Size, Is.EqualTo(864));
                Assert.That(tx.Transactions, !Is.Null);
                Assert.That(tx.Network, Is.EqualTo(NetworkType.Types.MAIN_NET)); 
                Assert.That(tx.TransactionsHash, Is.EqualTo("9D7D525E22C0DBEEA4D0F8E6C1AC4E301399C3EDD3CA7E6D2ACC6E4D13677CE6"));
                Assert.That(tx.Signature, Is.EqualTo("35B6E3B1C311AA6A957EF2AD12447AD790A5197454ECC27BCE02527257EE317E404367C416A41E53D8CA851393AC58F59343435230CC6F75EB4A49C784BDCD03"));
                Assert.That(tx.Deadline, Is.EqualTo(1));
                Assert.That(tx.MaxFee, Is.EqualTo(0));
                Assert.That(tx.Type, Is.EqualTo(TransactionTypes.Types.AGGREGATE_COMPLETE));
                Assert.That(tx.SignerPublicKey, Is.EqualTo("FA9F3974FE3B15585E6B72672C7D8BEAE27D1EDF6C4752BAFDB8B2FEA601C0CF"));

                tx.Cosignatures.ForEach(i => {

                    Assert.That(i.SignerPublicKey.Length, Is.EqualTo(64));
                    Assert.That(i.Signature.Length, Is.EqualTo(128));
                    Assert.That(i.Version, Is.EqualTo(0));

                });

                Assert.That(tx.Transactions[0].Transaction.SignerPublicKey, Is.EqualTo("FA9F3974FE3B15585E6B72672C7D8BEAE27D1EDF6C4752BAFDB8B2FEA601C0CF"));
                Assert.That(tx.Transactions[0].Transaction.Network, Is.EqualTo(NetworkType.Types.MAIN_NET)); // network shouldnt be twice - check why
                Assert.That(tx.Transactions[0].Transaction.Type, Is.EqualTo(TransactionTypes.Types.MULTISIG_ACCOUNT_MODIFICATION));
                Assert.That(tx.Transactions[0].Transaction.Version, Is.EqualTo(1));

                var embedded = (MultisigModification)tx.Transactions[0].Transaction;

                Assert.That(tx.Transactions[0].Id, Is.EqualTo("6644D77CED4FBE214609F1CF"));
                Assert.That(tx.Transactions[0].Meta.Index, Is.EqualTo(0));
                Assert.That(tx.Transactions[0].Meta.Height, Is.EqualTo(1));
                Assert.That(tx.Transactions[0].Meta.Timestamp, Is.EqualTo(0));
                Assert.That(tx.Transactions[0].Meta.FeeMultiplier, Is.EqualTo(0));
                Assert.That(tx.Transactions[0].Meta.AggregateHash, Is.EqualTo("E906272E7A715CD24D959A51CDFADC4CC8CA0E63097EA161C1DEBD31E9754A74"));
                Assert.That(tx.Transactions[0].Meta.AggregateId, Is.EqualTo("6644D77CED4FBE214609F1C3"));
             
                Assert.That(embedded.SignerPublicKey, Is.EqualTo("FA9F3974FE3B15585E6B72672C7D8BEAE27D1EDF6C4752BAFDB8B2FEA601C0CF"));
                Assert.That(embedded.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
                Assert.That(embedded.Version, Is.EqualTo(1));
                Assert.That(embedded.minApprovalDelta, Is.EqualTo(4));
                Assert.That(embedded.minRemovalDelta, Is.EqualTo(5));
                Assert.That(embedded.addressAdditions[0].Length, Is.EqualTo(48));
            });
        }

        [Test, Timeout(20000)]
        public async Task GetAggregateComplete()
        {
            string pubKey = "6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D";

            var client = new TransactionHttp("75.119.150.108", 3000);

            var response = await client.GetConfirmedTransaction("E906272E7A715CD24D959A51CDFADC4CC8CA0E63097EA161C1DEBD31E9754A74");

            var tx = ((Aggregate)response.Transaction);

            Assert.That(response.Meta.Hash, Is.EqualTo("E906272E7A715CD24D959A51CDFADC4CC8CA0E63097EA161C1DEBD31E9754A74"));
            Assert.That(response.Meta.Index, Is.EqualTo(25465));
            Assert.That(response.Meta.MerkleComponentHash, Is.EqualTo("904E12F6F155A858C89568A63C23E1F5CDB8AC5220969BB59BD22879FF334F83"));
            Assert.That(response.Meta.Height, Is.EqualTo(1));
            Assert.That(response.Meta.Timestamp, Is.EqualTo(0));
            Assert.That(response.Id, Is.EqualTo("6644D77CED4FBE214609F1C3"));

            Assert.That(tx.Size, Is.EqualTo(864));
            Assert.That(tx.Transactions, !Is.Null);
            Assert.That(tx.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.That(tx.TransactionsHash, Is.EqualTo("9D7D525E22C0DBEEA4D0F8E6C1AC4E301399C3EDD3CA7E6D2ACC6E4D13677CE6"));
            Assert.That(tx.Signature, Is.EqualTo("35B6E3B1C311AA6A957EF2AD12447AD790A5197454ECC27BCE02527257EE317E404367C416A41E53D8CA851393AC58F59343435230CC6F75EB4A49C784BDCD03"));
            Assert.That(tx.Deadline, Is.EqualTo(1));
            Assert.That(tx.MaxFee, Is.EqualTo(0));
            Assert.That(tx.Type, Is.EqualTo(TransactionTypes.Types.AGGREGATE_COMPLETE));
            Assert.That(tx.SignerPublicKey, Is.EqualTo("FA9F3974FE3B15585E6B72672C7D8BEAE27D1EDF6C4752BAFDB8B2FEA601C0CF"));

            Assert.That(tx.Transactions[0].Transaction.SignerPublicKey, Is.EqualTo("FA9F3974FE3B15585E6B72672C7D8BEAE27D1EDF6C4752BAFDB8B2FEA601C0CF"));
            Assert.That(tx.Transactions[0].Transaction.Network, Is.EqualTo(NetworkType.Types.MAIN_NET)); // network shouldnt be twice - check why
            Assert.That(tx.Transactions[0].Transaction.Type, Is.EqualTo(TransactionTypes.Types.MULTISIG_ACCOUNT_MODIFICATION));
            Assert.That(tx.Transactions[0].Transaction.Version, Is.EqualTo(1));
      
            var embedded = (MultisigModification)tx.Transactions[0].Transaction;

            Assert.That(embedded.addressAdditions[0].Length, Is.EqualTo(48));
            Assert.That(embedded.addressDeletions.Count, Is.EqualTo(0));
        } 
    }
}