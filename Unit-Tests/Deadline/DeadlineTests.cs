using io.nem2.sdk.src.Model2;
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
    }
}
