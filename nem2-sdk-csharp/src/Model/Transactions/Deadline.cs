namespace io.nem2.sdk.Model.Transactions
{
    public class Deadline
    {
        internal DateTime EpochDate { get; set; }

        internal DateTime Date { get; set; }

        public ulong Ticks { get; }  
       
        public DateTime GetDateTime()
        {
            return Date;
        }

        private TimeSpan GetTimeSinceEpoch()
        {
            EpochDate = new DateTime(2022, 10, 31, 21, 07, 47).ToUniversalTime();

            return DateTime.Now.ToUniversalTime().Subtract(EpochDate);
        }

        public Deadline(TimeSpan time)
        {
            var deadline = GetTimeSinceEpoch().Add(time);

            Date = EpochDate.Add(deadline).ToUniversalTime();

            Ticks = (ulong)Date.Subtract(EpochDate).TotalMilliseconds; 
        }

        public Deadline(int hours) : this(TimeSpan.FromHours(hours)) { }

        public static Deadline AddHours(int hours)
        {
            return new Deadline(hours);
        }

        public static Deadline AddMinutes(int mins)
        {
            return new Deadline(TimeSpan.FromMinutes(mins));
        }

        public static Deadline AddSeconds(int seconds)
        {
            return new Deadline(TimeSpan.FromSeconds(seconds));
        }
    }
}
