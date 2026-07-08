using Coppery;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Text;
using System.Text.Json;
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
            Composer = new ObjectComposer();
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
            => Observable.FromAsync(async ar => await Client.GetAsync(GetUri(path)))
                 .Select(e => FormResponse(ExtendResponse<T>(e)));

        internal IObservable<ExtendedHttpResponseMessege<T>> HttpGetAsync<T>(QueryModel queryModel, string[] path)
            => Observable.FromAsync(async ar => await Client.GetAsync(GetUri(path, queryModel)))
                 .Select(e => FormResponse(ExtendResponse<T>(e)));

        internal IObservable<ExtendedHttpResponseMessege<T[]>> HttpPostAsync<T>(string[] path, object content)
            => Observable.FromAsync(async ar => await Client.PostAsync(GetUri(path), new StringContent(
                    JsonSerializer.Serialize(content),
                    Encoding.UTF8,
                    "application/json")))
                  .Select( e => FormResponse(ExtendResponse<T[]>(e)));

        internal static ExtendedHttpResponseMessege<T> ExtendResponse<T>(HttpResponseMessage msg)
        {
            if (!msg.IsSuccessStatusCode)
            {
                throw new Exception(msg.Content.ReadAsStringAsync().Result);
            }

            return new ExtendedHttpResponseMessege<T>()
            {
                Response = msg

            };
        }
       
        internal ExtendedHttpResponseMessege<T[]> FormResponse<T>(ExtendedHttpResponseMessege<T[]> extendedResponse)
        {
            var objs = JsonNode.Parse(extendedResponse.Response.Content.ReadAsStringAsync().Result);

            var values = new T[objs.AsArray().Count];
            
            for (var x = 0; x < objs.AsArray().Count;  x++)
            {
                values[x] = Composer.GenerateObject<T>(objs.AsArray()[x]);
            }

            extendedResponse.ComposedResponse = values;

            return extendedResponse;
        }

        internal ExtendedHttpResponseMessege<T> FormResponse<T>(ExtendedHttpResponseMessege<T> extendedResponse)
        {
            if (extendedResponse.Response.IsSuccessStatusCode)
                extendedResponse.ComposedResponse = Composer.GenerateObject<T>(JsonNode.Parse(extendedResponse.Response.Content.ReadAsStringAsync().Result));

            return extendedResponse;
        }
    }
}
