using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using Newtonsoft.Json;
using System.Reactive.Linq;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories
{
    public class MetadataHttp : HttpRouter, IMetadataRepository
    {
        public MetadataHttp(string host, int port) : base(host, port) { }

        public IObservable<MetadataCollection> SearchMetadataEntries(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["metadata"], queryModel)))
               .Select(JsonConvert.DeserializeObject<MetadataCollection>);
        }

        public IObservable<Metadata> GetMetadata(string compositeHash)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["metadata", compositeHash])))
               .Select(JsonConvert.DeserializeObject<Metadata>);
        }

        public IObservable<MerkleRoot> GetMetadataMerkle(string compositeHash) 
        {
            var uriBuilder = new UriBuilder(Host)
            {
                Port = Port,
                Path = "/metadata/" + compositeHash + "/merkle"
            };

            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["metadata", compositeHash, "merkle"])))
               .Select(JsonConvert.DeserializeObject<MerkleRoot>);
        }
    }
}
