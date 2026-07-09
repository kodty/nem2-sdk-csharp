using Coppery;
using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Model;
using io.nem2.sdk.src.Model.Transactions;
using System.Reactive.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Clients
{
    public class TransactionHttp : HttpRouter, ITransactionRepository
    {
        public TransactionHttp(string host, int port) : base(host, port) {

            Composer = new ObjectComposer(TransactionTypes.ComposeEmbeddedTransaction);
        }

        public IObservable<ExtendedHttpResponseMessege<Datum<TransactionData>>> SearchConfirmedTransactions(QueryModel queryModel)
             => Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["transactions", "confirmed"], queryModel)))
                 .Select(func3);

        public IObservable<ExtendedHttpResponseMessege<Datum<TransactionData>>> SearchUnconfirmedTransactions(QueryModel queryModel)
             => Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["transactions", "unconfirmed"], queryModel)))
                 .Select(func3);

        public IObservable<ExtendedHttpResponseMessege<Datum<TransactionData>>> SearchPartialTransactions(QueryModel queryModel)
             => Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["transactions", "partial"], queryModel)))
                 .Select(func3);

        private ExtendedHttpResponseMessege<Datum<TransactionData>> func3(HttpResponseMessage e)
        {
            var r = ExtendResponse<Datum<TransactionData>>(e);
            r.ComposedResponse = new Datum<TransactionData>() { Data = ComposeTransactions(JsonNode.Parse(e.Content.ReadAsStringAsync().Result)["data"]) };
            return r;
        }

        public IObservable<ExtendedHttpResponseMessege<TransactionData>> GetConfirmedTransaction(string hash)
            => Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["transactions", "confirmed", hash])))
                 .Select(func2);

        public IObservable<ExtendedHttpResponseMessege<TransactionData>> GetUnconfirmedTransaction(string hash)
            => Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["transactions", "unconfirmed", hash])))
                .Select(func2);

        public IObservable<ExtendedHttpResponseMessege<TransactionData>> GetPartialTransaction(string hash)
            => Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["transactions", "partial", hash])))
                 .Select(func2);
        private ExtendedHttpResponseMessege<TransactionData> func2(HttpResponseMessage e)
        {
            var r = ExtendResponse<TransactionData>(e);
            r.ComposedResponse = ComposeTransaction(e.Content.ReadAsStringAsync().Result);
            return r;
        }

        public IObservable<ExtendedHttpResponseMessege<ExtendedBroadcastStatus[]>> GetTransactionStatus(string[] hashes)
            => HttpPostAsync<ExtendedBroadcastStatus>(["transactionStatus"], new { hashes }); 

        public IObservable<ExtendedHttpResponseMessege<List<TransactionData>>> GetConfirmedTransactions(string[] transactionIds)
             => Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["transactions", "confirmed"]), 
                 new StringContent(JsonSerializer.Serialize(new { transactionIds }), Encoding.UTF8, "application/json")
                 )).Select(func1);
      
        public IObservable<ExtendedHttpResponseMessege<List<TransactionData>>> GetUnconfirmedTransactions(string[] transactionIds)
             => Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["transactions", "unconfirmed"]), 
                 new StringContent(JsonSerializer.Serialize(new TransactionIdentifiers() { transactionIds = transactionIds }), Encoding.UTF8, "application/json")
                 )).Select(func1);

        public IObservable<ExtendedHttpResponseMessege<List<TransactionData>>> GetPartialTransactions(string[] transactionIds)
             => Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["transactions", "partial"]), 
                 new StringContent(JsonSerializer.Serialize(new TransactionIdentifiers() { transactionIds = transactionIds }), Encoding.UTF8, "application/json")
                 )).Select(func1);

        private ExtendedHttpResponseMessege<List<TransactionData>> func1(HttpResponseMessage e)
        {
            var r = ExtendResponse<List<TransactionData>>(e);
            r.ComposedResponse = ComposeTransactions(JsonNode.Parse(e.Content.ReadAsStringAsync().Result));
            return r;
        }

        public IObservable<TransactionAnnounceResponse> Announce(SignedTransaction signedTransaction)
        {
            return Observable.FromAsync(async ar => await Client.PutAsync(GetUri(["transactions"]), new StringContent(JsonSerializer.Serialize(new { payload = signedTransaction.Payload.ToHex() }), Encoding.UTF8, "application/json")))
                .Select(i =>  new TransactionAnnounceResponse() { Message = JsonNode.Parse(i.Content.ReadAsStringAsync().Result)["message"].ToString() });
        }

        public IObservable<TransactionAnnounceResponse> Announce(Payload payload)
        {
            return Observable.FromAsync(async ar => await Client.PutAsync(GetUri(["transactions"]), new StringContent(JsonSerializer.Serialize(new { payload = payload.payload.ToHex() }), Encoding.UTF8, "application/json")))
                .Select(i => new TransactionAnnounceResponse() { Message = JsonNode.Parse(i.Content.ReadAsStringAsync().Result)["message"].ToString() });
        }

        public IObservable<TransactionAnnounceResponse> AnnounceAggregateTransaction(SignedTransaction signedTransaction)
        {
            return Observable.FromAsync(async ar => await Client.PutAsync(GetUri(["transactions", "partial"]), new StringContent(JsonNode.Parse(signedTransaction.Payload).ToString(), Encoding.UTF8, "application/json")))
                .Select(i => new TransactionAnnounceResponse() { Message = JsonNode.Parse(i.Content.ToString())["message"].ToString() });
        }

        public IObservable<TransactionAnnounceResponse> AnnounceCosignatureTransaction(CosignatureSignedTransaction signedTransaction)
        {
            return Observable.FromAsync(async ar => await Client.PutAsync(GetUri(["transactions", "cosignature"]), new StringContent(JsonSerializer.Serialize(signedTransaction))))
                .Select(i => new TransactionAnnounceResponse() { Message = JsonNode.Parse(i.Content.ToString())["message"].ToString() });
        }

        private List<TransactionData> ComposeTransactions(JsonNode content)
        {
            List<TransactionData> txs = new List<TransactionData>();

            foreach (var t in content.AsArray())
            {
                txs.Add(ComposeTransaction(t.ToString()));
            }

            return txs;
        }

        internal dynamic ComposeTransaction(string data, bool embedded = false)
        {
            var tx = JsonObject.Parse(data).AsObject();

            var type = TransactionTypes.GetTransactionType(data, embedded);

            dynamic shell = Composer.GenerateObject(typeof(TransactionData), tx.AsObject());

            shell.Transaction = Composer.GenerateObject(type, tx["transaction"].AsObject());

            return shell;
        }
    }
}
