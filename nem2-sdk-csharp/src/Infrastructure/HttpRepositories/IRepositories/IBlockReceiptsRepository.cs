using io.nem2.sdk.src.Infrastructure.Buffers.Model;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories
{
    public interface IBlockReceiptsRepository
    {
        IObservable<TransactionStatements> SearchTransactionStatements(QueryModel queryModel);
        IObservable<AddressStatements> GetAddressStatements(QueryModel queryModel);
        IObservable<MosaicStatements> GetMosaicStatements(QueryModel queryModel);
    }
}
