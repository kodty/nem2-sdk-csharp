using io.nem2.sdk.Infrastructure;
using io.nem2.sdk.Infrastructure.Responses;
using io.nem2.sdk.Model;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Transactions;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Text.Json.Nodes;

namespace Coppery
{
    public class Listener : HttpRouter
    {
        public  WebsocketUID Uid { get; set; }

        private ClientWebSocket ClientSocket { get; }

        private Task LoopReads { get; set; }


        private readonly Subject<string> _subject = new Subject<string>();

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

                Uid = JsonSerializerExtension.Deserialize<WebsocketUID>(input);

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

            return _subject.Where(e => JsonSerializerExtension.Deserialize<SocketTopic>(e).Topic  == "block")  
               .Select(ReturnSocketBlockResponse);         
        }

        public IObservable<TransactionData> ConfirmedTransactionsGiven(Address address)
        {
            SubscribeToChannel(string.Concat("confirmedAdded/", address.Plain));

            return _subject.Where(e => JsonSerializerExtension.Deserialize<SocketTopic>(e).Topic == "confirmedAdded/" + address.Plain)
               .Select(e => ReturnSocketTransactionResponse(e));         
            
        }

        public IObservable<TransactionData> UnconfirmedTransactionsAdded(Address address)
        {
            SubscribeToChannel(string.Concat("unconfirmedAdded/", address.Plain));

            return _subject.Where(e => JsonSerializerExtension.Deserialize<SocketTopic>(e).Topic == "unconfirmedAdded/" + address.Plain)
                 .Select(e => ReturnSocketTransactionResponse(e));
        }

        public IObservable<TransactionData> UnconfirmedTransactionsRemoved(Address address)
        {
            SubscribeToChannel(string.Concat("unconfirmedRemoved/", address.Plain));

            return _subject.Where(e => JsonSerializerExtension.Deserialize<SocketTopic>(e).Topic == "unconfirmedRemoved/" + address.Plain)
                 .Select(e => ReturnSocketTransactionResponse(e));
        }

        public IObservable<TransactionData> AggregateBondedAdded(Address address)
        {
            SubscribeToChannel(string.Concat("partialAdded/", address.Plain));

            return _subject.Where(e => JsonSerializerExtension.Deserialize<SocketTopic>(e).Topic == "partialAdded/" + address.Plain)
                .Select(e => ReturnSocketTransactionResponse(e));
        }

        public IObservable<TransactionData> AggregateBondedRemoved(Address address)
        {
            SubscribeToChannel(string.Concat("partialRemoved/", address.Plain));

            return _subject.Where(e => JsonSerializerExtension.Deserialize<SocketTopic>(e).Topic == "partialRemoved/" + address.Plain)
                 .Select(e => ReturnSocketTransactionResponse(e));
        }

        private BlockInfo ReturnSocketBlockResponse(string data)
        {
            return JsonSerializerExtension.Deserialize<BlockInfo>(JsonNode.Parse(data)["data"]);

        }  
        private TransactionData ReturnSocketTransactionResponse(string data)
        {
            return JsonSerializerExtension.Deserialize<TransactionData>(JsonNode.Parse(data)["data"].ToString());
        }

        public IObservable<BroadcastStatus> GetTransactionStatus(Address address)
        {
            SubscribeToChannel(string.Concat("status/", address.Plain));

            return _subject.Where(e => JsonSerializerExtension.Deserialize<SocketTopic>(e).Topic == "status/" + address.Plain)
                .Select(e => JsonSerializerExtension.Deserialize<BroadcastStatus>(e));
        }

        public IObservable<CosignatureSignedTransaction> CosignatureAdded(Address address)
        {
            SubscribeToChannel(string.Concat("cosignature/", address.Plain));

            return _subject.Where(e => JsonSerializerExtension.Deserialize<SocketTopic>(e).Topic == "cosignature/" + address.Plain)
                .Select(e => JsonSerializerExtension.Deserialize<CosignatureSignedTransaction>(e));
        }

        private bool TransactionHasSignerOrReceptor(VerifiableTransaction transaction, Address address)
        {
            var isReceptor = false;

            if (transaction.Type == TransactionTypes.Types.TRANSFER.GetValue())
            {
                isReceptor = Address.EncodeAddress(((TransferTransaction_V1)((SimpleTransaction)transaction).TransactionExtension).Recipient) == address.Plain;
            }

            return Address.CreateFromPublicKey(transaction.Signer.ToHex(), address.NetworkByte).Plain == address.Plain || isReceptor;
        }

        public void Close()
        {
            ClientSocket.Abort();
            LoopReads.Dispose();
        }    
    }
}
