using io.nem2.sdk.Infrastructure.Interfaces;
using io.nem2.sdk.Infrastructure.Responses;

namespace io.nem2.sdk.Infrastructure.HttpClients
{
    public class BlockchainHttp : HttpRouter, IBlockchainRepository
    {
        public BlockchainHttp(string host, int port) : base(host, port) { }

        public IObservable<ExtendedHttpResponseMessege<ExtendedBlocksInfoData>> SearchBlocks(QueryModel queryModel)
            => HttpGetAsync<ExtendedBlocksInfoData>(queryModel, ["blocks"]);

        public IObservable<ExtendedHttpResponseMessege<ExtendedBlockInfo>> GetBlock(ulong height)
            => HttpGetAsync<ExtendedBlockInfo>(["blocks", height]);
        
        public IObservable<ExtendedHttpResponseMessege<Merkle_Path>> GetBlockTransactionMerkle(ulong height, string hash)
            => HttpGetAsync<Merkle_Path>(["blocks", height, "transactions", hash, "merkle"]);

        public IObservable<ExtendedHttpResponseMessege<Merkle_Path>> GetBlockRecieptMerkle(ulong height, string hash)
            => HttpGetAsync<Merkle_Path>(["blocks", height, "statements", hash, "merkle"]);
 
        public IObservable<ExtendedHttpResponseMessege<BlockchainInfo>> GetBlockchainInfo()
            => HttpGetAsync<BlockchainInfo>(["chain", "info"]);

        public IObservable<ExtendedHttpResponseMessege<Datum<ReceiptDatum>>> SearchTransactionStatements(QueryModel queryModel)
             => HttpGetAsync<Datum<ReceiptDatum>>(queryModel, ["statements", "transaction"]);

        public IObservable<ExtendedHttpResponseMessege<Datum<AddressDatum>>> GetAddressStatements(QueryModel queryModel)
            => HttpGetAsync<Datum<AddressDatum>>(queryModel, ["statements", "resolutions", "address"]);

        public IObservable<ExtendedHttpResponseMessege<Datum<MosaicDatum>>> GetMosaicStatements(QueryModel queryModel)
            => HttpGetAsync<Datum<MosaicDatum>>(queryModel, ["statements", "resolutions", "mosaic"]);

        public IObservable<ExtendedHttpResponseMessege<FinalizationProof>> GetFinalizationProofByHeight(ulong height)
            => HttpGetAsync<FinalizationProof>(["finalization", "proof", "height", height]);

        public IObservable<ExtendedHttpResponseMessege<FinalizationProof>> GetFinalizationProofByEpoch(ulong epoch)
            => HttpGetAsync<FinalizationProof>(["finalization", "proof", "epoch", epoch]);
    }
}
