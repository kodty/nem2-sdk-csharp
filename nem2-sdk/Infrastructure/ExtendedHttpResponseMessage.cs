namespace io.nem2.sdk.Infrastructure
{
    public class ExtendedHttpResponseMessege<T>
    {
        public HttpResponseMessage Response { get; set; }
        public T ComposedResponse { get; set; }
    }
}
