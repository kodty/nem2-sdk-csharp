using io.nem2.sdk.src.Infrastructure.HttpExtension;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories
{
    interface IAccountRepository
    {
        IObservable<ExtendedHttpResponseMessage<List<AccountData>>> SearchAccounts(QueryModel queryModel);
        IObservable<ExtendedHttpResponseMessage<AccountData>> GetAccount(string pubkOrAddress);
        IObservable<ExtendedHttpResponseMessage<List<AccountData>>> GetAccounts(List<string> publicKeys);
        IObservable<ExtendedHttpResponseMessage<MerkleRoot>> GetAccountMerkle(string pubkOrAddress);

        // restrictions
        IObservable<ExtendedHttpResponseMessage<List<RestrictionData>>> SearchAccountRestrictions(QueryModel queryModel);
        IObservable<ExtendedHttpResponseMessage<RestrictionData>> GetAccountRestriction(string compositeHash);
        IObservable<ExtendedHttpResponseMessage<MerkleRoot>> GetAccountRestrictionsMerkle(string compositeHash);
    }
}
