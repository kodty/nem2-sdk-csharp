using io.nem2.sdk.src.Infrastructure.HttpExtension;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories
{
    public interface IMetadataRepository
    {
        IObservable<ExtendedHttpResponseMessege<List<Metadata>>> SearchMetadataEntries(QueryModel queryModel);
        IObservable<ExtendedHttpResponseMessege<Metadata>> GetMetadata(string compositeHash);
        IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetMetadataMerkle(string compositeHash);
    }
}
