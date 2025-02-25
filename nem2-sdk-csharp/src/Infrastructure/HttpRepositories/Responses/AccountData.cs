// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="AccountDTO.cs" company="Nem.io">   
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

using Newtonsoft.Json;
using System.ComponentModel;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses
{
    public class AccountInfo
    {

        [JsonProperty("Version")]
        public int Version { get; set; }


        [JsonProperty("address")]    
        public string Address { get; set; }


        [JsonProperty("addressHeight")]
        public ulong AddressHeight { get; set; }


        [JsonProperty("publicKey")]
        public string PublicKey { get; set; }


        [JsonProperty("publicKeyHeight")]
        public ulong PublicKeyHeight { get; set; }


        [JsonProperty("accountType")]
        public int AccountType { get; set; }


        [JsonProperty("supplementalPublicKeys")]
        public SupplementalPublicKeys SupplementalPublicKeys { get; set; }


        [JsonProperty("acitivityBuckets")]
        public List<ActivityBucket> ActivityBuckets { get; set; }


        [JsonProperty("mosaics")]
        public List<MosaicTransfer> Mosaics { get; set; }


        [JsonProperty("importance")]
        public ulong Importance { get; set; }


        [JsonProperty("importanceHeight")]
        public ulong ImportanceHeight { get; set; }
    }

    public class ActivityBucket
    {
        [JsonProperty("startHeight")]
        public ulong StartHeight { get; set; }


        [JsonProperty("totalFeesPaid")]
        public ulong TotalFeesPaid { get; set; }


        [JsonProperty("beneficiaryCount")]
        public int BeneficiaryCount { get; set; }


        [JsonProperty("rawScore")]
        public ulong RawScore { get; set; }
    }

    public class SupplementalPublicKeys
    {
        [JsonProperty("linked")]
        [Description("remote")]
        public string Linked { get; set; }


        [JsonProperty("node")]
        public string Node { get; set; }


        [JsonProperty("vrf")]
        public string Vrf { get; set; }


        [JsonProperty("voting")]
        public List<VotingKeys> PublicKeys { get; set; }
    }

    public class VotingKeys
    {
        [JsonProperty("publicKey")]
        public string PublicKey { get; set; }


        [JsonProperty("startEpoch")]
        public int StartEpoch { get; set; }


        [JsonProperty("endEpoch")]
        public int EndEpoch { get; set; }
    }
}
