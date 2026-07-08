using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Clients
{
    public class AccountHttp : HttpRouter, IAccountRepository
    {
        public AccountHttp(string host, int port) : base(host, port){}
   
        public IObservable<ExtendedHttpResponseMessege<Datum<AccountData>>> SearchAccounts(QueryModel queryModel)
            => HttpGetAsync<Datum<AccountData>>(queryModel, ["accounts"]);

        public IObservable<ExtendedHttpResponseMessege<AccountData>> GetAccount(string pubkOrAddress)
            => HttpGetAsync<AccountData>(["accounts", pubkOrAddress]);          

        public IObservable<ExtendedHttpResponseMessege<AccountData[]>> GetAccounts(List<string> publicKeys)
            => HttpPostAsync<AccountData>(["accounts"], new { publicKeys });

        public IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetAccountMerkle(string pubkOrAddress)
            => HttpGetAsync<MerkleRoot>(["accounts", pubkOrAddress, "merkle"]);

        public IObservable<ExtendedHttpResponseMessege<RestrictionsData>> SearchAccountRestrictions(QueryModel queryModel)
            => HttpGetAsync<RestrictionsData>(queryModel, ["restrictions", "account"]);

        public IObservable<ExtendedHttpResponseMessege<RestrictionData>> GetAccountRestriction(string address)
            => HttpGetAsync<RestrictionData>(["restrictions", "account", address]);

        public IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetAccountRestrictionsMerkle(string address)
            => HttpGetAsync<MerkleRoot>(["restrictions", "account", address, "merkle"]);                 
    }
}
