using Coppery;
using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Model;
using io.nem2.sdk.src.Model.Accounts;
using io.nem2.sdk.src.Model.Transactions;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Text.Json.Nodes;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Clients.Listeners
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
            public ExtendedBroadcastStatus Data { get; set; }
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

            return _subject.Where(e => Composer.GenerateObject<SocketTopic>(JsonNode.Parse(e)).Topic == "block")  
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

            return _subject.Where(e => Composer.GenerateObject<SocketTopic>( e).Topic == "status/" + address.Plain)         
                .Select(e => Composer.GenerateObject<BroadcastStatus>(e));
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
