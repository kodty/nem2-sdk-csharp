using io.nem2.sdk.src.Infrastructure.Buffers.Model.Responses;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Model.Transactions;

namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    interface ITransactionRepository
    {
        // Get     
        IObservable<List<TransactionData>> SearchConfirmedTransactions(QueryModel queryModel);
        IObservable<List<TransactionData>> SearchUnconfirmedTransactions(QueryModel queryModel);
        IObservable<List<TransactionData>> SearchPartialTransactions(QueryModel queryModel);
        IObservable<TransactionData> GetConfirmedTransaction(string hash);
        IObservable<TransactionData> GetUnconfirmedTransaction(string hash);
        IObservable<TransactionData> GetPartialTransaction(string hash);

        // Post
        IObservable<List<TransactionData>> GetConfirmedTransactions(string[] transactionIds);
        IObservable<List<TransactionData>> GetUnconfirmedTransactions(string[] transactionIds);
        IObservable<List<TransactionData>> GetPartialTransactions(string[] transactionIds);

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
