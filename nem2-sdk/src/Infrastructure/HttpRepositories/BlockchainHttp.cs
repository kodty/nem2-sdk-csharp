using io.nem2.sdk.src.Infrastructure.HttpExtension;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;


namespace io.nem2.sdk.Infrastructure.HttpRepositories
{

    public class BlockchainHttp : HttpRouter, IBlockchainRepository
    {
        public BlockchainHttp(string host, int port) 
            : base(host, port) { }

        public IObservable<ExtendedHttpResponseMessege<ExtendedBlocksInfoData>> SearchBlocks(QueryModel queryModel)
        {
            return HttpGetAsync<ExtendedBlocksInfoData>(queryModel, ["blocks"]);
        }

        public IObservable<ExtendedHttpResponseMessege<ExtendedBlockInfo>> GetBlock(ulong height)
        {
            return HttpGetAsync<ExtendedBlockInfo>(["blocks", height]);
        }

        public IObservable<ExtendedHttpResponseMessege<Merkle_Path>> GetBlockTransactionMerkle(ulong height, string hash)
        {
            return HttpGetAsync<Merkle_Path>(["blocks", height, "transactions", hash, "merkle"]);
        }

        public IObservable<ExtendedHttpResponseMessege<Merkle_Path>> GetBlockRecieptMerkle(ulong height, string hash)
        {
            return HttpGetAsync<Merkle_Path>(["blocks", height, "statements", hash, "merkle"]);
        }
 
        public IObservable<ExtendedHttpResponseMessege<BlockchainInfo>> GetBlockchainInfo()
        {
            return HttpGetAsync<BlockchainInfo>(["chain", "info"]);
        }
    }
}
