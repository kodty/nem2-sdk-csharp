
namespace io.nem2.sdk.src.Infrastructure.HttpRepositories
{
    public class QueryModel
    {
        internal DefineRequest Request { get; set; }

        internal int[][] RequestParamMap =
        {
            [18, 17, 13, 14, 15, 12], // search accounts
            [25, 4, 18, 17, 13, 14, 15], // search blocks
            [26, 27, 22, 28, 11, 18, 17, 13, 14], // search meta data
            [16, 18, 17, 13, 14], // search mosaics
            [16, 21, 10, 2, 18, 17, 13, 14], // search namespaces
            [12, 6, 27, 18, 17, 13, 14], // search mosaic restrictions
            [(int)DefinedParams.address, 18, 17, 13, 14], // search account restrictions
            [9, 7, 29, 19, 20, 23, 27, 3, 18, 17, 13, 14],
            [9, 18, 17, 13, 14],
            [9, 18, 17, 13, 14],

            //
            [(int)DefinedParams.address, 
             (int)DefinedParams.recipientAddress, 25, 9, 7, 29, 8, 30, 32, 5, 31, 18, 17, 13, 14],
            
            //
            [(int)DefinedParams.address, 
             (int)DefinedParams.recipientAddress, 25, 9, 7, 29, 8, 30, 32, 5, 31, 18, 17, 13, 14],
           
            //
            [(int)DefinedParams.address, 
             (int)DefinedParams.recipientAddress, 25, 9, 7, 29, 8, 30, 32, 5, 31, 18, 17, 13, 14],
           
            //
            [(int)DefinedParams.address, 18, 17, 13, 14],

            //
            [(int)DefinedParams.address, 23, 18, 17, 13, 14]
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

        internal string ReturnPathParams()
        {
            if (ParamMap.Count == 1)
            {
                return ParamMap.Select(p => {
                    return p.Key.ToString() + p.Value.ToString();
                }).Single();
            }
            else return String.Join("&", ParamMap.Select(p => {

                    return (p.Key.ToString().Substring(1) + p.Value.ToString());
                }));
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
            SearchSecretLockTransactions = 14
        }

        public enum DefinedParams
        {
            address = 1,
            aliasType = 2,
            artifactId = 3,
            beneficiaryAddress = 4,
            embedded = 5,
            entryType = 6,
            fromHeight = 7,
            fromTransferAmount = 8,
            height = 9,
            level0 = 10,
            metadataType = 11,
            mosaicId = 12,
            offset = 13,
            order = 14,
            orderBy = 15,
            ownerAddress = 16,
            pageNumber = 17,
            pageSize = 18,
            recieptType = 19,
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
            transferMosaicId = 31,
            type = 32
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
