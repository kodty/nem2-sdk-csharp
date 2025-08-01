using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using System.Diagnostics;
using System.Reactive.Linq;

namespace io.nem2.sdk.src.Model
{
    public class Deadline
    {
        internal DateTime EpochDate { get; set; }

        internal DateTime Date { get; set; }

        public ulong Ticks { get; set; }  
       
        public DateTime GetDateTime()
        {
            return Date;
        }

        private TimeSpan GetTimeSinceEpoch()
        {
            EpochDate = new DateTime(2022, 10, 31, 22, 07, 47).ToUniversalTime();
            
            return DateTime.UtcNow.Subtract(EpochDate);
        }

        public Deadline(TimeSpan time)
        {
            var deadline = GetTimeSinceEpoch().Add(time);

            Date = EpochDate.Add(deadline);

            Ticks = (ulong)Date.Millisecond;
        }

        // use with NodeHttp.GetNodeTime() to get timestamp
        public Deadline(ulong timestamp, TimeSpan time)
        {
            Ticks = (ulong)(timestamp + time.TotalMilliseconds);
        }

        public Deadline(int hours) : this(TimeSpan.FromHours(hours)) { }

        public static Deadline AddHours(int hours)
        {
            return new Deadline(TimeSpan.FromHours(hours));
        }

        public static Deadline AddMinutes(int mins)
        {
            return new Deadline(TimeSpan.FromMinutes(mins));
        }

        public static Deadline AddSeconds(int seconds)
        {
            return new Deadline(TimeSpan.FromSeconds(seconds));
        }

        public static Deadline AutoDeadline(string node, int port)
        {
            var client = new NodeHttp(node, port);

            var timeStamp = client.GetNodeTime().Wait();

            return new Deadline(timeStamp.CommunicationTimestamps.SendTimestamp, TimeSpan.FromHours(23));
        }
    }
}
