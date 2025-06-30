// ***********************************************************************
// Assembly         : nem2-sdk
using System.Reactive.Linq;
using System.Text;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Text.Json;
using io.nem2.sdk.src.Export;
using io.nem2.sdk.Model2;

namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    public class NamespaceHttp : HttpRouter, INamespaceRepository
    {
        public NamespaceHttp(string host, int port) 
            : base(host, port) { }

        public IObservable<List<NamespaceDatum>> SearchNamespaces(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["namespaces"], queryModel)))
                .Select(r => { return new ResponseFilters<NamespaceDatum>(TypeSerializationCatalog.CustomTypes).FilterEvents(OverrideEnsureSuccessStatusCode(r), "data"); });
        }

        public IObservable<NamespaceDatum> GetNamespace(string namespaceId)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["namespaces", namespaceId])))
                .Select(r => { return new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<NamespaceDatum>(OverrideEnsureSuccessStatusCode(r)); });            
        }

        public IObservable<MerkleRoot> GetNamespaceMerkle(string namespaceId)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["namespaces", namespaceId, "merkle"])))
                .Select(r => { return new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<MerkleRoot>(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<List<NamespaceName>> GetNamespacesNames(List<string> namespaceIds)
        {
            var ids = new Namespace_Ids()
            {
                namespaceIds = namespaceIds
            };

            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["namespaces", "names"]), new StringContent(JsonSerializer.Serialize(ids), Encoding.UTF8, "application/json")))
                .Select(r => { return new ResponseFilters<NamespaceName>(TypeSerializationCatalog.CustomTypes).FilterEvents(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<List<AccountName>> GetAccountNames(List<string> addresses)
        {
            var ids = new Account_Ids()
            {
                addresses = addresses
            };

            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["namespaces", "account", "names"]), new StringContent(JsonSerializer.Serialize(ids), Encoding.UTF8, "application/json")))
               .Select(r => { return new ResponseFilters<AccountName>(TypeSerializationCatalog.CustomTypes).FilterEvents(OverrideEnsureSuccessStatusCode(r), "accountNames"); });
        }

        public IObservable<List<MosaicName>> GetMosaicNames(List<string> mosaicIds)
        {
            var ids = new MosaicIds()
            {
                mosaicIds = mosaicIds
            };

            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["namespaces", "mosaic", "names"]), new StringContent(JsonSerializer.Serialize(ids), Encoding.UTF8, "application/json")))
                 .Select(r => { return new ResponseFilters<MosaicName>(TypeSerializationCatalog.CustomTypes).FilterEvents(OverrideEnsureSuccessStatusCode(r), "mosaicNames"); });
        }
    }
}
