using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Model.Network;
using System.Reactive.Linq;

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
                        Assert.That(m.Id, Is.EqualTo("E74B99BA41F4AFEE"));
                        Assert.That(m.Amount, Is.GreaterThan(0));

                    });

                Assert.That(i.Meta.Height, Is.GreaterThan(0));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));
            });

            Assert.That(acc.Address.Plain, Is.EqualTo("NASYMBOLLK6FSL7GSEMQEAWN7VW55ZSZU25TBOA"));
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

                Assert.That(tx.SignerPublicKey, Is.EqualTo(pubKey));
                Assert.That(tx.Action, Is.EqualTo(1));
                Assert.That(tx.MosaicId.Length, Is.EqualTo(16));
                Assert.That(tx.Delta, Is.EqualTo(7842928625000000));
                Assert.That(tx.Signature, Is.EqualTo("6FC30E98378ADBA9F79D5CEF2ECBCB6D3AD6010FC265708E62419862534D51E3F56B688B55B01AE631281CC589FB1FEFF43D88141B13AD5C9C63A5E15D0E320A"));
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

                Assert.That(tx.SignerPublicKey, Is.EqualTo("6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D"));
                Assert.That(tx.SourceAddress.Length, Is.EqualTo(48));
                Assert.That(tx.Version, Is.EqualTo(1));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));
                Assert.That(i.Id.Length, Is.EqualTo(24));
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
            Assert.That(tx.Type, Is.EqualTo(TransactionTypes.Types.MOSAIC_DEFINITION));
            Assert.That(tx.Id, Is.EqualTo("6BED913FA20223F8"));
            Assert.That(tx.MaxFee, Is.EqualTo(0));


        }

        [Test, Timeout(20000)]
        public async Task GetAccountKeyLinkNonEmbeddedTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("7CEDB7FF3BA9D302B6870E5507C48C0433E988D96D5C5BFFCEA917E76D3BB87F");

            var tx = (KeyLink)response.Transaction;

            Assert.That(tx.LinkedPublicKey, Is.EqualTo("D753011066CA6FBB1161E31FA21848F7B9103F45EE6B68C9E3356F25E23C9F5B"));
            Assert.That(tx.LinkAction, Is.EqualTo(1));
            Assert.That(tx.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.That(tx.SignerPublicKey, Is.EqualTo("AFF16052217A847A6A71B326FEA9073CFF70D07FC5BA9026B3E05FB453C950DF"));
            Assert.That(tx.Deadline, Is.EqualTo(1));
            Assert.That(tx.MaxFee, Is.EqualTo(0));
            Assert.That(tx.Type, Is.EqualTo(TransactionTypes.Types.ACCOUNT_KEY_LINK));
        }

        [Test, Timeout(20000)]
        public async Task GetNodeKeyLinkNonEmbeddedTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("F6A12DDA59412CF3A74D558E631FF6C6F5E2B43620CDC950698BBD17FF8F0B57");

            var tx = (KeyLink)response.Transaction;

            Assert.That(tx.LinkedPublicKey, Is.EqualTo("4F0C4E6D5183BDBA7323D993A6EE40FDB35D285104CC292578E71A7847E6FA8C"));
            Assert.That(tx.LinkAction, Is.EqualTo(1));
            Assert.That(tx.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.That(tx.SignerPublicKey, Is.EqualTo("0B349D6FB4E93FAB29065D51B7A5375FFAF3856BA7F64DDE66B86579816D6E77"));
            Assert.That(tx.Deadline, Is.EqualTo(104802319));
            Assert.That(tx.MaxFee, Is.EqualTo(1000000));
            Assert.That(tx.Type, Is.EqualTo(TransactionTypes.Types.NODE_KEY_LINK));
        }

        [Test, Timeout(20000)]
        public async Task GetVRFKeyLinkNonEmbeddedTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("DF4ED49CC5C1E81C4E7A4821FB06F5E7C8CEBE21DF38CBA891C300B7B9BE3DBC");

            var tx = (KeyLink)response.Transaction;

            Assert.That(tx.LinkedPublicKey, Is.EqualTo("ECB4C26C21436CA9C3BEF0067CB2136AF88B6DBD7871227A3E5449FDDEC3AA03"));
            Assert.That(tx.LinkAction, Is.EqualTo(1));
            Assert.That(tx.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.That(tx.SignerPublicKey, Is.EqualTo("AFF16052217A847A6A71B326FEA9073CFF70D07FC5BA9026B3E05FB453C950DF"));
            Assert.That(tx.Deadline, Is.EqualTo(1));
            Assert.That(tx.MaxFee, Is.EqualTo(0));
            Assert.That(tx.Type, Is.EqualTo(TransactionTypes.Types.VRF_KEY_LINK));
        }

        [Test, Timeout(20000)]
        public async Task GetVotingKeyLinkNonEmbeddedTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("901807C96B582AACC140BE64CE3C18AF754E3DFBD2269AC573A5121097005DF8");

            var tx = (VotingKeyLink)response.Transaction;

            Assert.That(tx.LinkedPublicKey, Is.EqualTo("D49DA310E4829C9B6FC1A3C2DC0BEA0CCDE8A5A41FBF7A8F8FB45438DB5AAEDC"));
            Assert.That(tx.LinkAction, Is.EqualTo(1));
            Assert.That(tx.StartEpoch, Is.EqualTo(1));
            Assert.That(tx.EndEpoch, Is.EqualTo(180));
            Assert.That(tx.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.That(tx.SignerPublicKey, Is.EqualTo("AFF16052217A847A6A71B326FEA9073CFF70D07FC5BA9026B3E05FB453C950DF"));
            Assert.That(tx.Deadline, Is.EqualTo(1));
            Assert.That(tx.MaxFee, Is.EqualTo(0));
            Assert.That(tx.Type, Is.EqualTo(TransactionTypes.Types.VOTING_KEY_LINK));
        }

        [Test, Timeout(20000)]
        public async Task GetHashlockNonEmbeddedTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("4ED3697F2058FB41F5B80BA13F687CD864F719834906BAFE660EA140D26A8CAE");

            var tx = (HashLockT)response.Transaction;

            Assert.That(tx.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.That(tx.SignerPublicKey, Is.EqualTo("1799A50301C17D0BA45D2599193B49C4A5377640B3D6695B84F6320466958B5C"));
            Assert.That(tx.Deadline, Is.EqualTo(97529473));
            Assert.That(tx.MaxFee, Is.EqualTo(18400));
            Assert.That(tx.Type, Is.EqualTo(TransactionTypes.Types.HASH_LOCK));
            Assert.That(tx.Amount, Is.EqualTo(10000000));
            Assert.That(tx.Duration, Is.EqualTo(1000));
            Assert.That(tx.MosaicId, Is.EqualTo("6BED913FA20223F8"));
            Assert.That(tx.Hash, Is.EqualTo("5D02CD997313D5810AD077D4F6F0FCA2421602E11FF034C057BBA9C1A0FC867D"));
        }

        [Test, Timeout(20000)]
        public async Task GetSecretlockNonEmbeddedTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("9D4F3856CE6A748C6DA73DEFF92084C23D578032FA307E9E68EEA040189174C6");

            var tx = (SecretLockT)response.Transaction;

            Assert.That(tx.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.That(tx.SignerPublicKey, Is.EqualTo("D4A1468E54DD31B850CF9ABFFD32EFB98547091301668E777A43D3D88BEB76D8"));
            Assert.That(tx.Deadline, Is.EqualTo(8765036218));
            Assert.That(tx.MaxFee, Is.EqualTo(5225));
            Assert.That(tx.Type, Is.EqualTo(TransactionTypes.Types.SECRET_LOCK));
            Assert.That(tx.Amount, Is.EqualTo(1));
            Assert.That(tx.Duration, Is.EqualTo(20160));
            Assert.That(tx.MosaicId, Is.EqualTo("6BED913FA20223F8"));
            Assert.That(tx.Secret, Is.EqualTo("90A3DAF40EAEDAF79B79C8E8B9AF78EF59CF18A39866BD253404EAB737F41829"));
            Assert.That(tx.RecipientAddress, Is.EqualTo("68DDFBC7F8E3676FB5158D924C9E204496D8ADB1A36658EB"));
        }

        [Test, Timeout(20000)]
        public async Task GetSecretproofNonEmbeddedTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("81371DB98536CC911DB10C1D08AA69D495D1B2840850AFA15D618826F72AEE12");

            var tx = (SecretProofT)response.Transaction;

            Assert.That(tx.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
            Assert.That(tx.SignerPublicKey, Is.EqualTo("D4A1468E54DD31B850CF9ABFFD32EFB98547091301668E777A43D3D88BEB76D8"));
            Assert.That(tx.Deadline, Is.EqualTo(8765210999));
            Assert.That(tx.MaxFee, Is.EqualTo(5175));
            Assert.That(tx.Type, Is.EqualTo(TransactionTypes.Types.SECRET_PROOF));
            Assert.That(tx.HashAlgorithm, Is.EqualTo(0));
            Assert.That(tx.Proof, Is.EqualTo("614B1FCE6279B5A0EE68BCB0417F8FECB3AF9092"));
            Assert.That(tx.Secret, Is.EqualTo("90A3DAF40EAEDAF79B79C8E8B9AF78EF59CF18A39866BD253404EAB737F41829"));
            Assert.That(tx.RecipientAddress, Is.EqualTo("68DDFBC7F8E3676FB5158D924C9E204496D8ADB1A36658EB"));
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

                Assert.That(tx.RecipientAddress.Length, Is.EqualTo(48));
                Assert.That(tx.SignerPublicKey, Is.EqualTo(pubKey));
                Assert.That(tx.Mosaics[0].Amount, Is.GreaterThan(0));
                Assert.That(tx.Mosaics[0].Id.Length, Is.EqualTo(16));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));
                Assert.That(i.Id.Length, Is.EqualTo(24));
                Assert.That(tx.Type, Is.EqualTo(TransactionTypes.Types.TRANSFER));
            });
        }

        [Test, Timeout(20000)]
        public async Task GetMosaicSupplyRevocationTransaction()
        {

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("9B4D7D69E671E60D7862D7AFC183896A1758FD144C0C1A93BA1BA93191F1CDFE");

            var tx = (MosaicSupplyRevocation)response.Transaction;

            Assert.That(tx.Amount, Is.EqualTo(9));
            Assert.That(tx.SourceAddress, Is.EqualTo("68EC4E549EB78CF7623F1CF4CDE5FE5BBADA55C5D504DAF9"));
            Assert.That(tx.Size, Is.EqualTo(168));
            Assert.That(tx.MosaicId, Is.EqualTo("0CEDE2DEDDB4832F"));
            Assert.That(tx.Type, Is.EqualTo(TransactionTypes.Types.MOSAIC_SUPPLY_REVOCATION));
            Assert.That(tx.SignerPublicKey, Is.EqualTo("A2B0D50C7DB2724FEF6037821C86E62CC6C31F57AC166A36033267DA47424304"));
        }

        [Test, Timeout(20000)]
        public async Task GetAddressAliasTransaction()
        {

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("54B99E12887443F0B6A2DAA6120EF72384B1A2BBC1CA4AF345198E6E72653770");

            var tx = (AddressAlias)response.Transaction;


            Assert.That(tx.AliasAction, Is.EqualTo(1));
            Assert.That(tx.NamespaceId, Is.EqualTo("EF2AB54AFF2588B0"));
            Assert.That(tx.Address, Is.EqualTo("68F5D60E17C33D26DD24E0BE4E1C0DF26E8D758A250398B4"));
            Assert.That(tx.Size, Is.EqualTo(161));
            Assert.That(tx.Type, Is.EqualTo(TransactionTypes.Types.ADDRESS_ALIAS));
            Assert.That(tx.SignerPublicKey, Is.EqualTo("CE493A883EE3DE0DFF72ECC93E8AE6B5D7A1C67C4979BFF887EE0C379DAF11DE"));
        }

        [Test, Timeout(20000)]
        public async Task GetMosaicAliasTransaction()
        {

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("8135D5533F45765ADE747BFEB06474CB11EEB02E221ACBD42295F8F1D237C467");

            var tx = (MosaicAlias)response.Transaction;


            Assert.That(tx.AliasAction, Is.EqualTo(1));
            Assert.That(tx.NamespaceId, Is.EqualTo("E74B99BA41F4AFEE"));
            Assert.That(tx.MosaicId, Is.EqualTo("6BED913FA20223F8"));
            Assert.That(tx.Type, Is.EqualTo(TransactionTypes.Types.MOSAIC_ALIAS));
            Assert.That(tx.SignerPublicKey, Is.EqualTo("BE0B4CF546B7B4F4BBFCFF9F574FDA527C07A53D3FC76F8BB7DB746F8E8E0A9F"));
        }

        [Test, Timeout(20000)]
        public async Task GetRootNamespaceRegistrationTransaction()
        {

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("FF31ABA28DEA461AFC0C4A68F31AB7CCD86EFCAA6A3781B6B741B59A4DDC01C2");

            var tx = (RootNamespaceRegistration)response.Transaction;
        
            Assert.That(tx.RegistrationType, Is.EqualTo(0));
            Assert.That(tx.Duration, Is.EqualTo(0));
            Assert.That(tx.Id, Is.EqualTo("A95F1F8A96159516"));
            Assert.That(tx.Type, Is.EqualTo(TransactionTypes.Types.NAMESPACE_REGISTRATION));
            Assert.That(tx.Size, Is.EqualTo(152));

            Assert.That(tx.SignerPublicKey, Is.EqualTo("BE0B4CF546B7B4F4BBFCFF9F574FDA527C07A53D3FC76F8BB7DB746F8E8E0A9F"));
        }

        [Test, Timeout(20000)]
        public async Task GetAccountAddressRestrictionTransaction()
        {

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("2A5280F16603DCF1544619D87BB0BC367F29C32D3D52C5B659744B7CEE6301A6");

            var tx = (AccountRestriction)response.Transaction;

            Assert.That(tx.RestrictionFlags[0], Is.EqualTo(RestrictionTypes.Types.ADDRESS));
            Assert.That(tx.RestrictionAdditions[0], Is.EqualTo("68E1B300EDBBCE31ED8F922BFDEE477D47061E44CB46CC64"));
            Assert.That(tx.RestrictionDeletions.Count, Is.EqualTo(0));
            Assert.That(tx.Type, Is.EqualTo(TransactionTypes.Types.ACCOUNT_ADDRESS_RESTRICTION));
            Assert.That(tx.Size, Is.EqualTo(160));

            Assert.That(tx.SignerPublicKey, Is.EqualTo("832BFCCC60E3E76C3B9FC63C10751064FA9A9FCC5E00DE7F283F1D0B66A25486"));
        }

        [Test, Timeout(20000)]
        public async Task GetAccountMosaicRestrictionTransaction()
        {

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("B8A9E72ADC49D2A880C49D6D4A8F88D3B64137DC0EC0A399CD9A1A8FB4C16FC0");

            var tx = (AccountRestriction)response.Transaction;

            Assert.That(tx.RestrictionFlags[0], Is.EqualTo(RestrictionTypes.Types.BLOCK));
            Assert.That(tx.RestrictionFlags[1], Is.EqualTo(RestrictionTypes.Types.MOSAIC_ID));
            Assert.That(tx.RestrictionAdditions[0], Is.EqualTo("051FAEC15105C808"));
            Assert.That(tx.RestrictionDeletions.Count, Is.EqualTo(0));
            Assert.That(tx.Type, Is.EqualTo(TransactionTypes.Types.ACCOUNT_MOSAIC_RESTRICTION));
            Assert.That(tx.Size, Is.EqualTo(144));

            Assert.That(tx.SignerPublicKey, Is.EqualTo("BD15F73AEC98D613DC7290095B96328A76A0D41324E21F717E67FA2C73074D8D"));
        }

        [Test, Timeout(20000)] 
        public async Task GetAccountOperationRestrictionTransaction()
        {

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("53D3ED8322AF889A37DFB6DC42B07269E413D91B6ACE51E065A2030C0D4E5266");

            var tx = (AccountOperationRestriction)response.Transaction;

            Assert.That(tx.RestrictionFlags[0], Is.EqualTo(RestrictionTypes.Types.BLOCK));
            Assert.That(tx.RestrictionFlags[1], Is.EqualTo(RestrictionTypes.Types.OUTGOING));
            Assert.That(tx.RestrictionAdditions[0], Is.EqualTo(TransactionTypes.Types.ACCOUNT_ADDRESS_RESTRICTION));
            Assert.That(tx.RestrictionDeletions.Count, Is.EqualTo(0));
            Assert.That(tx.Type, Is.EqualTo(TransactionTypes.Types.ACCOUNT_OPERATION_RESTRICTION));
            Assert.That(tx.Size, Is.EqualTo(138));

            Assert.That(tx.SignerPublicKey, Is.EqualTo("95B0EA9B688FB7670D4FD5AC4754CB2DFCCEE04592D5D82CC24C342A5BA0B799"));
        }

        [Test, Timeout(20000)] 
        public async Task GetMosaicAddressRestrictionTransaction()
        {

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("FF44C70577FDE571DC03A3827A3DAD138EC9D21C3839C63217896B4E992F7897");

            var tx = (MosaicAddressRestriction)response.Transaction;

            Assert.That(tx.NewRestrictionValue, Is.EqualTo(1));
            Assert.That(tx.PreviousRestrictionValue, Is.EqualTo(18446744073709551615));
            Assert.That(tx.RestrictionKey, Is.EqualTo("CBED702BC844E81A"));
            Assert.That(tx.TargetAddress, Is.EqualTo("6827E508C5BB821AAB4541547D0307278DE378846AB824D7")); 
            Assert.That(tx.MosaicId, Is.EqualTo("613E6D0FC11F4530"));
            Assert.That(tx.Type, Is.EqualTo(TransactionTypes.Types.MOSAIC_ADDRESS_RESTRICTION));
            Assert.That(tx.Size, Is.EqualTo(184));
            Assert.That(tx.SignerPublicKey, Is.EqualTo("A39EA1EEA2BF80902ED5B573FC9DEE1EDF53FB6E05099669743DFA3E8233400E"));
        }

        [Test, Timeout(20000)]
        public async Task SearchTransferTransactionWithMessege()
        {
            string pubKey = "BE0B4CF546B7B4F4BBFCFF9F574FDA527C07A53D3FC76F8BB7DB746F8E8E0A9F";

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("11B55558B111E21CABAE7278DE2D3CF393A2384F65AF2C62B88872312FFD0101");


            var tx = (SimpleTransfer)response.Transaction;

            Assert.That(tx.Message, Is.EqualTo("FE2A8061577301E2402E3F75637E6EFD62DBA4580EE027304459C8C6C50C0E305766F88AE75F6734F6FA6C36A1E6F5093CBB53FC3F8F4BD34B5709DC46A3DB5104685E233024B972E5543FEC16B4458F712FD0AAA00E61CE3B716811DA4E3BB3F1F6851BCD0D58D892BF213BA3F3CE72918F70AA2F78B333654AB2AF8E09F8318C2A63F5"));
            Assert.That(tx.RecipientAddress, Is.EqualTo("68BA45B6240991DA609C702A2DC3ECC1BED47FA589ED331B"));
            Assert.That(tx.SignerPublicKey, Is.EqualTo("D32168A40E4A2DB9F1FB0D60554BFCE3142835CFFFF6D2BB104AE97F8B4829B4"));
            Assert.That(tx.Mosaics, Is.Empty);
            Assert.That(tx.Type, Is.EqualTo(TransactionTypes.Types.TRANSFER));
            Assert.That(response.Meta.Hash.Length, Is.EqualTo(64));
            Assert.That(response.Id.Length, Is.EqualTo(24));
        }
    }
}
