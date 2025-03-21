using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Model.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration_Tests
{
    internal class EmbeddedTransactions
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test, Timeout(20000)]
        public async Task GetEmbeddedAccountKeyLinkTransaction()
        {
            var client = new TransactionHttp("75.119.150.108", 3000);

            var tx = await client.GetConfirmedTransaction("55EB9659C81600F1760C4C0A4F8A7A5C90A39FCEE36E3165143B8E72BBC709E8");

            var aggregate = (Aggregate)tx.Transaction;

            var Voting = (EmbeddedKeyLink)aggregate.Transactions[2].Transaction;

            Assert.That(Voting.SignerPublicKey, Is.EqualTo("B26D01FC006EAC09B740A3C8F12C1055AE24AFD3268F0364C92D51800FC07361"));
            Assert.That(Voting.LinkedPublicKey, Is.EqualTo("ADE50C62D52F59EDF5559ABD258860786567D936F43ABDAE201A27CDADD260A3"));
            Assert.That(Voting.LinkAction, Is.EqualTo(1));
            Assert.That(Voting.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));

            Assert.That(Voting.Type, Is.EqualTo(TransactionTypes.Types.ACCOUNT_KEY_LINK));
            Assert.That(Voting.Version, Is.EqualTo(1));
        }

        [Test, Timeout(20000)]
        public async Task GetEmbeddedVotingKeyLinkTransaction()
        {
            var client = new TransactionHttp("75.119.150.108", 3000);

            var tx = await client.GetConfirmedTransaction("5C949FA7F9CFBEA30525B79224147D5C575C4232E28CA7CEA760B08E2018047F");

            var aggregate = (Aggregate)tx.Transaction;

            var Voting = (EmbeddedVotingKeyLink)aggregate.Transactions[2].Transaction;

            Assert.That(Voting.SignerPublicKey, Is.EqualTo("E7D93592228CD85854F94DFD3F224AC051759DF027CCF6B62F453107EF2C3692"));
            Assert.That(Voting.LinkedPublicKey, Is.EqualTo("860FBA46061603B80F374E877B2FA673E4AEE7BB61800A80BCF948A7A5601FFF"));
            Assert.That(Voting.LinkAction, Is.EqualTo(1));
            Assert.That(Voting.EndEpoch, Is.EqualTo(360));
            Assert.That(Voting.StartEpoch, Is.EqualTo(1));
            Assert.That(Voting.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
           
            Assert.That(Voting.Type, Is.EqualTo(TransactionTypes.Types.VOTING_KEY_LINK));
            Assert.That(Voting.Version, Is.EqualTo(1));
        }

            [Test, Timeout(20000)]
        public async Task GetEmbeddedKeyLinkTransaction()
        {
            var client = new TransactionHttp("75.119.150.108", 3000);

            var tx = await client.GetConfirmedTransaction("8E7CE907451516EAD51DE86348F51A2950F869A92FEF645CAD2990D5E9BB9121");

            var aggregate = (Aggregate)tx.Transaction;

            var VRF = (EmbeddedKeyLink)aggregate.Transactions[0].Transaction;

            Assert.That(VRF.LinkedPublicKey, Is.EqualTo("F343979EB993481620B8077FAB7E0D5079857C1093CF09856B38F900F214D0D3"));
            Assert.That(VRF.LinkAction, Is.EqualTo(1));
            Assert.That(VRF.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.That(VRF.SignerPublicKey, Is.EqualTo("615ABB16819BDC49EB0DB95121E0A1B52838877128A16C88A3CDE7D7CB3745C3"));
            Assert.That(VRF.Type, Is.EqualTo(TransactionTypes.Types.VRF_KEY_LINK));
            Assert.That(VRF.Version, Is.EqualTo(1));

            var Account = (EmbeddedKeyLink)aggregate.Transactions[1].Transaction;

            Assert.That(Account.LinkedPublicKey, Is.EqualTo("BEB4EC4F827EA284F462217295FA3576C0FFF22821509AA49BAB11367C82111E"));
            Assert.That(Account.LinkAction, Is.EqualTo(1));
            Assert.That(Account.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.That(Account.SignerPublicKey, Is.EqualTo("615ABB16819BDC49EB0DB95121E0A1B52838877128A16C88A3CDE7D7CB3745C3"));
            Assert.That(Account.Type, Is.EqualTo(TransactionTypes.Types.ACCOUNT_KEY_LINK));
            Assert.That(Account.Version, Is.EqualTo(1));

            var Node = (EmbeddedKeyLink)aggregate.Transactions[2].Transaction;

            Assert.That(Node.LinkedPublicKey, Is.EqualTo("70B26E947F57BF0CFCDC4968F73F29534DEB947A864B40CF742BE496E28742D3"));
            Assert.That(Node.LinkAction, Is.EqualTo(1));
            Assert.That(Node.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.That(Node.SignerPublicKey, Is.EqualTo("615ABB16819BDC49EB0DB95121E0A1B52838877128A16C88A3CDE7D7CB3745C3"));
            Assert.That(Node.Type, Is.EqualTo(TransactionTypes.Types.NODE_KEY_LINK));
            Assert.That(Node.Version, Is.EqualTo(1));
        }

        [Test, Timeout(20000)]
        public async Task GetEmbeddedSimpleTransfer()
        {
            var client = new TransactionHttp("75.119.150.108", 3000);

            var tx = await client.GetConfirmedTransaction("BFBD18CE27575CF154826C9ECFE587C472193AB035E8F8E4ABFEB6FE1E53520C");

            var aggregate = (Aggregate)tx.Transaction;

            var Embedded = aggregate.Transactions;

            foreach (var item in Embedded)
            {
                var i = (EmbeddedSimpleTransfer)item.Transaction;

                Assert.That(item.Meta.Index, Is.AnyOf(0, 1));
                Assert.That(item.Meta.Height, Is.EqualTo(1995));
                Assert.That(item.Meta.Timestamp, Is.EqualTo(144382262));
                Assert.That(item.Meta.AggregateId, Is.EqualTo("6644D7ADED4FBE21460AA2E3"));
                Assert.That(item.Meta.AggregateHash, Is.EqualTo("BFBD18CE27575CF154826C9ECFE587C472193AB035E8F8E4ABFEB6FE1E53520C"));
                Assert.That(item.Meta.FeeMultiplier, Is.EqualTo(138));

                Assert.That(i.Version, Is.EqualTo(1));
                Assert.That(i.RecipientAddress, Is.EqualTo("6894D305EBBE9669675AEEC0B00CCD20B09548C3503B0880"));
                Assert.That(i.Type, Is.EqualTo(TransactionTypes.Types.TRANSFER));
                Assert.That(i.SignerPublicKey, Is.EqualTo("3714C04D01D664E2DDBD5ED2BA8B314F991EBA50122A38EC92A46AD987510B9D"));
                Assert.That(i.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
                Assert.That(i.Mosaics[0].Id, Is.EqualTo("6BED913FA20223F8"));
                Assert.That(i.Mosaics[0].Amount, Is.EqualTo(1000));
            }
        }
    }
}
