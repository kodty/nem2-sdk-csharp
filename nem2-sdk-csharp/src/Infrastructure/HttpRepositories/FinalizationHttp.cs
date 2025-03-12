using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Infrastructure.Mapping;
using System.Reactive.Linq;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories
{
    public class FinalizationHttp : HttpRouter, IFinalizationRepository
    {
        public FinalizationHttp(string host, int port) :base(host, port) { }

        public IObservable<FinalizationProof> GetFinalizationProofByHeight(ulong height)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["finalization", "proof", "height", height])))
                .Select(ObjectComposer.GenerateObject<FinalizationProof>);
        }

        public IObservable<FinalizationProof> GetFinalizationProofByEpoch(ulong epoch)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["finalization", "proof", "epoch", epoch])))
                .Select(ObjectComposer.GenerateObject<FinalizationProof>);
        }
    }
}
