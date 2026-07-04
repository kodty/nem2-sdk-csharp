namespace io.nem2.sdk.src.Infrastructure.HttpRepositories
{
    public class ExtendedHttpResponseMessege<T>
    {
        public HttpResponseMessage Response { get; set; }
        public T ComposedResponse { get; set; }
    }
}
