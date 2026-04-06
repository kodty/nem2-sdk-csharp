using Integration_Tests;
using io.nem2.sdk.src.Model;
using System.Diagnostics;


namespace Unit_Tests.DeadlineTests
{
    internal class DeadlineTests
    {

        [Test]
        public void TestNetDeadlineAutoVsManual()
        {
            var deadline = Deadline.AutoDeadline(HttpSetUp.TestnetNode, 3000);
            var deadline2 = Deadline.AddHours(23);

            Assert.That(deadline.Ticks, Is.EqualTo(deadline2.Ticks));
        }
    }
}
