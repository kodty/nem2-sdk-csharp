﻿// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="IMessage.cs" company="Nem.io">
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

namespace io.nem2.sdk.Model.Transactions.Messages
{
    /// <summary>
    /// Class IMessage.
    /// </summary>
    public abstract class IMessage
    {
        /// <summary>
        /// Gets the type of the message.
        /// </summary>
        /// <returns>System.Byte.</returns>
        internal abstract byte GetMessageType();

        /// <summary>
        /// Gets the payload.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public abstract byte[] GetPayload();

        /// <summary>
        /// Gets the length.
        /// </summary>
        /// <returns>System.UInt32.</returns>
        public abstract uint GetLength();
    }
}
