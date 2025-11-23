using System.Reactive.Linq;
using System.Text;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Text.Json;
using io.nem2.sdk.src.Infrastructure.HttpExtension;

namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    public class MosaicHttp : HttpRouter, IMosaicRepository
    {
        public MosaicHttp(string host, int port) : base(host, port) { }

        public IObservable<ExtendedHttpResponseMessege<List<MosaicEvent>>> SearchMosaics(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["mosaics"], queryModel)))
                .Select(r => { return FormListResponse<MosaicEvent>(r, "data"); });
        }

        public IObservable<ExtendedHttpResponseMessege<MosaicEvent>> GetMosaic(string mosaicId)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["mosaics", mosaicId])))
                .Select(FormResponse<MosaicEvent>);
        }

        public IObservable<ExtendedHttpResponseMessege<List<MosaicEvent>>> GetMosaics(List<string> mosaicIds)
        {     
            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["mosaics"]), new StringContent(JsonSerializer.Serialize(new MosaicIds() { mosaicIds = mosaicIds }), Encoding.UTF8, "application/json")))
                .Select(r => { return FormListResponse<MosaicEvent>(r); });
        }

        public IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetMosaicMerkle(string mosaicId)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["mosaics", mosaicId, "merkle"])))
                .Select(FormResponse<MerkleRoot>);

        }

        public IObservable<ExtendedHttpResponseMessege<List<MosaicRestrictionData>>> SearchMosaicRestrictions(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["restrictions", "mosaic"], queryModel)))
               .Select(r => { return FormListResponse<MosaicRestrictionData>(r, "data"); });
        }

        public IObservable<ExtendedHttpResponseMessege<MosaicRestrictionData>> GetMosaicRestriction(string compositeHash)
        { 
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["restrictions", "mosaic", compositeHash])))
                .Select(FormResponse<MosaicRestrictionData>);
        }

        public IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetMosaicRestrictionMerkle(string compositeHash)
        { 
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["restrictions", "mosaic", compositeHash, "merkle"])))
                .Select(FormResponse<MerkleRoot>);
        }
    }
}
