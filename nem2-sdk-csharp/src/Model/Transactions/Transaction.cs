// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-30-2018
// ***********************************************************************
// <copyright file="Transaction.cs" company="Nem.io">
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

using io.nem2.sdk.Core.Crypto.Chaso.NaCl;
using io.nem2.sdk.Core.Utils;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.src.Model.Network;
using Org.BouncyCastle.Crypto.Digests;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using TweetNaclSharp;
using TweetNaclSharp.Core;
using TweetNaclSharp.Core.Extensions;
using static io.nem2.sdk.Infrastructure.HttpRepositories.TransactionHttp;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace io.nem2.sdk.Model.Transactions
{
    public abstract class Transaction
    {
        public ulong Fee { get; internal set; }

        public Deadline Deadline { get; internal set; }

        public NetworkType.Types NetworkType { get; internal set; }

        public int Version { get; set; }

        public TransactionTypes.Types TransactionType { get; internal set; }

        public PublicAccount Signer { get; internal set; }

        public string Signature { get; internal set; }

        public TransactionInfo TransactionInfo { get; internal set; }

        private byte[] Bytes { get; set; }

        private byte[] SignedBytes { get; set; }

        internal byte[] GetSigner()
        {
            return Signer == null ? new byte[32] : Signer.PublicKey.DecodeHexString();
        }

        public byte[] PrepareSignature(SecretKeyPair keyPair, byte[] networkGenHash)
        {
            Signer = PublicAccount.CreateFromPublicKey(keyPair.PublicKeyString, NetworkType);

            Bytes = GenerateBytes();

            byte[] signingBytes = Bytes.SubArray(4 + 64 + 32 + 8, Bytes.Length - 108);

            SignedBytes = networkGenHash.Concat(signingBytes).ToArray();

            Signature = keyPair.Sign(SignedBytes).ToHexLower();

            return Signature.FromHex();
        }


        public byte[] HashTransaction(byte[] signature, byte[] signer, byte[] genHash, byte[] headlessTxData)
        {
            var hash = new byte[32];

            var sha3Hasher = new Sha3Digest(256);
            
            sha3Hasher.BlockUpdate(signature, 0, signature.Length);
            sha3Hasher.BlockUpdate(signer, 0, signer.Length);
            sha3Hasher.BlockUpdate(genHash, 0, genHash.Length);
            sha3Hasher.BlockUpdate(headlessTxData, 0, headlessTxData.Length);
            sha3Hasher.DoFinal(hash, 0);

            return hash;
        }
        public SignedTransaction SignWith(SecretKeyPair keyPair, byte[] networkGenHash)
        {
            PrepareSignature(keyPair, networkGenHash);

            for (int x = 8; x < 64 + 8; x++) Bytes[x] = Signature.FromHex()[x - 8];

            var headlessTx = Bytes.SubArray(8 + 64 + 32 + 4, Bytes.Length - (8 + 64 + 32 + 4));
            
            var hash = HashTransaction(Signature.FromHex(), keyPair.PublicKey, networkGenHash, headlessTx);

            return SignedTransaction.Create(Bytes, SignedBytes, hash, keyPair.PublicKey, Signature.FromHex(), TransactionType);
        }

        internal byte[] ToAggregate()
        {
            var bytes = GenerateBytes();

            var aggregate = bytes.Take(4 + 64, 32 + 2 + 2)
                                 .Concat( 
                                        bytes.Take(4 + 64 + 32 + 2 + 2 + 8 + 8, bytes.Length - (4 + 64 + 32 + 2 + 2 + 8 + 8))
                                 ).ToArray();

            return BitConverter.GetBytes(aggregate.Length + 4).Concat(aggregate).ToArray();
        }

        public Transaction ToAggregate(PublicAccount signer)
        {
            Signer = PublicAccount.CreateFromPublicKey(signer.PublicKey, NetworkType);

            return this;
        }

        /// <summary>
        /// Generates the bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        internal abstract byte[] GenerateBytes();
    }
}
