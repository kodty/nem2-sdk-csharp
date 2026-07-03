using System.Reactive.Linq;
using System.Text;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Text.Json;
using io.nem2.sdk.Infrastructure.HttpRepositories;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Clients
{
    public class NamespaceHttp : HttpRouter, INamespaceRepository
    {
        public NamespaceHttp(string host, int port) 
            : base(host, port) { }

        public IObservable<ExtendedHttpResponseMessege<Datum<NamespaceData>>> SearchNamespaces(QueryModel queryModel)
        {
            return HttpGetAsync<Datum<NamespaceData>>(queryModel, ["namespaces"]);

        }

        public IObservable<ExtendedHttpResponseMessege<NamespaceData>> GetNamespace(string namespaceId)
        {
            return HttpGetAsync<NamespaceData>(["namespaces", namespaceId]);
        }

        public IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetNamespaceMerkle(string namespaceId)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["namespaces", namespaceId, "merkle"])))
                .Select(FormResponse<MerkleRoot>);
        }

        public IObservable<ExtendedHttpResponseMessege<NamespaceName[]>> GetNamespacesNames(List<string> namespaceIds)
        {
            var ids = new Namespace_Ids()
            {
                namespaceIds = namespaceIds
            };

            var content = new StringContent(JsonSerializer.Serialize(ids), Encoding.UTF8, "application/json");

            return HttpPostAsync<NamespaceName>(["namespaces", "names"], content);
        }

        public IObservable<ExtendedHttpResponseMessege<Account_Names>> GetAccountNames(List<string> addresses)
        {
            var ids = new Account_Ids()
            {
                addresses = addresses
            };

            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["namespaces", "account", "names"]), new StringContent(JsonSerializer.Serialize(ids), Encoding.UTF8, "application/json")))
                .Select(FormResponse<Account_Names>);
        }

        public IObservable<ExtendedHttpResponseMessege<Mosaic_Names>> GetMosaicNames(List<string> mosaicIds)
        {
            var ids = new MosaicIds()
            {
                mosaicIds = mosaicIds
            };

            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["namespaces", "mosaic", "names"]), new StringContent(JsonSerializer.Serialize(ids), Encoding.UTF8, "application/json")))
                  .Select(FormResponse<Mosaic_Names>);
        }
    }
}
