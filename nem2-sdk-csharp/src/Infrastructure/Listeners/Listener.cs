// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="Listener.cs" company="Nem.io">   
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

using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using io.nem2.sdk.Infrastructure.Buffers.Model;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Infrastructure.Mapping;
using Newtonsoft.Json.Linq;

namespace io.nem2.sdk.Infrastructure.Listeners
{
    public class Listener 
    {
        private WebsocketUID Uid { get; set; }

        private ClientWebSocket ClientSocket { get; }

        private Task LoopReads { get; set; }

        private readonly Subject<string> _subject = new Subject<string>();

        public string Domain { get; set; }

        public int Port { get; set; }

        public Listener(string domain, int port = 3000) //7902
        {
            ClientSocket = new ClientWebSocket();

            Domain = domain;

            Port = port;
        }

        public IObservable<bool> Open()
        {
            return Observable.Start(() =>
            {
                ClientSocket.ConnectAsync(new Uri(string.Concat("ws://", Domain, ":", Port, "/ws")), CancellationToken.None)
                    .GetAwaiter()
                    .GetResult();

                Uid = ObjectComposer.GenerateObject<WebsocketUID>(ReadSocket().Result);

                LoopReads = Task.Run(() => LoopRead());

                return Uid != null;
            });         
        }

        internal async void LoopRead()
        {
            while (true)
            {
                _subject.OnNext(await ReadSocket());
            }
        }

        internal async Task<string> ReadSocket()
        {
            var buffer = new ArraySegment<byte>(new byte[8192]);

            using (var stream = new MemoryStream())
            {
                WebSocketReceiveResult result;

                do
                {
                    result = await ClientSocket.ReceiveAsync(buffer, CancellationToken.None);

                    stream.Write(buffer.Array, buffer.Offset, result.Count);
                }
                while (!result.EndOfMessage);

                stream.Seek(0, SeekOrigin.Begin);

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        internal void SubscribeToChannel(string channel)
        {
            var encoded = Encoding.UTF8.GetBytes(string.Concat("{ \"uid\": \"", Uid.UID, "\", \"subscribe\":\"", channel, "\"}"));

            var buffer = new ArraySegment<byte>(encoded, 0, encoded.Length);

            ClientSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);

            Console.WriteLine("Subscribed to channel: " + channel + ", UID: " + Uid.UID);
        }

        public IObservable<BlockInfo> NewBlock()
        {
            SubscribeToChannel("block");

            return _subject.Where(e => JObject.Parse(e).Properties().ToArray().Any(i => i.Value.ToString().Contains("block")))
               .Select(ObjectComposer.GenerateObject<BlockInfo>);         
        }

        public IObservable<TransactionData> ConfirmedTransactionsGiven(Address address)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));

            SubscribeToChannel(string.Concat("confirmedAdded/", address.Plain));

            return _subject.Where(e => JObject.Parse(e).Properties().ToArray().Any(i => i.Value.ToString().Contains("confirmedAdded")))
               .Select(ResponseFilters<TransactionData>.FilterSingle);         
            
        }

        public IObservable<TransactionData> UnconfirmedTransactionsAdded(Address address)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));

            SubscribeToChannel(string.Concat("unconfirmedAdded/", address.Plain));

            return _subject.Where(e => JObject.Parse(e).Properties().ToArray().Any(i => i.Value.ToString().Contains("unconfirmedAdded")))
                 .Select(ResponseFilters<TransactionData>.FilterSingle);
        }

        public IObservable<TransactionData> UnconfirmedTransactionsRemoved(Address address)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));

            SubscribeToChannel(string.Concat("unconfirmedRemoved/", address.Plain));

            return _subject.Where(e => JObject.Parse(e).Properties().ToArray().Any(i => i.Value.ToString().Contains("unconfirmedRemoved")))
                 .Select(ResponseFilters<TransactionData>.FilterSingle);
        }

        public IObservable<TransactionData> AggregateBondedAdded(Address address)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));

            SubscribeToChannel(string.Concat("partialAdded/", address.Plain));

            return _subject.Where(e => JObject.Parse(e).Properties().ToArray().Any(i => i.Value.ToString().Contains("partialAdded")))
                .Select(ResponseFilters<TransactionData>.FilterSingle);
        }

        public IObservable<TransactionData> AggregateBondedRemoved(Address address)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));

            SubscribeToChannel(string.Concat("partialRemoved/", address.Plain));

            return _subject.Where(e => JObject.Parse(e).Properties().ToArray().Any(i => i.Value.ToString().Contains("partialRemoved")))
                 .Select(ResponseFilters<TransactionData>.FilterSingle);
        }
        public class TransactionStatus
        {
            public string Status { get; set; }
        }
        public IObservable<TransactionStatus> GetTransactionStatus(Address address)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));

            SubscribeToChannel(string.Concat("status/", address.Plain));

            return _subject.Where(e => JObject.Parse(e).Properties().ToArray().Any(i => i.Name.ToString().Contains("status")))          
                .Select(ObjectComposer.GenerateObject<TransactionStatus>);
        }

        public IObservable<CosignatureSignedTransaction> CosignatureAdded(Address address)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));

            SubscribeToChannel(string.Concat("cosignature/", address.Plain));

            return _subject.Where(e => JObject.Parse(e).Properties().ToArray().Any(i => i.Value.ToString().Contains("cosignature")))
                .Select(ObjectComposer.GenerateObject<CosignatureSignedTransaction>);
        }

        private bool TransactionFromAddress(Transaction transaction, Address address)
        {
            var transactionFromAddress = TransactionHasSignerOrReceptor(transaction, address);

            if (!transactionFromAddress && transaction.TransactionType.GetValue() == TransactionTypes.Types.AGGREGATE_COMPLETE.GetValue() && ((AggregateTransaction)transaction).Cosignatures != null)
            {
                transactionFromAddress = ((AggregateTransaction)transaction).Cosignatures.Any(e => Address.CreateFromPublicKey(e.Signer.PublicKey, address.NetworkByte).Plain == address.Plain);
            }


            return transactionFromAddress;
        }

        private bool TransactionHasSignerOrReceptor(Transaction transaction, Address address)
        {
            var isReceptor = false;

            if (transaction.TransactionType.GetValue() == TransactionTypes.Types.TRANSFER.GetValue())
            {
                isReceptor = ((TransferTransaction)transaction).Address.Plain == address.Plain;
            }

            return Address.CreateFromPublicKey(transaction.Signer.PublicKey, address.NetworkByte).Plain == address.Plain || isReceptor;
        }

        public void Close()
        {
            ClientSocket.Abort();
            LoopReads.Dispose();
        }

        public string GetUid()
        {
            return Uid.UID;
        }
    }
}
