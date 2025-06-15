// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-30-2018
// ***********************************************************************
// <copyright file="SignedTransaction.cs" company="Nem.io">
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

using System.Diagnostics;
using System.Text.RegularExpressions;
using io.nem2.sdk.Core.Crypto.Chaso.NaCl;
using TweetNaclSharp;

namespace io.nem2.sdk.Model.Transactions
{
    public class SignedTransaction
    {
        public string Payload { get; set; }

        public string Hash { get; set; }

        public string Signer { get; set; }

        public string Signature { get; set; }

        private byte[] SignedBytes { get; set; }

        public TransactionTypes.Types TransactionType { get; }

        public bool VerifySignature()
        {
            return NaclFast.SignDetachedVerify(SignedBytes, Signature.FromHex(), Signer.FromHex());
        }

        internal SignedTransaction(string payload, byte[] signedBytes, string hash, string signer, string signature, TransactionTypes.Types transactionType)
        {          
            if (hash.Length != 64 || !Regex.IsMatch(hash, @"\A\b[0-9a-fA-F]+\b\Z")) throw new ArgumentException("Invalid hash.");
            TransactionType = transactionType;
            Payload = payload;
            Hash = hash;
            Signer = signer;
            Signature = signature;  
            SignedBytes = signedBytes;  
        }

        public static SignedTransaction Create(byte[] payload, byte[] signedBytes, byte[] hash, byte[] signer, byte[] signature, TransactionTypes.Types transactionType)
        {
            if (payload == null) throw new ArgumentNullException(nameof(payload));
            if (hash == null) throw new ArgumentNullException(nameof(hash));
            if(hash.Length != 32) throw new ArgumentException("invalid hash length");
            if (signer == null) throw new ArgumentNullException(nameof(signer));
            if (signer.Length != 32) throw new ArgumentException("invalid signer length");

            return new SignedTransaction(payload.ToHexLower(), signedBytes, hash.ToHexLower(), signer.ToHexLower(), signature.ToHexLower(), transactionType);
        }
    }
}
