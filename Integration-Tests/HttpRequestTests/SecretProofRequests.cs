using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Export;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Reactive.Linq;

namespace Integration_Tests.HttpRequests
{
    internal class SecretProofRequests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test, Timeout(20000)]
        public async Task SearchSecretProofTransaction()
        {
            string pubKey = "D4A1468E54DD31B850CF9ABFFD32EFB98547091301668E777A43D3D88BEB76D8";

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);
            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.SECRET_PROOF.GetValue());

            var response = await client.SearchConfirmedTransactions(qModel);

            Assert.That(response.Count, Is.GreaterThan(0));

            response.ForEach(i =>
            {

                if (i.Transaction.Type == TransactionTypes.Types.SECRET_PROOF)
                {
                    var tx = (SecretProofT)i.Transaction;

                    Assert.IsTrue(tx.SignerPublicKey.IsHex(64));
                    Assert.That(tx.Secret.Length, Is.GreaterThan(0));
                    Assert.That(tx.Proof.Length, Is.GreaterThan(0));
                }
            });
        }
    }
}
