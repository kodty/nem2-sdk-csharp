using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.Buffers.Model;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.Mapping;
using System.Reactive.Linq;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories
{
    public class BlockReceiptsHttp : HttpRouter, IBlockReceiptsRepository
    {
        public BlockReceiptsHttp(string host, int port) : base(host, port) { }

        public IObservable<List<ReceiptDatum>> SearchTransactionStatements(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["statements", "transaction"])))
              .Select(a => ResponseFilters<ReceiptDatum>.FilterEvents(a, "data"));
        }

        public IObservable<List<AddressDatum>> GetAddressStatements(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["statements", "resolutions", "address"])))
              .Select(a => ResponseFilters<AddressDatum>.FilterEvents(a, "data"));
        }

        public IObservable<List<MosaicDatum>> GetMosaicStatements(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["statements", "resolutions", "mosaic"])))
              .Select(m => ResponseFilters<MosaicDatum>.FilterEvents(m, "data"));
        }
    }
}
