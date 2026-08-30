namespace Coppery
{
    public class ExtendedHttpResponseMessege<T>
    {
        public HttpResponseMessage Response { get; set; }
        public T ComposedResponse { get; set; }
    }
}
