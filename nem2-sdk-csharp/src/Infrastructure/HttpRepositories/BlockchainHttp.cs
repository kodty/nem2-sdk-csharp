
using System.Reactive.Linq;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Infrastructure.Mapping;


namespace io.nem2.sdk.Infrastructure.HttpRepositories
{

    public class BlockchainHttp : HttpRouter, IBlockchainRepository
    {
        public BlockchainHttp(string host, int port) 
            : base(host, port) { }

        public IObservable<List<BlockInfo>> SearchBlocks(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["blocks"], queryModel)))
                 .Select(r => { return ResponseFilters<BlockInfo>.FilterEvents(OverrideEnsureSuccessStatusCode(r), "data"); });
        }

        public IObservable<BlockInfo> GetBlock(ulong height)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["blocks", height])))
                .Select(r => { return ObjectComposer.GenerateObject<BlockInfo>(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<List<MerklePath>> GetBlockTransactionMerkle(ulong height, string hash)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["blocks", height, "transactions", hash, "merkle"])))
                .Select(r => { return ResponseFilters<MerklePath>.FilterEvents(OverrideEnsureSuccessStatusCode(r), "merklePath"); });
        }

        public IObservable<List<MerklePath>> GetBlockRecieptMerkle(ulong height, string hash)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["blocks", height, "reciepts", hash, "merkle"])))
              .Select(r => { return ResponseFilters<MerklePath>.FilterEvents(OverrideEnsureSuccessStatusCode(r), "merklePath"); });
        }
 
        public IObservable<BlockchainInfo> GetBlockchainInfo()
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["chain", "info"])))
                .Select(r => { return ObjectComposer.GenerateObject<BlockchainInfo>(OverrideEnsureSuccessStatusCode(r)); });
        }
    }
}
