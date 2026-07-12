using Coppery;
using io.nem2.sdk.Infrastructure.HttpClients;
using io.nem2.sdk.Infrastructure.Interfaces;
using io.nem2.sdk.Infrastructure.Responses;
using io.nem2.sdk.Model;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Transactions;

using io.nem2.sdk.Utils;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Text.Json.Nodes;

namespace io.nem2.sdk.Infrastructure
{
    public class Listener : HttpRouter
    {
        public  WebsocketUID Uid { get; set; }

        private ClientWebSocket ClientSocket { get; }

        private Task LoopReads { get; set; }


        private readonly Subject<string> _subject = new Subject<string>();

        private TransactionHttp TransactionHttpClient { get; set; }

        public class SocketTopic
        {
            public string Topic { get; set; }
        }

        public class WebsocketUID
        {
            public string Uid { get; set; }
        }

        public Listener(string domain, int port = 3000) : base(domain, port)
        {
            base.Composer.Function = TransactionTypes.CustomFunction;

            ClientSocket = new ClientWebSocket();
		}

        public IObservable<bool> Open()
        {
            return Observable.Start(() =>
            {
                ClientSocket.ConnectAsync(new Uri(string.Concat("ws://", Host, ":", Port, "/ws")), CancellationToken.None)
                    .GetAwaiter()
                    .GetResult();

                var input = JsonNode.Parse(ReadSocket().Result);

                Uid = Composer.GenerateObject<WebsocketUID>(input);

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
            var encoded = Encoding.UTF8.GetBytes(string.Concat("{ \"uid\": \"", Uid.Uid, "\", \"subscribe\":\"", channel, "\"}"));

            var buffer = new ArraySegment<byte>(encoded, 0, encoded.Length);

            ClientSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }
      
        public IObservable<BlockInfo> NewBlock()
        {
            SubscribeToChannel("block");
            
            return _subject.Where(e => Composer.GenerateObject<SocketTopic>(e).Topic == "block")  
               .Select(ReturnSocketBlockResponse);         
        }

        public IObservable<TransactionData> ConfirmedTransactionsGiven(Address address)
        {
            SubscribeToChannel(string.Concat("confirmedAdded/", address.Plain));

            return _subject.Where(e => Composer.GenerateObject<SocketTopic>(e).Topic == "confirmedAdded")
               .Select(e => ReturnSocketTransactionResponse(e));         
            
        }

        public IObservable<TransactionData> UnconfirmedTransactionsAdded(Address address)
        {
            SubscribeToChannel(string.Concat("unconfirmedAdded/", address.Plain));

            return _subject.Where(e => Composer.GenerateObject<SocketTopic>(e).Topic == "unconfirmedAdded")
                 .Select(e => ReturnSocketTransactionResponse(e));
        }

        public IObservable<TransactionData> UnconfirmedTransactionsRemoved(Address address)
        {
            SubscribeToChannel(string.Concat("unconfirmedRemoved/", address.Plain));

            return _subject.Where(e => Composer.GenerateObject<SocketTopic>(e).Topic == "unconfirmedRemoved")
                 .Select(e => ReturnSocketTransactionResponse(e));
        }

        public IObservable<TransactionData> AggregateBondedAdded(Address address)
        {
            SubscribeToChannel(string.Concat("partialAdded/", address.Plain));

            return _subject.Where(e => Composer.GenerateObject<SocketTopic>(e).Topic == "partialAdded")
                .Select(e => ReturnSocketTransactionResponse(e));
        }

        public IObservable<TransactionData> AggregateBondedRemoved(Address address)
        {
            SubscribeToChannel(string.Concat("partialRemoved/", address.Plain));

            return _subject.Where(e => Composer.GenerateObject<SocketTopic>(e).Topic == "partialRemoved")
                 .Select(e => ReturnSocketTransactionResponse(e));
        }

        /*
        {
            "topic": "block",
            "data": {
                "block": {
                    "signature": "491133FB93B7CC54E8779F6CA2C1090C886A089795C2501575369242F33D6B28B809E2FB74C73E46A0E304971675C191783962F9035FFBF0E6F24B660A18950C",
                    "signerPublicKey": "56FDD71F89193034761E5CBFB571491B1B72A7A6E72C5294493F0EFBFE170CBA",
                    "version": 1,
                    "network": 152,
                    "type": 33091,
                    "height": "3538061",
                    "timestamp": "116446812553",
                    "difficulty": "10025721356315",
                    "previousBlockHash": "C51A42682D2F996346C07FDA00B2C48A8777D88B736FE1380E6D2FB3829DC77C",
                    "transactionsHash": "0000000000000000000000000000000000000000000000000000000000000000",
                    "receiptsHash": "1CBA61DFBC8316FAEFB87D49CC98A69E6C386A9EBA9B5B8FDDEC526D2527FCED",
                    "stateHash": "BDD8FB8DDAEF036E417DEC3F5BE3603155324D52E85CE8001B36721DE7F93EF0",
                    "beneficiaryAddress": "9860B7FE54FD99456EF8621397D913D1D3B7CEA97D8A96A6",
                    "feeMultiplier": 0,
                    "proofGamma": "365A9A462D3345F89B9AF2C65B8663DBB63DA2D6B7DA1AB670564E52DC74E2DF",
                    "proofVerificationHash": "B711FFD039C0AD63DB23382EA443F687",
                    "proofScalar": "60258283DF63C5DC86939995F35D2F1484168FC963A42B4BD2C2B40D1522BE0D"
                },
                "meta": {
                    "hash": "82E7E10FCB0818A1581192EFE9D1544DFF0A91320D52469FB4238072D7B6A96A",
                    "generationHash": "C8E2736D88935DEC737496B426F7B7C93BF221A46078283E0AB9A968EDBD73F6"
                }
            }
        }
         */
        private BlockInfo ReturnSocketBlockResponse(string data)
        {
            var input = JsonNode.Parse(JsonNode.Parse(data)["data"].ToString());

            return Composer.GenerateObject<BlockInfo>(input); 
        }

        private TransactionData ReturnSocketTransactionResponse(string data)
        {
            var t = JsonNode.Parse(data)["data"].ToString();

            return Composer.GenerateObject(typeof(TransactionData), t);
        }

        public IObservable<BroadcastStatus> GetTransactionStatus(Address address)
        {
            SubscribeToChannel(string.Concat("status/", address.Plain));

            return _subject.Where(e => Composer.GenerateObject<SocketTopic>(e).Topic == "status/" + address.Plain)
                .Select(e => Composer.GenerateObject<BroadcastStatus>(JsonNode.Parse(e)["data"].ToString()));
        }

        public IObservable<CosignatureSignedTransaction> CosignatureAdded(Address address)
        {
            SubscribeToChannel(string.Concat("cosignature/", address.Plain));

            return _subject.Where(e => Composer.GenerateObject<SocketTopic>(e).Topic == "cosignature")
                .Select(e => Composer.GenerateObject<CosignatureSignedTransaction>(e));
        }

        private bool TransactionHasSignerOrReceptor(Transaction transaction, Address address)
        {
            var isReceptor = false;

            if (transaction.Type == TransactionTypes.Types.TRANSFER.GetValue())
            {
                isReceptor = AddressEncoder.EncodeAddress(((TransferTransaction_V1)transaction).Address) == address.Plain;
            }

            return Address.CreateFromPublicKey(transaction.EntityBody.Signer.ToHex(), address.NetworkByte).Plain == address.Plain || isReceptor;
        }

        public void Close()
        {
            ClientSocket.Abort();
            LoopReads.Dispose();
        }

        //private bool TransactionFromAddress(Transaction1 transaction, Address address)
        //{
        //    var transactionFromAddress = TransactionHasSignerOrReceptor(transaction, address);
        //
        //    if (!transactionFromAddress && transaction.Type == TransactionTypes.Types.AGGREGATE_COMPLETE.GetValue() && ((AggregateTransaction)transaction).Cosignatures != null)
        //    {
        //        transactionFromAddress = ((AggregateTransaction)transaction).Cosignatures.Any(e => Address.CreateFromPublicKey(e.Signer.PublicKey, address.NetworkByte).Plain == address.Plain);
        //    }
        //
        //    return transactionFromAddress;
        //}     
    }
}
