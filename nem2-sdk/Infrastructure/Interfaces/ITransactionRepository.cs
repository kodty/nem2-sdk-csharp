using io.nem2.sdk.Infrastructure.Responses;
using io.nem2.sdk.Model;

namespace io.nem2.sdk.Infrastructure.Interfaces
{
    interface ITransactionRepository
    {
        // Get     
        IObservable<ExtendedHttpResponseMessege<Datum<TransactionData>>> SearchConfirmedTransactions(QueryModel queryModel);
        IObservable<ExtendedHttpResponseMessege<Datum<TransactionData>>> SearchUnconfirmedTransactions(QueryModel queryModel);
        IObservable<ExtendedHttpResponseMessege<Datum<TransactionData>>> SearchPartialTransactions(QueryModel queryModel);
        IObservable<ExtendedHttpResponseMessege<TransactionData>> GetConfirmedTransaction(string hash);
        IObservable<ExtendedHttpResponseMessege<TransactionData>> GetUnconfirmedTransaction(string hash);
        IObservable<ExtendedHttpResponseMessege<TransactionData>> GetPartialTransaction(string hash);

        // Post
        IObservable<ExtendedHttpResponseMessege<TransactionData[]>> GetConfirmedTransactions(string[] transactionIds);
        IObservable<ExtendedHttpResponseMessege<TransactionData[]>> GetUnconfirmedTransactions(string[] transactionIds);
        IObservable<ExtendedHttpResponseMessege<TransactionData[]>> GetPartialTransactions(string[] transactionIds);

        // Put
        IObservable<TransactionAnnounceResponse> Announce(SignedTransaction payload);
        IObservable<TransactionAnnounceResponse> AnnounceAggregateTransaction(SignedTransaction payload);
        IObservable<TransactionAnnounceResponse> AnnounceCosignatureTransaction(CosignatureSignedTransaction payload);  
    }

    public class CosignatureSignedTransaction
    {
        public string ParentHash { get; set; }

        public string Signature { get; set; }

        public string Signer { get; set; }

        public int Version { get; set; }
    }
}
