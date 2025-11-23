using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpExtension;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Reactive.Linq;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories
{
    public class HashLockHttp : HttpRouter, IHashLockRepository
    {
        public HashLockHttp(string host, int port) : base(host, port) { }

        public IObservable<ExtendedHttpResponseMessege<List<HashLockEvent>>> SearchHashLocks(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["lock", "hash"], queryModel)))
              .Select(r => { return FormListResponse<HashLockEvent>(r, "data"); });
        }
        public IObservable<ExtendedHttpResponseMessege<HashLockEvent>> GetHashLockInfo(string hash)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["lock", "hash", hash])))
              .Select(FormResponse<HashLockEvent>);
        }
        public IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetHashLockMerkleInfo(string hash)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["lock", "hash", hash, "merkle"])))
               .Select(FormResponse<MerkleRoot>);
        }
    }
}
