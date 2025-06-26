using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Text.Json.Nodes;
using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Export;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

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

        public class SocketTopic
        {
            public string Topic { get; set; }
            public ExtendedBroadcastStatus Data { get; set; }
        }

        public class WebsocketUID
        {
            public string Uid { get; set; }
        }

        public Listener(string domain, int port = 3000)
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
            var encoded = Encoding.UTF8.GetBytes(string.Concat("{ \"uid\": \"", Uid.Uid, "\", \"subscribe\":\"", channel, "\"}"));

            var buffer = new ArraySegment<byte>(encoded, 0, encoded.Length);

            ClientSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }
      
        public IObservable<BlockInfo> NewBlock()
        {
            SubscribeToChannel("block");

            return _subject.Where(e => ObjectComposer.GenerateObject<SocketTopic>(e).Topic == "block")  
               .Select(ReturnSocketBlockResponse);         
        }

        public IObservable<TransactionData> ConfirmedTransactionsGiven(Address address)
        {
            SubscribeToChannel(string.Concat("confirmedAdded/", address.Plain));

            return _subject.Where(e => ObjectComposer.GenerateObject<SocketTopic>(e).Topic ==  "confirmedAdded")
               .Select(ReturnSocketTransactionResponse);         
            
        }

        public IObservable<TransactionData> UnconfirmedTransactionsAdded(Address address)
        {
            SubscribeToChannel(string.Concat("unconfirmedAdded/", address.Plain));

            return _subject.Where(e => ObjectComposer.GenerateObject<SocketTopic>(e).Topic == "unconfirmedAdded")
                 .Select(ReturnSocketTransactionResponse);
        }

        public IObservable<TransactionData> UnconfirmedTransactionsRemoved(Address address)
        {
            SubscribeToChannel(string.Concat("unconfirmedRemoved/", address.Plain));

            return _subject.Where(e => ObjectComposer.GenerateObject<SocketTopic>(e).Topic == "unconfirmedRemoved")
                 .Select(ReturnSocketTransactionResponse);
        }

        public IObservable<TransactionData> AggregateBondedAdded(Address address)
        {
            SubscribeToChannel(string.Concat("partialAdded/", address.Plain));

            return _subject.Where(e => ObjectComposer.GenerateObject<SocketTopic>(e).Topic == "partialAdded")
                .Select(ReturnSocketTransactionResponse);
        }

        public IObservable<TransactionData> AggregateBondedRemoved(Address address)
        {
            SubscribeToChannel(string.Concat("partialRemoved/", address.Plain));

            return _subject.Where(e => ObjectComposer.GenerateObject<SocketTopic>(e).Topic == "partialRemoved")
                 .Select(ReturnSocketTransactionResponse);
        }

        private BlockInfo ReturnSocketBlockResponse(string data)
        {
            return ObjectComposer.GenerateObject<BlockInfo>(JsonObject.Parse(data)["data"].ToString()); 
        }

        private TransactionData ReturnSocketTransactionResponse(string data)
        {
            return ResponseFilters<TransactionData>.FilterSingle(JsonObject.Parse(data)["data"].ToString());
        }

        public IObservable<SocketTopic> GetTransactionStatus(Address address)
        {
            SubscribeToChannel(string.Concat("status/", address.Plain));

            return _subject.Where(e => ObjectComposer.GenerateObject<SocketTopic>(e).Topic == "status/" + address.Plain)         
                .Select(ObjectComposer.GenerateObject<SocketTopic>);
        }

        public IObservable<CosignatureSignedTransaction> CosignatureAdded(Address address)
        {
            SubscribeToChannel(string.Concat("cosignature/", address.Plain));

            return _subject.Where(e => ObjectComposer.GenerateObject<SocketTopic>(e).Topic == "cosignature")
                .Select(ObjectComposer.GenerateObject<CosignatureSignedTransaction>);
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

        private bool TransactionFromAddress(Transaction transaction, Address address)
        {
            var transactionFromAddress = TransactionHasSignerOrReceptor(transaction, address);

            if (!transactionFromAddress && transaction.TransactionType.GetValue() == TransactionTypes.Types.AGGREGATE_COMPLETE.GetValue() && ((AggregateTransaction)transaction).Cosignatures != null)
            {
                transactionFromAddress = ((AggregateTransaction)transaction).Cosignatures.Any(e => Address.CreateFromPublicKey(e.Signer.PublicKey, address.NetworkByte).Plain == address.Plain);
            }

            return transactionFromAddress;
        }     
    }
}
