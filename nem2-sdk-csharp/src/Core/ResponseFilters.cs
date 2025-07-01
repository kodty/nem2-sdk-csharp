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

        internal List<T> FilterTransactions(Func<string, Type> GetTransactionType, string data, string path = null)
        {
            var tx = path == null ? JsonNode.Parse(data).AsArray() : JsonNode.Parse(data)[path];

            List<T> txs = new List<T>();

            foreach (var t in tx.AsArray())
            {
                txs.Add(FilterSingle2(GetTransactionType(t.ToString()), t.ToString()));
            }

            return txs;
        }

        internal string GetSpecifiedTx(JsonObject ob)
        {
            return ob["transaction"].ToString();
        }

        internal T FilterSingle2(Type type, string data)
        {
            var tx = JsonObject.Parse(data).AsObject();

            dynamic shell = new ObjectComposer(Args).GenerateObject<T>(tx.ToString());
         
            shell.Transaction = new ObjectComposer(Args).GenerateObject(type, JsonObject.Parse(data)["transaction"]);

            return shell;         
        }

        internal T FilterSingle(string data)
        {
            var tx = JsonObject.Parse(data).AsObject();

            dynamic shell = new ObjectComposer(Args).GenerateObject<T>(tx.ToString());

            var type = ((ushort)tx["transaction"]["type"]).GetEmbeddedTypeValue();

            return FilterSingle2(type, data);
        }
    } 
}

