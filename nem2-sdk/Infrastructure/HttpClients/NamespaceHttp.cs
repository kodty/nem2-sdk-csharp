using io.nem2.sdk.Infrastructure.Interfaces;
using io.nem2.sdk.Infrastructure.Responses;

namespace io.nem2.sdk.Infrastructure.HttpClients
{
    public class MosaicNamespaceHttp : HttpRouter, IMosaicNamespaceRepository
    {
        public MosaicNamespaceHttp(string host, int port) 
            : base(host, port) { }

        public IObservable<ExtendedHttpResponseMessege<Datum<NamespaceData>>> SearchNamespaces(QueryModel queryModel)
            => HttpGetAsync<Datum<NamespaceData>>(queryModel, ["namespaces"]);

        public IObservable<ExtendedHttpResponseMessege<NamespaceData>> GetNamespace(string namespaceId)
            => HttpGetAsync<NamespaceData>(["namespaces", namespaceId]);

        public IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetNamespaceMerkle(string namespaceId)
            => HttpGetAsync<MerkleRoot>(["namespaces", namespaceId, "merkle"]);

        public IObservable<ExtendedHttpResponseMessege<NamespaceName[]>> GetNamespacesNames(List<string> namespaceIds)
            => HttpPostAsync<NamespaceName>(["namespaces", "names"], new { namespaceIds } );

        public IObservable<ExtendedHttpResponseMessege<Account_Names>> GetAccountNames(List<string> addresses)
             => HttpPostAsync1<Account_Names>(["namespaces", "account", "names"], new { addresses } );
            

        public IObservable<ExtendedHttpResponseMessege<Mosaic_Names>> GetMosaicNames(List<string> mosaicIds)
            => HttpPostAsync1<Mosaic_Names>(["namespaces", "mosaic", "names"], new { mosaicIds });


        public IObservable<ExtendedHttpResponseMessege<Datum<MosaicEvent>>> SearchMosaics(QueryModel queryModel)
            => HttpGetAsync<Datum<MosaicEvent>>(queryModel, ["mosaics"]);

        public IObservable<ExtendedHttpResponseMessege<MosaicEvent>> GetMosaic(string mosaicId)
            => HttpGetAsync<MosaicEvent>(["mosaics", mosaicId]);

        public IObservable<ExtendedHttpResponseMessege<MosaicEvent[]>> GetMosaics(List<string> mosaicIds) // object list
            => HttpPostAsync<MosaicEvent>(["mosaics"], new { mosaicIds });

        public IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetMosaicMerkle(string mosaicId)
            => HttpGetAsync<MerkleRoot>(["mosaics", mosaicId, "merkle"]);

        public IObservable<ExtendedHttpResponseMessege<Datum<MosaicRestrictionData>>> SearchMosaicRestrictions(QueryModel queryModel)
            => HttpGetAsync<Datum<MosaicRestrictionData>>(queryModel, ["restrictions", "mosaic"]);

        public IObservable<ExtendedHttpResponseMessege<MosaicRestrictionData>> GetMosaicRestriction(string compositeHash)
            => HttpGetAsync<MosaicRestrictionData>(["restrictions", "mosaic", compositeHash]);

        public IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetMosaicRestrictionMerkle(string compositeHash)
            => HttpGetAsync<MerkleRoot>(["restrictions", "mosaic", compositeHash, "merkle"]);
    }
}
