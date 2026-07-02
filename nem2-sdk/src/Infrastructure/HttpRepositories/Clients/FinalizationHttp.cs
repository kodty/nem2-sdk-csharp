using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Clients
{
    public class FinalizationHttp : HttpRouter, IFinalizationRepository
    {
        public FinalizationHttp(string host, int port) :base(host, port) { }

        public IObservable<ExtendedHttpResponseMessege<FinalizationProof>> GetFinalizationProofByHeight(ulong height)
        {
            return HttpGetAsync<FinalizationProof>(["finalization", "proof", "height", height]);
        }

        public IObservable<ExtendedHttpResponseMessege<FinalizationProof>> GetFinalizationProofByEpoch(ulong epoch)
        {
            return HttpGetAsync<FinalizationProof>(["finalization", "proof", "epoch", epoch]);
        }
    }
}
