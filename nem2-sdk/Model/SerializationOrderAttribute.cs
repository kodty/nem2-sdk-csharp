namespace io.nem2.sdk.Model
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class OrderAttribute : Attribute
    {
        private readonly int order_;
        public OrderAttribute(int order = 0)
        {
            order_ = order;
        }

        public int Order { get { return order_; } }
    }
}
