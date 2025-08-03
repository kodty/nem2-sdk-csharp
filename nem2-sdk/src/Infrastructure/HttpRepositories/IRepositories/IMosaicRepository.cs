using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    interface IMosaicRepository
    {
        IObservable<List<MosaicEvent>> SearchMosaics(QueryModel queryModel);
        IObservable<MosaicEvent> GetMosaic(string mosaicId);
        IObservable<List<MosaicEvent>> GetMosaics(List<string> mosaicIds);
        IObservable<MerkleRoot> GetMosaicMerkle(string mosaicId);

        // restrictions
        IObservable<List<MosaicRestrictionData>> SearchMosaicRestrictions(QueryModel queryModel);
        IObservable<MosaicRestrictionData> GetMosaicRestriction(string compositeHash);
        IObservable<MerkleRoot> GetMosaicRestrictionMerkle(string compositeHash);
    }
}
