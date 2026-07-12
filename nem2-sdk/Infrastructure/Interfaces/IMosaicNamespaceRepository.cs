using io.nem2.sdk.Infrastructure.Responses;

namespace io.nem2.sdk.Infrastructure.Interfaces
{
    interface IMosaicNamespaceRepository
    {
        // Get
        IObservable<ExtendedHttpResponseMessege<Datum<NamespaceData>>> SearchNamespaces(QueryModel queryModel);
        IObservable<ExtendedHttpResponseMessege<NamespaceData>> GetNamespace(string namespaceId);
        IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetNamespaceMerkle(string namespaceId);

        // Post
        IObservable<ExtendedHttpResponseMessege<NamespaceName[]>> GetNamespacesNames(List<string> namespaceIds);
        IObservable<ExtendedHttpResponseMessege<Account_Names>> GetAccountNames(List<string> addresses);
        IObservable<ExtendedHttpResponseMessege<Mosaic_Names>> GetMosaicNames(List<string> mosaicIds);

        IObservable<ExtendedHttpResponseMessege<Datum<MosaicEvent>>> SearchMosaics(QueryModel queryModel);
        IObservable<ExtendedHttpResponseMessege<MosaicEvent>> GetMosaic(string mosaicId);
        IObservable<ExtendedHttpResponseMessege<MosaicEvent[]>> GetMosaics(List<string> mosaicIds);
        IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetMosaicMerkle(string mosaicId);

        // restrictions
        IObservable<ExtendedHttpResponseMessege<Datum<MosaicRestrictionData>>> SearchMosaicRestrictions(QueryModel queryModel);
        IObservable<ExtendedHttpResponseMessege<MosaicRestrictionData>> GetMosaicRestriction(string compositeHash);
        IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetMosaicRestrictionMerkle(string compositeHash);
    }
}
