using io.nem2.sdk.Model;

namespace Unit_Tests.DeadlineTests
{
    internal class DeadlineTests
    {

        [Test]
        public void TestNetDeadlineAutoVsManual()
        {
            var deadline = new Deadline(NetworkType.Types.TEST_NET, 23);
            var deadline2 = Deadline.AddHours(23);
            var zeroDeadline = Deadline.AddHours(0);
            var epochDate = new DateTime(2022, 10, 31, 21, 07, 47);
            var now = DateTime.UtcNow;

            Assert.That(deadline.Ticks - deadline2.Ticks < 2);
            Assert.That((ulong)(now - epochDate).TotalMilliseconds == zeroDeadline.Ticks);
        }
    }
}
