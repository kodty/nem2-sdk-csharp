using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories
{
    interface IAccountRepository
    {
        IObservable<List<AccountData>> SearchAccounts(QueryModel queryModel);
        IObservable<AccountData> GetAccount(string pubkOrAddress);
        IObservable<List<AccountData>> GetAccounts(List<string> publicKeys);
        IObservable<MerkleRoot> GetAccountMerkle(string pubkOrAddress);

        // restrictions
        IObservable<List<RestrictionData>> SearchAccountRestrictions(QueryModel queryModel);
        IObservable<RestrictionData> GetAccountRestriction(string compositeHash);
        IObservable<MerkleRoot> GetAccountRestrictionsMerkle(string compositeHash);
    }
}
