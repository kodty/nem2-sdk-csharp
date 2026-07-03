using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Clients
{
    public class SecretLockHttp : HttpRouter, ISecretLockRepository
    {
        public SecretLockHttp(string host, int port) : base(host, port) { }
        
        public IObservable<ExtendedHttpResponseMessege<Datum<SecretLockEvent>>> SearchSecretLocks(QueryModel queryModel)
        {
             return HttpGetAsync<Datum<SecretLockEvent>>(["lock", "secret"]);      
        }

        public IObservable<ExtendedHttpResponseMessege<SecretLockEvent>> GetSecretLock(string hash)
        {
            return HttpGetAsync<SecretLockEvent>(["lock", "secret", hash]); 
        }

        public IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetSecretLockMerkle(string hash)
        {
            return HttpGetAsync<MerkleRoot>(["lock", "secret", hash, "merkle"]);
        }
    }
}
