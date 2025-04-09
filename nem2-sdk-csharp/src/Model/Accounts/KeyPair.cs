﻿// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="KeyPair.cs" company="Nem.io">   
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
using io.nem2.sdk.Core.Crypto.Chaso.NaCl.Internal.Ed25519ref10;
using System.Security.Cryptography;
using TweetNaclSharp;

namespace io.nem2.sdk.Model.Accounts
{
    /// <summary>
    /// The KeyPair structure describes a private key and public key in two formats, and contains static classes from creating the structure from raw data.
    /// </summary>
    /// <seealso cref="IKeyPair" />
    public class KeyPair : IKeyPair
    {
        
        /// <inheritdoc />
        public byte[] PrivateKey { get; }

        /// <inheritdoc />
        public byte[] PublicKey { get; }

        /// <summary>
        /// Gets the private key string.
        /// </summary>
        /// <value>The private key string.</value>
        public string PrivateKeyString => PrivateKey.ToHexLower().ToUpper();

        /// <summary>
        /// Gets the public key string.
        /// </summary>
        /// <value>The public key string.</value>
        public string PublicKeyString => PublicKey.ToHexLower().ToUpper();

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyPair"/> class.
        /// </summary>
        /// <param name="privateKey">The private key.</param>
        /// <param name="publicKey">The public key.</param>
        /// <exception cref="ArgumentNullException">
        /// privateKey
        /// or
        /// publicKey
        /// </exception>
        /// <exception cref="ArgumentException">
        /// privateKey
        /// or
        /// publicKey
        /// </exception>
        internal KeyPair(string privateKey, string publicKey)
        {
            if (publicKey == null) throw new ArgumentNullException(nameof(publicKey));
            if (publicKey.Length != 64) throw new ArgumentException(nameof(publicKey));

            PrivateKey = privateKey.FromHex();

            PublicKey = publicKey.FromHex();
        }

        /// <summary>
        /// Creates from private key.
        /// </summary>
        /// <param name="privateKey">The private key.</param>
        /// <returns>KeyPair.</returns>
        /// <exception cref="ArgumentNullException">privateKey</exception>
        /// <exception cref="ArgumentException">privateKey</exception>
        public static KeyPair CreateFromPrivateKey(string privateKey)
        {
            if (privateKey == null) throw new ArgumentNullException(nameof(privateKey));
            if (privateKey.Length != 64) throw new ArgumentException(nameof(privateKey));

            var privateKeyArray = privateKey.FromHex();

            GroupElementP3 A;
            int i;

            byte[] h = new byte[64];
            byte[] pk = new byte[32];

            SHA512.HashData(privateKeyArray, h);

            ScalarOperations.sc_clamp(h, 0);
            GroupOperations.ge_scalarmult_base(out A, h, 0);
            GroupOperations.ge_p3_tobytes(pk, 0, ref A);

            Array.Clear(h, 0, h.Length);

            return new KeyPair(privateKey, pk.ToHexLower());
        }

        /// <summary>
        /// Signs the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>System.Byte[].</returns>
        /// <exception cref="ArgumentNullException">data</exception>
        public byte[] Sign(byte[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            return NaclFast.Sign(data, PrivateKey.Concat(PublicKey).ToArray());
        }      
    }
}
