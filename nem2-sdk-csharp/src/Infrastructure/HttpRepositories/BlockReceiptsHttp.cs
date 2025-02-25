using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.Buffers.Model;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using Newtonsoft.Json;
using System.Reactive.Linq;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories
{
    internal class BlockReceiptsHttp : HttpRouter, IBlockReceiptsRepository
    {
        public BlockReceiptsHttp(string host, int port) : base(host, port) { }

        public IObservable<TransactionStatements> SearchTransactionStatements(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(Host + ":" + Port + "/statements/transaction"))
              .Select(JsonConvert.DeserializeObject<TransactionStatements>);
        }

        public IObservable<AddressStatements> GetAddressStatements(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(Host + ":" + Port + "/statements/resolutions/address"))
              .Select(JsonConvert.DeserializeObject<AddressStatements>);
        }

        public IObservable<MosaicStatements> GetMosaicStatements(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(Host + ":" + Port + "/statements/resolutions/mosaic"))
              .Select(JsonConvert.DeserializeObject<MosaicStatements>);
        }
    }
}
