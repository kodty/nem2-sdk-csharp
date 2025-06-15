using System.Diagnostics;
using io.nem2.sdk.Model.Transactions;

namespace Unit_Tests.DeadlineTests
{
    internal class DeadlineTests
    {

        [Test]
        public void TestNetDeadline()
        {
            var deadline = new Deadline(23);

            Assert.That(DateTime.Now.ToUniversalTime(), Is.LessThan(deadline.GetDateTime()));
            Assert.That(DateTime.Now.ToUniversalTime().AddHours(24), Is.GreaterThan(deadline.GetDateTime()));
        }
    }
}
