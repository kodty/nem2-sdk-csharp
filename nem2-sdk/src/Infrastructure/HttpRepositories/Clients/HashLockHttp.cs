using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Clients
{
    public class HashLockHttp : HttpRouter, IHashLockRepository
    {
        public HashLockHttp(string host, int port) : base(host, port) { }

        public IObservable<ExtendedHttpResponseMessege<Datum<HashLockEvent>>> SearchHashLocks(QueryModel queryModel)
            => HttpGetAsync<Datum<HashLockEvent>>(queryModel, ["lock", "hash"]);
        
        public IObservable<ExtendedHttpResponseMessege<HashLockEvent>> GetHashLockInfo(string hash)
            => HttpGetAsync<HashLockEvent>(["lock", "hash", hash]);

        public IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetHashLockMerkleInfo(string hash)
            => HttpGetAsync<MerkleRoot>(["lock", "hash", hash, "merkle"]);             
    }
}
