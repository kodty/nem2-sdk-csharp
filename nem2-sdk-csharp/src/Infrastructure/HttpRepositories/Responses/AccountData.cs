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
    public class AccountData
    {
       public AccountInfo Account {  get; set; }
       public string Id { get; set; }
    }

    public class AccountInfo
    {
        public int Version { get; set; }

        public string Address { get; set; }

        public ulong AddressHeight { get; set; }

        public string PublicKey { get; set; }

        public ulong PublicKeyHeight { get; set; }

        public int AccountType { get; set; }

        public SupplementalPublicKeys SupplementalPublicKeys { get; set; }

        public List<ActivityBucket> ActivityBuckets { get; set; }

        public List<MosaicTransfer> Mosaics { get; set; }

        public ulong Importance { get; set; }

        public ulong ImportanceHeight { get; set; }
    }

    public class ActivityBucket
    {
        public ulong StartHeight { get; set; }

        public ulong TotalFeesPaid { get; set; }

        public int BeneficiaryCount { get; set; }

        public ulong RawScore { get; set; }
    }

    public class Linked
    {
        public string PublicKey { get; set; }
    }
    public class Node
    {
        public string PublicKey { get; set; }
    }
    public class VRF
    {
        public string PublicKey { get; set; }
    }

    public class SupplementalPublicKeys
    {
        public Linked Linked { get; set; }

        public Node Node { get; set; }

        public VRF Vrf { get; set; }

        public Voting Voting { get; set; }
        
    }

    public class Voting
    {
        public List<VotingKeys> PublicKeys { get; set; }
    }
    public class VotingKeys
    {
        public string PublicKey { get; set; }

        public int StartEpoch { get; set; }

        public int EndEpoch { get; set; }
    }
}
