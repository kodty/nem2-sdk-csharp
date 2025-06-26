using System.Reactive.Linq;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Infrastructure.Buffers.Model.Responses;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

using System.Text.Json;
using System.Text;
using io.nem2.sdk.src.Export;
using System.Diagnostics;
using io.nem2.sdk.Model2;
using io.nem2.sdk.Core.Crypto.Chaso.NaCl;
using System.Text.Json.Nodes;

namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    public class TransactionHttp : HttpRouter, ITransactionRepository
    {
        public TransactionHttp(string host, int port) 
            : base(host, port) {}

        public IObservable<List<TransactionData>> SearchConfirmedTransactions(QueryModel queryModel)
        {          
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["transactions", "confirmed"], queryModel)))
               .Select(r => { return ResponseFilters<TransactionData>.FilterTransactions(OverrideEnsureSuccessStatusCode(r), "data"); });
        }

        public IObservable<List<TransactionData>> SearchUnconfirmedTransactions(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["transactions", "unconfirmed"], queryModel)))
               .Select(r => { return ResponseFilters<TransactionData>.FilterTransactions(OverrideEnsureSuccessStatusCode(r), "data"); });
        }

        public IObservable<List<TransactionData>> SearchPartialTransactions(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["transactions", "partial"], queryModel)))
                .Select(r => { return ResponseFilters<TransactionData>.FilterTransactions(OverrideEnsureSuccessStatusCode(r), "data"); });
        }

        public IObservable<TransactionData> GetConfirmedTransaction(string hash)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["transactions", "confirmed", hash])))
               .Select(r => { return ResponseFilters<TransactionData>.FilterSingle(OverrideEnsureSuccessStatusCode(r)); });
        }
        public IObservable<TransactionData> GetUnconfirmedTransaction(string hash)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["transactions", "unconfirmed", hash])))
               .Select(r => { return ResponseFilters<TransactionData>.FilterSingle(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<TransactionData> GetPartialTransaction(string hash)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["transactions", "partial", hash])))
               .Select(r => { return ResponseFilters<TransactionData>.FilterSingle(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<ExtendedBroadcastStatus> GetTransactionStatus(string hash)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["transactionStatus", hash])))
               .Select(r => { return ObjectComposer.GenerateObject<ExtendedBroadcastStatus>(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<List<TransactionData>> GetConfirmedTransactions(string[] transactionIds)
        {
            var postBody = JsonSerializer.Serialize(new TransactionIdentifiers() { transactionIds = transactionIds });

            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["transactions", "confirmed"]), new StringContent(postBody, Encoding.UTF8, "application/json")))
                 .Select(r => { return ResponseFilters<TransactionData>.FilterTransactions(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<List<TransactionData>> GetUnconfirmedTransactions(string[] transactionIds)
        {
            var postBody = JsonSerializer.Serialize(new TransactionIdentifiers() { transactionIds = transactionIds });

            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["transactions", "unconfirmed"]), new StringContent(postBody, Encoding.UTF8, "application/json")))
                 .Select(r => { return ResponseFilters<TransactionData>.FilterTransactions(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<List<TransactionData>> GetPartialTransactions(string[] transactionIds)
        {
            var postBody = JsonSerializer.Serialize(new TransactionIdentifiers() { transactionIds = transactionIds });

            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["transactions", "partial"]), new StringContent(postBody, Encoding.UTF8, "application/json")))
                 .Select(r => { return ResponseFilters<TransactionData>.FilterTransactions(OverrideEnsureSuccessStatusCode(r)); });
        }

        public class _Payload
        {
            public string payload { get; set; }
        }

        public IObservable<TransactionAnnounceResponse> Announce(SignedTransaction signedTransaction)
        { 
            return Observable.FromAsync(async ar => await Client.PutAsync(GetUri(["transactions"]), new StringContent(JsonSerializer.Serialize(new _Payload() { payload = signedTransaction.Payload }), Encoding.UTF8, "application/json")))
                .Select(i =>  new TransactionAnnounceResponse() { Message = JsonObject.Parse(i.Content.ReadAsStringAsync().Result)["message"].ToString() });
        }

        public IObservable<TransactionAnnounceResponse> Announce(Payload payload)
        {
            return Observable.FromAsync(async ar => await Client.PutAsync(GetUri(["transactions"]), new StringContent(JsonSerializer.Serialize(new _Payload() { payload = payload.payload.ToHexLower() }), Encoding.UTF8, "application/json")))
                .Select(i => new TransactionAnnounceResponse() { Message = JsonObject.Parse(i.Content.ReadAsStringAsync().Result)["message"].ToString() });
        }

        public IObservable<TransactionAnnounceResponse> AnnounceAggregateTransaction(SignedTransaction signedTransaction)
        {
            return Observable.FromAsync(async ar => await Client.PutAsync(GetUri(["transactions", "partial"]), new StringContent(JsonObject.Parse(signedTransaction.Payload).ToString(), Encoding.UTF8, "application/json")))
                .Select(i => new TransactionAnnounceResponse() { Message = JsonObject.Parse(i.Content.ToString())["message"].ToString() });
        }

        public IObservable<TransactionAnnounceResponse> AnnounceCosignatureTransaction(CosignatureSignedTransaction signedTransaction)
        {
            return Observable.FromAsync(async ar => await Client.PutAsync(GetUri(["transactions", "cosignature"]), new StringContent(JsonSerializer.Serialize(signedTransaction))))
                .Select(i => new TransactionAnnounceResponse() { Message = JsonObject.Parse(i.Content.ToString())["message"].ToString() });
        }
    }
}
