// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="QueryParams.cs" company="Nem.io">   
// Copyright 2018 NEM
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.ComponentModel;
using System.Diagnostics;

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

            ParamMap.Add(ParamMap.Count == 0 ? ("?" + param.ToString() + "=") : ((ParamMap.Count + 1).ToString() + "&" + param.ToString() + "="), value);

            /*
            if (param == DefinedParams.pageSize)
            {
                ParamMap.Add("?" + DefinedParams.pageSize + "=", value);
                return;
            }

            if (param == DefinedParams.order)
            {
                ParamMap.Add("?order=", value);
                return;
            }
            if (param == DefinedParams.orderBy)
            {
                ParamMap.Add("?orderBy=", value);
                return;
            }
            if (param == DefinedParams.offset)
            {
                ParamMap.Add("?offset=", value);
                return;
            }
            if (param == DefinedParams.mosaicId)
            {
                ParamMap.Add("?mosaicId=", value);
                return;
            }
            
            if (param == DefinedParams.pageNumber)
            {
                ParamMap.Add("?pageNumber=", value);
                return;
            }
            */
        }

        public void SetParam(DefinedParams param, int value) => SetParam(param, value.ToString());

        public void SetParam(DefinedParams param, Order order) => SetParam(param, order.ToString());

        public void SetParam(DefinedParams param, OrderBy orderBy) => SetParam(param, orderBy.ToString());

        public void RemoveParam(DefinedParams param)
        {

            ParamMap.Remove("?" + param.ToString() + "=");


            /*
            if (param == DefinedParams.order)
            {
                ParamMap.Remove("?order=");
                return;
            }
            if (param == DefinedParams.orderBy)
            {
                ParamMap.Remove("?orderBy=");
                return;
            }
            if (param == DefinedParams.offset)
            {
                ParamMap.Remove("?offset=");
                return;
            }
            if (param == DefinedParams.mosaicId)
            {
                ParamMap.Remove("?mosaicId=");
                return;
            }
            if (param == DefinedParams.pageSize)
            {
                ParamMap.Remove("?" + DefinedParams.pageSize + "=");
                return;
            }
            if (param == DefinedParams.pageNumber)
            {
                ParamMap.Remove("?pageNumber=");
                return;
            }
            */
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
            [Description("desc")]
            Desc = 0,
            [Description("asc")]
            Asc = 1
        }

        public enum OrderBy
        {
            [Description("balance")]
            Balance = 0,
            [Description("id")]
            Id = 1
        }
    }
}
