using System.Diagnostics;
using io.nem2.sdk.Model.Transactions;

namespace Unit_Tests.DeadlineTests
{
    internal class DeadlineTests
    {

        [Test]
        public void TestNetDeadline()
        {

            var deadline = new Deadline(1000);
            var deadline2 = Deadline.AddHours(1000);

            Assert.That(deadline.Ticks, Is.EqualTo(deadline2.Ticks));
            Debug.Write(deadline.Ticks);
        }
    }
}
