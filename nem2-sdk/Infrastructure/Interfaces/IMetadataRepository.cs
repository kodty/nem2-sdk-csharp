using io.nem2.sdk.Infrastructure.Responses;

namespace io.nem2.sdk.Infrastructure.Interfaces
{
    public interface IMetadataRepository
    {
        IObservable<ExtendedHttpResponseMessege<Datum<Metadata>>> SearchMetadataEntries(QueryModel queryModel);
        IObservable<ExtendedHttpResponseMessege<Metadata>> GetMetadata(string compositeHash);
        IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetMetadataMerkle(string compositeHash);
    }
}
