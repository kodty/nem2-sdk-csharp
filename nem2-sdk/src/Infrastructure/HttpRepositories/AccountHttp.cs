using io.nem2.sdk.src.Infrastructure.Buffers.Model;
using io.nem2.sdk.src.Infrastructure.HttpExtension;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Reactive.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    public class AccountHttp : HttpRouter, IAccountRepository
    {
        public AccountHttp(string host, int port) : base(host, port)
        {
            
        }

        public IObservable<ExtendedHttpResponseMessege<Datum<AccountData>>> SearchAccounts(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["accounts"], queryModel)))
                 .Select(FormResponse<Datum<AccountData>>);
        }

        public IObservable<ExtendedHttpResponseMessege<AccountData>> GetAccount(string pubkOrAddress)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["accounts", pubkOrAddress])))
                .Select(FormResponse<AccountData>);           
        }

        public IObservable<ExtendedHttpResponseMessege<List<AccountData>>> GetAccounts(List<string> accounts) // flag
        {
            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["accounts"]), new StringContent(JsonSerializer.Serialize(new Public_Keys() { publicKeys = accounts }), Encoding.UTF8, "application/json")))
                  .Select(FormObjectList<AccountData>);
        }


        public IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetAccountMerkle(string pubkOrAddress)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["accounts", pubkOrAddress, "merkle"])))
                .Select(FormResponse<MerkleRoot>);
        }

        public IObservable<ExtendedHttpResponseMessege<RestrictionsData>> SearchAccountRestrictions(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["restrictions", "account"], queryModel)))
                .Select(FormResponse<RestrictionsData>);
        }

        public IObservable<ExtendedHttpResponseMessege<RestrictionData>> GetAccountRestriction(string address)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["restrictions", "account", address])))
                 .Select(FormResponse<RestrictionData>);
        }

        public IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetAccountRestrictionsMerkle(string address)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["restrictions", "account", address, "merkle" ])))
                .Select(FormResponse<MerkleRoot>);
        }
    }
}
