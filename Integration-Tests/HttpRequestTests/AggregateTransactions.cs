using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Model.Transactions;
using CopperCurve;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Diagnostics;
using System.Reactive.Linq;
using io.nem2.sdk.src.Model2;
using io.nem2.sdk.src.Model2.Accounts;

namespace Integration_Tests.HttpRequests
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
            Assert.IsTrue(acc.Address.Plain.IsBase32(39));

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);


            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.AGGREGATE_COMPLETE.GetValue());
            qModel.SetParam(QueryModel.DefinedParams.embedded, true);

            var response = await client.SearchConfirmedTransactions(qModel);

            Assert.That(response.Count, Is.GreaterThan(0));

            response.ForEach(i =>
            {
                ((Aggregate)i.Transaction).Transactions
                    .ForEach(m =>
                    {
                        Debug.WriteLine(m.Transaction.Type);

                    });
            });
        }
    }
}