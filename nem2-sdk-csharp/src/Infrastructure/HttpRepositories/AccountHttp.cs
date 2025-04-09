using System.Reactive.Linq;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Infrastructure.Mapping;
using System.Text;
using System.Text.Json;


namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    public class AccountHttp : HttpRouter, IAccountRepository
    {
        public AccountHttp(string host, int port) 
            : base(host, port){ }

        public IObservable<List<AccountData>> SearchAccounts(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["accounts"], queryModel)))
                 .Select(r => { return ResponseFilters<AccountData>.FilterEvents(OverrideEnsureSuccessStatusCode(r), "data"); });
        }

        public IObservable<AccountData> GetAccount(string pubkOrAddress)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["accounts",pubkOrAddress])))
                 .Select(r => { return ObjectComposer.GenerateObject<AccountData>(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<List<AccountData>> GetAccounts(List<string> accounts) // flag
        {
            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["accounts"]), new StringContent(JsonSerializer.Serialize(new Public_Keys() { publicKeys = accounts }), Encoding.UTF8, "application/json")))
                 .Select(r => {  return ResponseFilters<AccountData>.FilterEvents(OverrideEnsureSuccessStatusCode(r)); });
        }


        public IObservable<MerkleRoot> GetAccountMerkle(string pubkOrAddress)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["accounts", pubkOrAddress, "merkle"])))
                 .Select(r => { return ObjectComposer.GenerateObject<MerkleRoot>(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<List<RestrictionData>> SearchAccountRestrictions(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["restrictions", "account"], queryModel)))
               .Select(r => { return ResponseFilters<RestrictionData>.FilterEvents(OverrideEnsureSuccessStatusCode(r), "data"); });
        }

        public IObservable<RestrictionData> GetAccountRestriction(string address)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["restrictions", "account", address])))
                .Select(r => { return ObjectComposer.GenerateObject<RestrictionData>(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<MerkleRoot> GetAccountRestrictionsMerkle(string address)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["restrictions", "account", address, "merkle" ])))
                .Select(r => { return ObjectComposer.GenerateObject<MerkleRoot>(OverrideEnsureSuccessStatusCode(r)); });
        }
    }
}
