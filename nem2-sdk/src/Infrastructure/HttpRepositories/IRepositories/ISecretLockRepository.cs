using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories
{
    internal interface ISecretLockRepository
    {
        IObservable<List<SecretLockEvent>> SearchSecretLocks(QueryModel queryModel);
        IObservable<SecretLockEvent> GetSecretLock(string hash);
        IObservable<MerkleRoot> GetSecretLockMerkle(string hash);
    }
}
