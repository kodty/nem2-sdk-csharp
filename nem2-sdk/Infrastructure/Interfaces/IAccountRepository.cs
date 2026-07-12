using io.nem2.sdk.Infrastructure.Responses;

namespace io.nem2.sdk.Infrastructure.Interfaces
{
    interface IAccountRepository
    {
        IObservable<ExtendedHttpResponseMessege<Datum<AccountData>>> SearchAccounts(QueryModel queryModel);
        IObservable<ExtendedHttpResponseMessege<AccountData>> GetAccount(string pubkOrAddress);
        IObservable<ExtendedHttpResponseMessege<AccountData[]>> GetAccounts(List<string> publicKeys);
        IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetAccountMerkle(string pubkOrAddress);

        // restrictions
        IObservable<ExtendedHttpResponseMessege<RestrictionsData>> SearchAccountRestrictions(QueryModel queryModel);
        IObservable<ExtendedHttpResponseMessege<RestrictionData>> GetAccountRestriction(string compositeHash);
        IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetAccountRestrictionsMerkle(string compositeHash);
    }
}
