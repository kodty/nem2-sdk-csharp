using io.nem2.sdk.Infrastructure.Interfaces;
using io.nem2.sdk.Infrastructure.Responses;
using io.nem2.sdk.Model.Accounts;

namespace io.nem2.sdk.Infrastructure.HttpClients
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
