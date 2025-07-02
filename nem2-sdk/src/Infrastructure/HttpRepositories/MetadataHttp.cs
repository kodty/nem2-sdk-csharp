using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Export;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Model2;
using System.Reactive.Linq;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories
{
    public class MetadataHttp : HttpRouter, IMetadataRepository
    {
        public MetadataHttp(string host, int port) : base(host, port) { }

        public IObservable<List<Metadata>> SearchMetadataEntries(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["metadata"], queryModel)))
               .Select(r => { return new ResponseFilters<Metadata>(TypeSerializationCatalog.CustomTypes).FilterEvents(OverrideEnsureSuccessStatusCode(r), "data"); });
        }

        public IObservable<Metadata> GetMetadata(string compositeHash)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["metadata", compositeHash])))
               .Select(r => { return new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<Metadata>(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<MerkleRoot> GetMetadataMerkle(string compositeHash) 
        {
            var uriBuilder = new UriBuilder(Host)
            {
                Port = Port,
                Path = "/metadata/" + compositeHash + "/merkle"
            };

            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["metadata", compositeHash, "merkle"])))
               .Select(r => { return new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<MerkleRoot>(OverrideEnsureSuccessStatusCode(r)); });
        }
    }
}
