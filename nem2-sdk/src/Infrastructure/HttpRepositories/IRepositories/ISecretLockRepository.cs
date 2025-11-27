using io.nem2.sdk.src.Infrastructure.HttpExtension;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories
{
    internal interface ISecretLockRepository
    {
        IObservable<ExtendedHttpResponseMessege<Datum<SecretLockEvent>>> SearchSecretLocks(QueryModel queryModel);
        IObservable<ExtendedHttpResponseMessege<SecretLockEvent>> GetSecretLock(string hash);
        IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetSecretLockMerkle(string hash);
    }
}
