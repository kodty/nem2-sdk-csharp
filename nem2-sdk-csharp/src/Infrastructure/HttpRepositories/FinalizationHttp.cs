using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Model2;
using io.nem2.sdk.src.Export;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Reactive.Linq;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories
{
    public class FinalizationHttp : HttpRouter, IFinalizationRepository
    {
        public FinalizationHttp(string host, int port) :base(host, port) { }

        public IObservable<FinalizationProof> GetFinalizationProofByHeight(ulong height)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["finalization", "proof", "height", height])))
                .Select(r => { return new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<FinalizationProof>(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<FinalizationProof> GetFinalizationProofByEpoch(ulong epoch)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["finalization", "proof", "epoch", epoch])))
                .Select(r => { return new ObjectComposer(TypeSerializationCatalog.CustomTypes).GenerateObject<FinalizationProof>(OverrideEnsureSuccessStatusCode(r)); });
        }
    }
}
