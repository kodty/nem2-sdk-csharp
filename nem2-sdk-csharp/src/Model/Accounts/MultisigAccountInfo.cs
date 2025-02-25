﻿// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="MultisigAccountInfo.cs" company="Nem.io">   
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

namespace io.nem2.sdk.Model.Accounts
{

    public class MultisigAccountInfo
    {
       
        public PublicAccount Account { get; }
       
        public int MinApproval { get; }
        
        public int MinRemoval { get; }
       
        public List<PublicAccount> Cosignatories { get; }
       
        public List<PublicAccount> MultisigAccounts { get; }
        /// <summary>
        /// Checks if an account is cosignatory of the multisig account.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns><c>true</c> if the specified account has cosigners; otherwise, <c>false</c>.</returns>
        public bool HasCosigners(PublicAccount account) => Cosignatories.Contains(account);
        /// <summary>
        /// Checks if the multisig account is cosignatory of an account.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns><c>true</c> if [is cosigner of multisig account] [the specified account]; otherwise, <c>false</c>.</returns>
        public bool IsCosignerOfMultisigAccount(PublicAccount account) => MultisigAccounts.Contains(account);
        /// <summary>
        /// Checks if the account is a multisig account.
        /// </summary>
        /// <value><c>true</c> if this instance is multisig; otherwise, <c>false</c>.</value>
        public bool IsMultisig => MinApproval != 0 && MinRemoval != 0;

        
        public MultisigAccountInfo(PublicAccount account, int minApproval, int minRemoval, List<PublicAccount> cosignatories, List<PublicAccount> multisigAccounts)
        {
            Account = account;
            MinApproval = minApproval;
            MinRemoval = minRemoval;
            Cosignatories = cosignatories;
            MultisigAccounts = multisigAccounts;
        }
    }
}
