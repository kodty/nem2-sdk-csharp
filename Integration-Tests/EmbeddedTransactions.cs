using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Model.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration_Tests
{
    internal class EmbeddedTransactions
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test, Timeout(20000)]
        public async Task GetEmbeddedSimpleTransfer()
        {
            var client = new TransactionHttp("75.119.150.108", 3000);

            var tx = await client.GetConfirmedTransaction("BFBD18CE27575CF154826C9ECFE587C472193AB035E8F8E4ABFEB6FE1E53520C");

            var aggregate = (Aggregate)tx.Transaction;

            var Embedded = aggregate.Transactions;

            foreach (var item in Embedded)
            {
                var i = (EmbeddedSimpleTransfer)item.Transaction;

                Assert.That(item.Meta.Index, Is.AnyOf(0, 1));
                Assert.That(item.Meta.Height, Is.EqualTo(1995));
                Assert.That(item.Meta.Timestamp, Is.EqualTo(144382262));
                Assert.That(item.Meta.AggregateId, Is.EqualTo("6644D7ADED4FBE21460AA2E3"));
                Assert.That(item.Meta.AggregateHash, Is.EqualTo("BFBD18CE27575CF154826C9ECFE587C472193AB035E8F8E4ABFEB6FE1E53520C"));
                Assert.That(item.Meta.FeeMultiplier, Is.EqualTo(138));

                Assert.That(i.Version, Is.EqualTo(1));
                Assert.That(i.RecipientAddress, Is.EqualTo("6894D305EBBE9669675AEEC0B00CCD20B09548C3503B0880"));
                Assert.That(i.Type, Is.EqualTo(TransactionTypes.Types.TRANSFER));
                Assert.That(i.SignerPublicKey, Is.EqualTo("3714C04D01D664E2DDBD5ED2BA8B314F991EBA50122A38EC92A46AD987510B9D"));
                Assert.That(i.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
                Assert.That(i.Mosaics[0].Id, Is.EqualTo("6BED913FA20223F8"));
                Assert.That(i.Mosaics[0].Amount, Is.EqualTo(1000));
            }
        }
    }
}
