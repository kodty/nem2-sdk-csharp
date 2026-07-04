using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Clients
{
    public class BlockchainHttp : HttpRouter, IBlockchainRepository
    {
        public BlockchainHttp(string host, int port) : base(host, port) { }

        public IObservable<ExtendedHttpResponseMessege<ExtendedBlocksInfoData>> SearchBlocks(QueryModel queryModel)
            => HttpGetAsync<ExtendedBlocksInfoData>(queryModel, ["blocks"]);

        public IObservable<ExtendedHttpResponseMessege<ExtendedBlockInfo>> GetBlock(ulong height)
            => HttpGetAsync<ExtendedBlockInfo>(["blocks", height]);
        
        public IObservable<ExtendedHttpResponseMessege<Merkle_Path>> GetBlockTransactionMerkle(ulong height, string hash)
            => HttpGetAsync<Merkle_Path>(["blocks", height, "transactions", hash, "merkle"]);

        public IObservable<ExtendedHttpResponseMessege<Merkle_Path>> GetBlockRecieptMerkle(ulong height, string hash)
            => HttpGetAsync<Merkle_Path>(["blocks", height, "statements", hash, "merkle"]);
 
        public IObservable<ExtendedHttpResponseMessege<BlockchainInfo>> GetBlockchainInfo()
            => HttpGetAsync<BlockchainInfo>(["chain", "info"]);
    }
}
