using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using System.Reactive.Linq;
using Coppery;
using System.Diagnostics;
using io.nem2.sdk.src.Model;
using io.nem2.sdk.src.Model.Accounts;

namespace Integration_Tests.HttpRequests
{
    internal class RegularTransactions
    {

        [SetUp]
        public void Setup()
        {
        }


        [Test, Timeout(20000)]
        public async Task SearchTransactions()
        {
            string pubKey = "BE0B4CF546B7B4F4BBFCFF9F574FDA527C07A53D3FC76F8BB7DB746F8E8E0A9F";
            PublicAccount acc = new PublicAccount(pubKey, NetworkType.Types.MAIN_NET);

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.TRANSFER.GetValue());
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.HASH_LOCK.GetValue());

            var response = await client.SearchConfirmedTransactions(qModel);

            Assert.That(response.Count, Is.GreaterThan(0));

            response.ForEach(i =>
            {
                ((SimpleTransfer)i.Transaction).Mosaics
                    .ForEach(m =>
                    {
                        Assert.IsTrue(m.Id.IsHex(16));
                        Assert.That(m.Amount, Is.GreaterThan(0));

                    });

                Assert.That(i.Meta.Height, Is.GreaterThan(0));
                Assert.IsTrue(i.Meta.Hash.IsHex(64));
            });

