using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration_Tests
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

            var client = new TransactionHttp("75.119.150.108", 3000);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);
            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.SECRET_PROOF.GetValue());

            var response = await client.SearchConfirmedTransactions(qModel);

            response.ForEach(i => {

                if (i.Transaction.Type == TransactionTypes.Types.SECRET_PROOF)
                {
                    var tx = ((SecretProofT)i.Transaction);

                    Assert.That(tx.SignerPublicKey, Is.EqualTo(pubKey));
                    Assert.That(tx.Secret.Length, Is.GreaterThan(0));
                }
            });
        }
    }
}
