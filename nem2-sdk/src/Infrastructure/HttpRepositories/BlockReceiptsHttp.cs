using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.Buffers.Model;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using System.Reactive.Linq;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories
{
    public class BlockReceiptsHttp : HttpRouter, IBlockReceiptsRepository
    {
        public BlockReceiptsHttp(string host, int port) : base(host, port) { }

        public IObservable<List<ReceiptDatum>> SearchTransactionStatements(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["statements", "transaction"])))
              .Select(r => { return Composer.FilterEvents<ReceiptDatum>(OverrideEnsureSuccessStatusCode(r), "data"); });
        }

        public IObservable<List<AddressDatum>> GetAddressStatements(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["statements", "resolutions", "address"])))
              .Select(r => { return Composer.FilterEvents<AddressDatum>(OverrideEnsureSuccessStatusCode(r), "data"); });
        }

        public IObservable<List<MosaicDatum>> GetMosaicStatements(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["statements", "resolutions", "mosaic"])))
              .Select(r => { return Composer.FilterEvents<MosaicDatum>(OverrideEnsureSuccessStatusCode(r), "data"); });
        }
    }
}
