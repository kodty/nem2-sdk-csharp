using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpExtension;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Reactive.Linq;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories
{
    public class FinalizationHttp : HttpRouter, IFinalizationRepository
    {
        public FinalizationHttp(string host, int port) :base(host, port) { }

        public IObservable<ExtendedHttpResponseMessege<FinalizationProof>> GetFinalizationProofByHeight(ulong height)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["finalization", "proof", "height", height])))
                 .Select(FormResponse<FinalizationProof>);
        }

        public IObservable<ExtendedHttpResponseMessege<FinalizationProof>> GetFinalizationProofByEpoch(ulong epoch)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["finalization", "proof", "epoch", epoch])))
                 .Select(FormResponse<FinalizationProof>);
        }
    }
}
