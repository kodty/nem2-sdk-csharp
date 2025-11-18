using Coppery;
using io.nem2.sdk.src.Infrastructure.HttpExtension;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Model;
using System.Diagnostics;
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

        public string OverrideEnsureSuccessStatusCode(HttpResponseMessage r)
        {
            var result = r.Content.ReadAsStringAsync().Result;

            if (!r.IsSuccessStatusCode)
                throw new HttpRequestException(r.Content.ReadAsStringAsync().Result);

            return result;
        }

        public static Type GetTransactionType(string t, bool embedded = false)
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

        internal ExtendedHttpResponseMessage<List<T>> FormListResponse<T>(HttpResponseMessage msg)
        {
            var extendedResponse = new ExtendedHttpResponseMessage<List<T>>();

            extendedResponse.Response = msg;

            if (msg.IsSuccessStatusCode)
                extendedResponse.ComposedResponse = Composer.FilterEvents<T>(msg.Content.ReadAsStringAsync().Result, "data");

            return extendedResponse;
        }

        internal ExtendedHttpResponseMessage<T> FormResponse<T>(HttpResponseMessage msg)
        {
            var extendedResponse = new ExtendedHttpResponseMessage<T>();

            extendedResponse.Response = msg;

            if (msg.IsSuccessStatusCode)
                extendedResponse.ComposedResponse = Composer.GenerateObject<T>(msg.Content.ReadAsStringAsync().Result);

            return extendedResponse;
        }
    }
}
