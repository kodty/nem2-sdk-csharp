using io.nem2.sdk.src.Infrastructure.HttpExtension;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories
{
    public interface IHashLockRepository
    {
        IObservable<ExtendedHttpResponseMessege<List<HashLockEvent>>> SearchHashLocks(QueryModel queryModel);
        IObservable<ExtendedHttpResponseMessege<HashLockEvent>> GetHashLockInfo(string hash);
        IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetHashLockMerkleInfo(string hash);
    }
}
