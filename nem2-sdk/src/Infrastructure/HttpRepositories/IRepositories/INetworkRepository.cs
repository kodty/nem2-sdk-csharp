using io.nem2.sdk.src.Infrastructure.HttpExtension;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories
{
    interface INetworkRepository
    {
        IObservable<ExtendedHttpResponseMessege<NetworkInfo>> GetNetwork();
        IObservable<ExtendedHttpResponseMessege<NetworkRentalFees>> GetRentalFees();
        IObservable<ExtendedHttpResponseMessege<NetworkTransactionFees>> GetTransactionFees();
        IObservable<ExtendedHttpResponseMessege<NetworkProperties>> GetNetworkProperties();
    }
}
