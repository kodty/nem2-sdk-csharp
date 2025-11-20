using io.nem2.sdk.src.Infrastructure.HttpExtension;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories
{
    public interface IFinalizationRepository
    {
        IObservable<ExtendedHttpResponseMessege<FinalizationProof>> GetFinalizationProofByHeight(ulong height);
        IObservable<ExtendedHttpResponseMessege<FinalizationProof>> GetFinalizationProofByEpoch(ulong epoch);
    }
}
