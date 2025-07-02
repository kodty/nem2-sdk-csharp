using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories
{
    public interface IMetadataRepository
    {
        IObservable<List<Metadata>> SearchMetadataEntries(QueryModel queryModel);
        IObservable<Metadata> GetMetadata(string compositeHash);
        IObservable<MerkleRoot> GetMetadataMerkle(string compositeHash);
    }
}
