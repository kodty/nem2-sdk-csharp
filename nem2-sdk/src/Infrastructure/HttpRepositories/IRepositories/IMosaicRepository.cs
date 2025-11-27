using io.nem2.sdk.src.Infrastructure.HttpExtension;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    interface IMosaicRepository
    {
        IObservable<ExtendedHttpResponseMessege<Datum<MosaicEvent>>> SearchMosaics(QueryModel queryModel);
        IObservable<ExtendedHttpResponseMessege<MosaicEvent>> GetMosaic(string mosaicId);
        IObservable<ExtendedHttpResponseMessege<List<MosaicEvent>>> GetMosaics(List<string> mosaicIds);
        IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetMosaicMerkle(string mosaicId);

        // restrictions
        IObservable<ExtendedHttpResponseMessege<Datum<MosaicRestrictionData>>> SearchMosaicRestrictions(QueryModel queryModel);
        IObservable<ExtendedHttpResponseMessege<MosaicRestrictionData>> GetMosaicRestriction(string compositeHash);
        IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetMosaicRestrictionMerkle(string compositeHash);
    }
}
