using io.nem2.sdk.src.Infrastructure.Buffers.Model;
using io.nem2.sdk.src.Infrastructure.HttpExtension;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories
{
    public interface IBlockReceiptsRepository
    {
        IObservable<ExtendedHttpResponseMessege<List<ReceiptDatum>>> SearchTransactionStatements(QueryModel queryModel);
        IObservable<ExtendedHttpResponseMessege<List<AddressDatum>>> GetAddressStatements(QueryModel queryModel);
        IObservable<ExtendedHttpResponseMessege<List<MosaicDatum>>> GetMosaicStatements(QueryModel queryModel);
    }
}
