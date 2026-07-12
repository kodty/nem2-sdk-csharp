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

            Assert.That(deadline.Ticks, Is.EqualTo(deadline2.Ticks));
        }
    }
}
