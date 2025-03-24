using System.Reactive.Linq;
using System.Text;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Infrastructure.Mapping;
using System.Text.Json;

namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    public class MosaicHttp : HttpRouter, IMosaicRepository
    {
        public MosaicHttp(string host, int port) : base(host, port) { }

        public IObservable<List<MosaicEvent>> SearchMosaics(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["mosaics"], queryModel)))
                .Select(m => ResponseFilters<MosaicEvent>.FilterEvents(m, "data"));
        }

        public IObservable<MosaicEvent> GetMosaic(string mosaicId)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["mosaics", mosaicId])))
                .Select(r => { return ObjectComposer.GenerateObject<MosaicEvent>(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<List<MosaicEvent>> GetMosaics(List<string> mosaicIds)
        {     
            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["mosaics"]), new StringContent(JsonSerializer.Serialize(new MosaicIds() { mosaicIds = mosaicIds }), Encoding.UTF8, "application/json")))
                .Select(r => { return ResponseFilters<MosaicEvent>.FilterEvents(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<MerkleRoot> GetMosaicMerkle(string mosaicId)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["mosaics", mosaicId, "merkle"])))
                .Select(r => { return ObjectComposer.GenerateObject<MerkleRoot>(OverrideEnsureSuccessStatusCode(r)); });

        }

        public IObservable<List<MosaicRestrictionData>> SearchMosaicRestrictions(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["restrictions", "mosaic"], queryModel)))
               .Select(r => { return ResponseFilters<MosaicRestrictionData>.FilterEvents(OverrideEnsureSuccessStatusCode(r), "data"); });
        }

        public IObservable<MosaicRestrictionData> GetMosaicRestriction(string compositeHash)
        { 
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["restrictions", "mosaic", compositeHash])))
                .Select(r => { return ObjectComposer.GenerateObject<MosaicRestrictionData>(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<MerkleRoot> GetMosaicRestrictionMerkle(string compositeHash)
        { 
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["restrictions", "mosaic", compositeHash, "merkle"])))
                .Select(ObjectComposer.GenerateObject<MerkleRoot>);
        }
    }
}
