// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="BlockInfoDTO.cs" company="Nem.io">   
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

using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.src.Infrastructure.Buffers.Model.JsonConverters;
using Newtonsoft.Json;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses
{
    public class BlockInfo
    {

        [JsonProperty("meta")]
        public Meta Meta { get; set; }


        [JsonProperty("block")]
        public Block Block { get; set; }


        [JsonProperty("id")]
        public string BlockID { get; set; }
    }

    public class Block
    {
        [JsonProperty("size")]
        public int Size { get; set; }


        [JsonProperty("signature")]
        public string Signature { get; set; }


        [JsonProperty("signerPublicKey")]
        public string SignerPublicKey { get; set; }


        [JsonProperty("version")]
        public int Version { get; set; }


        [JsonProperty("network")]
        public int NetworkType { get; set; }


        [JsonProperty("type")]
        public int Type { get; set; }


        [JsonProperty("height")]     
        public ulong Height { get; set; }


        [JsonProperty("timestamp")]    
        public ulong Timestamp { get; set; }


        [JsonProperty("difficulty")]
        public ulong Difficulty { get; set; }


        [JsonProperty("proofGamma")]
        public string ProofGamma { get; set; }


        [JsonProperty("proofVerificationHash")]
        public string ProofVerificationHash { get; set; }


        [JsonProperty("proofScalar")]
        public string ProofScalar { get; set; }


        [JsonProperty("previousBlockHash")]
        public string PreviousBlockHash { get; set; }


        [JsonProperty("transactionsHash")]
        public string BlockTransactionsHash { get; set; }


        [JsonProperty("receiptsHash")]
        public string ReceiptsHash { get; set; }


        [JsonProperty("stateHash")]
        public string StateHash { get; set; }


        [JsonProperty("beneficiaryAddress")]
        public string DecodedBeneficiaryAddress { get; set; }


        [JsonProperty("feeMultiplier")]
        public int FeeMultiplier { get; set; }


        [JsonProperty("votingEligibleAccountsCount")]
        public int VotingEligibleAccountsCount { get; set; }


        [JsonProperty("harvestingEligibleAccountsCount")]
        public int HarvestingEligibleAccountsCount { get; set; }


        [JsonProperty("totalVotingBalance")]
        public ulong TotalVotingBalance { get; set; }


        [JsonProperty("previousImportanceBlockHash")]
        public ulong PreviousImportanceBlockHash { get; set; }
    }

    public class Meta
    {

        [JsonProperty("hash")]
        public string Hash { get; set; }


        [JsonProperty("generationHash")]
        public string GenerationHash { get; set; }


        [JsonProperty("totalFee")]
        public ulong TotalFee { get; set; }


        [JsonProperty("totalTransactionsCount")]
        public int TotalTransactionsCount { get; set; }


        [JsonProperty("stateHashSubCacheMerkleRoots")]
        public string[] StateHashSubCacheMerkleRoots { get; set; }


        [JsonProperty("transactionsCount")]
        public int TransactionsCount { get; set; }


        [JsonProperty("statementsCount")]
        public int StatementsCount { get; set; }
    }
}
