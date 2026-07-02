using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.Buffers.Model;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Reactive.Linq;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Clients
{
    public class BlockReceiptsHttp : HttpRouter, IBlockReceiptsRepository
    {
        public BlockReceiptsHttp(string host, int port) : base(host, port) { }

        public IObservable<ExtendedHttpResponseMessege<Datum<ReceiptDatum>>> SearchTransactionStatements(QueryModel queryModel)
        {
            return HttpGetAsync<Datum<ReceiptDatum>>(queryModel, ["statements", "transaction"]);    
        }

        public IObservable<ExtendedHttpResponseMessege<Datum<AddressDatum>>> GetAddressStatements(QueryModel queryModel)
        {
            return HttpGetAsync<Datum<AddressDatum>>(queryModel, ["statements", "resolutions", "address"]);
        }

        public IObservable<ExtendedHttpResponseMessege<Datum<MosaicDatum>>> GetMosaicStatements(QueryModel queryModel)
        {
            return HttpGetAsync<Datum<MosaicDatum>>(queryModel, ["statements", "resolutions", "mosaic"]);
        }
    }
}
