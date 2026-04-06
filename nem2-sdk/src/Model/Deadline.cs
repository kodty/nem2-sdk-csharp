using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using System.Reactive.Linq;

namespace io.nem2.sdk.src.Model
{
    public class Deadline
    {
        private static DateTime TestNet = new DateTime(2022, 10, 31, 21, 07, 47);

        private static DateTime MainNet = new DateTime(2021, 03, 16, 00, 06, 25);

        private static DateTime CustomDateTime { get; set; }

        public enum NetworkType
        {
            MAINNET = 0,
            TESTNET = 1,
            CUSTOMNET
        }
        internal DateTime EpochDate { get; set; }

        internal DateTime Date { get; set; }

        public ulong Ticks { get; set; }  
       
        public DateTime GetDateTime()
        {
            return Date;
        }

        public Deadline(NetworkType type, TimeSpan time)
        {
            switch (type)
            {
                case NetworkType.MAINNET:
                    EpochDate = MainNet;
                    break;
                case NetworkType.TESTNET:
                    EpochDate = TestNet;
                    break;
                case NetworkType.CUSTOMNET:
                    EpochDate = CustomDateTime;
                    break;
            }

            var now = DateTime.UtcNow;

            var deadline = now - EpochDate;

            Ticks = (ulong)deadline.Add(time).TotalMilliseconds;
        }

        public Deadline(ulong timestamp, TimeSpan time)
        {
            Ticks = (ulong)(timestamp + time.TotalMilliseconds);
        }

        public Deadline(NetworkType type, int hours) : this(type, TimeSpan.FromHours(hours)) { }

        public static Deadline AddHours(int hours, NetworkType type = NetworkType.TESTNET)
        {
            return new Deadline(type, TimeSpan.FromHours(hours));
        }

        public static Deadline AddMinutes(int mins, NetworkType type = NetworkType.TESTNET)
        {
            return new Deadline(type, TimeSpan.FromMinutes(mins));
        }

        public static Deadline AddSeconds(int seconds, NetworkType type = NetworkType.TESTNET)
        {
            return new Deadline(type, TimeSpan.FromSeconds(seconds));
        }

        public static Deadline AutoDeadline(string node, int port)
        {
            var client = new NodeHttp(node, port);

            var timeStamp = client.GetNodeTime().Wait();

            return new Deadline(timeStamp.ComposedResponse.CommunicationTimestamps.SendTimestamp, TimeSpan.FromHours(23));
        }
    }
}
