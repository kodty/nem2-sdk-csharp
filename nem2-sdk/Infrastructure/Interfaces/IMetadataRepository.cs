using io.nem2.sdk.Infrastructure;
using io.nem2.sdk.Infrastructure.Responses;

namespace Coppery
{
    public interface IMetadataRepository
    {
        IObservable<ExtendedHttpResponseMessege<Datum<Metadata>>> SearchMetadataEntries(QueryModel queryModel);
        IObservable<ExtendedHttpResponseMessege<Metadata>> GetMetadata(string compositeHash);
        IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetMetadataMerkle(string compositeHash);
    }
}
