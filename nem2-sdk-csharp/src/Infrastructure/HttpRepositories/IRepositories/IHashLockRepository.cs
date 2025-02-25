using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories
{
    public interface IHashLockRepository
    {
        IObservable<List<HashLockEvent>> SearchHashLocks(QueryModel queryModel);
        IObservable<HashLockEvent> GetHashLockInfo(string hash);
        IObservable<MerkleRoot> GetHashLockMerkleInfo(string hash);
    }
}
