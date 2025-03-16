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

            var response = await client.GetConfirmedTransactions(new string[] { "6644D77CED4FBE214609F1C3", "6644D78DED4FBE21460A2439" });

            var aggTx1 = ((Aggregate)response[0].Transaction);
            var aggTx2 = ((Aggregate)response[1].Transaction);


            Assert.That(response[0].Id, Is.EqualTo("6644D77CED4FBE214609F1C3"));
            Assert.That(response[0].Meta.Hash, Is.EqualTo("E906272E7A715CD24D959A51CDFADC4CC8CA0E63097EA161C1DEBD31E9754A74"));
            Assert.That(response[0].Meta.Index, Is.EqualTo(25465));
            Assert.That(response[0].Meta.Timestamp, Is.EqualTo(0));
            Assert.That(response[0].Meta.MerkleComponentHash, Is.EqualTo("904E12F6F155A858C89568A63C23E1F5CDB8AC5220969BB59BD22879FF334F83"));
            Assert.That(response[0].Meta.FeeMultiplier, Is.EqualTo(0));

            Assert.That(response[1].Id, Is.EqualTo("6644D78DED4FBE21460A2439"));
            Assert.That(response[1].Meta.Hash, Is.EqualTo("7E3049EBF37DD84C2C52C96A4234281326F3FA434DCFBDA71CF68A194ACB5059"));
            Assert.That(response[1].Meta.Index, Is.EqualTo(0)); 
            Assert.That(response[1].Meta.Height, Is.EqualTo(117));
            Assert.That(response[1].Meta.Timestamp, Is.EqualTo(88309778));
            Assert.That(response[1].Meta.MerkleComponentHash, Is.EqualTo("7E3049EBF37DD84C2C52C96A4234281326F3FA434DCFBDA71CF68A194ACB5059"));
            Assert.That(response[1].Meta.FeeMultiplier, Is.EqualTo(100));

            Assert.That(aggTx1.Size, Is.EqualTo(864));
            Assert.That(aggTx1.Transactions, !Is.Null);
            Assert.That(aggTx1.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.That(aggTx1.TransactionsHash, Is.EqualTo("9D7D525E22C0DBEEA4D0F8E6C1AC4E301399C3EDD3CA7E6D2ACC6E4D13677CE6"));
            Assert.That(aggTx1.Signature, Is.EqualTo("35B6E3B1C311AA6A957EF2AD12447AD790A5197454ECC27BCE02527257EE317E404367C416A41E53D8CA851393AC58F59343435230CC6F75EB4A49C784BDCD03"));
            Assert.That(aggTx1.Deadline, Is.EqualTo(1));
            Assert.That(aggTx1.MaxFee, Is.EqualTo(0));
            Assert.That(aggTx1.Type, Is.EqualTo(TransactionTypes.Types.AGGREGATE_COMPLETE));
            Assert.That(aggTx1.SignerPublicKey, Is.EqualTo("FA9F3974FE3B15585E6B72672C7D8BEAE27D1EDF6C4752BAFDB8B2FEA601C0CF"));

            Assert.That(aggTx2.Size, Is.EqualTo(312));
            Assert.That(aggTx2.Transactions, !Is.Null);
            Assert.That(aggTx2.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.That(aggTx2.TransactionsHash, Is.EqualTo("DF5C0B4D7CC979FA385D4785FCA2A5D9A8F172C0BAB90883BF167DFE9C78A13B"));
            Assert.That(aggTx2.Signature, Is.EqualTo("4F51E0B27765A3FA70E34D5232B2DCF94FE5A125E31B47C5B3CD4A428F136C994C769A20389ED147D087E9E37DF8D00BBB753E808BCE57565764FC0A84D6B60B"));
            Assert.That(aggTx2.Deadline, Is.EqualTo(95507057));
            Assert.That(aggTx2.MaxFee, Is.EqualTo(31200));
            Assert.That(aggTx2.Type, Is.EqualTo(TransactionTypes.Types.AGGREGATE_COMPLETE));
            Assert.That(aggTx2.SignerPublicKey, Is.EqualTo("7E43EC810A64FCCA5F9FBF6FC3E51AA89A0507762DC7E6B8047DCACBE97A8D4B"));

            aggTx1.Cosignatures.ForEach(i => {

                Assert.That(i.SignerPublicKey.Length, Is.EqualTo(64));
                Assert.That(i.Signature.Length, Is.EqualTo(128));
                Assert.That(i.Version, Is.EqualTo(0));

            });

            Assert.That(aggTx1.Transactions[0].Transaction.SignerPublicKey, Is.EqualTo("FA9F3974FE3B15585E6B72672C7D8BEAE27D1EDF6C4752BAFDB8B2FEA601C0CF"));
            Assert.That(aggTx1.Transactions[0].Transaction.Network, Is.EqualTo(NetworkType.Types.MAIN_NET)); // network shouldnt be twice - check why
            Assert.That(aggTx1.Transactions[0].Transaction.Type, Is.EqualTo(TransactionTypes.Types.MULTISIG_ACCOUNT_MODIFICATION));
            Assert.That(aggTx1.Transactions[0].Transaction.Version, Is.EqualTo(1));
           
            Assert.That(aggTx2.Transactions[1].Transaction.SignerPublicKey, Is.EqualTo("7E43EC810A64FCCA5F9FBF6FC3E51AA89A0507762DC7E6B8047DCACBE97A8D4B"));
            Assert.That(aggTx2.Transactions[1].Transaction.Network, Is.EqualTo(NetworkType.Types.MAIN_NET)); // network shouldnt be twice - check why
            Assert.That(aggTx2.Transactions[1].Transaction.Type, Is.EqualTo(TransactionTypes.Types.MOSAIC_SUPPLY_CHANGE));
            Assert.That(aggTx2.Transactions[1].Transaction.Version, Is.EqualTo(1));

            var embedded1 = (EmbeddedMultisigModification)aggTx1.Transactions[0].Transaction;
            var embedded2 = (EmbeddedMosaicSupplyChange)aggTx2.Transactions[1].Transaction;

            Assert.That(aggTx1.Transactions[0].Id, Is.EqualTo("6644D77CED4FBE214609F1CF"));
            Assert.That(aggTx1.Transactions[0].Meta.Index, Is.EqualTo(0));
            Assert.That(aggTx1.Transactions[0].Meta.Height, Is.EqualTo(1));
            Assert.That(aggTx1.Transactions[0].Meta.Timestamp, Is.EqualTo(0));
            Assert.That(aggTx1.Transactions[0].Meta.FeeMultiplier, Is.EqualTo(0));
            Assert.That(aggTx1.Transactions[0].Meta.AggregateHash, Is.EqualTo("E906272E7A715CD24D959A51CDFADC4CC8CA0E63097EA161C1DEBD31E9754A74"));
            Assert.That(aggTx1.Transactions[0].Meta.AggregateId, Is.EqualTo("6644D77CED4FBE214609F1C3"));

            Assert.That(aggTx2.Transactions[1].Id, Is.EqualTo("6644D78DED4FBE21460A243B"));
            Assert.That(aggTx2.Transactions[1].Meta.Index, Is.EqualTo(1));
            Assert.That(aggTx2.Transactions[1].Meta.Height, Is.EqualTo(117));
            Assert.That(aggTx2.Transactions[1].Meta.Timestamp, Is.EqualTo(88309778));
            Assert.That(aggTx2.Transactions[1].Meta.FeeMultiplier, Is.EqualTo(100));
            Assert.That(aggTx2.Transactions[1].Meta.AggregateHash, Is.EqualTo("7E3049EBF37DD84C2C52C96A4234281326F3FA434DCFBDA71CF68A194ACB5059"));
            Assert.That(aggTx2.Transactions[1].Meta.AggregateId, Is.EqualTo("6644D78DED4FBE21460A2439"));
            
            Assert.That(embedded1.SignerPublicKey, Is.EqualTo("FA9F3974FE3B15585E6B72672C7D8BEAE27D1EDF6C4752BAFDB8B2FEA601C0CF"));
            Assert.That(embedded1.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.That(embedded1.Version, Is.EqualTo(1));
            Assert.That(embedded1.minApprovalDelta, Is.EqualTo(4));
            Assert.That(embedded1.minRemovalDelta, Is.EqualTo(5));
            Assert.That(embedded1.addressAdditions[0].Length, Is.EqualTo(48));

            Assert.That(embedded2.Delta, Is.EqualTo(3800000));
            Assert.That(embedded2.Action, Is.EqualTo(1));
            Assert.That(embedded2.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.That(embedded2.MosaicId, Is.EqualTo("63078E73FBCC2CAC"));
            Assert.That(embedded2.Version, Is.EqualTo(1));
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
      
            var embedded = (EmbeddedMultisigModification)tx.Transactions[0].Transaction;

            Assert.That(embedded.addressAdditions[0].Length, Is.EqualTo(48));
            Assert.That(embedded.addressDeletions.Count, Is.EqualTo(0));
        } 
    }
}