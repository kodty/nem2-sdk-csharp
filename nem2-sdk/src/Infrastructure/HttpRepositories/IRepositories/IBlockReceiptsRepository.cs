using io.nem2.sdk.src.Infrastructure.Buffers.Model;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories
{
    public interface IBlockReceiptsRepository
    {
        IObservable<List<ReceiptDatum>> SearchTransactionStatements(QueryModel queryModel);
        IObservable<List<AddressDatum>> GetAddressStatements(QueryModel queryModel);
        IObservable<List<MosaicDatum>> GetMosaicStatements(QueryModel queryModel);
    }
}
