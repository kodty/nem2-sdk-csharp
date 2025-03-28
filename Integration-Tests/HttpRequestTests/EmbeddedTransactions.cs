using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Model.Network;
using System.Reactive.Linq;

namespace Integration_Tests.HttpRequests
{
    internal class EmbeddedTransactions
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test, Timeout(20000)]
        public async Task GetEmbeddedTransferWithMessage()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("95837D9332DD2ED42C6FED83DC9EA0907E0046A4EEAEE761E185F5E6FAA2EA4C");

            var embedded = (EmbeddedSimpleTransfer)((Aggregate)response.Transaction).Transactions[0].Transaction;

            Assert.That(embedded.Message, Is.EqualTo("00E5B091E381AAE38184E381A7E38199E38191E381A9E38081E381BFE38293E382B8E383A0E58F82E58AA0E381AEE3818AE7A4BCE381A7E38199E38082"));
        }

        [Test, Timeout(20000)]
        public async Task GetEmbeddedAccountKeyLinkTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

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
        public async Task GetEmbeddedVRFKeyLinkTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var tx = await client.GetConfirmedTransaction("55EB9659C81600F1760C4C0A4F8A7A5C90A39FCEE36E3165143B8E72BBC709E8");

            var aggregate = (Aggregate)tx.Transaction;

            var Voting = (EmbeddedKeyLink)aggregate.Transactions[3].Transaction;

            Assert.That(Voting.SignerPublicKey, Is.EqualTo("B26D01FC006EAC09B740A3C8F12C1055AE24AFD3268F0364C92D51800FC07361"));
            Assert.That(Voting.LinkedPublicKey, Is.EqualTo("6A1D20D5D699FBF289A6594A1C18E12A1169BB2608F0A0E3A15D11AFD33EE30F"));
            Assert.That(Voting.LinkAction, Is.EqualTo(1));
            Assert.That(Voting.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.That(Voting.Type, Is.EqualTo(TransactionTypes.Types.VRF_KEY_LINK));
            Assert.That(Voting.Version, Is.EqualTo(1));
        }

        [Test, Timeout(20000)]
        public async Task GetEmbeddedMosaicMetadataTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var tx = await client.GetConfirmedTransaction("384CF682EFFE8CBA4CF8A7F2C832B89547828DA024ECAC25E50A61B5C9E400F2");

            var aggregate = (Aggregate)tx.Transaction;

            var Voting = (EmbeddedMosaicMetadata)aggregate.Transactions[4].Transaction;

            Assert.That(Voting.SignerPublicKey, Is.EqualTo("C78E8455A9192F222D3A490B8D92DFDF6B0E9E30B04FD1CBA415B1FA848EA8E8"));
            Assert.That(Voting.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.That(Voting.Type, Is.EqualTo(TransactionTypes.Types.MOSAIC_METADATA));
            Assert.That(Voting.Version, Is.EqualTo(1));

            Assert.That(Voting.Value, Is.EqualTo("636F6D73612D6E6366742D312E31"));
            Assert.That(Voting.ValueSize, Is.EqualTo(14));
            Assert.That(Voting.ValueSizeDelta, Is.EqualTo(14));
            Assert.That(Voting.ScopedMetadataKey, Is.EqualTo("8D9A3BDD21391AA2"));
            Assert.That(Voting.TargetAddress, Is.EqualTo("688FA72E197170C1366358934C520AC679B7D097662DE274"));
            Assert.That(Voting.TargetMosaicId, Is.EqualTo("5E2D18B856E4AED4"));
        }

        [Test, Timeout(20000)]
        public async Task GetEmbeddedNamespaceMetadataTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var tx = await client.GetConfirmedTransaction("F97F223C7C9011DCDECD11F13EBBBB257840BFC4A3D12DA2AD2E8F3047AAD01B");

            var aggregate = (Aggregate)tx.Transaction;

            var Voting = (EmbeddedNamespaceMetadata)aggregate.Transactions[0].Transaction;

            Assert.That(Voting.SignerPublicKey, Is.EqualTo("9EEEE7F96711D0A3D146AD8DE73BA2CF34AC77ACA7A4055DEB25109D69768C51"));
            Assert.That(Voting.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.That(Voting.Type, Is.EqualTo(TransactionTypes.Types.NAMESPACE_METADATA));
            Assert.That(Voting.Version, Is.EqualTo(1));

            Assert.That(Voting.Value.Length, Is.EqualTo(2012));
            Assert.That(Voting.ValueSize, Is.EqualTo(1006));
            Assert.That(Voting.ValueSizeDelta, Is.EqualTo(1006));
            Assert.That(Voting.ScopedMetadataKey, Is.EqualTo("E1E9AACD3CDA0F96"));
            Assert.That(Voting.TargetAddress, Is.EqualTo("68B600B64F71AF064B56178410BBCF945937FB43F6738F73"));
            Assert.That(Voting.TargetNamespaceId, Is.EqualTo("C47F86D81AC6D480"));
        }

        [Test, Timeout(20000)]
        public async Task GetEmbeddedSupplyChangeTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var tx = await client.GetConfirmedTransaction("7E3049EBF37DD84C2C52C96A4234281326F3FA434DCFBDA71CF68A194ACB5059");

            var aggregate = (Aggregate)tx.Transaction;

            var change = (EmbeddedMosaicSupplyChange)aggregate.Transactions[1].Transaction;

            Assert.That(change.Delta, Is.EqualTo(3800000));
            Assert.That(change.SignerPublicKey, Is.EqualTo("7E43EC810A64FCCA5F9FBF6FC3E51AA89A0507762DC7E6B8047DCACBE97A8D4B"));
            Assert.That(change.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.That(change.Version, Is.EqualTo(1));
            Assert.That(change.MosaicId, Is.EqualTo("63078E73FBCC2CAC"));
            Assert.That(change.Action, Is.EqualTo(1));
            Assert.That(change.Type, Is.EqualTo(TransactionTypes.Types.MOSAIC_SUPPLY_CHANGE));
        }

        [Test, Timeout(20000)]
        public async Task GetEmbeddedMosaicDefinitionTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("7E3049EBF37DD84C2C52C96A4234281326F3FA434DCFBDA71CF68A194ACB5059");

            var tx = (Aggregate)response.Transaction;
            var embedded = tx.Transactions;
            var definition = (EmbeddedMosaicDefinition)embedded[0].Transaction;

            Assert.That(tx.TransactionsHash, Is.EqualTo("DF5C0B4D7CC979FA385D4785FCA2A5D9A8F172C0BAB90883BF167DFE9C78A13B"));
            Assert.That(tx.SignerPublicKey, Is.EqualTo("7E43EC810A64FCCA5F9FBF6FC3E51AA89A0507762DC7E6B8047DCACBE97A8D4B"));
            Assert.That(embedded.Count, Is.EqualTo(2));
            Assert.That(definition.Id, Is.EqualTo("63078E73FBCC2CAC"));
            Assert.That(definition.Flags, Is.EqualTo(3));
            Assert.That(definition.Duration, Is.EqualTo(0));
            Assert.That(definition.Divisibility, Is.EqualTo(0));
            Assert.That(definition.Nonce, Is.EqualTo(3525458556));
        }

        [Test, Timeout(20000)]
        public async Task GetEmbeddedMosaicSupplyChangeTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("7E3049EBF37DD84C2C52C96A4234281326F3FA434DCFBDA71CF68A194ACB5059");

            var tx = (Aggregate)response.Transaction;
            var embedded = tx.Transactions;
            var definition = (EmbeddedMosaicSupplyChange)embedded[1].Transaction;

            Assert.That(tx.TransactionsHash, Is.EqualTo("DF5C0B4D7CC979FA385D4785FCA2A5D9A8F172C0BAB90883BF167DFE9C78A13B"));
            Assert.That(tx.SignerPublicKey, Is.EqualTo("7E43EC810A64FCCA5F9FBF6FC3E51AA89A0507762DC7E6B8047DCACBE97A8D4B"));
            Assert.That(embedded.Count, Is.EqualTo(2));
            Assert.That(definition.MosaicId, Is.EqualTo("63078E73FBCC2CAC"));
            Assert.That(definition.Delta, Is.EqualTo(3800000));
            Assert.That(definition.Action, Is.EqualTo(1));
        }

        [Test, Timeout(20000)]
        public async Task GetEmbeddedMosaicRestrictionTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var tx = await client.GetConfirmedTransaction("55EB9659C81600F1760C4C0A4F8A7A5C90A39FCEE36E3165143B8E72BBC709E8");

            var aggregate = (Aggregate)tx.Transaction;

            var restriction = (EmbeddedAccountMosaicRestriction)aggregate.Transactions[1].Transaction;

            Assert.That(restriction.Type, Is.EqualTo(TransactionTypes.Types.ACCOUNT_MOSAIC_RESTRICTION));
            Assert.That(restriction.SignerPublicKey, Is.EqualTo("B26D01FC006EAC09B740A3C8F12C1055AE24AFD3268F0364C92D51800FC07361"));
            Assert.That(restriction.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.That(restriction.Version, Is.EqualTo(1));
            Assert.That(restriction.RestrictionAdditions[0], Is.EqualTo("6BED913FA20223F8"));
            Assert.That(restriction.RestrictionDeletions.Count, Is.EqualTo(0));
            Assert.That(restriction.RestrictionFlags[0], Is.EqualTo(RestrictionTypes.Types.MOSAIC_ID));
        }

        [Test, Timeout(20000)]
        public async Task GetEmbeddedAccountMetadataTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var tx = await client.GetConfirmedTransaction("F6421D85126AD4E4795848BFB1E6FCAC606F6C0B8D89A6D9825E36C83D594F3C");

            var aggregate = (Aggregate)tx.Transaction;

            var restriction = (EmbeddedAccountMetadata)aggregate.Transactions[0].Transaction;

            Assert.That(restriction.Type, Is.EqualTo(TransactionTypes.Types.ACCOUNT_METADATA));
            Assert.That(restriction.SignerPublicKey, Is.EqualTo("8B6E6AFB952A59E35A2207A81C44E645BF73FB654CA42AFD08877318B6E29885"));
            Assert.That(restriction.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.That(restriction.Version, Is.EqualTo(1));
            Assert.That(restriction.ScopedMetadataKey, Is.EqualTo("951688612CBDE45D"));
            Assert.That(restriction.TargetAddress, Is.EqualTo("68E81D5B9653DDAF7EF0394CA2505281A601A4042CB5CD5E"));
            Assert.That(restriction.Value, Is.EqualTo("3644363137323642373537333230373337393644363236463643323037303631373237343739"));
            Assert.That(restriction.ValueSize, Is.EqualTo(38));
            Assert.That(restriction.ValueSizeDelta, Is.EqualTo(38));

        }

        [Test, Timeout(20000)]
        public async Task GetEmbeddedVotingKeyLinkTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

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
        public async Task GetEmbeddedSecretLockTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var tx = await client.GetConfirmedTransaction("38504353F21B9D4FE327923E8813EF39549CAA01AFC978F25F0CCF1C206F6F7E");

            var aggregate = (Aggregate)tx.Transaction;

            var Voting = (EmbeddedSecretLockT)aggregate.Transactions[0].Transaction;

            Assert.That(Voting.SignerPublicKey, Is.EqualTo("69C2325CFE51AF39444822C17C09E17DFA1DA8D4828234C8A41B1316E9E0D3CC"));
            Assert.That(Voting.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.That(Voting.Type, Is.EqualTo(TransactionTypes.Types.SECRET_LOCK));
            Assert.That(Voting.Version, Is.EqualTo(1));
            Assert.That(Voting.MosaicId, Is.EqualTo("6BED913FA20223F8"));
            Assert.That(Voting.Duration, Is.EqualTo(8576));
            Assert.That(Voting.Amount, Is.EqualTo(250000000));
            Assert.That(Voting.HashAlgorithm, Is.EqualTo(0));
            Assert.That(Voting.Secret, Is.EqualTo("05CABA4BCB96D98FD124AC9A1A3AD75582145A421685E58F4BC4436438E4DFDE"));
            Assert.That(Voting.RecipientAddress, Is.EqualTo("68613BF2E2FD362C92A9A6F7A530B43BB8A945BEF74B20E8"));
        }


        [Test, Timeout(20000)]
        public async Task GetEmbeddedSecretProofTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var tx = await client.GetConfirmedTransaction("890B8D3B84A65FF5C3A4F743CD0A4A98D6F92250D4DB53463CBF2010C4CC7F39");

            var aggregate = (Aggregate)tx.Transaction;

            var Voting = (EmbeddedSecretProofT)aggregate.Transactions[0].Transaction;

            Assert.That(Voting.SignerPublicKey.Length, Is.EqualTo(64));
            Assert.That(Voting.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.That(Voting.Type, Is.EqualTo(TransactionTypes.Types.SECRET_PROOF));
            Assert.That(Voting.Version, Is.EqualTo(1));
            Assert.That(Voting.HashAlgorithm, Is.EqualTo(0));
            Assert.That(Voting.Secret, Is.EqualTo("6B69F838867471B81A38B908C1ADA48A101E1A4F6711E5217F5A51E969A3ABB6"));
            Assert.That(Voting.RecipientAddress, Is.EqualTo("68E0286048AE7ACB3789B41578A5B78B560B6A45F5AC8E55"));
            Assert.That(Voting.Proof.Length, Is.EqualTo(40));
        }

        [Test, Timeout(20000)]
        public async Task GetEmbeddedAddressAliasTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var tx = await client.GetConfirmedTransaction("5C15C196C2B0924142B45901EF99AA821C8DA9096EC9B5FAD61DCC605B977028");

            var aggregate = (Aggregate)tx.Transaction;

            var Voting = (EmbeddedAddressAlias)aggregate.Transactions[0].Transaction;

            Assert.That(Voting.SignerPublicKey.Length, Is.EqualTo(64));
            Assert.That(Voting.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.That(Voting.Type, Is.EqualTo(TransactionTypes.Types.ADDRESS_ALIAS));
            Assert.That(Voting.Version, Is.EqualTo(1));
            Assert.That(Voting.AliasAction, Is.EqualTo(1));
            Assert.That(Voting.NamespaceId, Is.EqualTo("F021153ACEBD5810"));
            Assert.That(Voting.Address, Is.EqualTo("685B64616B3A5368CF20AE78D30DB9EB911A71A95A07F03F"));

        }

        [Test, Timeout(20000)]
        public async Task GetEmbeddedMosaicSupplyRevocationTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var tx = await client.GetConfirmedTransaction("377CEFFC0FB99B9289E627D14729D9341AC082832EBFC54A25098594E732FB82");

            var aggregate = (Aggregate)tx.Transaction;

            var Voting = (EmbeddedMosaicSupplyRevocation)aggregate.Transactions[1].Transaction;

            Assert.That(Voting.SignerPublicKey, Is.EqualTo("1B22ADD1B1EA8920153EDA0F27936A1EE941388F27284618599D8F83EBE92A8B"));
            Assert.That(Voting.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.That(Voting.Type, Is.EqualTo(TransactionTypes.Types.MOSAIC_SUPPLY_REVOCATION));
            Assert.That(Voting.Version, Is.EqualTo(1));
            Assert.That(Voting.MosaicId, Is.EqualTo("5CD6CBFDEE0F1D8E"));
            Assert.That(Voting.SourceAddress, Is.EqualTo("687CDA0541F388DF9D46138DA8516558B98453D98E75A02E"));
            Assert.That(Voting.Amount, Is.EqualTo(8));
        }

        [Test, Timeout(20000)]
        public async Task GetEmbeddedKeyLinkTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

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
        public async Task GetEmbeddedAccountMosaicRestriction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var acc = new PublicAccount("C807BE28855D0C87A8A2C032E51790CCB9158C15CBACB8B222E27DFFFEB3697D", NetworkType.Types.MAIN_NET);

            var transaction = await client.GetConfirmedTransaction("30FA71E6D1E34DF1E430A07E1B0817BED9A4ED6B0245B7471B0557380A700E1B");

            var restriction = (EmbeddedAccountMosaicRestriction)((Aggregate)transaction.Transaction).Transactions[1].Transaction;

            Assert.That(restriction.RestrictionFlags[0], Is.EqualTo(RestrictionTypes.Types.MOSAIC_ID));
            Assert.That(restriction.RestrictionAdditions[0], Is.EqualTo("6BED913FA20223F8"));
            Assert.That(restriction.RestrictionDeletions.Count, Is.EqualTo(0)); // flag
        }

        [Test, Timeout(20000)]
        public async Task GetEmbeddedSimpleTransfer()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var tx = await client.GetConfirmedTransaction("BFBD18CE27575CF154826C9ECFE587C472193AB035E8F8E4ABFEB6FE1E53520C");

            var aggregate = (Aggregate)tx.Transaction;

            var Embedded = aggregate.Transactions;

            Assert.That(Embedded.Count, Is.GreaterThan(0));

            foreach (var item in Embedded)
            {
                var i = (EmbeddedSimpleTransfer)item.Transaction;

                Assert.That(item.Meta.Index, Is.AnyOf(0, 1));
                Assert.That(item.Meta.Height, Is.EqualTo(1995));
                Assert.That(item.Meta.Timestamp, Is.EqualTo(144382262));
                Assert.That(item.Meta.AggregateId, Is.EqualTo("666420199E09C6EE960222F3"));
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

        [Test, Timeout(20000)]
        public async Task GetAggregatesComplete()
        {
            string pubKey = "6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D";

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransactions(new string[] { "66641FAD9E09C6EE96018A30", "66641FCA9E09C6EE9601A449" });

            var aggTx1 = (Aggregate)response[0].Transaction;
            var aggTx2 = (Aggregate)response[1].Transaction;


            Assert.That(response[0].Id, Is.EqualTo("66641FAD9E09C6EE96018A30"));
            Assert.That(response[0].Meta.Hash, Is.EqualTo("E906272E7A715CD24D959A51CDFADC4CC8CA0E63097EA161C1DEBD31E9754A74"));
            Assert.That(response[0].Meta.Index, Is.EqualTo(25465));
            Assert.That(response[0].Meta.Timestamp, Is.EqualTo(0));
            Assert.That(response[0].Meta.MerkleComponentHash, Is.EqualTo("904E12F6F155A858C89568A63C23E1F5CDB8AC5220969BB59BD22879FF334F83"));
            Assert.That(response[0].Meta.FeeMultiplier, Is.EqualTo(0));

            Assert.That(response[1].Id, Is.EqualTo("66641FCA9E09C6EE9601A449"));
            Assert.That(response[1].Meta.Hash, Is.EqualTo("7E3049EBF37DD84C2C52C96A4234281326F3FA434DCFBDA71CF68A194ACB5059"));
            Assert.That(response[1].Meta.Index, Is.EqualTo(0));
            Assert.That(response[1].Meta.Height, Is.EqualTo(117));
            Assert.That(response[1].Meta.Timestamp, Is.EqualTo(88309778));
            Assert.That(response[1].Meta.MerkleComponentHash, Is.EqualTo("7E3049EBF37DD84C2C52C96A4234281326F3FA434DCFBDA71CF68A194ACB5059"));
            Assert.That(response[1].Meta.FeeMultiplier, Is.EqualTo(100));

            Assert.That(aggTx1.Size, Is.EqualTo(864));
            Assert.That(aggTx1.Transactions, !Is.Null);
            Assert.That(aggTx1.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.That(aggTx1.TransactionsHash, Is.EqualTo("9D7D525E22C0DBEEA4D0F8E6C1AC4E301399C3EDD3CA7E6D2ACC6E4D13677CE6"));
            Assert.That(aggTx1.Signature, Is.EqualTo("35B6E3B1C311AA6A957EF2AD12447AD790A5197454ECC27BCE02527257EE317E404367C416A41E53D8CA851393AC58F59343435230CC6F75EB4A49C784BDCD03"));
            Assert.That(aggTx1.Deadline, Is.EqualTo(1));
            Assert.That(aggTx1.MaxFee, Is.EqualTo(0));
            Assert.That(aggTx1.Type, Is.EqualTo(TransactionTypes.Types.AGGREGATE_COMPLETE));
            Assert.That(aggTx1.SignerPublicKey, Is.EqualTo("FA9F3974FE3B15585E6B72672C7D8BEAE27D1EDF6C4752BAFDB8B2FEA601C0CF"));

            Assert.That(aggTx2.Size, Is.EqualTo(312));
            Assert.That(aggTx2.Transactions, !Is.Null);
            Assert.That(aggTx2.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.That(aggTx2.TransactionsHash, Is.EqualTo("DF5C0B4D7CC979FA385D4785FCA2A5D9A8F172C0BAB90883BF167DFE9C78A13B"));
            Assert.That(aggTx2.Signature, Is.EqualTo("4F51E0B27765A3FA70E34D5232B2DCF94FE5A125E31B47C5B3CD4A428F136C994C769A20389ED147D087E9E37DF8D00BBB753E808BCE57565764FC0A84D6B60B"));
            Assert.That(aggTx2.Deadline, Is.EqualTo(95507057));
            Assert.That(aggTx2.MaxFee, Is.EqualTo(31200));
            Assert.That(aggTx2.Type, Is.EqualTo(TransactionTypes.Types.AGGREGATE_COMPLETE));
            Assert.That(aggTx2.SignerPublicKey, Is.EqualTo("7E43EC810A64FCCA5F9FBF6FC3E51AA89A0507762DC7E6B8047DCACBE97A8D4B"));

            aggTx1.Cosignatures.ForEach(i =>
            {

                Assert.That(i.SignerPublicKey.Length, Is.EqualTo(64));
                Assert.That(i.Signature.Length, Is.EqualTo(128));
                Assert.That(i.Version, Is.EqualTo(0));

            });

            Assert.That(aggTx1.Transactions[0].Transaction.SignerPublicKey, Is.EqualTo("FA9F3974FE3B15585E6B72672C7D8BEAE27D1EDF6C4752BAFDB8B2FEA601C0CF"));
            Assert.That(aggTx1.Transactions[0].Transaction.Network, Is.EqualTo(NetworkType.Types.MAIN_NET)); // network shouldnt be twice - check why
            Assert.That(aggTx1.Transactions[0].Transaction.Type, Is.EqualTo(TransactionTypes.Types.MULTISIG_ACCOUNT_MODIFICATION));
            Assert.That(aggTx1.Transactions[0].Transaction.Version, Is.EqualTo(1));

            Assert.That(aggTx2.Transactions[1].Transaction.SignerPublicKey, Is.EqualTo("7E43EC810A64FCCA5F9FBF6FC3E51AA89A0507762DC7E6B8047DCACBE97A8D4B"));
            Assert.That(aggTx2.Transactions[1].Transaction.Network, Is.EqualTo(NetworkType.Types.MAIN_NET)); // network shouldnt be twice - check why
            Assert.That(aggTx2.Transactions[1].Transaction.Type, Is.EqualTo(TransactionTypes.Types.MOSAIC_SUPPLY_CHANGE));
            Assert.That(aggTx2.Transactions[1].Transaction.Version, Is.EqualTo(1));

            var embedded1 = (EmbeddedMultisigModification)aggTx1.Transactions[0].Transaction;
            var embedded2 = (EmbeddedMosaicSupplyChange)aggTx2.Transactions[1].Transaction;

            Assert.That(aggTx1.Transactions[0].Id, Is.EqualTo("66641FAD9E09C6EE96018A31"));
            Assert.That(aggTx1.Transactions[0].Meta.Index, Is.EqualTo(0));
            Assert.That(aggTx1.Transactions[0].Meta.Height, Is.EqualTo(1));
            Assert.That(aggTx1.Transactions[0].Meta.Timestamp, Is.EqualTo(0));
            Assert.That(aggTx1.Transactions[0].Meta.FeeMultiplier, Is.EqualTo(0));
            Assert.That(aggTx1.Transactions[0].Meta.AggregateHash, Is.EqualTo("E906272E7A715CD24D959A51CDFADC4CC8CA0E63097EA161C1DEBD31E9754A74"));
            Assert.That(aggTx1.Transactions[0].Meta.AggregateId, Is.EqualTo("66641FAD9E09C6EE96018A30"));

            Assert.That(aggTx2.Transactions[1].Id, Is.EqualTo("66641FCA9E09C6EE9601A44B"));
            Assert.That(aggTx2.Transactions[1].Meta.Index, Is.EqualTo(1));
            Assert.That(aggTx2.Transactions[1].Meta.Height, Is.EqualTo(117));
            Assert.That(aggTx2.Transactions[1].Meta.Timestamp, Is.EqualTo(88309778));
            Assert.That(aggTx2.Transactions[1].Meta.FeeMultiplier, Is.EqualTo(100));
            Assert.That(aggTx2.Transactions[1].Meta.AggregateHash, Is.EqualTo("7E3049EBF37DD84C2C52C96A4234281326F3FA434DCFBDA71CF68A194ACB5059"));
            Assert.That(aggTx2.Transactions[1].Meta.AggregateId, Is.EqualTo("66641FCA9E09C6EE9601A449"));

            Assert.That(embedded1.SignerPublicKey, Is.EqualTo("FA9F3974FE3B15585E6B72672C7D8BEAE27D1EDF6C4752BAFDB8B2FEA601C0CF"));
            Assert.That(embedded1.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.That(embedded1.Version, Is.EqualTo(1));
            Assert.That(embedded1.Type, Is.EqualTo(TransactionTypes.Types.MULTISIG_ACCOUNT_MODIFICATION));
            Assert.That(embedded1.minApprovalDelta, Is.EqualTo(4));
            Assert.That(embedded1.minRemovalDelta, Is.EqualTo(5));
            Assert.That(embedded1.addressAdditions[0].Length, Is.EqualTo(48));

            Assert.That(embedded2.Delta, Is.EqualTo(3800000));
            Assert.That(embedded2.Action, Is.EqualTo(1));
            Assert.That(embedded2.Type, Is.EqualTo(TransactionTypes.Types.MOSAIC_SUPPLY_CHANGE));
            Assert.That(embedded2.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.That(embedded2.MosaicId, Is.EqualTo("63078E73FBCC2CAC"));
            Assert.That(embedded2.Version, Is.EqualTo(1));
        }

        [Test, Timeout(20000)]
        public async Task GetMultisigModification()
        {
            string pubKey = "6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D";

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("E906272E7A715CD24D959A51CDFADC4CC8CA0E63097EA161C1DEBD31E9754A74");

            var tx = (Aggregate)response.Transaction;

            Assert.That(response.Meta.Hash, Is.EqualTo("E906272E7A715CD24D959A51CDFADC4CC8CA0E63097EA161C1DEBD31E9754A74"));
            Assert.That(response.Meta.Index, Is.EqualTo(25465));
            Assert.That(response.Meta.MerkleComponentHash, Is.EqualTo("904E12F6F155A858C89568A63C23E1F5CDB8AC5220969BB59BD22879FF334F83"));
            Assert.That(response.Meta.Height, Is.EqualTo(1));
            Assert.That(response.Meta.Timestamp, Is.EqualTo(0));
            Assert.That(response.Id, Is.EqualTo("66641FAD9E09C6EE96018A30"));

            Assert.That(tx.Size, Is.EqualTo(864));
            Assert.That(tx.Transactions, !Is.Null);
            Assert.That(tx.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.That(tx.TransactionsHash, Is.EqualTo("9D7D525E22C0DBEEA4D0F8E6C1AC4E301399C3EDD3CA7E6D2ACC6E4D13677CE6"));
            Assert.That(tx.Signature, Is.EqualTo("35B6E3B1C311AA6A957EF2AD12447AD790A5197454ECC27BCE02527257EE317E404367C416A41E53D8CA851393AC58F59343435230CC6F75EB4A49C784BDCD03"));
            Assert.That(tx.Deadline, Is.EqualTo(1));
            Assert.That(tx.MaxFee, Is.EqualTo(0));
            Assert.That(tx.Type, Is.EqualTo(TransactionTypes.Types.AGGREGATE_COMPLETE));
            Assert.That(tx.SignerPublicKey, Is.EqualTo("FA9F3974FE3B15585E6B72672C7D8BEAE27D1EDF6C4752BAFDB8B2FEA601C0CF"));

            Assert.That(tx.Transactions[0].Transaction.SignerPublicKey, Is.EqualTo("FA9F3974FE3B15585E6B72672C7D8BEAE27D1EDF6C4752BAFDB8B2FEA601C0CF"));
            Assert.That(tx.Transactions[0].Transaction.Network, Is.EqualTo(NetworkType.Types.MAIN_NET)); // network shouldnt be twice - check why
            Assert.That(tx.Transactions[0].Transaction.Type, Is.EqualTo(TransactionTypes.Types.MULTISIG_ACCOUNT_MODIFICATION));
            Assert.That(tx.Transactions[0].Transaction.Version, Is.EqualTo(1));

            var embedded = (EmbeddedMultisigModification)tx.Transactions[0].Transaction;

            Assert.That(embedded.minRemovalDelta, Is.EqualTo(5));
            Assert.That(embedded.addressAdditions[0].Length, Is.EqualTo(48));
            Assert.That(embedded.addressDeletions.Count, Is.EqualTo(0));
        }
    }
}
