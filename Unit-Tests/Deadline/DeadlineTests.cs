using io.nem2.sdk.src.Model;
using System.Diagnostics;


namespace Unit_Tests.DeadlineTests
{
    internal class DeadlineTests
    {

        [Test]
        public void TestNetDeadline()
        {
            var deadline = new Deadline(0);

            Debug.WriteLine(deadline.Ticks);
        }

        [Test]
        public void TestNetDeadlineAutoVsManual()
        {
            var deadline = Deadline.AutoDeadline("153.126.132.254", 3000);
            var deadline2 = Deadline.AddHours(23);
            var deadline3 = Deadline.AddHours(1380);
            var deadline4 = Deadline.AddMinutes(82800);

            Assert.That(deadline.Ticks, Is.EqualTo(deadline.Ticks));
        }
    }
}
