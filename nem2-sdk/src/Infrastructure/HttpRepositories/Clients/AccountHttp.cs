using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Text;
using System.Text.Json;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Clients
{
    public class AccountHttp : HttpRouter, IAccountRepository
    {
        public AccountHttp(string host, int port) : base(host, port)
        {
            
        }
   
        public IObservable<ExtendedHttpResponseMessege<Datum<AccountData>>> SearchAccounts(QueryModel queryModel)
        {
            return HttpGetAsync<Datum<AccountData>>(queryModel, ["accounts"]);
        }

        public IObservable<ExtendedHttpResponseMessege<AccountData>> GetAccount(string pubkOrAddress)
        {
            return HttpGetAsync<AccountData>(["accounts", pubkOrAddress]);          
        }

        public IObservable<ExtendedHttpResponseMessege<AccountData[]>> GetAccounts(List<string> accounts) // flag
        {
            var content = new StringContent(JsonSerializer.Serialize(new Public_Keys() { publicKeys = accounts }), Encoding.UTF8, "application/json");

            return HttpPostAsync<AccountData[]>(["accounts"], content);
        }

        public IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetAccountMerkle(string pubkOrAddress)
        {
            return HttpGetAsync<MerkleRoot>(["accounts", pubkOrAddress, "merkle"]);
        }

        public IObservable<ExtendedHttpResponseMessege<RestrictionsData>> SearchAccountRestrictions(QueryModel queryModel)
        {
            return HttpGetAsync<RestrictionsData>(queryModel, ["restrictions", "account"]);
        }

        public IObservable<ExtendedHttpResponseMessege<RestrictionData>> GetAccountRestriction(string address)
        {
            return HttpGetAsync<RestrictionData>(["restrictions", "account", address]);
        }

        public IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetAccountRestrictionsMerkle(string address)
        {
            return HttpGetAsync<MerkleRoot>(["restrictions", "account", address, "merkle"]);          
        }
    }
}
