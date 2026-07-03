using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Reactive.Linq;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Clients
{
    public class HashLockHttp : HttpRouter, IHashLockRepository
    {
        public HashLockHttp(string host, int port) : base(host, port) { }

        public IObservable<ExtendedHttpResponseMessege<Datum<HashLockEvent>>> SearchHashLocks(QueryModel queryModel)
        {
            return HttpGetAsync<Datum<HashLockEvent>>(queryModel, ["lock", "hash"]);
        }
        public IObservable<ExtendedHttpResponseMessege<HashLockEvent>> GetHashLockInfo(string hash)
        {
            return HttpGetAsync<HashLockEvent>(["lock", "hash", hash]);
        }
        public IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetHashLockMerkleInfo(string hash)
        {
            return HttpGetAsync<MerkleRoot>(["lock", "hash", hash, "merkle"]);             
        }
    }
}
