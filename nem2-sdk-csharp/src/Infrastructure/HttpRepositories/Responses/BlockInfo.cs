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

using io.nem2.sdk.src.Model.Network;


namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses
{
    public class BlockInfo
    {
        public Meta Meta { get; set; }

        public Block Block { get; set; }

        public string Id { get; set; }
    }

    public class Block
    {
        public int Size { get; set; }

        public string Signature { get; set; }

        public string SignerPublicKey { get; set; }

        public int Version { get; set; }

        public NetworkType.Types Network { get; set; }

        public int Type { get; set; }
     
        public ulong Height { get; set; }
   
        public ulong Timestamp { get; set; }

        public ulong Difficulty { get; set; }

        public string ProofGamma { get; set; }

        public string ProofVerificationHash { get; set; }

        public string ProofScalar { get; set; }

        public string PreviousBlockHash { get; set; }

        public string BlockTransactionsHash { get; set; }

        public string ReceiptsHash { get; set; }

        public string StateHash { get; set; }

        public string DecodedBeneficiaryAddress { get; set; }

        public int FeeMultiplier { get; set; }

        public int VotingEligibleAccountsCount { get; set; }

        public int HarvestingEligibleAccountsCount { get; set; }

        public ulong TotalVotingBalance { get; set; }

        public ulong PreviousImportanceBlockHash { get; set; }
    }

    public class Meta
    {

        public string Hash { get; set; }

        public string GenerationHash { get; set; }

        public ulong TotalFee { get; set; }

        public int TotalTransactionsCount { get; set; }

        public List<string> StateHashSubCacheMerkleRoots { get; set; }

        public int TransactionsCount { get; set; }

        public int StatementsCount { get; set; }
    }
}
