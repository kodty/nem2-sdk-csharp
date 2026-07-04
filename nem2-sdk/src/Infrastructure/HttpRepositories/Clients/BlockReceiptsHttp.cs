using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.Buffers.Model;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Clients
{
    public class BlockReceiptsHttp : HttpRouter, IBlockReceiptsRepository
    {
        public BlockReceiptsHttp(string host, int port) : base(host, port) { }

        public IObservable<ExtendedHttpResponseMessege<Datum<ReceiptDatum>>> SearchTransactionStatements(QueryModel queryModel)
            => HttpGetAsync<Datum<ReceiptDatum>>(queryModel, ["statements", "transaction"]);    

        public IObservable<ExtendedHttpResponseMessege<Datum<AddressDatum>>> GetAddressStatements(QueryModel queryModel)
            => HttpGetAsync<Datum<AddressDatum>>(queryModel, ["statements", "resolutions", "address"]);
        
        public IObservable<ExtendedHttpResponseMessege<Datum<MosaicDatum>>> GetMosaicStatements(QueryModel queryModel)
            => HttpGetAsync<Datum<MosaicDatum>>(queryModel, ["statements", "resolutions", "mosaic"]);
    }
}
