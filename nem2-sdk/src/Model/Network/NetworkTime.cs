
namespace io.nem2.sdk.Model.Network
{
    public static class NetworkTime
    {
        public static ulong EpochTimeInMilliSeconds()
        {
            var d = new DateTime(2016, 4, 1).ToUniversalTime();

            var n = DateTime.Now.ToUniversalTime();
            
            var span = n - d;

            return (ulong)span.TotalMilliseconds;
        }
    }
}
