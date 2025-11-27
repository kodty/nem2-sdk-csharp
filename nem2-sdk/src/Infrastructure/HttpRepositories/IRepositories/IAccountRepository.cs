using io.nem2.sdk.src.Infrastructure.HttpExtension;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories
{
    interface IAccountRepository
    {
        IObservable<ExtendedHttpResponseMessege<Datum<AccountData>>> SearchAccounts(QueryModel queryModel);
        IObservable<ExtendedHttpResponseMessege<AccountData>> GetAccount(string pubkOrAddress);
        IObservable<ExtendedHttpResponseMessege<List<AccountData>>> GetAccounts(List<string> publicKeys);
        IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetAccountMerkle(string pubkOrAddress);

        // restrictions
        IObservable<ExtendedHttpResponseMessege<RestrictionsData>> SearchAccountRestrictions(QueryModel queryModel);
        IObservable<ExtendedHttpResponseMessege<RestrictionData>> GetAccountRestriction(string compositeHash);
        IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetAccountRestrictionsMerkle(string compositeHash);
    }
}
