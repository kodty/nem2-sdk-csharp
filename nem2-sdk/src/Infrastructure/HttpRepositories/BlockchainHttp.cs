
using System.Reactive.Linq;
using io.nem2.sdk.src.Export;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Model2;


namespace io.nem2.sdk.Infrastructure.HttpRepositories
{

    public class BlockchainHttp : HttpRouter, IBlockchainRepository
    {
        public BlockchainHttp(string host, int port) 
            : base(host, port) { }

        public IObservable<List<ExtendedBlockInfo>> SearchBlocks(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["blocks"], queryModel)))
                 .Select(r => { return Composer.FilterEvents<ExtendedBlockInfo>(OverrideEnsureSuccessStatusCode(r), "data"); });
        }

        public IObservable<ExtendedBlockInfo> GetBlock(ulong height)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["blocks", height])))
                .Select(r => { return Composer.GenerateObject<ExtendedBlockInfo>(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<List<MerklePath>> GetBlockTransactionMerkle(ulong height, string hash)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["blocks", height, "transactions", hash, "merkle"])))
                 .Select(r => { return Composer.FilterEvents<MerklePath>(OverrideEnsureSuccessStatusCode(r), "merklePath"); });
        }

        public IObservable<List<MerklePath>> GetBlockRecieptMerkle(ulong height, string hash)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["blocks", height, "reciepts", hash, "merkle"])))
              .Select(r => { return Composer.FilterEvents<MerklePath>(OverrideEnsureSuccessStatusCode(r), "merklePath"); });
        }
 
        public IObservable<BlockchainInfo> GetBlockchainInfo()
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["chain", "info"])))
                .Select(r => { return Composer.GenerateObject<BlockchainInfo>(OverrideEnsureSuccessStatusCode(r)); });
        }
    }
}
