using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Clients
{
    public class MetadataHttp : HttpRouter, IMetadataRepository
    {
        public MetadataHttp(string host, int port) : base(host, port) { }

        public IObservable<ExtendedHttpResponseMessege<Datum<Metadata>>> SearchMetadataEntries(QueryModel queryModel)
            => HttpGetAsync<Datum<Metadata>>(queryModel, ["metadata"] );

        public IObservable<ExtendedHttpResponseMessege<Metadata>> GetMetadata(string compositeHash)
            => HttpGetAsync<Metadata>(["metadata", compositeHash]);

        public IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetMetadataMerkle(string compositeHash) 
            => HttpGetAsync<MerkleRoot>(["metadata", compositeHash, "merkle"]);           
    }
}
