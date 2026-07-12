namespace io.nem2.sdk.Model
{
    public class Deadline
    {
        private static DateTime TestNet = new DateTime(2022, 10, 31, 21, 07, 47);

        private static DateTime MainNet = new DateTime(2021, 03, 16, 00, 06, 25);

        internal DateTime EpochDate { get; set; }

        internal DateTime Date { get; set; }

        public ulong Ticks { get; set; }  
       
        public DateTime GetDateTime()
        {
            return Date;
        }

        public Deadline(NetworkType.Types type, TimeSpan time)
        {
            switch (type)
            {
                case NetworkType.Types.MAIN_NET:
                    throw new Exception("Network unsupported code not tested sufficiently. Supported in development branch at your own risk"); //EpochDate = MainNet;
            
                case NetworkType.Types.TEST_NET:
                    EpochDate = TestNet;
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

        public Deadline(NetworkType.Types type, int hours) : this(type, TimeSpan.FromHours(hours)) { }

        public static Deadline AddHours(int hours, NetworkType.Types type = NetworkType.Types.TEST_NET)
        {
            return new Deadline(type, TimeSpan.FromHours(hours));
        }

        public static Deadline AddMinutes(int mins, NetworkType.Types type = NetworkType.Types.TEST_NET)
        {
            return new Deadline(type, TimeSpan.FromMinutes(mins));
        }

        public static Deadline AddSeconds(int seconds, NetworkType.Types type = NetworkType.Types.TEST_NET)
        {
            return new Deadline(type, TimeSpan.FromSeconds(seconds));
        }
    }
}
