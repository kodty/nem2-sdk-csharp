using io.nem2.sdk.src.Infrastructure.HttpExtension;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories
{
    interface IBlockchainRepository
    {
        IObservable<ExtendedHttpResponseMessege<ExtendedBlocksInfoData>> SearchBlocks(QueryModel queryModel);
        IObservable<ExtendedHttpResponseMessege<ExtendedBlockInfo>> GetBlock(ulong height);
        IObservable<ExtendedHttpResponseMessege<List<MerklePath>>> GetBlockTransactionMerkle(ulong height, string hash);
        IObservable<ExtendedHttpResponseMessege<List<MerklePath>>> GetBlockRecieptMerkle(ulong height, string hash);
    }
}
