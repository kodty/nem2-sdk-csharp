namespace io.nem2.sdk.src.Infrastructure.HttpRepositories
{
    public class QueryModel
    {
        internal DefineRequest Request { get; set; }

        internal int[][] RequestParamMap =
        {
        /*0*/ [(int)DefinedParams.height, (int)DefinedParams.pageSize, (int)DefinedParams.pageNumber, (int)DefinedParams.offset, (int)DefinedParams.order],
        /*1*/ [(int)DefinedParams.height, (int)DefinedParams.pageSize, (int)DefinedParams.pageNumber, (int)DefinedParams.offset, (int)DefinedParams.order],
        /*2*/ [(int)DefinedParams.address, (int)DefinedParams.pageSize, (int)DefinedParams.pageNumber, (int)DefinedParams.offset, (int)DefinedParams.order], // search account restrictions
        /*3*/ [(int)DefinedParams.pageSize, (int)DefinedParams.pageNumber, (int)DefinedParams.offset, (int)DefinedParams.order, (int)DefinedParams.orderBy, (int)DefinedParams.mosaicId], // search accounts
        /*4*/ [(int)DefinedParams.signerPublicKey, (int)DefinedParams.beneficiaryAddress, (int)DefinedParams.pageSize, (int)DefinedParams.pageNumber, (int)DefinedParams.offset, (int)DefinedParams.order, (int)DefinedParams.orderBy], // search blocks
        /*5*/ [(int)DefinedParams.address, (int)DefinedParams.recipientAddress, (int)DefinedParams.signerPublicKey, (int)DefinedParams.height, (int)DefinedParams.fromHeight, (int)DefinedParams.toHeight, (int)DefinedParams.fromTransferAmount, (int)DefinedParams.toTransferAmount, (int)DefinedParams.type, (int)DefinedParams.embedded, (int)DefinedParams.transferMosaicId, (int)DefinedParams.pageSize, (int)DefinedParams.pageNumber, (int)DefinedParams.offset, (int)DefinedParams.order],
        /*6*/ [(int)DefinedParams.address, (int)DefinedParams.pageSize, (int)DefinedParams.pageNumber, (int)DefinedParams.offset, (int)DefinedParams.order],
        /*7*/ [(int)DefinedParams.sourceAddress, (int)DefinedParams.targetAddress, (int)DefinedParams.scopedMetadataKey, (int)DefinedParams.targetId, (int)DefinedParams.metadataType, (int)DefinedParams.pageSize, (int)DefinedParams.pageNumber, (int)DefinedParams.offset, (int)DefinedParams.order], // search meta data
        /*8*/ [(int)DefinedParams.mosaicId, (int)DefinedParams.entryType, (int)DefinedParams.targetAddress, (int)DefinedParams.pageSize, (int)DefinedParams.pageNumber, (int)DefinedParams.offset, (int)DefinedParams.order], // search mosaic restrictions
        /*9*/ [(int)DefinedParams.ownerAddress, (int)DefinedParams.pageSize, (int)DefinedParams.pageNumber, (int)DefinedParams.offset, (int)DefinedParams.order], // search mosaics
        /*10*/[(int)DefinedParams.ownerAddress, (int)DefinedParams.registrationType, (int)DefinedParams.level0, (int)DefinedParams.aliasType, (int)DefinedParams.pageSize, (int)DefinedParams.pageNumber, (int)DefinedParams.offset, (int)DefinedParams.order], // search namespaces
        /*11*/[(int)DefinedParams.address, (int)DefinedParams.recipientAddress, (int)DefinedParams.signerPublicKey, (int)DefinedParams.height, (int)DefinedParams.fromHeight, (int)DefinedParams.toHeight, (int)DefinedParams.fromTransferAmount, (int)DefinedParams.toTransferAmount, (int)DefinedParams.type, (int)DefinedParams.embedded, (int)DefinedParams.transferMosaicId, (int)DefinedParams.pageSize, (int)DefinedParams.pageNumber, (int)DefinedParams.offset, (int)DefinedParams.order],
        /*12*/[(int)DefinedParams.address, (int)DefinedParams.secret, (int)DefinedParams.pageSize, (int)DefinedParams.pageNumber, (int)DefinedParams.offset, (int)DefinedParams.order],
        /*13*/[(int)DefinedParams.type, (int)DefinedParams.recipientAddress, (int)DefinedParams.senderAddress, (int)DefinedParams.targetAddress, (int)DefinedParams.height, (int)DefinedParams.fromHeight, (int)DefinedParams.toHeight, (int)DefinedParams.artifactId, (int)DefinedParams.pageSize, (int)DefinedParams.pageNumber, (int)DefinedParams.offset, (int)DefinedParams.order],
        /*14*/[(int)DefinedParams.address, (int)DefinedParams.recipientAddress, (int)DefinedParams.signerPublicKey, (int)DefinedParams.height, (int)DefinedParams.fromHeight, (int)DefinedParams.toHeight, (int)DefinedParams.fromTransferAmount, (int)DefinedParams.toTransferAmount, (int)DefinedParams.type, (int)DefinedParams.embedded, (int)DefinedParams.transferMosaicId, (int)DefinedParams.pageSize, (int)DefinedParams.pageNumber, (int)DefinedParams.offset, (int)DefinedParams.order],
        };

        private Dictionary<string, string> ParamMap = new Dictionary<string, string>();

        public QueryModel() { }

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

            ParamMap.Add(ParamMap.Count == 0 ? ("?" + param.ToString() + "=") : ((ParamMap.Count + 1).ToString() + param.ToString() + "="), value);
        }

        public void SetParam(DefinedParams param, bool value) => SetParam(param, value.ToString().ToLower()); 

        public void SetParam(DefinedParams param, int value) => SetParam(param, value.ToString());

        public void SetParam(DefinedParams param, Order order) => SetParam(param, order.ToString());

        public void SetParam(DefinedParams param, OrderBy orderBy) => SetParam(param, orderBy.ToString());

        public void RemoveParam(DefinedParams param)
        {
            ParamMap.Remove("?" + param.ToString() + "=");
        }
        public void Flush()
        {
            ParamMap.Clear();
        }

        public string ReturnPathParams()
        {
            if (ParamMap.Count == 1)
            {
                return ParamMap.Select(p => {
                    return p.Key.ToString() + p.Value.ToString();
                }).Single();
            }
            else return string.Join("&", ParamMap.Select(p => {

                    return (p.Key.ToString().Substring(1) + p.Value.ToString());
                }));
        }

        public enum DefineRequest
        {
            GetRecieptsAddressResolutionStatements = 0,
            GetRecieptsMosaicResolutionStatements = 1,
            SearchAccountRestrictions = 2,
            SearchAccounts = 3,
            SearchBlocks = 4,
            SearchConfirmedTransactions = 5,
            SearchHashLockTransactions = 6,
            SearchMetaDataEntries = 7,
            SearchMosaicRestrictions = 8,
            SearchMosaics = 9,
            SearchNamespaces = 10,
            SearchPartialTransactions = 11,
            SearchSecretLockTransactions = 12,
            SearchTransactionStatements = 13,
            SearchUnconfirmedTransactions = 14, // todo
        }

        public enum DefinedParams
        {
            address = 1,
            aliasType = 2,
            artifactId = 3,
            beneficiaryAddress = 4, // must be Base32 encoded - contrary to hex beneficiary provided by /blocks
            embedded = 5,
            entryType = 6,
            fromHeight = 7,
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
            toTransferAmount = 30,
            transferMosaicId = 31
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
