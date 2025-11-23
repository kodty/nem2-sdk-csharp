using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpExtension;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Reactive.Linq;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories
{
    public class MetadataHttp : HttpRouter, IMetadataRepository
    {
        public MetadataHttp(string host, int port) : base(host, port) { }

        public IObservable<ExtendedHttpResponseMessege<List<Metadata>>> SearchMetadataEntries(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["metadata"], queryModel)))
               .Select(r => { return FormListResponse<Metadata>(r, "data"); });
        }

        public IObservable<ExtendedHttpResponseMessege<Metadata>> GetMetadata(string compositeHash)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["metadata", compositeHash])))
               .Select(FormResponse<Metadata>);
        }

        public IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetMetadataMerkle(string compositeHash) 
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["metadata", compositeHash, "merkle"])))
               .Select(FormResponse<MerkleRoot>);
        }
    }
}
