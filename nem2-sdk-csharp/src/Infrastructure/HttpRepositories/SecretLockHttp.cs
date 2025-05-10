using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Export;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Reactive.Linq;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories
{
    public class SecretLockHttp : HttpRouter, ISecretLockRepository
    {
        public SecretLockHttp(string host, int port) : base(host, port) { }
        
        public IObservable<List<SecretLockEvent>> SearchSecretLocks(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri([ "lock", "secret"])))
              .Select(r => { return ResponseFilters<SecretLockEvent>.FilterEvents(OverrideEnsureSuccessStatusCode(r), "data"); });
        }

        public IObservable<SecretLockEvent> GetSecretLock(string hash)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["lock", "secret", hash])))
              .Select(r => { return ObjectComposer.GenerateObject<SecretLockEvent>(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<MerkleRoot> GetSecretLockMerkle(string hash)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["lock", "secret", hash, "merkle"])))
              .Select(r => { return ObjectComposer.GenerateObject<MerkleRoot>(OverrideEnsureSuccessStatusCode(r)); });
        }
    }
}
