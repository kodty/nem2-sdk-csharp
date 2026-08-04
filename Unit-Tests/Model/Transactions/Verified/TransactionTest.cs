using Coppery;
using Integration_Tests;
using io.nem2.sdk.Infrastructure.HttpClients;
using io.nem2.sdk.Model;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Articles;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.Model.Transactions.Messages;
using io.nem2.sdk.Utils;
using System.Reactive.Linq;

namespace Unit_Tests.Model.Transactions.Verified
{
    internal class TransactionTest
    {

        [Test, Timeout(20000)]
        public async Task CreateAggregateTransactionTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            // embedded transaction

            var transfer = Transaction.Create(
               new TransferTransaction_V1(
                   address: Address.CreateFromEncoded("TDX7QVF6XXMJNDFFRIOYTV4N3GSVUGNTWVCIMZQ"),
                   messege: EmptyMessage.Create(),
                   mosaic: Mosaic.CreateFromHexIdentifier("72C0212E67A08BCE", 1000000)
                   ),
                NetworkType.Types.TEST_NET
               );

            transfer.SetSigner(keys.PublicKeyString);

            // aggregate transaction complete

            var aggregate = VerifiableTransaction.Create(
                new AggregatePayload([transfer]),
                NetworkType.Types.TEST_NET, 
                10000000,
                Deadline.AddHours(1));

            aggregate.SetSigner(keys.PublicKeyString);

            var result = aggregate.SignTransaction(keys, HttpSetUp.genHash);

            var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            var a = await client.Announce(result);

            Thread.Sleep(4321);
            var status = await client.GetTransactionStatus(result.Hash);

            Assert.AreEqual(status.ComposedResponse.Code, "Success");
        }

        [Test, Timeout(20000)]
        public async Task CreateTransferTransactionTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = VerifiableTransaction.Create(
                new TransferTransaction_V1(
                    address: Address.CreateFromEncoded("TDX7QVF6XXMJNDFFRIOYTV4N3GSVUGNTWVCIMZQ"),
                    messege: EmptyMessage.Create(),
                    mosaic: Mosaic.CreateFromHexIdentifier("72C0212E67A08BCE", 1000000)
                    ),
                 NetworkType.Types.TEST_NET,
                1000000,
                Deadline.AddHours(1)
                );

            transfer.SetSigner(keys.PublicKeyString);

            transfer.Deadline = DataConverter.ConvertFrom(117756998097);
            
            var result = transfer.SignTransaction(keys, HttpSetUp.genHash);

