using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories
{
    interface IBlockchainRepository
    {
        IObservable<List<ExtendedBlockInfo>> SearchBlocks(QueryModel queryModel);
        IObservable<ExtendedBlockInfo> GetBlock(ulong height);
        IObservable<List<MerklePath>> GetBlockTransactionMerkle(ulong height, string hash);
        IObservable<List<MerklePath>> GetBlockRecieptMerkle(ulong height, string hash);
    }
}
