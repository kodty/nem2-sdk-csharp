
namespace io.nem2.sdk.Model.Transactions
{
    public class Deadline
    {
        internal DateTime EpochDate { get; set; }

        internal ulong Ticks { get; }   

        public Deadline(TimeSpan time)
        {
            EpochDate = new DateTime(2022, 10, 31, 21, 07, 47).ToUniversalTime();

            var now = DateTime.Now.ToUniversalTime();

            var deadline = now - EpochDate;

            Ticks = (ulong)deadline.Add(time).TotalMilliseconds;
        }

        public Deadline(DateTime dateTime, TimeSpan time)
        {
            EpochDate = dateTime.ToUniversalTime();

            var now = DateTime.Now.ToUniversalTime();

            var deadline = now - EpochDate;

            Ticks = (ulong)deadline.Add(time).TotalMilliseconds;
        }

        public Deadline(ulong ticks)
        {
            EpochDate = new DateTime(2022, 10, 31, 21, 07, 47).ToUniversalTime();
        
            Ticks = ticks;
        }

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
    }
}
