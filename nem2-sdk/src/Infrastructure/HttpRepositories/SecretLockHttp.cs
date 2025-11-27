using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpExtension;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Reactive.Linq;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories
{
    public class SecretLockHttp : HttpRouter, ISecretLockRepository
    {
        public SecretLockHttp(string host, int port) : base(host, port) { }
        
        public IObservable<ExtendedHttpResponseMessege<Datum<SecretLockEvent>>> SearchSecretLocks(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri([ "lock", "secret"])))
               .Select(FormResponse<Datum<SecretLockEvent>>);
        }

        public IObservable<ExtendedHttpResponseMessege<SecretLockEvent>> GetSecretLock(string hash)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["lock", "secret", hash])))
              .Select(FormResponse<SecretLockEvent>);
        }

        public IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetSecretLockMerkle(string hash)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["lock", "secret", hash, "merkle"])))
              .Select(FormResponse<MerkleRoot>);
        }
    }
}
