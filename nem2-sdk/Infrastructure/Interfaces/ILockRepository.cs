using io.nem2.sdk.Infrastructure.Responses;

namespace io.nem2.sdk.Infrastructure.Interfaces
{
    public interface ILockRepository
    {
        IObservable<ExtendedHttpResponseMessege<Datum<HashLockEvent>>> SearchHashLocks(QueryModel queryModel);
        IObservable<ExtendedHttpResponseMessege<HashLockEvent>> GetHashLockInfo(string hash);
        IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetHashLockMerkleInfo(string hash);

        IObservable<ExtendedHttpResponseMessege<Datum<SecretLockEvent>>> SearchSecretLocks(QueryModel queryModel);
        IObservable<ExtendedHttpResponseMessege<SecretLockEvent>> GetSecretLock(string hash);
        IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetSecretLockMerkle(string hash);
    }
}
