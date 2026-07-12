using io.nem2.sdk.Infrastructure.Interfaces;
using io.nem2.sdk.Infrastructure.Responses;

namespace io.nem2.sdk.Infrastructure.HttpClients
{
    public class LockHttp : HttpRouter, ILockRepository
    {
        public LockHttp(string host, int port) : base(host, port) { }

        public IObservable<ExtendedHttpResponseMessege<Datum<HashLockEvent>>> SearchHashLocks(QueryModel queryModel)
            => HttpGetAsync<Datum<HashLockEvent>>(queryModel, ["lock", "hash"]);
        
        public IObservable<ExtendedHttpResponseMessege<HashLockEvent>> GetHashLockInfo(string hash)
            => HttpGetAsync<HashLockEvent>(["lock", "hash", hash]);

        public IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetHashLockMerkleInfo(string hash)
            => HttpGetAsync<MerkleRoot>(["lock", "hash", hash, "merkle"]);

        public IObservable<ExtendedHttpResponseMessege<Datum<SecretLockEvent>>> SearchSecretLocks(QueryModel queryModel)
    => HttpGetAsync<Datum<SecretLockEvent>>(["lock", "secret"]);

        public IObservable<ExtendedHttpResponseMessege<SecretLockEvent>> GetSecretLock(string hash)
            => HttpGetAsync<SecretLockEvent>(["lock", "secret", hash]);

        public IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetSecretLockMerkle(string hash)
            => HttpGetAsync<MerkleRoot>(["lock", "secret", hash, "merkle"]);
    }
}
