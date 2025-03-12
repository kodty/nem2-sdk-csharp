using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Infrastructure.Mapping;
using System.Reactive.Linq;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories
{
    public class HashLockHttp : HttpRouter, IHashLockRepository
    {
        public HashLockHttp(string host, int port) : base(host, port) { }

        public IObservable<List<HashLockEvent>> SearchHashLocks(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["lock", "hash"])))
              .Select(h => ResponseFilters<HashLockEvent>.FilterEvents(h, "data"));
        }
        public IObservable<HashLockEvent> GetHashLockInfo(string hash)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["lock", "hash", hash])))
              .Select(ObjectComposer.GenerateObject<HashLockEvent>);
        }
        public IObservable<MerkleRoot> GetHashLockMerkleInfo(string hash)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["lock", "hash", hash, "merkle"])))
              .Select(ObjectComposer.GenerateObject<MerkleRoot>);
        }
    }
}
