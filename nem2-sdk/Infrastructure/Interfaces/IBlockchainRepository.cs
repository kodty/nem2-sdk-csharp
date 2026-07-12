using io.nem2.sdk.Infrastructure.Responses;

namespace io.nem2.sdk.Infrastructure.Interfaces
{
    interface IBlockchainRepository
    {
        IObservable<ExtendedHttpResponseMessege<ExtendedBlocksInfoData>> SearchBlocks(QueryModel queryModel);
        IObservable<ExtendedHttpResponseMessege<ExtendedBlockInfo>> GetBlock(ulong height);
        IObservable<ExtendedHttpResponseMessege<Merkle_Path>> GetBlockTransactionMerkle(ulong height, string hash);
        IObservable<ExtendedHttpResponseMessege<Merkle_Path>> GetBlockRecieptMerkle(ulong height, string hash);
        IObservable<ExtendedHttpResponseMessege<Datum<ReceiptDatum>>> SearchTransactionStatements(QueryModel queryModel);
        IObservable<ExtendedHttpResponseMessege<Datum<AddressDatum>>> GetAddressStatements(QueryModel queryModel);
        IObservable<ExtendedHttpResponseMessege<Datum<MosaicDatum>>> GetMosaicStatements(QueryModel queryModel);
        IObservable<ExtendedHttpResponseMessege<FinalizationProof>> GetFinalizationProofByHeight(ulong height);
        IObservable<ExtendedHttpResponseMessege<FinalizationProof>> GetFinalizationProofByEpoch(ulong epoch);
    }
}
