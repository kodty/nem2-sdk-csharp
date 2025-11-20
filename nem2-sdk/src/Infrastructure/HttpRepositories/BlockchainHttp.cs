
using System.Reactive.Linq;
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

        public IObservable<ExtendedHttpResponseMessege<List<ExtendedBlockInfo>>> SearchBlocks(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["blocks"], queryModel)))
                  .Select(r => { return FormListResponse<ExtendedBlockInfo>(r, "data"); });
        }

        public IObservable<ExtendedHttpResponseMessege<ExtendedBlockInfo>> GetBlock(ulong height)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["blocks", height])))
                 .Select(FormResponse<ExtendedBlockInfo>);
        }

        public IObservable<ExtendedHttpResponseMessege<List<MerklePath>>> GetBlockTransactionMerkle(ulong height, string hash)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["blocks", height, "transactions", hash, "merkle"])))
                  .Select(r => { return FormListResponse<MerklePath>(r, "merklePath"); });
        }

        public IObservable<ExtendedHttpResponseMessege<List<MerklePath>>> GetBlockRecieptMerkle(ulong height, string hash)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["blocks", height, "reciepts", hash, "merkle"])))
               .Select(r => { return FormListResponse<MerklePath>(r, "merklePath"); });
        }
 
        public IObservable<ExtendedHttpResponseMessege<BlockchainInfo>> GetBlockchainInfo()
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["chain", "info"])))
               .Select(FormResponse<BlockchainInfo>);
        }
    }
}