            Assert.IsTrue(acc.Address.Plain.IsBase32(39));
        }

        [Test, Timeout(20000)]
        public async Task SearchMosaicSupplyChangeTransaction()
        {
            string pubKey = "BE0B4CF546B7B4F4BBFCFF9F574FDA527C07A53D3FC76F8BB7DB746F8E8E0A9F";

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.MOSAIC_SUPPLY_CHANGE.GetValue());

            var response = await client.SearchConfirmedTransactions(qModel);

            Assert.That(response.Count, Is.GreaterThan(0));

            response.ForEach(i =>
            {

                var tx = (MosaicSupplyChange)i.Transaction;

                Assert.IsTrue(tx.SignerPublicKey.IsHex(64));
                Assert.That(tx.Action, Is.EqualTo(1));
                Assert.IsTrue(tx.MosaicId.IsHex(16));
                Assert.That(tx.Delta, Is.EqualTo(7842928625000000));
                Assert.IsTrue(tx.Signature.IsHex(128));
                Assert.That(tx.Version, Is.EqualTo(1));
                Assert.That(i.Meta.Index, Is.EqualTo(4));
            });
        }

        [Test, Timeout(20000)]
        public async Task SearchMosaicSupplyRevocationTransaction()
        {
            string pubKey = "6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D";

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.MOSAIC_SUPPLY_REVOCATION.GetValue());

            var response = await client.SearchConfirmedTransactions(qModel);

            response.ForEach(i =>
            {

                var tx = (MosaicSupplyRevocation)i.Transaction;

                Assert.IsTrue(tx.SignerPublicKey.IsHex(64));
                Assert.IsTrue(tx.SourceAddress.IsHex(48));
                Assert.That(tx.Version, Is.EqualTo(1));
                Assert.IsTrue(i.Meta.Hash.IsHex(64));
                Assert.That(i.Id.IsHex(24));
            });
        }

        [Test, Timeout(20000)]
        public async Task GetMosaicDefinitionTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("B48A3BCE25D31A458303489D8EC02006CB74B72F05E046E5D7428C654CDC0625");

            var tx = (MosaicDefinition)response.Transaction;

            Assert.That(tx.Nonce, Is.EqualTo(0));
            Assert.That(tx.Duration, Is.EqualTo(0));
            Assert.That(tx.Divisibility, Is.EqualTo(6));
            Assert.That(tx.Flags, Is.EqualTo(2));
            Assert.That(tx.Type.GetRawValue(), Is.EqualTo(TransactionTypes.Types.MOSAIC_DEFINITION));
            Assert.IsTrue(tx.Id.IsHex(16));
            Assert.That(tx.MaxFee, Is.EqualTo(0));


        }

        [Test, Timeout(20000)]
        public async Task GetAccountKeyLinkNonEmbeddedTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("7CEDB7FF3BA9D302B6870E5507C48C0433E988D96D5C5BFFCEA917E76D3BB87F");

            var tx = (KeyLink)response.Transaction;

            Assert.IsTrue(tx.LinkedPublicKey.IsHex(64));
            Assert.That(tx.LinkAction, Is.EqualTo(1));
            Assert.That(tx.Network.GetNetworkValue(), Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.IsTrue(tx.SignerPublicKey.IsHex(64));
            Assert.That(tx.Deadline, Is.EqualTo(1));
            Assert.That(tx.MaxFee, Is.EqualTo(0));
            Assert.That(tx.Type.GetRawValue(), Is.EqualTo(TransactionTypes.Types.ACCOUNT_KEY_LINK));
        }

        [Test, Timeout(20000)]
        public async Task GetNodeKeyLinkNonEmbeddedTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("F6A12DDA59412CF3A74D558E631FF6C6F5E2B43620CDC950698BBD17FF8F0B57");

            var tx = (KeyLink)response.Transaction;

            Assert.IsTrue(tx.LinkedPublicKey.IsHex(64));
            Assert.That(tx.LinkAction, Is.EqualTo(1));
            Assert.That(tx.Network.GetNetworkValue(), Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.IsTrue(tx.SignerPublicKey.IsHex(64));
            Assert.That(tx.Deadline, Is.EqualTo(104802319));
            Assert.That(tx.MaxFee, Is.EqualTo(1000000));
            Assert.That(tx.Type.GetRawValue(), Is.EqualTo(TransactionTypes.Types.NODE_KEY_LINK));
        }

        [Test, Timeout(20000)]
        public async Task GetVRFKeyLinkNonEmbeddedTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("DF4ED49CC5C1E81C4E7A4821FB06F5E7C8CEBE21DF38CBA891C300B7B9BE3DBC");

            var tx = (KeyLink)response.Transaction;

            Assert.IsTrue(tx.LinkedPublicKey.IsHex(64));
            Assert.That(tx.LinkAction, Is.EqualTo(1));
            Assert.That(tx.Network.GetNetworkValue(), Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.IsTrue(tx.SignerPublicKey.IsHex(64));
            Assert.That(tx.Deadline, Is.EqualTo(1));
            Assert.That(tx.MaxFee, Is.EqualTo(0));
            Assert.That(tx.Type.GetRawValue(), Is.EqualTo(TransactionTypes.Types.VRF_KEY_LINK));
        }

        [Test, Timeout(20000)]
        public async Task GetVotingKeyLinkNonEmbeddedTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("901807C96B582AACC140BE64CE3C18AF754E3DFBD2269AC573A5121097005DF8");

            var tx = (VotingKeyLink)response.Transaction;

            Assert.IsTrue(tx.LinkedPublicKey.IsHex(64));
            Assert.That(tx.LinkAction, Is.EqualTo(1));
            Assert.That(tx.StartEpoch, Is.EqualTo(1));
            Assert.That(tx.EndEpoch, Is.EqualTo(180));
            Assert.That(tx.Network.GetNetworkValue(), Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.That(tx.SignerPublicKey.IsHex(64));
            Assert.That(tx.Deadline, Is.EqualTo(1));
            Assert.That(tx.MaxFee, Is.EqualTo(0));
            Assert.That(tx.Type.GetRawValue(), Is.EqualTo(TransactionTypes.Types.VOTING_KEY_LINK));
        }

        [Test, Timeout(20000)]
        public async Task GetHashlockNonEmbeddedTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("4ED3697F2058FB41F5B80BA13F687CD864F719834906BAFE660EA140D26A8CAE");

            var tx = (HashLockT)response.Transaction;

            Assert.That(tx.Network.GetNetworkValue(), Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.IsTrue(tx.SignerPublicKey.IsHex(64));
            Assert.That(tx.Deadline, Is.EqualTo(97529473));
            Assert.That(tx.MaxFee, Is.EqualTo(18400));
            Assert.That(tx.Type.GetRawValue(), Is.EqualTo(TransactionTypes.Types.HASH_LOCK));
            Assert.That(tx.Amount, Is.EqualTo(10000000));
            Assert.That(tx.Duration, Is.EqualTo(1000));
            Assert.That(tx.MosaicId, Is.EqualTo("6BED913FA20223F8"));
            Assert.That(tx.Hash.IsHex(64));
        }

        [Test, Timeout(20000)]
        public async Task GetSecretlockNonEmbeddedTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("9D4F3856CE6A748C6DA73DEFF92084C23D578032FA307E9E68EEA040189174C6");

            var tx = (SecretLockT)response.Transaction;

            Assert.That(tx.Network.GetNetworkValue(), Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.IsTrue(tx.SignerPublicKey.IsHex(64));
            Assert.That(tx.Deadline, Is.EqualTo(8765036218));
            Assert.That(tx.MaxFee, Is.EqualTo(5225));
            Assert.That(tx.Type.GetRawValue(), Is.EqualTo(TransactionTypes.Types.SECRET_LOCK));
            Assert.That(tx.Amount, Is.EqualTo(1));
            Assert.That(tx.Duration, Is.EqualTo(20160));
            Assert.IsTrue(tx.MosaicId.IsHex(16));
            Assert.That(tx.Secret, Is.EqualTo("90A3DAF40EAEDAF79B79C8E8B9AF78EF59CF18A39866BD253404EAB737F41829"));
            Assert.IsTrue(tx.RecipientAddress.IsHex(48));
        }

        [Test, Timeout(20000)]
        public async Task GetSecretproofNonEmbeddedTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("81371DB98536CC911DB10C1D08AA69D495D1B2840850AFA15D618826F72AEE12");

            var tx = (SecretProofT)response.Transaction;

            Assert.That(tx.Network.GetNetworkValue(), Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.IsTrue(tx.SignerPublicKey.IsHex(64));
            Assert.That(tx.Deadline, Is.EqualTo(8765210999));
            Assert.That(tx.MaxFee, Is.EqualTo(5175));
            Assert.That(tx.Type.GetRawValue(), Is.EqualTo(TransactionTypes.Types.SECRET_PROOF));
            Assert.That(tx.HashAlgorithm, Is.EqualTo(0));
            Assert.That(tx.Proof, Is.EqualTo("614B1FCE6279B5A0EE68BCB0417F8FECB3AF9092"));
            Assert.IsTrue(tx.Secret.IsHex(64));
            Assert.IsTrue(tx.RecipientAddress.IsHex(48));
        }

        [Test, Timeout(20000)]
        public async Task SearchTransferTransaction()
        {
            string pubKey = "BE0B4CF546B7B4F4BBFCFF9F574FDA527C07A53D3FC76F8BB7DB746F8E8E0A9F";

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.TRANSFER.GetValue());

            var response = await client.SearchConfirmedTransactions(qModel);

            Assert.That(response.Count, Is.GreaterThan(0));

            response.ForEach(i =>
            {

                var tx = (SimpleTransfer)i.Transaction;

                Assert.IsTrue(tx.RecipientAddress.IsHex(48));
                Assert.IsTrue(tx.SignerPublicKey.IsHex(64));
                Assert.That(tx.Mosaics[0].Amount, Is.GreaterThan(0));
                Assert.IsTrue(tx.Mosaics[0].Id.IsHex(16));
                Assert.IsTrue(i.Meta.Hash.IsHex(64));
                Assert.IsTrue(i.Id.IsHex(24));
                Assert.That(TransactionTypes.Types.TRANSFER, Is.EqualTo(tx.Type.GetRawValue()));
            });
        }

        [Test, Timeout(20000)]
        public async Task GetMosaicSupplyRevocationTransaction()
        {

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("9B4D7D69E671E60D7862D7AFC183896A1758FD144C0C1A93BA1BA93191F1CDFE");

            var tx = (MosaicSupplyRevocation)response.Transaction;

            Assert.That(tx.Amount, Is.EqualTo(9));
            Assert.IsTrue(tx.SourceAddress.IsHex(48));
            Assert.That(tx.Size, Is.EqualTo(168));
            Assert.IsTrue(tx.MosaicId.IsHex(16));
            Assert.That(tx.Type.GetRawValue(), Is.EqualTo(TransactionTypes.Types.MOSAIC_SUPPLY_REVOCATION));
            Assert.IsTrue(tx.SignerPublicKey.IsHex(64));
        }

        [Test, Timeout(20000)]
        public async Task GetAddressAliasTransaction()
        {

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("54B99E12887443F0B6A2DAA6120EF72384B1A2BBC1CA4AF345198E6E72653770");

            var tx = (AddressAlias)response.Transaction;


            Assert.That(tx.AliasAction, Is.EqualTo(1));
            Assert.IsTrue(tx.NamespaceId.IsHex(16));
            Assert.IsTrue(tx.Address.IsHex(48));
            Assert.That(tx.Size, Is.EqualTo(161));
            Assert.That(tx.Type.GetRawValue(), Is.EqualTo(TransactionTypes.Types.ADDRESS_ALIAS));
            Assert.IsTrue(tx.SignerPublicKey.IsHex(64));
        }

        [Test, Timeout(20000)]
        public async Task GetMosaicAliasTransaction()
        {

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("8135D5533F45765ADE747BFEB06474CB11EEB02E221ACBD42295F8F1D237C467");

            var tx = (MosaicAlias)response.Transaction;


            Assert.That(tx.AliasAction, Is.EqualTo(1));
            Assert.IsTrue(tx.NamespaceId.IsHex(16));
            Assert.IsTrue(tx.MosaicId.IsHex(16));
            Assert.That(tx.Type.GetRawValue(), Is.EqualTo(TransactionTypes.Types.MOSAIC_ALIAS));
            Assert.That(tx.SignerPublicKey.IsHex(64));
        }

        [Test, Timeout(20000)]
        public async Task GetRootNamespaceRegistrationTransaction()
        {

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("FF31ABA28DEA461AFC0C4A68F31AB7CCD86EFCAA6A3781B6B741B59A4DDC01C2");

            var tx = (RootNamespaceRegistration)response.Transaction;
        
            Assert.That(tx.RegistrationType, Is.EqualTo(0));
            Assert.That(tx.Duration, Is.EqualTo(0));
            Assert.That(tx.Id.IsHex(16));
            Assert.That(tx.Type.GetRawValue(), Is.EqualTo(TransactionTypes.Types.NAMESPACE_REGISTRATION));
            Assert.That(tx.Size, Is.EqualTo(152));

            Assert.IsTrue(tx.SignerPublicKey.IsHex(64));
        }

        [Test, Timeout(20000)]
        public async Task GetTransferTransaction()
        {
            //2AC16BC578E3A1C7BF731A3040465C320786987E2C782D4FA709C8E5992247AB

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("2AC16BC578E3A1C7BF731A3040465C320786987E2C782D4FA709C8E5992247AB");

            var tx = (SimpleTransfer)response.Transaction;

            Assert.That(tx.SignerPublicKey, Is.EqualTo("BE0B4CF546B7B4F4BBFCFF9F574FDA527C07A53D3FC76F8BB7DB746F8E8E0A9F"));
            Assert.That(tx.RecipientAddress, Is.EqualTo("68FD492EE69DD21970DA18521D2B6EE22F09E4B0E11D1044"));

        }
        [Test, Timeout(20000)]
        public async Task GetAccountAddressRestrictionTransaction()
        {

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("2A5280F16603DCF1544619D87BB0BC367F29C32D3D52C5B659744B7CEE6301A6");

            var tx = (AccountRestriction)response.Transaction;

            Assert.That(tx.RestrictionFlags.ExtractRestrictionFlags()[0], Is.EqualTo(RestrictionTypes.Types.ADDRESS));
            Assert.IsTrue(tx.RestrictionAdditions[0].IsHex(48));
            Assert.That(tx.RestrictionDeletions.Count, Is.EqualTo(0));
            Assert.That(tx.Type.GetRawValue(), Is.EqualTo(TransactionTypes.Types.ACCOUNT_ADDRESS_RESTRICTION));
            Assert.That(tx.Size, Is.EqualTo(160));

            Assert.IsTrue(tx.SignerPublicKey.IsHex(64));
        }

        [Test, Timeout(20000)]
        public async Task GetAccountMosaicRestrictionTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("B8A9E72ADC49D2A880C49D6D4A8F88D3B64137DC0EC0A399CD9A1A8FB4C16FC0");

            var tx = (AccountRestriction)response.Transaction;

            Assert.That(tx.RestrictionFlags.ExtractRestrictionFlags()[0], Is.EqualTo(RestrictionTypes.Types.BLOCK));
            Assert.That(tx.RestrictionFlags.ExtractRestrictionFlags()[1], Is.EqualTo(RestrictionTypes.Types.MOSAIC_ID));
            Assert.That(tx.RestrictionAdditions[0].IsHex(16));
            Assert.That(tx.RestrictionDeletions.Count, Is.EqualTo(0));
            Assert.That(tx.Type.GetRawValue(), Is.EqualTo(TransactionTypes.Types.ACCOUNT_MOSAIC_RESTRICTION));
            Assert.That(tx.Size, Is.EqualTo(144));

            Assert.IsTrue(tx.SignerPublicKey.IsHex(64));
        }

        [Test, Timeout(20000)] 
        public async Task GetAccountOperationRestrictionTransaction()
        {

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("53D3ED8322AF889A37DFB6DC42B07269E413D91B6ACE51E065A2030C0D4E5266");

            var tx = (AccountOperationRestriction)response.Transaction;

            Assert.That(tx.RestrictionFlags.ExtractRestrictionFlags()[0], Is.EqualTo(RestrictionTypes.Types.BLOCK));
            Assert.That(tx.RestrictionFlags.ExtractRestrictionFlags()[1], Is.EqualTo(RestrictionTypes.Types.OUTGOING));
            Assert.That(tx.RestrictionAdditions[0].GetRawValue(), Is.EqualTo(TransactionTypes.Types.ACCOUNT_ADDRESS_RESTRICTION));
            Assert.That(tx.RestrictionDeletions.Count, Is.EqualTo(0));
            Assert.That(tx.Type.GetRawValue(), Is.EqualTo(TransactionTypes.Types.ACCOUNT_OPERATION_RESTRICTION));
            Assert.That(tx.Size, Is.EqualTo(138));

            Assert.IsTrue(tx.SignerPublicKey.IsHex(64));
        }

        [Test, Timeout(20000)] 
        public async Task GetMosaicAddressRestrictionTransaction()
        {

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("FF44C70577FDE571DC03A3827A3DAD138EC9D21C3839C63217896B4E992F7897");

            var tx = (MosaicAddressRestriction)response.Transaction;

            Assert.That(tx.NewRestrictionValue, Is.EqualTo(1));
            Assert.That(tx.PreviousRestrictionValue, Is.EqualTo(18446744073709551615));
            Assert.That(tx.RestrictionKey.IsHex(16));
            Assert.That(tx.TargetAddress.IsHex(48)); 
            Assert.That(tx.MosaicId.IsHex(16));
            Assert.That(tx.Type.GetRawValue(), Is.EqualTo(TransactionTypes.Types.MOSAIC_ADDRESS_RESTRICTION));
            Assert.That(tx.Size, Is.EqualTo(184));
            Assert.That(tx.SignerPublicKey.IsHex(64));
        }

        [Test, Timeout(20000)]
        public async Task SearchTransferTransactionWithMessege()
        {
            string pubKey = "BE0B4CF546B7B4F4BBFCFF9F574FDA527C07A53D3FC76F8BB7DB746F8E8E0A9F";

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("11B55558B111E21CABAE7278DE2D3CF393A2384F65AF2C62B88872312FFD0101");


            var tx = (SimpleTransfer)response.Transaction;

            Assert.That(tx.Message, Is.EqualTo("FE2A8061577301E2402E3F75637E6EFD62DBA4580EE027304459C8C6C50C0E305766F88AE75F6734F6FA6C36A1E6F5093CBB53FC3F8F4BD34B5709DC46A3DB5104685E233024B972E5543FEC16B4458F712FD0AAA00E61CE3B716811DA4E3BB3F1F6851BCD0D58D892BF213BA3F3CE72918F70AA2F78B333654AB2AF8E09F8318C2A63F5"));
            Assert.That(tx.RecipientAddress.IsHex(48));
            Assert.That(tx.SignerPublicKey.IsHex(64));
            Assert.That(tx.Mosaics, Is.Empty);
            Assert.That(tx.Type.GetRawValue(), Is.EqualTo(TransactionTypes.Types.TRANSFER));
            Assert.That(response.Meta.Hash.IsHex(64));
            Assert.That(response.Id.IsHex(24));
        }
    }
}
