using Coppery;
using io.nem2.sdk.src.Infrastructure.Buffers.Model.Responses;
using io.nem2.sdk.src.Infrastructure.HttpExtension;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Model;
using io.nem2.sdk.src.Model.Transactions;
using System.Reactive.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    public class TransactionHttp : HttpRouter, ITransactionRepository
    {
        public TransactionHttp(string host, int port) : base(host, port) 
        {
        }

        public IObservable<ExtendedHttpResponseMessege<List<TransactionData>>> SearchConfirmedTransactions(QueryModel queryModel)
        {          
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["transactions", "confirmed"], queryModel)))
               .Select(r => { return FormTransactionResponse(r, "data"); });
        }

        public IObservable<ExtendedHttpResponseMessege<List<TransactionData>>> SearchUnconfirmedTransactions(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["transactions", "unconfirmed"], queryModel)))
              .Select(r => { return FormTransactionResponse(r, "data"); });
        }

        public IObservable<ExtendedHttpResponseMessege<List<TransactionData>>> SearchPartialTransactions(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["transactions", "partial"], queryModel)))
                .Select(r => { return FormTransactionResponse(r, "data"); });
        }
              
        public IObservable<ExtendedHttpResponseMessege<TransactionData>> GetConfirmedTransaction(string hash)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["transactions", "confirmed", hash])))
              .Select(r => { return FormTransactionResponse(r); });
        }

        public IObservable<ExtendedHttpResponseMessege<TransactionData>> GetUnconfirmedTransaction(string hash)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["transactions", "unconfirmed", hash])))
              .Select(r => { return FormTransactionResponse(r); });
        }

        public IObservable<ExtendedHttpResponseMessege<TransactionData>> GetPartialTransaction(string hash)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["transactions", "partial", hash])))
              .Select(r => { return FormTransactionResponse(r); });
        }

        public IObservable<ExtendedHttpResponseMessege<ExtendedBroadcastStatus>> GetTransactionStatus(string hash)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["transactionStatus", hash])))
               .Select(FormResponse<ExtendedBroadcastStatus>);
        }

        public IObservable<ExtendedHttpResponseMessege<List<TransactionData>>> GetConfirmedTransactions(string[] transactionIds)
        {
            var postBody = JsonSerializer.Serialize(new TransactionIdentifiers() { transactionIds = transactionIds });

            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["transactions", "confirmed"]), new StringContent(postBody, Encoding.UTF8, "application/json")))
                 .Select(r => { return FormTransactionResponse(r, null); });
        }

        public IObservable<ExtendedHttpResponseMessege<List<TransactionData>>> GetUnconfirmedTransactions(string[] transactionIds)
        {
            var postBody = JsonSerializer.Serialize(new TransactionIdentifiers() { transactionIds = transactionIds });

            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["transactions", "unconfirmed"]), new StringContent(postBody, Encoding.UTF8, "application/json")))
                  .Select(r => { return FormTransactionResponse(r, null); });
        }

        public IObservable<ExtendedHttpResponseMessege<List<TransactionData>>> GetPartialTransactions(string[] transactionIds)
        {
            var postBody = JsonSerializer.Serialize(new TransactionIdentifiers() { transactionIds = transactionIds });

            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["transactions", "partial"]), new StringContent(postBody, Encoding.UTF8, "application/json")))
                 .Select(r => { return FormTransactionResponse(r, null); });
        }

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
                extendedResponse.ComposedResponse = Composer.ComposeTransaction<TransactionData>(msg.Content.ReadAsStringAsync().Result);

            return extendedResponse;
        }

        private ExtendedHttpResponseMessege<List<TransactionData>> FormTransactionResponse(HttpResponseMessage msg, string path = null)
        {
            var extendedResponse = ExtendResponse<List<TransactionData>>(msg);

            if (msg.IsSuccessStatusCode)
                extendedResponse.ComposedResponse = Composer.ComposeTransactions<TransactionData>(msg.Content.ReadAsStringAsync().Result, path);

            return extendedResponse;
        }
    }
}
