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
        public TransactionHttp(string host, int port) : base(host, port) 
        {
        }

        public IObservable<ExtendedHttpResponseMessege<List<TransactionData>>> SearchConfirmedTransactions(QueryModel queryModel)
             => Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["transactions", "confirmed"], queryModel)))
                 .Select(r => { return FormTransactionResponse(r, "data"); });

        public IObservable<ExtendedHttpResponseMessege<List<TransactionData>>> SearchUnconfirmedTransactions(QueryModel queryModel)
             => Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["transactions", "unconfirmed"], queryModel)))
                 .Select(r => { return FormTransactionResponse(r, "data"); });

        public IObservable<ExtendedHttpResponseMessege<List<TransactionData>>> SearchPartialTransactions(QueryModel queryModel)
             => Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["transactions", "partial"], queryModel)))
                 .Select(r => { return FormTransactionResponse(r, "data"); });
              
        public IObservable<ExtendedHttpResponseMessege<TransactionData>> GetConfirmedTransaction(string hash)
             => Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["transactions", "confirmed", hash])))
                 .Select(FormTransactionResponse);

        public IObservable<ExtendedHttpResponseMessege<TransactionData>> GetUnconfirmedTransaction(string hash)
            => Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["transactions", "unconfirmed", hash])))
                .Select(FormTransactionResponse);

        public IObservable<ExtendedHttpResponseMessege<TransactionData>> GetPartialTransaction(string hash)
             => Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["transactions", "partial", hash])))
                 .Select(FormTransactionResponse);

        public IObservable<ExtendedHttpResponseMessege<ExtendedBroadcastStatus>> GetTransactionStatus(string hash)
            => HttpGetAsync<ExtendedBroadcastStatus>(["transactionStatus", hash]); 

        public IObservable<ExtendedHttpResponseMessege<List<TransactionData>>> GetConfirmedTransactions(string[] transactionIds)
             => Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["transactions", "confirmed"]), 
                new StringContent(
                    JsonSerializer.Serialize(
                        new TransactionIdentifiers() { transactionIds = transactionIds }), 
                    Encoding.UTF8, 
                    "application/json")
                )).Select(r => { return FormTransactionResponse(r, null); });        

        public IObservable<ExtendedHttpResponseMessege<List<TransactionData>>> GetUnconfirmedTransactions(string[] transactionIds)
             => Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["transactions", "unconfirmed"]), new StringContent(JsonSerializer.Serialize(new TransactionIdentifiers() { transactionIds = transactionIds }), Encoding.UTF8, "application/json")))
                 .Select(r => { return FormTransactionResponse(r, null); });

        public IObservable<ExtendedHttpResponseMessege<List<TransactionData>>> GetPartialTransactions(string[] transactionIds)
             => Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["transactions", "partial"]), 
                 new StringContent(
                     JsonSerializer.Serialize(
                         new TransactionIdentifiers() { transactionIds = transactionIds }), 
                     Encoding.UTF8, 
                     "application/json")
                 )).Select(r => { return FormTransactionResponse(r, null); });
        

        public class _Payload
        {
            public string payload { get; set; }
        }

        public IObservable<TransactionAnnounceResponse> Announce(SignedTransaction signedTransaction)
        {
            return Observable.FromAsync(async ar => await Client.PutAsync(GetUri(["transactions"]), new StringContent(JsonSerializer.Serialize(new _Payload() { payload = signedTransaction.Payload.ToHex() }), Encoding.UTF8, "application/json")))
                .Select(i =>  new TransactionAnnounceResponse() { Message = JsonNode.Parse(i.Content.ReadAsStringAsync().Result)["message"].ToString() });
        }

        public IObservable<TransactionAnnounceResponse> Announce(Payload payload)
        {
            return Observable.FromAsync(async ar => await Client.PutAsync(GetUri(["transactions"]), new StringContent(JsonSerializer.Serialize(new _Payload() { payload = payload.payload.ToHex() }), Encoding.UTF8, "application/json")))
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

        private ExtendedHttpResponseMessege<TransactionData> FormTransactionResponse(HttpResponseMessage msg)
        {
            var extendedResponse = ExtendResponse<TransactionData>(msg);

            if (msg.IsSuccessStatusCode)
                extendedResponse.ComposedResponse = ComposeTransaction(typeof(TransactionData), msg.Content.ReadAsStringAsync().Result);

            return extendedResponse;
        }

        private ExtendedHttpResponseMessege<List<TransactionData>> FormTransactionResponse(HttpResponseMessage msg, string path = null)
        {
            var extendedResponse = ExtendResponse<List<TransactionData>>(msg);

            if (msg.IsSuccessStatusCode)
            {
                var tx = path == null ? JsonNode.Parse(msg.Content.ReadAsStringAsync().Result) : JsonNode.Parse(msg.Content.ReadAsStringAsync().Result)[path];

                List<TransactionData> txs = new List<TransactionData>();

                foreach (var t in tx.AsArray())
                {
                    txs.Add(ComposeTransaction(typeof(TransactionData), t.ToString()));
                }

                extendedResponse.ComposedResponse = txs;
            }
              
            return extendedResponse;
        }
    }
}
