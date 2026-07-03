using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Reactive.Linq;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Clients
{
    public class NetworkHttp : HttpRouter, INetworkRepository
    {
        public NetworkHttp(string host, int port) : base(host, port) { }

        public IObservable<ExtendedHttpResponseMessege<NetworkInfo>> GetNetwork()
        {
            return HttpGetAsync<NetworkInfo>(["network"]);
        }

        public IObservable<ExtendedHttpResponseMessege<NetworkRentalFees>> GetRentalFees()
        {
            return HttpGetAsync<NetworkRentalFees>(["network", "fees", "rental"]);          
        }

        public IObservable<ExtendedHttpResponseMessege<NetworkTransactionFees>> GetTransactionFees()
        {
            return HttpGetAsync<NetworkTransactionFees>(["network", "fees", "transaction"]);           
        }

        public IObservable<ExtendedHttpResponseMessege<NetworkProperties>> GetNetworkProperties()
        {
            return HttpGetAsync<NetworkProperties>(["network", "properties"]);
        }
    }
}
