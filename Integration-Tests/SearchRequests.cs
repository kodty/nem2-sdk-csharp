using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Infrastructure.Buffers.Model.JsonConverters;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Model.Network;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reactive.Linq;

namespace Integration_Tests
{
    public class Tests
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
            Assert.That(acc.Address.Plain, Is.EqualTo("NASYMBOLLK6FSL7GSEMQEAWN7VW55ZSZU25TBOA"));

            var hashClient = new TransactionHttp("75.119.150.108", 3000);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.TRANSFER.GetValue());
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.HASH_LOCK.GetValue());

            var response = await hashClient.SearchConfirmedTransactions(qModel);

            response.ForEach(i => {
                ((SimpleTransfer)i.Transaction).Mosaics
                    .ForEach(m =>
                    {
                        Assert.That(m.Id, Is.EqualTo("E74B99BA41F4AFEE"));
                        Assert.That(m.Amount, Is.GreaterThan(0));

                    });

                Assert.That(i.Meta.Height, Is.GreaterThan(0));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));
            });
        }

        [Test, Timeout(20000)]
        public async Task SearchTransferTransaction()
        {
            string pubKey = "BE0B4CF546B7B4F4BBFCFF9F574FDA527C07A53D3FC76F8BB7DB746F8E8E0A9F";

            var hashClient = new TransactionHttp("75.119.150.108", 3000);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.TRANSFER.GetValue());

            var response = await hashClient.SearchConfirmedTransactions(qModel);

            response.ForEach(i => {
                
                var tx = ((SimpleTransfer)i.Transaction);

                Assert.That(tx.RecipientAddress, Is.EqualTo("68CA6F5728706A29F42FB0F727F0850DC27ED7C3685232A6"));
                Assert.That(tx.SignerPublicKey, Is.EqualTo(pubKey));
                Assert.That(tx.Mosaics[0].Amount, Is.GreaterThan(0));
                Assert.That(tx.Mosaics[0].Id, Is.EqualTo("E74B99BA41F4AFEE"));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));
                Assert.That(i.Id.Length, Is.EqualTo(24));
                Assert.That(tx.Type, Is.EqualTo(TransactionTypes.Types.TRANSFER));
            });
        }

        [Test, Timeout(20000)] 
        public async Task SearchTransferTransactionWithMessege()
        {
            string pubKey = "BE0B4CF546B7B4F4BBFCFF9F574FDA527C07A53D3FC76F8BB7DB746F8E8E0A9F";

            var hashClient = new TransactionHttp("75.119.150.108", 3000);

            var response = await hashClient.GetConfirmedTransaction("11B55558B111E21CABAE7278DE2D3CF393A2384F65AF2C62B88872312FFD0101");

            
            var tx = ((SimpleTransfer)response.Transaction);

            Assert.That(tx.Message, Is.EqualTo("FE2A8061577301E2402E3F75637E6EFD62DBA4580EE027304459C8C6C50C0E305766F88AE75F6734F6FA6C36A1E6F5093CBB53FC3F8F4BD34B5709DC46A3DB5104685E233024B972E5543FEC16B4458F712FD0AAA00E61CE3B716811DA4E3BB3F1F6851BCD0D58D892BF213BA3F3CE72918F70AA2F78B333654AB2AF8E09F8318C2A63F5"));
            Assert.That(tx.RecipientAddress, Is.EqualTo("68BA45B6240991DA609C702A2DC3ECC1BED47FA589ED331B"));
            Assert.That(tx.SignerPublicKey, Is.EqualTo("D32168A40E4A2DB9F1FB0D60554BFCE3142835CFFFF6D2BB104AE97F8B4829B4"));
            Assert.That(tx.Mosaics, Is.Empty);
            Assert.That(tx.Type, Is.EqualTo(TransactionTypes.Types.TRANSFER));
            Assert.That(response.Meta.Hash.Length, Is.EqualTo(64));
            Assert.That(response.Id.Length, Is.EqualTo(24));
        }


        [Test, Timeout(20000)]
        public async Task SearcNameSpaceRegistrationTransaction()
        {
            string pubKey = "BE0B4CF546B7B4F4BBFCFF9F574FDA527C07A53D3FC76F8BB7DB746F8E8E0A9F";

            var hashClient = new TransactionHttp("75.119.150.108", 3000);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.NAMESPACE_REGISTRATION.GetValue());

            var response = await hashClient.SearchConfirmedTransactions(qModel);

            response.ForEach(i => {

                var tx = ((NamespaceRegistration)i.Transaction);

                if (tx.RegistrationType == 0)
                {
                    tx = (RootNamespaceRegistration)tx;
                    Assert.That(tx.SignerPublicKey, Is.EqualTo(pubKey));
                    Assert.That(tx.Name, Is.EqualTo("symbol"));
                    Assert.That(tx.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
                }
                if (tx.RegistrationType == 1)
                {
                    tx = (ChildNamespaceRegistration)tx;
                    Assert.That(tx.SignerPublicKey, Is.EqualTo(pubKey));
                    Assert.That(tx.Name, Is.EqualTo("xym"));
                    Assert.That(tx.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
                }
            });
        }

        [Test, Timeout(20000)]
        public async Task SearchMosaicSupplyTransaction()
        {
            string pubKey = "BE0B4CF546B7B4F4BBFCFF9F574FDA527C07A53D3FC76F8BB7DB746F8E8E0A9F";

            var hashClient = new TransactionHttp("75.119.150.108", 3000);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.MOSAIC_SUPPLY_CHANGE.GetValue());

            var response = await hashClient.SearchConfirmedTransactions(qModel);

            response.ForEach(i => {

                var tx = ((MosaicSupplyChange)i.Transaction);

                Assert.That(tx.SignerPublicKey, Is.EqualTo(pubKey));
                Assert.That(tx.Action, Is.EqualTo(1));
                Assert.That(tx.Signature, Is.EqualTo("6FC30E98378ADBA9F79D5CEF2ECBCB6D3AD6010FC265708E62419862534D51E3F56B688B55B01AE631281CC589FB1FEFF43D88141B13AD5C9C63A5E15D0E320A"));
                Assert.That(tx.Version, Is.EqualTo(1));
                Assert.That(i.Meta.Index, Is.EqualTo(4));
            });
        }

        [Test, Timeout(20000)]
        public async Task SearchMosaicSupplyRevocationTransaction()
        {
            string pubKey = "6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D";

            var hashClient = new TransactionHttp("75.119.150.108", 3000);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.MOSAIC_SUPPLY_REVOCATION.GetValue());

            var response = await hashClient.SearchConfirmedTransactions(qModel);

            response.ForEach(i => {

                var tx = ((MosaicSupplyRevocation)i.Transaction);

                Assert.That(tx.SignerPublicKey, Is.EqualTo("6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D"));
                Assert.That(tx.SourceAddress.Length, Is.EqualTo(48));
                Assert.That(tx.Version, Is.EqualTo(1));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));
                Assert.That(i.Id.Length, Is.EqualTo(24));
            });
        }

        [Test, Timeout(20000)]
        public async Task SearchMosaicDefinitionTransaction()
        {
            string pubKey = "6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D";

            var hashClient = new TransactionHttp("75.119.150.108", 3000);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.MOSAIC_DEFINITION.GetValue());

            var response = await hashClient.SearchConfirmedTransactions(qModel);

            response.ForEach(i => {

                var tx = ((MosaicDefinition)i.Transaction);

                Assert.That(tx.Duration, !Is.EqualTo(null));
                Assert.That(tx.SignerPublicKey, Is.EqualTo("6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D"));
                Assert.That(i.Meta, !Is.EqualTo(null));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));
                Assert.That(tx.Version, Is.EqualTo(1));
            });
        }

        [Test, Timeout(20000)]
        public async Task SearchHashLockTransaction()
        {
            string pubKey = "1799A50301C17D0BA45D2599193B49C4A5377640B3D6695B84F6320466958B5C";

            var hashClient = new TransactionHttp("75.119.150.108", 3000);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.HASH_LOCK.GetValue());

            var response = await hashClient.SearchConfirmedTransactions(qModel);

            response.ForEach(i => {

                var tx = ((HashLockT)i.Transaction);

                Assert.That(tx.SignerPublicKey, Is.EqualTo(pubKey));
                Assert.That(tx.Amount, Is.GreaterThan(0));
                Assert.That(i.Meta, !Is.EqualTo(null));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));

            });
        }

        [Test, Timeout(20000)]
        public async Task SearchSecretLockTransaction()
        {
            string pubKey1 = "1799A50301C17D0BA45D2599193B49C4A5377640B3D6695B84F6320466958B5C";
            string pubKey = "D4A1468E54DD31B850CF9ABFFD32EFB98547091301668E777A43D3D88BEB76D8";
            var hashClient = new TransactionHttp("75.119.150.108", 3000);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);

            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.SECRET_LOCK.GetValue());
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.HASH_LOCK.GetValue());

            var response = await hashClient.SearchConfirmedTransactions(qModel);

            response.ForEach(i => {

                
                if (i.Transaction.Type.GetValue() == 16722)
                {
                    var tx = ((SecretLockT)i.Transaction);

                    Assert.That(tx.SignerPublicKey, Is.EqualTo(pubKey));
                    Assert.That(tx.Secret.Length, Is.GreaterThan(0));
                }
                if (i.Transaction.Type.GetValue() == 16712)
                {
                    var tx = ((HashLockT)i.Transaction);

                    Assert.That(tx.SignerPublicKey, Is.EqualTo(pubKey));
                    Assert.That(tx.Hash.Length, Is.GreaterThan(0));
                }

            });
        }

        [Test, Timeout(20000)]
        public async Task SearchSecretProofTransaction()
        {
            string pubKey1 = "1799A50301C17D0BA45D2599193B49C4A5377640B3D6695B84F6320466958B5C";
            string pubKey = "D4A1468E54DD31B850CF9ABFFD32EFB98547091301668E777A43D3D88BEB76D8";
            var hashClient = new TransactionHttp("75.119.150.108", 3000);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);

            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.SECRET_PROOF.GetValue());

            var response = await hashClient.SearchConfirmedTransactions(qModel);

            response.ForEach(i => {

                if (i.Transaction.Type.GetValue() == 16722)
                {
                    var tx = ((SecretProofT)i.Transaction);

                    Assert.That(tx.SignerPublicKey, Is.EqualTo(pubKey));
                    Assert.That(tx.Secret.Length, Is.GreaterThan(0));
                }
            });
        }

        [Test, Timeout(20000)]
        public async Task SearchAddressAlias()
        {
            string pubKey = "6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D";

            var hashClient = new TransactionHttp("75.119.150.108", 3000);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.ADDRESS_ALIAS.GetValue());

            var response = await hashClient.SearchConfirmedTransactions(qModel);

            response.ForEach(i => {

                var tx = ((AddressAlias)i.Transaction);

                Assert.That(tx.Address, Is.EqualTo("684575A96630EC6C0B9FBF3408007213321AFF07A7837E50"));
                Assert.That(tx.SignerPublicKey, Is.EqualTo("6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D"));
                Assert.That(i.Meta, !Is.EqualTo(null));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));
                Assert.That(i.Id.Length, Is.EqualTo(24));
                Assert.That(tx.Version, Is.EqualTo(1));
                Assert.That(tx.AliasAction, Is.GreaterThanOrEqualTo(0));
            });
        }

        [Test, Timeout(20000)]
        public async Task SearchMosaicAlias()
        {
            string pubKey = "BE0B4CF546B7B4F4BBFCFF9F574FDA527C07A53D3FC76F8BB7DB746F8E8E0A9F";

            var hashClient = new TransactionHttp("75.119.150.108", 3000);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.MOSAIC_ALIAS.GetValue());

            var response = await hashClient.SearchConfirmedTransactions(qModel);

            response.ForEach(i => {

                var tx = ((MosaicAlias)i.Transaction);

                Assert.That(tx.AliasAction, Is.GreaterThan(-1));
                Assert.That(tx.SignerPublicKey, Is.EqualTo(pubKey));
                Assert.That(i.Meta, !Is.EqualTo(null));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));
                Assert.That(i.Id.Length, Is.EqualTo(24));
                Assert.That(tx.Version, Is.EqualTo(1));
            });
        }

        [Test, Timeout(20000)]
        public async Task SearchNodeKeyLinkTransaction()
        {
            string pubKey = "0B349D6FB4E93FAB29065D51B7A5375FFAF3856BA7F64DDE66B86579816D6E77";

            var hashClient = new TransactionHttp("75.119.150.108", 3000);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.NODE_KEY_LINK.GetValue());

            var response = await hashClient.SearchConfirmedTransactions(qModel);

            response.ForEach(i => {

                var tx = ((KeyLink)i.Transaction);

                Assert.That(tx.SignerPublicKey, Is.EqualTo(pubKey));
                Assert.That(tx.LinkedPublicKey.Length, Is.EqualTo(64)); 
            });
        }

        [Test, Timeout(20000)]
        public async Task SearchAccountKeyLinkTransaction()
        {
            var hashClient = new TransactionHttp("75.119.150.108", 3000);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.ACCOUNT_KEY_LINK.GetValue());

            var response = await hashClient.SearchConfirmedTransactions(qModel);

            response.ForEach(i => {

                var tx = ((KeyLink)i.Transaction);

                Assert.That(i.Id.Length, Is.EqualTo(24));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));
                Assert.That(i.Meta.MerkleComponentHash.Length, Is.EqualTo(64));
                Assert.That(tx.SignerPublicKey.Length, Is.EqualTo(64));
                Assert.That(tx.LinkedPublicKey.Length, Is.EqualTo(64));
            });
        }

        [Test, Timeout(20000)]
        public async Task SearchVotingKeyLinkTransaction()
        {
            string pubKey = "AFF16052217A847A6A71B326FEA9073CFF70D07FC5BA9026B3E05FB453C950DF";

            var hashClient = new TransactionHttp("75.119.150.108", 3000);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.VOTING_KEY_LINK.GetValue());

            var response = await hashClient.SearchConfirmedTransactions(qModel);

            response.ForEach(i => {

                var tx = ((VotingKeyLink)i.Transaction);

                Assert.That(i.Id.Length, Is.EqualTo(24));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));
                Assert.That(i.Meta.MerkleComponentHash.Length, Is.EqualTo(64));
                Assert.That(tx.SignerPublicKey, Is.EqualTo(pubKey));
                Assert.That(tx.LinkedPublicKey.Length, Is.EqualTo(64));
            });
        }

        [Test, Timeout(20000)]
        public async Task SearchVRFKeyLinkTransaction()
        {
            string pubKey = "AFF16052217A847A6A71B326FEA9073CFF70D07FC5BA9026B3E05FB453C950DF";

            var hashClient = new TransactionHttp("75.119.150.108", 3000);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.VRF_KEY_LINK.GetValue());

            var response = await hashClient.SearchConfirmedTransactions(qModel);

            response.ForEach(i => {

                var tx = ((KeyLink)i.Transaction);

                Assert.That(i.Id.Length, Is.EqualTo(24));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));
                Assert.That(i.Meta.MerkleComponentHash.Length, Is.EqualTo(64));
                Assert.That(tx.SignerPublicKey, Is.EqualTo(pubKey));
                Assert.That(tx.LinkedPublicKey.Length, Is.EqualTo(64));
            });
        }





        [Test, Timeout(20000)]
        public async Task SearchMosaicAddressRestriction()
        {
            string pubKey = "832BFCCC60E3E76C3B9FC63C10751064FA9A9FCC5E00DE7F283F1D0B66A25486";

            var hashClient = new TransactionHttp("75.119.150.108", 3000);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.MOSAIC_ADDRESS_RESTRICTION.GetValue());

            var response = await hashClient.SearchConfirmedTransactions(qModel);

            response.ForEach(i => {

                var tx = ((MosaicAddressRestriction)i.Transaction);

                Assert.That(tx.RestrictionKey.Length, Is.GreaterThan(0));
                Assert.That(tx.SignerPublicKey, Is.EqualTo("832BFCCC60E3E76C3B9FC63C10751064FA9A9FCC5E00DE7F283F1D0B66A25486"));
                Assert.That(i.Meta, !Is.EqualTo(null));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));
                Assert.That(i.Id.Length, Is.EqualTo(24));
                Assert.That(tx.Version, Is.EqualTo(1));
            });
        }

        [Test, Timeout(20000)]
        public async Task SearchAccountOpperationRestriction()
        {
            string pubKey = "6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D";

            var hashClient = new TransactionHttp("75.119.150.108", 3000);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.ACCOUNT_OPERATION_RESTRICTION.GetValue());

            var response = await hashClient.SearchConfirmedTransactions(qModel);

            response.ForEach(i => {

                var tx = ((AccountOpperationRestriction)i.Transaction);

                Assert.That(tx.Type, Is.EqualTo(TransactionTypes.Types.ACCOUNT_OPERATION_RESTRICTION));
                Assert.That(tx.SignerPublicKey, Is.EqualTo("6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D"));
                Assert.That(i.Meta, !Is.EqualTo(null));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));
                Assert.That(i.Id.Length, Is.EqualTo(24));
                Assert.That(tx.Version, Is.EqualTo(1));
            });
        }

        [Test, Timeout(20000)]
        public async Task SearchAccountMosaicRestriction()
        {
            string pubKey = "6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D";

            var hashClient = new TransactionHttp("75.119.150.108", 3000);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.ACCOUNT_MOSAIC_RESTRICTION.GetValue());

            var response = await hashClient.SearchConfirmedTransactions(qModel);

            response.ForEach(i => {

                var tx = ((AccountMosaicRestriction)i.Transaction);

                Assert.That(tx.SignerPublicKey.Length, Is.GreaterThan(0));
                Assert.That(tx.SignerPublicKey, Is.EqualTo("6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D"));
                Assert.That(i.Meta, !Is.EqualTo(null));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));
                Assert.That(i.Id.Length, Is.EqualTo(24));
                Assert.That(tx.Version, Is.EqualTo(1));
            });
        }

        [Test, Timeout(20000)]
        public async Task SearchAccountAddressRestriction()
        {
            string pubKey = "6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D";

            var hashClient = new TransactionHttp("75.119.150.108", 3000);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.MOSAIC_ADDRESS_RESTRICTION.GetValue());

            var response = await hashClient.SearchConfirmedTransactions(qModel);

            response.ForEach(i => {

                var tx = ((MosaicAddressRestriction)i.Transaction);

                Assert.That(tx.RestrictionKey.Length, Is.GreaterThan(0));
                Assert.That(tx.SignerPublicKey, Is.EqualTo("6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D"));
                Assert.That(i.Meta, !Is.EqualTo(null));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));
                Assert.That(tx.Version, Is.EqualTo(1));
            });
        }

        [Test, Timeout(20000)]
        public async Task GetAggregatesComplete()
        {
            string pubKey = "6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D";

            var hashClient = new TransactionHttp("75.119.150.108", 3000);

            var response = await hashClient.GetConfirmedTransactions(new string[] { "6644D77CED4FBE214609F1C3", "6644D77CED4FBE214609F1C3" });

            response.ForEach(i => {

                var tx = ((Aggregate)i.Transaction);

                Assert.That(i.Id, Is.EqualTo("6644D77CED4FBE214609F1C3"));
                Assert.That(i.Meta.Hash, Is.EqualTo("E906272E7A715CD24D959A51CDFADC4CC8CA0E63097EA161C1DEBD31E9754A74"));
                Assert.That(i.Meta.Index, Is.EqualTo(25465));
                Assert.That(i.Meta.Timestamp, Is.EqualTo(0)); 
                Assert.That(i.Meta.MerkleComponentHash, Is.EqualTo("904E12F6F155A858C89568A63C23E1F5CDB8AC5220969BB59BD22879FF334F83"));
                Assert.That(i.Meta.FeeMultiplier, Is.EqualTo(0));
                Assert.That(tx.Size, Is.EqualTo(864));
                Assert.That(tx.Transactions, !Is.Null);
                Assert.That(tx.Network, Is.EqualTo(NetworkType.Types.MAIN_NET)); 
                Assert.That(tx.TransactionsHash, Is.EqualTo("9D7D525E22C0DBEEA4D0F8E6C1AC4E301399C3EDD3CA7E6D2ACC6E4D13677CE6"));
                Assert.That(tx.Signature, Is.EqualTo("35B6E3B1C311AA6A957EF2AD12447AD790A5197454ECC27BCE02527257EE317E404367C416A41E53D8CA851393AC58F59343435230CC6F75EB4A49C784BDCD03"));
                Assert.That(tx.Deadline, Is.EqualTo(1));
                Assert.That(tx.MaxFee, Is.EqualTo(0));
                Assert.That(tx.Type, Is.EqualTo(TransactionTypes.Types.AGGREGATE_COMPLETE));
                Assert.That(tx.SignerPublicKey, Is.EqualTo("FA9F3974FE3B15585E6B72672C7D8BEAE27D1EDF6C4752BAFDB8B2FEA601C0CF"));

                tx.Cosignatures.ForEach(i => {

                    Assert.That(i.SignerPublicKey.Length, Is.EqualTo(64));
                    Assert.That(i.Signature.Length, Is.EqualTo(128));
                    Assert.That(i.Version, Is.EqualTo(0));

                });

                Assert.That(tx.Transactions[0].Transaction.SignerPublicKey, Is.EqualTo("FA9F3974FE3B15585E6B72672C7D8BEAE27D1EDF6C4752BAFDB8B2FEA601C0CF"));
                Assert.That(tx.Transactions[0].Transaction.Network, Is.EqualTo(NetworkType.Types.MAIN_NET)); // network shouldnt be twice - check why
                Assert.That(tx.Transactions[0].Transaction.Type, Is.EqualTo(TransactionTypes.Types.MULTISIG_ACCOUNT_MODIFICATION));
                Assert.That(tx.Transactions[0].Transaction.Version, Is.EqualTo(1));

                var embedded = (MultisigModification)tx.Transactions[0].Transaction;

                Assert.That(tx.Transactions[0].Id, Is.EqualTo("6644D77CED4FBE214609F1CF"));
                Assert.That(tx.Transactions[0].Meta.Index, Is.EqualTo(0));
                Assert.That(tx.Transactions[0].Meta.Height, Is.EqualTo(1));
                Assert.That(tx.Transactions[0].Meta.Timestamp, Is.EqualTo(0));
                Assert.That(tx.Transactions[0].Meta.FeeMultiplier, Is.EqualTo(0));
                Assert.That(tx.Transactions[0].Meta.AggregateHash, Is.EqualTo("E906272E7A715CD24D959A51CDFADC4CC8CA0E63097EA161C1DEBD31E9754A74"));
                Assert.That(tx.Transactions[0].Meta.AggregateId, Is.EqualTo("6644D77CED4FBE214609F1C3"));
             
                Assert.That(embedded.SignerPublicKey, Is.EqualTo("FA9F3974FE3B15585E6B72672C7D8BEAE27D1EDF6C4752BAFDB8B2FEA601C0CF"));
                Assert.That(embedded.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
                Assert.That(embedded.Version, Is.EqualTo(1));
                Assert.That(embedded.minApprovalDelta, Is.EqualTo(4));
                Assert.That(embedded.minRemovalDelta, Is.EqualTo(5));
                Assert.That(embedded.addressAdditions[0].Length, Is.EqualTo(48));
            });
        }

        [Test, Timeout(20000)]
        public async Task GetAggregateComplete()
        {
            string pubKey = "6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D";

            var hashClient = new TransactionHttp("75.119.150.108", 3000);

            var response = await hashClient.GetConfirmedTransaction("E906272E7A715CD24D959A51CDFADC4CC8CA0E63097EA161C1DEBD31E9754A74");

            var tx = ((Aggregate)response.Transaction);

            Assert.That(response.Meta.Hash, Is.EqualTo("E906272E7A715CD24D959A51CDFADC4CC8CA0E63097EA161C1DEBD31E9754A74"));
            Assert.That(response.Meta.Index, Is.EqualTo(25465));
            Assert.That(response.Meta.MerkleComponentHash, Is.EqualTo("904E12F6F155A858C89568A63C23E1F5CDB8AC5220969BB59BD22879FF334F83"));
            Assert.That(response.Meta.Height, Is.EqualTo(1));
            Assert.That(response.Meta.Timestamp, Is.EqualTo(0));
            Assert.That(response.Id, Is.EqualTo("6644D77CED4FBE214609F1C3"));

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
      
            var embedded = (MultisigModification)tx.Transactions[0].Transaction;

            Assert.That(embedded.addressAdditions[0].Length, Is.EqualTo(48));
            Assert.That(embedded.addressDeletions.Count, Is.EqualTo(0));
        }
    }
}