// ***********************************************************************
// Assembly         : nem2-sdk
using System.Reactive.Linq;
using System.Text;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Infrastructure.Mapping;
using System.Text.Json;

namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    public class NamespaceHttp : HttpRouter, INamespaceRepository
    {
        public NamespaceHttp(string host, int port) 
            : base(host, port) { }

        public IObservable<List<NamespaceDatum>> SearchNamespaces(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["namespaces"], queryModel)))
                .Select(r => { return ResponseFilters<NamespaceDatum>.FilterEvents(OverrideEnsureSuccessStatusCode(r), "data"); });
        }

        public IObservable<NamespaceDatum> GetNamespace(string namespaceId)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["namespaces", namespaceId])))
                .Select(r => { return ObjectComposer.GenerateObject<NamespaceDatum>(OverrideEnsureSuccessStatusCode(r)); });            
        }

        public IObservable<MerkleRoot> GetNamespaceMerkle(string namespaceId)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["namespaces", namespaceId, "merkle"])))
                .Select(r => { return ObjectComposer.GenerateObject<MerkleRoot>(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<List<NamespaceName>> GetNamespacesNames(List<string> namespaceIds)
        {
            var ids = new Namespace_Ids()
            {
                namespaceIds = namespaceIds
            };

            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["namespaces", "names"]), new StringContent(JsonSerializer.Serialize(ids), Encoding.UTF8, "application/json")))
                .Select(r => { return ResponseFilters<NamespaceName>.FilterEvents(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<List<AccountName>> GetAccountNames(List<string> addresses)
        {
            var ids = new Account_Ids()
            {
                addresses = addresses
            };

            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["namespaces", "account", "names"]), new StringContent(JsonSerializer.Serialize(ids), Encoding.UTF8, "application/json")))
               .Select(r => { return ResponseFilters<AccountName>.FilterEvents(OverrideEnsureSuccessStatusCode(r), "accountNames"); });
        }

        public IObservable<List<MosaicName>> GetMosaicNames(List<string> mosaicIds)
        {
            var ids = new MosaicIds()
            {
                mosaicIds = mosaicIds
            };

            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["namespaces", "mosaic", "names"]), new StringContent(JsonSerializer.Serialize(ids), Encoding.UTF8, "application/json")))
                 .Select(r => { return ResponseFilters<MosaicName>.FilterEvents(OverrideEnsureSuccessStatusCode(r), "mosaicNames"); });
        }
    }
}
