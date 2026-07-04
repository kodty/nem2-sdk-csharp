using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Text;
using System.Text.Json;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Clients
{
    public class MosaicHttp : HttpRouter, IMosaicRepository
    {
        public MosaicHttp(string host, int port) : base(host, port) { }

        public IObservable<ExtendedHttpResponseMessege<Datum<MosaicEvent>>> SearchMosaics(QueryModel queryModel)
            => HttpGetAsync<Datum<MosaicEvent>>(queryModel, ["mosaics" ]);            

        public IObservable<ExtendedHttpResponseMessege<MosaicEvent>> GetMosaic(string mosaicId)
            => HttpGetAsync<MosaicEvent>(["mosaics", mosaicId]);

        public IObservable<ExtendedHttpResponseMessege<MosaicEvent[]>> GetMosaics(List<string> mosaicIds) // object list
            => HttpPostAsync<MosaicEvent>(["mosaics"], 
                    new StringContent(
                            JsonSerializer.Serialize(
                                new MosaicIds() { mosaicIds = mosaicIds }), 
                            Encoding.UTF8, 
                            "application/json"
                        ));

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
