using io.nem2.sdk.Infrastructure;
using io.nem2.sdk.Infrastructure.HttpClients;
using io.nem2.sdk.Infrastructure.Responses;
using io.nem2.sdk.Model;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Articles;
using io.nem2.sdk.Model.Transactions;
using System.Diagnostics;
using System.Reactive.Linq;
using TweetNaclSharp;

namespace Integration_Tests.HttpRequestTests
{
    public class AggregateTransactions
    {

        [SetUp]
        public void Setup()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.AGGREGATE_COMPLETE.GetValue());
        }

        [Test, Timeout(20000)]
        public async Task RecomposeToVerifiabe()
        {
            var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            var result = await client.GetConfirmedTransaction("F30769768E5279428A85688FBC072B59B052713CF655596332B8CC961EF8FEA3");

            var transaction = (Aggregate)result.ComposedResponse.Transaction;

            var mosaicMeta = (EmbeddedMosaicMetadata)transaction.Transactions[0].Transaction;
            
            var pMosaicMeta = Transaction.Create(
                new MosaicMetadataTransaction(
                    mosaicMeta.TargetAddress, 
                    Mosaic.CreateFromHexIdentifier(mosaicMeta.TargetMosaicId.FromHex().Reverse().ToArray().ToHex(), 0), 
                    mosaicMeta.ScopedMetadataKey.FromHex().Reverse().ToArray().ToHex(), 
                    mosaicMeta.ValueSizeDelta,
                    mosaicMeta.ValueSize, 
                    mosaicMeta.Value.FromHex()),
                NetworkType.Types.TEST_NET
                );

            Debug.WriteLine(mosaicMeta.TargetMosaicId.FromHex().Reverse().ToArray().ToHex());
            Debug.WriteLine(mosaicMeta.ScopedMetadataKey.FromHex().Reverse().ToArray().ToHex());
            Assert.True(transaction.Deadline == 119511733869);
            Assert.True(transaction.MaxFee == 32800);
            Assert.True(pMosaicMeta.Version == 1);
            Assert.True(pMosaicMeta.Network == 152);
            Assert.True(pMosaicMeta.Type.GetRawValue() == TransactionTypes.Types.MOSAIC_METADATA);
            Assert.True(pMosaicMeta.TransactionExtension.TargetAddress.ToHex() == "981D7BA03EAD345251DE0BBDF14512B5C4478DB83A99F4DD");
            Assert.True(pMosaicMeta.TransactionExtension.TargetMosaicId.ToHex() == "7FCA347ED173375B");
            Assert.True(pMosaicMeta.TransactionExtension.ScopedMetadataKey.ToHex() == "6176617461720000");
            Assert.True(pMosaicMeta.TransactionExtension.Value.ToHex() == "89504E470D0A1A0A0000000D49484452000000010000000108000000003A7E9B550000000A49444154185763F80F00010101005A4D6FF10000000049454E44AE426082");
            Assert.True(pMosaicMeta.TransactionExtension.ValueSize == mosaicMeta.ValueSize);
            Assert.True(pMosaicMeta.TransactionExtension.ValueSizeDelta == mosaicMeta.ValueSizeDelta);

            pMosaicMeta.Signer = mosaicMeta.SignerPublicKey.FromHex();

            var payload = new AggregatePayload([pMosaicMeta], true);

            var tx = VerifiableTransaction.Create(
                payload,
                transaction.Network.GetNetworkValue(),
                transaction.MaxFee,
                new Deadline(NetworkType.Types.TEST_NET, 0)
                );

            tx.Deadline = transaction.Deadline;
            tx.Fee = transaction.MaxFee;
            tx.Signature = transaction.Signature.FromHex();
            tx.Signer = transaction.SignerPublicKey.FromHex();
            tx.TransactionExtension.TransactionsHash = "D8D0F0AD8E68A01C4122F465AC08548EF171D44B055712EB44927037B6E9790C".FromHex();
                  
            Assert.True(tx.Version == 3);
            Assert.True(tx.Network == 152);
            Assert.True(tx.Type.GetRawValue() == TransactionTypes.Types.AGGREGATE_COMPLETE);
           

            var preparedTransaction = tx.Prepare();

            Debug.WriteLine(preparedTransaction.Payload.ToHex());
            Debug.WriteLine(preparedTransaction.PayloadSigned.ToHex());
            Assert.True(NaclFast.SignDetachedVerify(HttpSetUp.genHash.FromHex().Concat(preparedTransaction.PayloadSigned.Take(52)).ToArray(), transaction.Signature.FromHex(), transaction.SignerPublicKey.FromHex()));
            Assert.That(tx.Hash == "F30769768E5279428A85688FBC072B59B052713CF655596332B8CC961EF8FEA3");
        }

        [Test, Timeout(20000)]
        public async Task SearchAggTransactions2() //F30769768E5279428A85688FBC072B59B052713CF655596332B8CC961EF8FEA3
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var result = await client.GetConfirmedTransaction("EFD336765554B74EBEF2FAF0F3CE834D8B7F4C461E1454CF6F304A0E178A2171");

            var transaction = ((SimpleTransfer)result.ComposedResponse.Transaction);

            var tx = VerifiableTransaction.Create(
                new TransferTransaction_V1(
                    Address.CreateFromHex(transaction.RecipientAddress),
                    transaction.Message == null ? EmptyMessage.Create() : PlainMessage.Create(transaction.Message),
                    Mosaic.CreateFromHexIdentifier(transaction.Mosaics[0].Id, transaction.Mosaics[0].Amount)
                    ),
                transaction.Network.GetNetworkValue(),
                transaction.MaxFee,
                new Deadline(NetworkType.Types.MAIN_NET, 0)
                );

            tx.Deadline = 170879695142; 
            tx.Signature = transaction.Signature.FromHex();
            tx.Signer = transaction.SignerPublicKey.FromHex();

            var payload = tx.Prepare();

            tx.Hash = VerifiableTransaction.HashTransaction(transaction.Signature.FromHex(), transaction.SignerPublicKey.FromHex(), HttpSetUp.maingenHash.FromHex().Concat(payload.PayloadSigned).ToArray()).ToHex();

            Assert.True(NaclFast.SignDetachedVerify(HttpSetUp.maingenHash.FromHex().Concat(payload.PayloadSigned).ToArray(), transaction.Signature.FromHex(), transaction.SignerPublicKey.FromHex()));
            Assert.That(tx.Hash == "EFD336765554B74EBEF2FAF0F3CE834D8B7F4C461E1454CF6F304A0E178A2171");
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

            Assert.That(response.ComposedResponse.Data.Count, Is.GreaterThan(0));

            response.ComposedResponse.Data.ForEach(i =>
            {
                Assert.That(((Aggregate)i.Transaction).Type, Is.EqualTo(16705));
                Assert.That(((Aggregate)i.Transaction).TransactionsHash.Length, Is.EqualTo(64));

            });
        }
    }
}