using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Infrastructure.Mapping;
using System.Reactive.Linq;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories
{
    public class SecretLockHttp : HttpRouter, ISecretLockRepository
    {
        public SecretLockHttp(string host, int port) : base(host, port) { }
        
        public IObservable<List<SecretLockEvent>> SearchSecretLocks(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri([ "lock", "secret"])))
              .Select(s => ResponseFilters<SecretLockEvent>.FilterEvents(s, "data"));
        }

        public IObservable<SecretLockEvent> GetSecretLock(string hash)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["lock", "secret", hash])))
              .Select(ObjectComposer.GenerateObject<SecretLockEvent>);
        }

        public IObservable<MerkleRoot> GetSecretLockMerkle(string hash)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["lock", "secret", hash, "merkle"])))
              .Select(ObjectComposer.GenerateObject<MerkleRoot>);
        }
    }
}
