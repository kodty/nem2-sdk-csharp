using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Export;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Model2;
using System.Reactive.Linq;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories
{
    public class HashLockHttp : HttpRouter, IHashLockRepository
    {
        public HashLockHttp(string host, int port) : base(host, port) { }

        public IObservable<List<HashLockEvent>> SearchHashLocks(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["lock", "hash"], queryModel)))
              .Select(r => { return new ObjectComposer(TypeSerializationCatalog.CustomTypes).FilterEvents<HashLockEvent>(OverrideEnsureSuccessStatusCode(r), "data"); });
        }
        public IObservable<HashLockEvent> GetHashLockInfo(string hash)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["lock", "hash", hash])))
              .Select(r => { return new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<HashLockEvent>(OverrideEnsureSuccessStatusCode(r)); });
        }
        public IObservable<MerkleRoot> GetHashLockMerkleInfo(string hash)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["lock", "hash", hash, "merkle"])))
              .Select(r => { return new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<MerkleRoot>(OverrideEnsureSuccessStatusCode(r)); });
        }
    }
}
