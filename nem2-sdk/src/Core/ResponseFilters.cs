using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Text.Json.Nodes;

namespace io.nem2.sdk.src.Export
{
    internal class ResponseFilters<T> where T : class
    {
        internal object[] Args { get; set; }

        public ResponseFilters(object[] args) {
            Args = args;
        }

        internal List<T> FilterEvents(string data, string path = null)
        {
            var evs = path == null ? JsonNode.Parse(data) : JsonNode.Parse(data)[path];

            List<T> events = new List<T>();

            foreach (var e in evs.AsArray())
            {
                events.Add((T)new ObjectComposer(Args).GenerateObject(typeof(T), e.AsObject()));
            }

            return events;
        }

        internal List<T> FilterTransactions(Func<string, bool, Type> GetTransactionType, string data, string path = null, bool embedded = false)
        {
            var tx = path == null ? JsonNode.Parse(data).AsArray() : JsonNode.Parse(data)[path];

            List<T> txs = new List<T>();

            foreach (var t in tx.AsArray())
            {
                txs.Add(FilterSingle(GetTransactionType, t.ToString(), embedded));
            }

            return txs;
        }

        internal T FilterSingle(Func<string, bool, Type> GetTransactionType, string data, bool embedded = false)
        {
            var tx = JsonObject.Parse(data).AsObject();

            var composer = new ObjectComposer(Args, GetTransactionType);

            var type = GetTransactionType(data, embedded);

            dynamic shell = composer.GenerateObject<T>(tx.ToString());
         
            shell.Transaction = composer.GenerateObject(type, tx["transaction"].AsObject());

            return shell;         
        }
    } 
}

