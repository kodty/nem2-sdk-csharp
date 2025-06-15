using System.Diagnostics;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Model.Network;


namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    public class HttpRouter
    {
        internal HttpClient Client { get; }

        internal string Host { get; set; }

        internal int Port { get; set; }

        private NetworkType.Types _NetworkType { get; set; }

        protected HttpRouter(string host, int port)
        {
            if (string.IsNullOrEmpty(host)) throw new ArgumentException("Value cannot be null or empty.", nameof(host));

            Host = host;
            Port = port;
            Client = new HttpClient();                    
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
            Debug.WriteLine(result);
            if (!r.IsSuccessStatusCode)
                throw new HttpRequestException(r.Content.ReadAsStringAsync().Result);

            return result;
        }
    }
}
