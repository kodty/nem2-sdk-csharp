using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Model.Network;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration_Tests
{
    public class RestrictionRequests
    {
        [SetUp]
        public async Task SetUp()
        {
        }

        [Test, Timeout(20000)]
        public async Task GetAccountMosaicRestriction()
        {
            var client = new AccountHttp(HttpSetUp.Node, HttpSetUp.Port);

            var acc = new PublicAccount("C807BE28855D0C87A8A2C032E51790CCB9158C15CBACB8B222E27DFFFEB3697D", NetworkType.Types.MAIN_NET);

            var restriction = await client.GetAccountRestriction(acc.Address.Plain);

            Assert.That(Address.CreateFromHex(restriction.AccountRestrictions.Address).Plain, Is.EqualTo(acc.Address.Plain));
            Assert.That(restriction.AccountRestrictions.Restrictions[0].RestrictionFlags, Is.EqualTo(2));
            Assert.That(restriction.AccountRestrictions.Restrictions[0].Values[0], Is.EqualTo("6BED913FA20223F8"));
        }

        [Test, Timeout(20000)]
        public async Task SearchAccountMosaicRestriction()
        {
            var client = new TransactionHttp("75.119.150.108", 3000);

            var acc = new PublicAccount("C807BE28855D0C87A8A2C032E51790CCB9158C15CBACB8B222E27DFFFEB3697D", NetworkType.Types.MAIN_NET);

            var transaction = await client.GetConfirmedTransaction("30FA71E6D1E34DF1E430A07E1B0817BED9A4ED6B0245B7471B0557380A700E1B");

            var restriction = (EmbeddedAccountMosaicRestriction)((Aggregate)transaction.Transaction).Transactions[1].Transaction;

            Assert.That(restriction.RestrictionFlags, Is.EqualTo(2));
            Assert.That(restriction.RestrictionAdditions[0], Is.EqualTo("6BED913FA20223F8"));
            Assert.That(restriction.RestrictionDeletions.Count, Is.EqualTo(0)); // flag
        }
    }
}
