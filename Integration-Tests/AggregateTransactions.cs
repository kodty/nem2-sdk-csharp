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
    }
}