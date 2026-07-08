using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

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
            => HttpPostAsync<NamespaceName>(["namespaces", "names"], new { namespaceIds } );

        public IObservable<ExtendedHttpResponseMessege<Account_Names>> GetAccountNames(List<string> addresses)
             => HttpPostAsync(ExtendResponse<Account_Names>, ["namespaces", "account", "names"], new { addresses } );
            

        public IObservable<ExtendedHttpResponseMessege<Mosaic_Names>> GetMosaicNames(List<string> mosaicIds)
            => HttpPostAsync(ExtendResponse<Mosaic_Names>, ["namespaces", "mosaic", "names"], new { mosaicIds });
    }
}
