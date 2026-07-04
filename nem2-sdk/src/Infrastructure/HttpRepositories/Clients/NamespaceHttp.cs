using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Model.Articles;
using System.Reactive.Linq;
using System.Text;
using System.Text.Json;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Clients
{
    public class NamespaceHttp : HttpRouter, INamespaceRepository
    {
        public NamespaceHttp(string host, int port) 
            : base(host, port) { }

        public IObservable<ExtendedHttpResponseMessege<Datum<NamespaceData>>> SearchNamespaces(QueryModel queryModel)
            => HttpGetAsync<Datum<NamespaceData>>(queryModel, ["namespaces"]);

        public IObservable<ExtendedHttpResponseMessege<NamespaceData>> GetNamespace(string namespaceId)
            => HttpGetAsync<NamespaceData>(["namespaces", namespaceId]);

        public IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetNamespaceMerkle(string namespaceId)
            => HttpGetAsync<MerkleRoot>(["namespaces", namespaceId, "merkle"]);

        public IObservable<ExtendedHttpResponseMessege<NamespaceName[]>> GetNamespacesNames(List<string> namespaceIds)
            => HttpPostAsync<NamespaceName>(["namespaces", "names"], 
                new StringContent(
                    JsonSerializer.Serialize(
                        new Namespace_Ids(){
                            namespaceIds = namespaceIds
                        }), 
                    Encoding.UTF8, 
                    "application/json")
                );

        public IObservable<ExtendedHttpResponseMessege<Account_Names>> GetAccountNames(List<string> addresses)
            => Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["namespaces", "account", "names"]),
                new StringContent(
                    JsonSerializer.Serialize(
                        new Account_Ids()
                        {
                            addresses = addresses
                        }),
                    Encoding.UTF8,
                    "application/json")
                )).Select(e => FormResponse(ExtendResponse<Account_Names>(e)));


        public IObservable<ExtendedHttpResponseMessege<Mosaic_Names>> GetMosaicNames(List<string> mosaicIds)
            => Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["namespaces", "mosaic", "names"]), 
                new StringContent(
                    JsonSerializer.Serialize(
                        new MosaicIds()
                        {
                            mosaicIds = mosaicIds
                        }),
                    Encoding.UTF8,
                    "application/json")
                )).Select(e => FormResponse(ExtendResponse<Mosaic_Names>(e)));
    }
}
