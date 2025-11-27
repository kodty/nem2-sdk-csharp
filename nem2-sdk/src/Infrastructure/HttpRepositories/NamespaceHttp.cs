using System.Reactive.Linq;
using System.Text;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Text.Json;
using io.nem2.sdk.src.Infrastructure.HttpExtension;

namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    public class NamespaceHttp : HttpRouter, INamespaceRepository
    {
        public NamespaceHttp(string host, int port) 
            : base(host, port) { }

        public IObservable<ExtendedHttpResponseMessege<Datum<NamespaceData>>> SearchNamespaces(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["namespaces"], queryModel)))
                .Select(FormResponse<Datum<NamespaceData>>);
        }

        public IObservable<ExtendedHttpResponseMessege<NamespaceData>> GetNamespace(string namespaceId)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["namespaces", namespaceId])))
                .Select(FormResponse<NamespaceData>);
        }

        public IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetNamespaceMerkle(string namespaceId)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["namespaces", namespaceId, "merkle"])))
                .Select(FormResponse<MerkleRoot>);
        }

        public IObservable<ExtendedHttpResponseMessege<List<NamespaceName>>> GetNamespacesNames(List<string> namespaceIds)
        {
            var ids = new Namespace_Ids()
            {
                namespaceIds = namespaceIds
            };

            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["namespaces", "names"]), new StringContent(JsonSerializer.Serialize(ids), Encoding.UTF8, "application/json")))
                .Select(FormObjectList<NamespaceName>);
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
