using System.Reactive.Linq;
using System.Text;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Text.Json;
using io.nem2.sdk.src.Export;
using io.nem2.sdk.src.Model2;

namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    public class MosaicHttp : HttpRouter, IMosaicRepository
    {
        public MosaicHttp(string host, int port) : base(host, port) { }

        public IObservable<List<MosaicEvent>> SearchMosaics(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["mosaics"], queryModel)))
                .Select(m => new ObjectComposer(TypeSerializationCatalog.CustomTypes).FilterEvents<MosaicEvent>(m, "data"));
        }

        public IObservable<MosaicEvent> GetMosaic(string mosaicId)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["mosaics", mosaicId])))
                .Select(r => { return new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<MosaicEvent>(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<List<MosaicEvent>> GetMosaics(List<string> mosaicIds)
        {     
            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["mosaics"]), new StringContent(JsonSerializer.Serialize(new MosaicIds() { mosaicIds = mosaicIds }), Encoding.UTF8, "application/json")))
                .Select(r => { return new ObjectComposer(TypeSerializationCatalog.CustomTypes).FilterEvents<MosaicEvent>(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<MerkleRoot> GetMosaicMerkle(string mosaicId)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["mosaics", mosaicId, "merkle"])))
                .Select(r => { return new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<MerkleRoot>(OverrideEnsureSuccessStatusCode(r)); });

        }

        public IObservable<List<MosaicRestrictionData>> SearchMosaicRestrictions(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["restrictions", "mosaic"], queryModel)))
               .Select(r => { return new ObjectComposer(TypeSerializationCatalog.CustomTypes).FilterEvents<MosaicRestrictionData>(OverrideEnsureSuccessStatusCode(r), "data"); });
        }

        public IObservable<MosaicRestrictionData> GetMosaicRestriction(string compositeHash)
        { 
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["restrictions", "mosaic", compositeHash])))
                .Select(r => { return new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<MosaicRestrictionData>(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<MerkleRoot> GetMosaicRestrictionMerkle(string compositeHash)
        { 
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["restrictions", "mosaic", compositeHash, "merkle"])))
                .Select(new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<MerkleRoot>);
        }
    }
}
