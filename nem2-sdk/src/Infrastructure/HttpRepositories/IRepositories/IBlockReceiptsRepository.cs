using io.nem2.sdk.src.Infrastructure.Buffers.Model;
using io.nem2.sdk.src.Infrastructure.HttpExtension;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories
{
    public interface IBlockReceiptsRepository
    {
        IObservable<ExtendedHttpResponseMessege<Datum<ReceiptDatum>>> SearchTransactionStatements(QueryModel queryModel);
        IObservable<ExtendedHttpResponseMessege<Datum<AddressDatum>>> GetAddressStatements(QueryModel queryModel);
        IObservable<ExtendedHttpResponseMessege<Datum<MosaicDatum>>> GetMosaicStatements(QueryModel queryModel);
    }
}
