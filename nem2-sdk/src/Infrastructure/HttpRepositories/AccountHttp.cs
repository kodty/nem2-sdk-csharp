using System.Reactive.Linq;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Text;
using System.Text.Json;
using io.nem2.sdk.src.Infrastructure.HttpExtension;

namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    public class AccountHttp : HttpRouter, IAccountRepository
    {
        public AccountHttp(string host, int port) : base(host, port)
        {
            
        }

        public IObservable<ExtendedHttpResponseMessage<List<AccountData>>> SearchAccounts(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["accounts"], queryModel)))
                 .Select(FormListResponse<AccountData>);               
        }

        public IObservable<ExtendedHttpResponseMessage<AccountData>> GetAccount(string pubkOrAddress)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["accounts", pubkOrAddress])))
                .Select(FormResponse<AccountData>);           
        }

        public IObservable<ExtendedHttpResponseMessage<List<AccountData>>> GetAccounts(List<string> accounts) // flag
        {
            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["accounts"]), new StringContent(JsonSerializer.Serialize(new Public_Keys() { publicKeys = accounts }), Encoding.UTF8, "application/json")))
                  .Select(FormListResponse<AccountData>);
        }


        public IObservable<ExtendedHttpResponseMessage<MerkleRoot>> GetAccountMerkle(string pubkOrAddress)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["accounts", pubkOrAddress, "merkle"])))
                .Select(FormResponse<MerkleRoot>);
        }

        public IObservable<ExtendedHttpResponseMessage<List<RestrictionData>>> SearchAccountRestrictions(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["restrictions", "account"], queryModel)))
                .Select(FormListResponse<RestrictionData>);
        }

        public IObservable<ExtendedHttpResponseMessage<RestrictionData>> GetAccountRestriction(string address)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["restrictions", "account", address])))
                 .Select(FormResponse<RestrictionData>);
        }

        public IObservable<ExtendedHttpResponseMessage<MerkleRoot>> GetAccountRestrictionsMerkle(string address)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["restrictions", "account", address, "merkle" ])))
                .Select(FormResponse<MerkleRoot>);
        }
    }
}
