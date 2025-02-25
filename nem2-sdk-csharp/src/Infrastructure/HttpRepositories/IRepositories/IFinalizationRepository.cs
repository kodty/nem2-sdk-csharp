using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories
{
    public interface IFinalizationRepository
    {
        IObservable<FinalizationProof> GetFinalizationProofByHeight(ulong height);
        IObservable<FinalizationProof> GetFinalizationProofByEpoch(ulong epoch);
    }
}