            Assert.That(result.VerifiablePayload.ToHex() == HttpSetUp.genHash + "0198544140420F0000000000D131DD6A1B00000098EFF854BEBDD8968CA58A1D89D78DD9A55A19B3B54486660000010000000000CE8BA0672E21C07240420F0000000000");
            Assert.That(result.Payload.ToHex(), Is.EqualTo("B000000000000000115504A388D963BF8B64400920CEBBC04597C0EC97E429C5B2660614440FD6A97E5A122FB7ADF2AC7DADA41CDEB23915E00BE23FE5F06B2B6896C4964E440600F8D6857FBE59B1E30C6EF73C208E3082AB0102352C8B67175E24B83D371DF3F7000000000198544140420F0000000000D131DD6A1B00000098EFF854BEBDD8968CA58A1D89D78DD9A55A19B3B54486660000010000000000CE8BA0672E21C07240420F0000000000"));
        }

        [Test, Timeout(20000)]
        public async Task CreateHashLockTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
               .CreateTransferTransaction(
                   Address.CreateFromEncoded(HttpSetUp.TestRecipient),
                   EmptyMessage.Create(),
                   Mosaic.CreateFromHexIdentifier("72C0212E67A08BCE", 1000000),
                   1000000
               );

            transfer.SetSigner(keys.PublicKeyString);
            transfer.Deadline = DataConverter.ConvertFrom(118252829489);

            var transferResult = transfer.SignTransaction(keys, HttpSetUp.genHash);

            Assert.That(transferResult.Payload.ToHex(), Is.EqualTo("B000000000000000FFF5CBDC9346843342BA5AF9A777C6D53B51591668D081653D9AB17440CCA8F189B1BBAA414C7DBD0044BBD1440904EF2C7D36421FDD887F5667C830B1FF9504F8D6857FBE59B1E30C6EF73C208E3082AB0102352C8B67175E24B83D371DF3F7000000000198544140420F000000000031FB6A881B00000098D9807AC250198EA57D689A7239DFA3B52E1506A3F71FDC0000010000000000CE8BA0672E21C07240420F0000000000"));

            var hashlock = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateHashLockTransaction(
                    "72C0212E67A08BCE",
                    10000000,
                    2880,
                    transferResult.Hash,
                    1000000
                );

            hashlock.SetSigner(keys.PublicKeyString);
            hashlock.Deadline = DataConverter.ConvertFrom(118252829538);

            var hashlockResult = hashlock.SignTransaction(keys, HttpSetUp.genHash);

            Assert.That(hashlockResult.Payload.ToHex(), Is.EqualTo("B8000000000000004B410773EDF2D53DF47DD2B5802E25C786B7041BF129619C908A56C3CC1B571CA4AA0B9133354D349E845AF87CDD753FCEBE28CB770449D0E970F235E07BB90CF8D6857FBE59B1E30C6EF73C208E3082AB0102352C8B67175E24B83D371DF3F7000000000198484140420F000000000062FB6A881B000000CE8BA0672E21C0728096980000000000400B000000000000D556C9E5630C16F3CBDCCB2C693219FD32AF95D8A8CC17F3A4844A6C35554B8E"));
        }

        public async Task CreateAggregateBondedTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);
            var keys2 = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.privKey);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateTransferTransaction(
                    Address.CreateFromEncoded("TDX7QVF6XXMJNDFFRIOYTV4N3GSVUGNTWVCIMZQ"),
                    PlainMessage.Create("hello"),
                    Mosaic.CreateFromHexIdentifier("72C0212E67A08BCE", 1000000),
                    1000000
                );

                transfer.SetSigner(keys.PublicKeyString);

            var transfer2 = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateTransferTransaction(
                    Address.CreateFromEncoded("TA3GCBHJBTRCEHVYVHCNUCULY2NB76W7MVECFUY"),
                    PlainMessage.Create("hello"),
                    Mosaic.CreateFromHexIdentifier("72C0212E67A08BCE", 200),
                    800000
                );

                transfer2.SetSigner(keys2.PublicKeyString);

            var aggregateBonded = VerifiableTransaction.Create(
                new AggregatePayload([transfer, transfer2]),
                NetworkType.Types.TEST_NET,
                10000000,
                Deadline.AddHours(1));


            //aggregateBonded.Cosign([keys2]);

            //transfer.Deadline = DataConverter.ConvertFrom((ulong)117756998097);

            var result = aggregateBonded.SignTransaction(keys, HttpSetUp.genHash);

            //Debug.WriteLine(result.Payload.ToHex());
            //Debug.WriteLine(DataConverter.ConvertTo<ulong>(aggregateBonded.Deadline));
            //Debug.WriteLine(DataConverter.ConvertTo<ulong>(aggregateBonded.Fee));

            //var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);
            //
            //var a = await client.Announce(result);
            //
            //Thread.Sleep(4321);
            //var status = await client.GetTransactionStatus(result.Hash);
            //
            //Assert.AreEqual(status.ComposedResponse.Code, "Success");

            // Assert.That(result.Payload.ToHex(), Is.EqualTo("B000000000000000115504A388D963BF8B64400920CEBBC04597C0EC97E429C5B2660614440FD6A97E5A122FB7ADF2AC7DADA41CDEB23915E00BE23FE5F06B2B6896C4964E440600F8D6857FBE59B1E30C6EF73C208E3082AB0102352C8B67175E24B83D371DF3F7000000000198544140420F0000000000D131DD6A1B00000098EFF854BEBDD8968CA58A1D89D78DD9A55A19B3B54486660000010000000000CE8BA0672E21C07240420F0000000000"));
        }

        [Test, Timeout(20000)]
        public async Task CreateMosaicDefinitionTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                 .CreateMosaicDefinitionTransaction(
                    DataConverter.ConvertFrom(IdGenerator.GenerateMosaicId(AddressEncoder.DecodeAddress(PublicAccount.CreateFromPublicKey(keys.PublicKeyString, NetworkType.Types.TEST_NET).Address.Plain), 827369870)).ToHex(),
                    827369870,
                    new MosaicProperties(true, true, true, true, 6, 259200),
                    1000000);

            transfer.SetSigner(keys.PublicKeyString);
            transfer.Deadline = DataConverter.ConvertFrom(118101078075);

            var result = transfer.SignTransaction(keys, HttpSetUp.genHash);

            Assert.That(result.Payload.ToHex(), Is.EqualTo("9600000000000000A1FDC28F80C59F9C3B01345EB67EBC8E6DE4511ED369F089F4B5F8FD08D9805CB574B35F67C78E5072E6B2F9AB3A5441E6389A8CE5675E2C7321A02153A4D508F8D6857FBE59B1E30C6EF73C208E3082AB0102352C8B67175E24B83D371DF3F70000000001984D4140420F00000000003B705F7F1B0000002D1CF40F59A0B04E80F40300000000008EA950310F06"));
        }

        [Test, Timeout(20000)]
        public async Task CreateL0NamespaceRentalTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var root = IdGenerator.GenerateId(0, "plasma", true);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateNamespaceRegistrationTransaction(
                    259200,
                    0,
                    root,
                    NamespaceTypes.Types.RootNamespace,
                    "plasma",
                    100000);

            transfer.SetSigner(keys.PublicKeyString);
            transfer.Deadline = DataConverter.ConvertFrom(118099407728);

            var result = transfer.SignTransaction(keys, HttpSetUp.genHash);

            Assert.That(result.Payload.ToHex(), Is.EqualTo("98000000000000000A7E8459A239F63B9F01EBC5CFB4929A37574BE94574AFF683E0B46CB239D18E502329816E0BDA476856822A2B9EA4AF7EC54188D60E449FCE4585D7822EE901F8D6857FBE59B1E30C6EF73C208E3082AB0102352C8B67175E24B83D371DF3F70000000001984E41A08601000000000070F3457F1B00000080F403000000000086E4FDE34B139F8F0006706C61736D61"));
        }

        [Test, Timeout(2000)]
        public async Task CreateL1NamespaceRentalTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var root = IdGenerator.GenerateId(0, "plasma", true);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateNamespaceRegistrationTransaction(
                    0,
                    root,
                    IdGenerator.GenerateId(root, "aeternae", true),
                    NamespaceTypes.Types.SubNamespace,
                    "aeternae",
                    100000);

            transfer.SetSigner(keys.PublicKeyString);
            transfer.Deadline = DataConverter.ConvertFrom(118099685418);

            var result = transfer.SignTransaction(keys, HttpSetUp.genHash);

            Assert.That(result.Payload.ToHex(), Is.EqualTo("9A000000000000000C0FE784CCB6AA273F2C7FB12C9256977391F35AA9C1FFCAB4D558306A1F84AA924A563EB296EDAE2CC475CF91DBBF24BF7DC36A062DA06F2E698F4743F4AB0CF8D6857FBE59B1E30C6EF73C208E3082AB0102352C8B67175E24B83D371DF3F70000000001984E41A0860100000000002A304A7F1B00000086E4FDE34B139F8FEC0566A626196BDC010861657465726E6165"));
        }


        [Test, Timeout(30000)]
        public async Task CreateMosaicAliasTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var id = DataConverter.ConvertFrom(IdGenerator.GenerateMosaicId(AddressEncoder.DecodeAddress(PublicAccount.CreateFromPublicKey(keys.PublicKeyString, NetworkType.Types.TEST_NET).Address.Plain), 827369870)).ToHex();

            var root = IdGenerator.GenerateId(0, "plasma", true);

            var sub = IdGenerator.GenerateId(root, "aeternae", true);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
               .CreateMosaicAliasTransaction(
                   id,
                   DataConverter.ConvertFrom(sub).ToHex(),
                   0x1,
                   1000000);

            transfer.SetSigner(keys.PublicKeyString);
            transfer.Deadline = DataConverter.ConvertFrom(118099831562);

            var result = transfer.SignTransaction(keys, HttpSetUp.genHash);

            Assert.That(result.Payload.ToHex(), Is.EqualTo("91000000000000008EC719051ACD6B559675CAC49E22A0582DF01FBC323E8C1F361A68F184192A89322A6432D83D74EF9F8CA94703DE9A9743908368FE6609AB26A3F5AA3D26A702F8D6857FBE59B1E30C6EF73C208E3082AB0102352C8B67175E24B83D371DF3F70000000001984E4340420F00000000000A6B4C7F1B000000EC0566A626196BDC2D1CF40F59A0B04E01"));
        }

        [Test, Timeout(30000)]
        public async Task CreateMosaicSupplyChangeTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var root = IdGenerator.GenerateId(0, "plasma", true);

            var sub = IdGenerator.GenerateId(root, "aeternae", true);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateMosaicSupplyChangeTransaction(
                    10000000000000,
                    DataConverter.ConvertFrom(sub).ToHex(),
                    MosaicSupplyType.Type.INCREASE,
                    1000000);

            transfer.SetSigner(keys.PublicKeyString);
            transfer.Deadline = DataConverter.ConvertFrom(118101233052);

            var result = transfer.SignTransaction(keys, HttpSetUp.genHash);
            
            Assert.That(result.Payload.ToHex(), Is.EqualTo("91000000000000006FC93BB7634B9E2D34F6806E0FDA34C0D50E606FF9EE7512E99254FC4C584FABADCB455A08ED0B77151FFC5373B2F909F726F0706FA05AC6C24DDC57D900E90DF8D6857FBE59B1E30C6EF73C208E3082AB0102352C8B67175E24B83D371DF3F70000000001984D4240420F00000000009CCD617F1B000000EC0566A626196BDC00A0724E1809000001"));
        }

        [Test, Timeout(30000)]
        public async Task CreateMosaicSupplyRevocationTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);
            
            var root = IdGenerator.GenerateId(0, "plasma", true);
            
            var sub = IdGenerator.GenerateId(root, "aeternae", true);
            
            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateMosaicReclamationTransaction(
                    Address.CreateFromEncoded("TAKSZ42GO35ENLHYRUBKE6EMSM4UUQAKUACXB5A"),
                    DataConverter.ConvertFrom(sub).ToHex(),
                    100000000,
                    1000000);      
            
            transfer.SetSigner(keys.PublicKeyString);
            transfer.Deadline = DataConverter.ConvertFrom(118250895120);

            var result = transfer.SignTransaction(keys, HttpSetUp.genHash);

            Assert.That(result.Payload.ToHex(), Is.EqualTo("A8000000000000000B0A8B65E950AEA89119C5AFB08F58A3657FE02FFA08B7B5F8C25DFD3FDA46C3E9AAB83299628BD52BB9CAE1B52C2C77DE6DC6FBE21671162868F0746DD2990BF8D6857FBE59B1E30C6EF73C208E3082AB0102352C8B67175E24B83D371DF3F70000000001984D4340420F000000000010774D881B00000098152CF34676FA46ACF88D02A2788C93394A400AA00570F4EC0566A626196BDC00E1F50500000000"));
        }

        [Test, Timeout(20000)]
        public async Task CreateSecretLockTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateSecretLockTransaction(
                    "72C0212E67A08BCE",
                    10,
                    1880,
                    "A6A7110A8D6A6FF5901235955DEA7EC0A0F5AFE717B14AAA5D6DF5869F7695CA",
                    HashType.Types.SHA3_512,
                    "TBEAFD6ZBP2J7LTUUWYC2A2ZLXONTWU2ABVCIBA", 
                    1000000
                );

            transfer.SetSigner(keys.PublicKeyString);
            transfer.Deadline = DataConverter.ConvertFrom(117986581510);

            var result = transfer.SignTransaction(keys, HttpSetUp.genHash);

            Assert.That(result.Payload.ToHex(), Is.EqualTo("D100000000000000812AB8910C1CF35A5FE6DF2AA7D40500EACBBF363CE6D6E79238E4EE22FC17D74B0826D18E5C3EE7FDB5921772A9E8A8D96487A957E0FEAC97096F3B7B489201F8D6857FBE59B1E30C6EF73C208E3082AB0102352C8B67175E24B83D371DF3F7000000000198524140420F0000000000065C8C781B0000009848028FD90BF49FAE74A5B02D03595DDCD9DA9A006A2404A6A7110A8D6A6FF5901235955DEA7EC0A0F5AFE717B14AAA5D6DF5869F7695CACE8BA0672E21C0720A00000000000000580700000000000000"));
        }

        [Test, Timeout(20000)]
        public async Task CreateSecretProofTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateSecretProofTransaction(
                    "TBEAFD6ZBP2J7LTUUWYC2A2ZLXONTWU2ABVCIBA",
                    "A6A7110A8D6A6FF5901235955DEA7EC0A0F5AFE717B14AAA5D6DF5869F7695CA",
                    HashType.Types.SHA3_512,
                    "955DEA7EC0A0",
                    1000000
                );

            transfer.SetSigner(keys.PublicKeyString);
            transfer.Deadline = DataConverter.ConvertFrom(117994138799);

            var result = transfer.SignTransaction(keys, HttpSetUp.genHash);

            Assert.That(result.Payload.ToHex(), Is.EqualTo("C10000000000000011881967B919D823EC7628C1338F3127A2BBE0498BBB4030A9E67E78534490FE05D93122561C61C5412E988BB36B364EE2AFE321ABFD376CDD3F8CBEA3BD620AF8D6857FBE59B1E30C6EF73C208E3082AB0102352C8B67175E24B83D371DF3F7000000000198524240420F0000000000AFACFF781B0000009848028FD90BF49FAE74A5B02D03595DDCD9DA9A006A2404A6A7110A8D6A6FF5901235955DEA7EC0A0F5AFE717B14AAA5D6DF5869F7695CA060000955DEA7EC0A0"));
        }

        [Test, Timeout(20000)]
        public async Task CreateAccountKeyLinkTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateKeyLinkTransaction(
                    TransactionTypes.Types.ACCOUNT_KEY_LINK,
                    "F885063A6A798EE7BF34CEEE1E6FE17377E15E54590C90FE783F99690226C033",
                    0x1,
                    1000000
                );

            transfer.SetSigner(keys.PublicKeyString);
            transfer.Deadline = DataConverter.ConvertFrom(117996356712);

            var result = transfer.SignTransaction(keys, HttpSetUp.genHash);

            Assert.That(result.Payload.ToHex(), Is.EqualTo("A100000000000000AE45DAAB6E538FF628B9E2F5B3DB7041B01FDC62ABFBD836E8826348CE8082D82AB971554A8D805AC2402DCB199C77BE31CCF4A36DD0915F787E64869FAB9604F8D6857FBE59B1E30C6EF73C208E3082AB0102352C8B67175E24B83D371DF3F70000000001984C4140420F0000000000688421791B000000F885063A6A798EE7BF34CEEE1E6FE17377E15E54590C90FE783F99690226C03301"));
        }

        [Test, Timeout(20000)]
        public async Task CreateNodeKeyLinkTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateKeyLinkTransaction(
                    TransactionTypes.Types.NODE_KEY_LINK,
                    "4F250755A54BB32675D5639D32A5B098A4B65FC86A232E0E8EEE1AB64E801091",
                    0x1,
                    1000000
                );
           
            transfer.SetSigner(keys.PublicKeyString);
            transfer.Deadline = DataConverter.ConvertFrom(118001735974);

            var result = transfer.SignTransaction(keys, HttpSetUp.genHash);
           
            Assert.That(result.Payload.ToHex(), Is.EqualTo("A100000000000000C8E4800E71AD8A09E80FA3F5CF3D8683D80D6C87E5AB1F31C13C98DDE2EBD1C953142FABB04BB23D74BB5A12BBB19FC3D38406E549E03D0E3408326FAF450801F8D6857FBE59B1E30C6EF73C208E3082AB0102352C8B67175E24B83D371DF3F70000000001984C4240420F0000000000269973791B0000004F250755A54BB32675D5639D32A5B098A4B65FC86A232E0E8EEE1AB64E80109101"));
        }

        [Test, Timeout(20000)]
        public async Task CreateVRFKeyLinkTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateKeyLinkTransaction(
                    TransactionTypes.Types.VRF_KEY_LINK,
                    "6A59D229673DC22D6EC7BF9173932D32B5567AAFAF1C23AFE6A427EEA275A368",
                    0x1,
                    1000000
                );

            transfer.SetSigner(keys.PublicKeyString);
            transfer.Deadline = DataConverter.ConvertFrom(118001919791);

            var result = transfer.SignTransaction(keys, HttpSetUp.genHash);

            Assert.That(result.Payload.ToHex(), Is.EqualTo("A10000000000000017AB406B5DEC4E4C5364E30AAFEA65D74B09A600318AE4DDF31757F04CB7A836030CB16B358558CB5CF1CD2AE61CEA0F15F723F02DBF39E84878B49BB0B83008F8D6857FBE59B1E30C6EF73C208E3082AB0102352C8B67175E24B83D371DF3F7000000000198434240420F00000000002F6776791B0000006A59D229673DC22D6EC7BF9173932D32B5567AAFAF1C23AFE6A427EEA275A36801"));
        }

        [Test, Timeout(20000)]
        public async Task CreateVotingKeyLinkTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateVotingKeyLinkTransaction(
                    4986,
                    5696,
                    "542E24FBBE86278CD2C3AA2F43E39F5330DA2C1AC48ED74DD4E11C916A5B3BE1",
                    0x1,
                    1000000
                );
          
            transfer.SetSigner(keys.PublicKeyString);
            transfer.Deadline = DataConverter.ConvertFrom(118006993952);

            var result = transfer.SignTransaction(keys, HttpSetUp.genHash);

            Assert.That(result.Payload.ToHex(), Is.EqualTo("A9000000000000009C90AA81DC27ACD136EF4F7C33C22D4C7FAAC42A499090BAAEB4AFE1CB2D0EFA4CF34BA273A72071140A44B14C22E184729B72CEC80D84EEF6F11942B3AD030DF8D6857FBE59B1E30C6EF73C208E3082AB0102352C8B67175E24B83D371DF3F7000000000198434140420F000000000020D4C3791B000000542E24FBBE86278CD2C3AA2F43E39F5330DA2C1AC48ED74DD4E11C916A5B3BE17A1300004016000001"));
        }
    }
}
