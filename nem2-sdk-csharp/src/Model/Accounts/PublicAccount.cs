// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="PublicAccount.cs" company="Nem.io">   
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
using System.Text.RegularExpressions;
using io.nem2.sdk.src.Model.Network;

namespace io.nem2.sdk.Model.Accounts
{

    public class PublicAccount
    {

        public Address Address { get; }

        public string PublicKey { get; }

        internal NetworkType.Types NetworkType { get; }


        public PublicAccount(string publicKey, NetworkType.Types networkType)
        {
            if (publicKey == null) throw new ArgumentNullException(nameof(publicKey));
            if (!Regex.IsMatch(publicKey, @"\A\b[0-9a-fA-F]+\b\Z")) throw  new ArgumentException("invalid public key length");
            if (publicKey.Length != 64) throw new ArgumentException("invalid public key not hex");           
            if (!Enum.IsDefined(typeof(NetworkType.Types), networkType))
                throw new InvalidEnumArgumentException(nameof(networkType), (int) networkType,
                    typeof(NetworkType.Types));

            Address = Address.CreateFromPublicKey(publicKey, networkType);
            PublicKey = publicKey;
            NetworkType = networkType;
        }


        public static PublicAccount CreateFromPublicKey(string publicKey, NetworkType.Types networkType)
        {
            return new PublicAccount(publicKey, networkType);
        }


        public bool Equals(PublicAccount other)
        {
            return Equals(Address, other.Address) && string.Equals(PublicKey, other.PublicKey);
        }


        public override int GetHashCode()
        {
            unchecked
            {
                return ((Address != null ? Address.GetHashCode() : 0) * 397) ^ (PublicKey != null ? PublicKey.GetHashCode() : 0);
            }
        }


        public NetworkType.Types GetAccountNetworkType()
        {
            return NetworkType;
        }
    }
}
