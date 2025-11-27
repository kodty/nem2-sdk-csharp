using System.Reactive.Linq;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Text;
using System.Text.Json;
using io.nem2.sdk.src.Infrastructure.HttpExtension;
using System.Text.Json.Nodes;

namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    public class AccountHttp : HttpRouter, IAccountRepository
    {
        public AccountHttp(string host, int port) : base(host, port)
        {
            
        }

        public IObservable<ExtendedHttpResponseMessege<AccountsData>> SearchAccounts(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["accounts"], queryModel)))
                 .Select(FormResponse<AccountsData>);
        }

        public IObservable<ExtendedHttpResponseMessege<AccountData>> GetAccount(string pubkOrAddress)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["accounts", pubkOrAddress])))
                .Select(FormResponse<AccountData>);           
        }

        public IObservable<ExtendedHttpResponseMessege<List<AccountData>>> GetAccounts(List<string> accounts) // flag
        {
            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["accounts"]), new StringContent(JsonSerializer.Serialize(new Public_Keys() { publicKeys = accounts }), Encoding.UTF8, "application/json")))
                  .Select(r => {

                      var extended = ExtendResponse<List<AccountData>>(r);

                      var objs = JsonNode.Parse(r.Content.ReadAsStringAsync().Result);

                      List<AccountData> data = new List<AccountData>();

                      foreach (var o in objs.AsArray())
                          data.Add(Composer.GenerateObject<AccountData>(o.ToString()));

                      extended.ComposedResponse = data;
                      
                      return extended;
                  });
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
