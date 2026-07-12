using Coppery;
using io.nem2.sdk.Infrastructure.Interfaces;
using io.nem2.sdk.Infrastructure.Responses;
using io.nem2.sdk.Model;
using io.nem2.sdk.Model.Transactions;
using System.Reactive.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace io.nem2.sdk.Infrastructure.HttpClients
{
    public class TransactionHttp : HttpRouter, ITransactionRepository
    {
        public TransactionHttp(string host, int port) : base(host, port) {

            Composer = new ObjectComposer(TransactionTypes.CustomFunction);
        }

        public IObservable<ExtendedHttpResponseMessege<Datum<TransactionData>>> SearchConfirmedTransactions(QueryModel queryModel)
            => HttpGetAsync<Datum<TransactionData>>(queryModel, ["transactions", "confirmed"]);

        public IObservable<ExtendedHttpResponseMessege<Datum<TransactionData>>> SearchUnconfirmedTransactions(QueryModel queryModel)
             => HttpGetAsync<Datum<TransactionData>>(queryModel, ["transactions", "unconfirmed"]);

        public IObservable<ExtendedHttpResponseMessege<Datum<TransactionData>>> SearchPartialTransactions(QueryModel queryModel)
            => HttpGetAsync<Datum<TransactionData>>(queryModel, ["transactions", "partial"]);

        public IObservable<ExtendedHttpResponseMessege<TransactionData>> GetConfirmedTransaction(string hash)
            => HttpGetAsync<TransactionData>(["transactions", "confirmed", hash]);

        public IObservable<ExtendedHttpResponseMessege<TransactionData>> GetUnconfirmedTransaction(string hash)
            => HttpGetAsync<TransactionData>(["transactions", "unconfirmed", hash]);

        public IObservable<ExtendedHttpResponseMessege<TransactionData>> GetPartialTransaction(string hash)
            => HttpGetAsync<TransactionData>(["transactions", "partial", hash]);

        public IObservable<ExtendedHttpResponseMessege<ExtendedBroadcastStatus>> GetTransactionStatus(string hash)
           => HttpGetAsync<ExtendedBroadcastStatus>(["transactionStatus", hash]);

        public IObservable<ExtendedHttpResponseMessege<ExtendedBroadcastStatus[]>> GetTransactionStatus(string[] hashes)
            => HttpPostAsync<ExtendedBroadcastStatus>(["transactionStatus"], new { hashes }); 

        public IObservable<ExtendedHttpResponseMessege<TransactionData[]>> GetConfirmedTransactions(string[] transactionIds)
            => HttpPostAsync<TransactionData>(["transactions", "confirmed"], new { transactionIds });

        public IObservable<ExtendedHttpResponseMessege<TransactionData[]>> GetUnconfirmedTransactions(string[] transactionIds)
             => HttpPostAsync<TransactionData>(["transactions", "unconfirmed"], new { transactionIds });

        public IObservable<ExtendedHttpResponseMessege<TransactionData[]>> GetPartialTransactions(string[] transactionIds)
             => HttpPostAsync<TransactionData>(["transactions", "partial"], new { transactionIds });

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
    }
}
