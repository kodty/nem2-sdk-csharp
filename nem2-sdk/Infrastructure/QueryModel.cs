namespace io.nem2.sdk.Infrastructure
{
    public class QueryModel
    {
        internal DefineRequest Request { get; set; }

        internal readonly int[][] RequestParamMap =
        {
         /* 0 search accounts                         */ [(int)DefinedParams.pageSize, (int)DefinedParams.pageNumber, (int)DefinedParams.offset, (int)DefinedParams.order, (int)DefinedParams.orderBy, (int)DefinedParams.mosaicId],
         /* 1 search blocks                           */ [(int)DefinedParams.signerPublicKey, (int)DefinedParams.beneficiaryAddress, (int)DefinedParams.pageSize, (int)DefinedParams.pageNumber, (int)DefinedParams.offset, (int)DefinedParams.order, (int)DefinedParams.orderBy, (int)DefinedParams.fromTimestamp, (int)DefinedParams.toTimestamp],
         /* 2 search meta data                        */ [(int)DefinedParams.sourceAddress, (int)DefinedParams.targetAddress, (int)DefinedParams.scopedMetadataKey, (int)DefinedParams.targetId, (int)DefinedParams.metadataType, (int)DefinedParams.pageSize, (int)DefinedParams.pageNumber, (int)DefinedParams.offset, (int)DefinedParams.order],
         /* 3 search mosaics                          */ [(int)DefinedParams.ownerAddress, (int)DefinedParams.pageSize, (int)DefinedParams.pageNumber, (int)DefinedParams.offset, (int)DefinedParams.order],
         /* 4 search namespaces                       */ [(int)DefinedParams.ownerAddress, (int)DefinedParams.registrationType, (int)DefinedParams.level0, (int)DefinedParams.aliasType, (int)DefinedParams.pageSize, (int)DefinedParams.pageNumber, (int)DefinedParams.offset, (int)DefinedParams.order],
         /* 5 search mosaic restrictions              */ [(int)DefinedParams.mosaicId, (int)DefinedParams.entryType, (int)DefinedParams.targetAddress, (int)DefinedParams.pageSize, (int)DefinedParams.pageNumber, (int)DefinedParams.offset, (int)DefinedParams.order],
         /* 6 search account restrictions             */ [(int)DefinedParams.address, (int)DefinedParams.pageSize, (int)DefinedParams.pageNumber, (int)DefinedParams.offset, (int)DefinedParams.order],
         /* 7 search transaction statements           */ [(int)DefinedParams.height, (int)DefinedParams.fromHeight, (int)DefinedParams.toHeight, (int)DefinedParams.receiptType, (int)DefinedParams.recipientAddress, (int)DefinedParams.senderAddress, (int)DefinedParams.targetAddress, (int)DefinedParams.artifactId, (int)DefinedParams.pageSize, (int)DefinedParams.pageNumber, (int)DefinedParams.offset, (int)DefinedParams.order],
         /* 8 receipt address resolution statements   */ [(int)DefinedParams.height, (int)DefinedParams.pageSize, (int)DefinedParams.pageNumber, (int)DefinedParams.offset, (int)DefinedParams.order], 
         /* 9 receipt mosaic resolution statements    */ [(int)DefinedParams.height, (int)DefinedParams.pageSize, (int)DefinedParams.pageNumber, (int)DefinedParams.offset, (int)DefinedParams.order], 
         /* 10 search confirmed transactions          */ [(int)DefinedParams.address, (int)DefinedParams.recipientAddress, (int)DefinedParams.signerPublicKey, (int)DefinedParams.height, (int)DefinedParams.fromHeight, (int)DefinedParams.toHeight, (int)DefinedParams.fromTransferAmount, (int)DefinedParams.toTransferAmount, (int)DefinedParams.type, (int)DefinedParams.embedded, (int)DefinedParams.transferMosaicId, (int)DefinedParams.pageSize, (int)DefinedParams.pageNumber, (int)DefinedParams.offset, (int)DefinedParams.order],
         /* 11 search unconfirmed transactions        */ [(int)DefinedParams.address, (int)DefinedParams.recipientAddress, (int)DefinedParams.signerPublicKey, (int)DefinedParams.height, (int)DefinedParams.fromHeight, (int)DefinedParams.toHeight, (int)DefinedParams.fromTransferAmount, (int)DefinedParams.toTransferAmount, (int)DefinedParams.type, (int)DefinedParams.embedded, (int)DefinedParams.transferMosaicId, (int)DefinedParams.pageSize, (int)DefinedParams.pageNumber, (int)DefinedParams.offset, (int)DefinedParams.order],
         /* 12 search partial transactions            */ [(int)DefinedParams.address, (int)DefinedParams.recipientAddress, (int)DefinedParams.signerPublicKey, (int)DefinedParams.height, (int)DefinedParams.fromHeight, (int)DefinedParams.toHeight, (int)DefinedParams.fromTransferAmount, (int)DefinedParams.toTransferAmount, (int)DefinedParams.type, (int)DefinedParams.embedded, (int)DefinedParams.transferMosaicId, (int)DefinedParams.pageSize, (int)DefinedParams.pageNumber, (int)DefinedParams.offset, (int)DefinedParams.order],
         /* 13 search hashlock transaction statements */ [(int)DefinedParams.address, (int)DefinedParams.pageSize, (int)DefinedParams.pageNumber, (int)DefinedParams.offset, (int)DefinedParams.order],     
         /* 14 secret lock transaction statements     */ [(int)DefinedParams.address, (int)DefinedParams.secret, (int)DefinedParams.pageSize, (int)DefinedParams.pageNumber, (int)DefinedParams.offset, (int)DefinedParams.order],
        };

        private Dictionary<string, string> ParamMap = new Dictionary<string, string>();

        public QueryModel(DefineRequest request)
        {
            Request = request;
        }

        internal void ParamRequestCompatible(DefinedParams param)
        {
            var map = RequestParamMap[(int)Request].ToList();

            if (!map.Contains((int)param))
            {
                throw new Exception("invalid parameter for query: " + String.Join(",", map));
            }
        }

        public void SetParam(DefinedParams param, string value)
        {
            ParamRequestCompatible(param);

            ParamMap.Add(ParamMap.Count().ToString() 
                        + ((ushort)Request).ToString() 
                        + ((ushort)param).ToString(), (ParamMap.Count == 0 ? (param.ToString() + "=") : ("&" + param.ToString() + "=")) + value);
        }

        public void SetParam(DefinedParams param, bool value) => SetParam(param, value.ToString().ToLower()); 

        public void SetParam(DefinedParams param, int value) => SetParam(param, value.ToString());

        public void SetParam(DefinedParams param, Order order) => SetParam(param, order.ToString());

        public void SetParam(DefinedParams param, OrderBy orderBy) => SetParam(param, orderBy.ToString());

        public void RemoveParam(DefinedParams param)
        {
            ParamMap.Remove(((ushort)param).ToString());
        }

        public void Flush()
        {
            ParamMap.Clear();
        }

        public string ReturnPathParams()
        {
            return string.Concat(ParamMap.Select(p => p.Value.ToString()));
        }

        public enum DefineRequest
        {
            SearchAccounts = 0,
            SearchBlocks = 1,
            SearchMetaDataEntries = 2,
            SearchMosaics = 3,
            SearchNamespaces = 4,
            SearchMosaicRestrictions = 5,
            SearchAccountRestrictions = 6,
            SearchTransactionStatements = 7,
            GetRecieptsAddressResolutionStatements = 8,
            GetRecieptsMosaicResolutionStatements = 9,
            SearchConfirmedTransactions = 10,
            SearchUnconfirmedTransactions = 11,
            SearchPartialTransactions = 12,
            SearchHashLockTransactions = 13,
            SearchSecretLockEntries = 14,        
        }

        public enum DefinedParams
        {
            address = 0,
            aliasType = 1,
            artifactId = 2,
            beneficiaryAddress = 3,
            embedded = 4,
            entryType = 5,
            fromHeight = 6,
            fromTimestamp = 7,
            fromTransferAmount = 8,
            height = 9,
            level0 = 10, // do level 1 & 2
            metadataType = 11,
            mosaicId = 12,
            offset = 13,
            order = 14,
            orderBy = 15,
            ownerAddress = 16,
            pageNumber = 17,
            pageSize = 18,
            type = 19,
            recipientAddress = 20,
            registrationType = 21,
            scopedMetadataKey = 22,
            secret = 23,
            senderAddress = 24,
            signerPublicKey = 25,
            sourceAddress = 26,
            targetAddress = 27,
            targetId = 28,
            toHeight = 29,
            toTimestamp = 30,
            toTransferAmount = 31,
            transferMosaicId = 32,
            receiptType = 33,  
        }

        public enum Order
        {
            Desc = 0,

            Asc = 1
        }

        public enum OrderBy
        {
            Balance = 0,

            Id = 1
        }
    }
}
