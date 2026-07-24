using Coppery;
using Integration_Tests;
using io.nem2.sdk.Model;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Articles;
using io.nem2.sdk.Model.Transactions.Messages;


namespace Unit_Tests.Model.Transactions.Embedded
{
    internal class TransferTransactionTest
    {

        public async Task CreateSupplyChangeEmbeddedTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

        }

        public async Task CreateEmbeddedTransferAndSupplyChangeTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

        }
    }
}
