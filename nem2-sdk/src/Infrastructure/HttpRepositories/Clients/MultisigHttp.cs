using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Model.Accounts;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Clients
{
    public class MultisigHttp : HttpRouter, IMultisigRepository
    {
        public MultisigHttp(string host, int port) : base(host, port) { }

        public IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetMultisigMerkleInfo(string address)
            => HttpGetAsync<MerkleRoot>(["accounts", address, "multisig", "merkle"]);

        public IObservable<ExtendedHttpResponseMessege<MultisigAccountInfo>> GetMultisigInfo(string address)
            => HttpGetAsync<MultisigAccountInfo>(["accounts", address, "multisig", "merkle"]);

        public IObservable<ExtendedHttpResponseMessege<MultisigAccountGraphInfo>> GetMultisigGraphInfo(string address)
            => HttpGetAsync<MultisigAccountGraphInfo>(["accounts", address, "multisig", "graph"]);
    }
}
