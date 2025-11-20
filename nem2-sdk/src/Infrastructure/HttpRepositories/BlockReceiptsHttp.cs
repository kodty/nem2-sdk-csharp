using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.Buffers.Model;
using io.nem2.sdk.src.Infrastructure.HttpExtension;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Reactive.Linq;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories
{
    public class BlockReceiptsHttp : HttpRouter, IBlockReceiptsRepository
    {
        public BlockReceiptsHttp(string host, int port) : base(host, port) { }

        public IObservable<ExtendedHttpResponseMessege<List<ReceiptDatum>>> SearchTransactionStatements(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["statements", "transaction"])))
              .Select(r => { return FormListResponse<ReceiptDatum>(r, "data"); });
        }

        public IObservable<ExtendedHttpResponseMessege<List<AddressDatum>>> GetAddressStatements(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["statements", "resolutions", "address"])))
              .Select(r => { return FormListResponse<AddressDatum>(r, "data"); });
        }

        public IObservable<ExtendedHttpResponseMessege<List<MosaicDatum>>> GetMosaicStatements(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["statements", "resolutions", "mosaic"])))
              .Select(r => { return FormListResponse<MosaicDatum>(r, "data"); });
        }
    }
}
