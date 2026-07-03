using Coppery;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Model;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Text.Json.Nodes;

namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    public class HttpRouter
    {
        internal HttpClient Client { get; }

        internal string Host { get; set; }

        internal int Port { get; set; }

        internal ObjectComposer Composer { get; set; }

        protected HttpRouter(string host, int port)
        {
            if (string.IsNullOrEmpty(host)) throw new ArgumentException("Value cannot be null or empty.", nameof(host));

            Host = host;
            Port = port;
            Client = new HttpClient();  
            Composer = new ObjectComposer(TypeSerializationCatalog.CustomTypes, GetTransactionType);
        }

        internal Uri GetUri(object[] segs)
        {
            var uri = new UriBuilder(Host)
            {
                Port = Port,
                Path = "/" + String.Join("/", segs)
            };

            Debug.WriteLine(uri.Uri);

            return uri.Uri;
        }

        internal Uri GetUri(string[] segs, QueryModel queryModel)
        {
           var uri = new UriBuilder(Host)
            {
                Port = Port,
                Path = "/" + String.Join("/", segs),
                Query = queryModel.ReturnPathParams()
            };

            Debug.WriteLine(uri.Uri);

            return uri.Uri;
        }

        internal IObservable<ExtendedHttpResponseMessege<T>> HttpGetAsync<T>(object[] path)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(path)))
                 .Select(e => FormResponse(ExtendResponse<T>(e)));
        }

        internal IObservable<ExtendedHttpResponseMessege<T>> HttpGetAsync<T>(QueryModel queryModel, string[] path)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(path, queryModel)))
                 .Select(e => FormResponse(ExtendResponse<T>(e)));
        }

        internal IObservable<ExtendedHttpResponseMessege<T[]>> HttpPostAsync<T>(string[] path, HttpContent content)
        {
            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(path), content))
                  .Select(e => FormResponse(ExtendResponse<T[]>(e)));
        }

        internal static Type GetTransactionType(string t, bool embedded = false)
        {
            var type = (ushort)JsonObject.Parse(t)
                                      .AsObject()["transaction"]["type"];

            if (type == 16718)
            {
                type += (ushort)JsonObject.Parse(t)
                                      .AsObject()["transaction"]["registrationType"];
            }

            return embedded ? type.GetEmbeddedTypeValue() : type.GetTypeValue();
        }

        internal static ExtendedHttpResponseMessege<T> ExtendResponse<T>(HttpResponseMessage msg)
        {
            var extendedResponse = new ExtendedHttpResponseMessege<T>();

            extendedResponse.Response = msg;

            return extendedResponse;
        }
       
        internal ExtendedHttpResponseMessege<T[]> FormResponse<T>(ExtendedHttpResponseMessege<T[]> extendedResponse)
        {
            var objs = JsonNode.Parse(extendedResponse.Response.Content.ReadAsStringAsync().Result);

            var values = new T[objs.AsArray().Count];
            
            for (var x = 0; x < objs.AsArray().Count;  x++)
            {
                values[x] = Composer.GenerateObject(typeof(T), objs.AsArray()[x]);
            }

            extendedResponse.ComposedResponse = values;

            return extendedResponse;
        }

        internal ExtendedHttpResponseMessege<T> FormResponse<T>(ExtendedHttpResponseMessege<T> extendedResponse)
        {

            if (extendedResponse.Response.IsSuccessStatusCode)
                extendedResponse.ComposedResponse = Composer.GenerateObject<T>(extendedResponse.Response.Content.ReadAsStringAsync().Result);

            return extendedResponse;
        }

        internal T ComposeTransaction<T>(string data, bool embedded = false)
        {
            return ComposeTransaction(typeof(T), data, embedded);
        }

        internal dynamic ComposeTransaction(Type genType, string data, bool embedded = false)
        {
            var tx = JsonObject.Parse(data).AsObject();

            var type = GetTransactionType(data, embedded);

            dynamic shell = Composer.GenerateObject(genType, tx.AsObject());

            shell.Transaction = Composer.GenerateObject(type, tx["transaction"].AsObject());

            return shell;
        }
    }
}
