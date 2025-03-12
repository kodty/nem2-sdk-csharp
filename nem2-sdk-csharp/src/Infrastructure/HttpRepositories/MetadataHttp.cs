using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Infrastructure.Mapping;
using System.Reactive.Linq;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories
{
    public class MetadataHttp : HttpRouter, IMetadataRepository
    {
        public MetadataHttp(string host, int port) : base(host, port) { }

        public IObservable<List<Metadata>> SearchMetadataEntries(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["metadata"], queryModel)))
               .Select(m => ResponseFilters<Metadata>.FilterEvents(m, "data"));
        }

        public IObservable<Metadata> GetMetadata(string compositeHash)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["metadata", compositeHash])))
               .Select(ObjectComposer.GenerateObject<Metadata>);
        }

        public IObservable<MerkleRoot> GetMetadataMerkle(string compositeHash) 
        {
            var uriBuilder = new UriBuilder(Host)
            {
                Port = Port,
                Path = "/metadata/" + compositeHash + "/merkle"
            };

            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["metadata", compositeHash, "merkle"])))
               .Select(ObjectComposer.GenerateObject<MerkleRoot>);
        }
    }
}
