using Coppery;
using Integration_Tests;
using io.nem2.sdk.Infrastructure.HttpClients;
using io.nem2.sdk.Model;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Articles;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.Model.Transactions.Messages;
using io.nem2.sdk.Utils;
using System.Diagnostics;
using System.Reactive.Linq;

namespace Unit_Tests.Model.Transactions.Verified
{
    internal class TransactionTest
    {
        [Test, Timeout(20000)]
        public async Task CreateTransferTransactionTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateTransferTransaction(
                    address: Address.CreateFromEncoded("TDX7QVF6XXMJNDFFRIOYTV4N3GSVUGNTWVCIMZQ"),
                    messege: EmptyMessage.Create(),
                    mosaic: Mosaic.CreateFromHexIdentifier("72C0212E67A08BCE", 1000000),
                    fee: 1000000,
                    embedded: false
                );

            transfer.SetSigner(keys.PublicKeyString);

            transfer.Fee = DataConverter.ConvertFrom((ulong)1000000);
            transfer.Deadline = DataConverter.ConvertFrom((ulong)117756998097);
            
            var result = transfer.SignTransaction(keys, HttpSetUp.genHash);
            Debug.WriteLine(result.Payload.ToHex());

            Assert.That(result.VerifiablePayload.ToHex() == HttpSetUp.genHash + "0198544140420F0000000000D131DD6A1B00000098EFF854BEBDD8968CA58A1D89D78DD9A55A19B3B54486660000010000000000CE8BA0672E21C07240420F0000000000");
            Assert.That(result.Payload.ToHex(), Is.EqualTo("B000000000000000115504A388D963BF8B64400920CEBBC04597C0EC97E429C5B2660614440FD6A97E5A122FB7ADF2AC7DADA41CDEB23915E00BE23FE5F06B2B6896C4964E440600F8D6857FBE59B1E30C6EF73C208E3082AB0102352C8B67175E24B83D371DF3F7000000000198544140420F0000000000D131DD6A1B00000098EFF854BEBDD8968CA58A1D89D78DD9A55A19B3B54486660000010000000000CE8BA0672E21C07240420F0000000000"));
        }

        [Test, Timeout(20000)]
        public async Task CreateHashLockTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
               .CreateTransferTransaction(
                   Address.CreateFromEncoded("TDX7QVF6XXMJNDFFRIOYTV4N3GSVUGNTWVCIMZQ"),
                   EmptyMessage.Create(),
                   Mosaic.CreateFromHexIdentifier("72C0212E67A08BCE", 1000000),
                   1000000,
                   false
               );

            transfer.SetSigner(keys.PublicKeyString);

            transfer.Fee = DataConverter.ConvertFrom((ulong)1000000);
            transfer.Deadline = DataConverter.ConvertFrom((ulong)117756998097);

            var transferResult = transfer.SignTransaction(keys, HttpSetUp.genHash);

            var hashlock = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateHashLockTransaction(
                    "72C0212E67A08BCE",
                    10000000,
                    2880,
                    transferResult.Hash,
                    1000000,
                    false
                );

            hashlock.SetSigner(keys.PublicKeyString);

            hashlock.Fee = DataConverter.ConvertFrom((ulong)1000000);
            hashlock.Deadline = DataConverter.ConvertFrom((ulong)117757956956);

            var hashlockResult = hashlock.SignTransaction(keys, HttpSetUp.genHash);

            Assert.That(hashlockResult.Payload.ToHex(), Is.EqualTo("B800000000000000A6A7110A8D6A6FF5901235955DEA7EC0A0F5AFE717B14AAA5D6DF5869F7695C6CF87B5BDA105B7D1724812544A846585701BB9C6F4E225170F55DF9AD9132205F8D6857FBE59B1E30C6EF73C208E3082AB0102352C8B67175E24B83D371DF3F7000000000198484140420F00000000005CD3EB6A1B000000CE8BA0672E21C0728096980000000000400B000000000000FD492A6AD4BA0A2CD73277C4390BFCA885C17693DD6463F4418D0A6553A586D3"));
        }

        [Test, Timeout(20000)]
        public async Task CreateAggregateBondedTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);
            var keys2 = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.privKey);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateTransferTransaction(
                    Address.CreateFromEncoded("TDX7QVF6XXMJNDFFRIOYTV4N3GSVUGNTWVCIMZQ"),
                    PlainMessage.Create("hello"),
                    Mosaic.CreateFromHexIdentifier("72C0212E67A08BCE", 1000000),
                    1000000,
                    true
                );

                transfer.SetSigner(keys.PublicKeyString);

            var transfer2 = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateTransferTransaction(
                    Address.CreateFromEncoded("TA3GCBHJBTRCEHVYVHCNUCULY2NB76W7MVECFUY"),
                    PlainMessage.Create("hello"),
                    Mosaic.CreateFromHexIdentifier("72C0212E67A08BCE", 200),
                    800000,
                    true
                );

                transfer.SetSigner(keys2.PublicKeyString);

            var aggregateBonded = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateAggregateBonded(
                    [
                        transfer.PrepareEmbedded(keys.PublicKeyString), 
                        transfer2.PrepareEmbedded(keys2.PublicKeyString)
                    ],
                    keys.PublicKey,
                    4321000
                );

            aggregateBonded.Cosign([keys2]);

            //transfer.Fee = DataConverter.ConvertFrom((ulong)1000000);
            //transfer.Deadline = DataConverter.ConvertFrom((ulong)117756998097);

            var result = aggregateBonded.SignTransaction(keys, HttpSetUp.genHash);

            Debug.WriteLine(result.Payload.ToHex());
            Debug.WriteLine(DataConverter.ConvertTo<ulong>(aggregateBonded.Deadline));
            Debug.WriteLine(DataConverter.ConvertTo<ulong>(aggregateBonded.Fee));

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
                    DataConverter.ConvertFrom(IdGenerator.GenerateMosaicId(AddressEncoder.DecodeAddress(PublicAccount.CreateFromPublicKey(keys.PublicKeyString, NetworkType.Types.TEST_NET).Address.Plain), 125959)).ToHex(),
                    125959,
                    new MosaicProperties(true, true, false, 6, 1000000),
                    500000,
                    false);

            transfer.SetSigner(keys.PublicKeyString);

            //transfer.Fee = DataConverter.ConvertFrom((ulong)500000);
            //transfer.Deadline = DataConverter.ConvertFrom((ulong)117657395737);

            var result = transfer.SignTransaction(keys, HttpSetUp.genHash);

            Debug.WriteLine(result.Payload.ToHex());
            Debug.WriteLine(DataConverter.ConvertTo<ulong>(transfer.Deadline));
            Debug.WriteLine(DataConverter.ConvertTo<ulong>(transfer.Fee));

            //var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);
            //
            //var a = await client.Announce(result);
            //
            //var status = await client.GetTransactionStatus(result.Hash);
            //
            //Assert.AreEqual(status.ComposedResponse.Code, "Success");

            //Assert.That(result.Payload.ToHex(), Is.EqualTo(""));
        }

        [Test, Timeout(20000)]
        public async Task CreateL0NamespaceRentalTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var root = IdGenerator.GenerateId(0, "plasma", true);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateNamespaceRegistrationTransaction(
                    525600,
                    0,
                    root,
                    NamespaceTypes.Types.RootNamespace,
                    "plasma",
                    100000,
                    false);

            transfer.SetSigner(keys.PublicKeyString);        
            
            //transfer.Fee = DataConverter.ConvertFrom((ulong)100000);
            //transfer.Deadline = DataConverter.ConvertFrom((ulong)117657800500);

            var result = transfer.SignTransaction(keys, HttpSetUp.genHash);

            Debug.WriteLine(result.Payload.ToHex());
            Debug.WriteLine(DataConverter.ConvertTo<ulong>(transfer.Deadline));
            Debug.WriteLine(DataConverter.ConvertTo<ulong>(transfer.Fee));

            //var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);
            //
            //var a = await client.Announce(result);
            //
            //var status = await client.GetTransactionStatus(result.Hash);
            //
            //Assert.AreEqual(status.ComposedResponse.Code, "Success");

            //Assert.That(result.Payload.ToHex(), Is.EqualTo(""));
        }

        public async Task CreateL1NamespaceRentalTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var root = IdGenerator.GenerateId(0, "Plasma", true);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateNamespaceRegistrationTransaction(
                    0,
                    root,
                    IdGenerator.GenerateId(root, "Aeternae", true),
                    NamespaceTypes.Types.SubNamespace,
                    "Aeternae",
                    100000,
                    false);

            transfer.SetSigner(keys.PublicKeyString);
            
            //transfer.Fee = DataConverter.ConvertFrom((ulong)100000);
            //transfer.Deadline = DataConverter.ConvertFrom((ulong)117657800500);

            var result = transfer.SignTransaction(keys, HttpSetUp.genHash);

            Debug.WriteLine(result.Payload.ToHex());
            Debug.WriteLine(DataConverter.ConvertTo<ulong>(transfer.Deadline));
            Debug.WriteLine(DataConverter.ConvertTo<ulong>(transfer.Fee));

            //var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);
            //
            //var a = await client.Announce(result);
            //
            //var status = await client.GetTransactionStatus(result.Hash);
            //
            //Assert.AreEqual(status.ComposedResponse.Code, "Success");
            
            //Assert.That(result.Payload.ToHex(), Is.EqualTo(""));
        }


        [Test, Timeout(30000)]
        public async Task CreateMosaicAliasTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var id = DataConverter.ConvertFrom(IdGenerator.GenerateMosaicId(AddressEncoder.DecodeAddress(PublicAccount.CreateFromPublicKey(keys.PublicKeyString, NetworkType.Types.TEST_NET).Address.Plain), 125959)).ToHex();

            var root = IdGenerator.GenerateId(0, "Plasma", true);

            var sub = IdGenerator.GenerateId(root, "Aeternae", true);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
               .CreateMosaicAliasTransaction(
                   id,
                   DataConverter.ConvertFrom(sub).ToHex(),
                   0x1,
                   1000000,
                   false);

            transfer.SetSigner(keys.PublicKeyString);

            //transfer.Fee = DataConverter.ConvertFrom((ulong)100000);
            //transfer.Deadline = DataConverter.ConvertFrom((ulong)117657800500);

            var result = transfer.SignTransaction(keys, HttpSetUp.genHash);

            Debug.WriteLine(result.Payload.ToHex());
            Debug.WriteLine(DataConverter.ConvertTo<ulong>(transfer.Deadline));
            Debug.WriteLine(DataConverter.ConvertTo<ulong>(transfer.Fee));

            //var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);
            //
            //var a = await client.Announce(result);
            //
            //var status = await client.GetTransactionStatus(result.Hash);
            //
            //Assert.AreEqual(status.ComposedResponse.Code, "Success");

            //Assert.That(result.Payload.ToHex(), Is.EqualTo(""));
        }

        [Test, Timeout(30000)]
        public async Task CreateMosaicSupplyChangeTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var root = IdGenerator.GenerateId(0, "Plasma", true);

            var sub = IdGenerator.GenerateId(root, "Aeternae", true);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateMosaicSupplyChangeTransaction(
                    1000000,
                    "",
                    MosaicSupplyType.Type.INCREASE,
                    1000000,
                    false);

            transfer.SetSigner(keys.PublicKeyString);

            //transfer.Fee = DataConverter.ConvertFrom((ulong)100000);
            //transfer.Deadline = DataConverter.ConvertFrom((ulong)117657800500);

            var result = transfer.SignTransaction(keys, HttpSetUp.genHash);

            Debug.WriteLine(result.Payload.ToHex());
            Debug.WriteLine(DataConverter.ConvertTo<ulong>(transfer.Deadline));
            Debug.WriteLine(DataConverter.ConvertTo<ulong>(transfer.Fee));

            //var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);
            //
            //var a = await client.Announce(result);
            //
            //var status = await client.GetTransactionStatus(result.Hash);
            //
            //Assert.AreEqual(status.ComposedResponse.Code, "Success");

            //Assert.That(result.Payload.ToHex(), Is.EqualTo(""));
        }

        [Test, Timeout(30000)]
        public async Task CreateMosaicSupplyRevocationTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var root = IdGenerator.GenerateId(0, "Plasma", true);

            var sub = IdGenerator.GenerateId(root, "Aeternae", true);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateMosaicSupplyRevocationTransaction(
                    Address.CreateFromEncoded(""),
                    DataConverter.ConvertFrom((ulong)sub).ToHex(),
                    1000000,
                    1000000,
                    false);

            transfer.SetSigner(keys.PublicKeyString);

            //transfer.Fee = DataConverter.ConvertFrom((ulong)100000);
            //transfer.Deadline = DataConverter.ConvertFrom((ulong)117657800500);

            var result = transfer.SignTransaction(keys, HttpSetUp.genHash);

            Debug.WriteLine(result.Payload.ToHex());
            Debug.WriteLine(DataConverter.ConvertTo<ulong>(transfer.Deadline));
            Debug.WriteLine(DataConverter.ConvertTo<ulong>(transfer.Fee));

            //var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);
            //
            //var a = await client.Announce(result);
            //
            //var status = await client.GetTransactionStatus(result.Hash);
            //
            //Assert.AreEqual(status.ComposedResponse.Code, "Success");

            //Assert.That(result.Payload.ToHex(), Is.EqualTo(""));
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
                    1000000,
                    false
                );

            transfer.SetSigner(keys.PublicKeyString);

            transfer.Fee = DataConverter.ConvertFrom((ulong)1000000);
            transfer.Deadline = DataConverter.ConvertFrom((ulong)117986581510);

            var result = transfer.SignTransaction(keys, HttpSetUp.genHash);

            Assert.That(result.Payload.ToHex(), Is.EqualTo("D100000000000000812AB8910C1CF35A5FE6DF2AA7D40500EACBBF363CE6D6E79238E4EE22FC17D74B0826D18E5C3EE7FDB5921772A9E8A8D96487A957E0FEAC97096F3B7B489201F8D6857FBE59B1E30C6EF73C208E3082AB0102352C8B67175E24B83D371DF3F7000000000198524140420F0000000000065C8C781B0000009848028FD90BF49FAE74A5B02D03595DDCD9DA9A006A2404A6A7110A8D6A6FF5901235955DEA7EC0A0F5AFE717B14AAA5D6DF5869F7695CACE8BA0672E21C0720A00000000000000580700000000000000"));
        }

        [Test, Timeout(20000)]
        public async Task CreateSecretProofTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateSecretProofTransaction(
                    "",
                    "",
                    HashType.Types.SHA3_512,
                    "",
                    1000000,
                    false
                );

            transfer.SetSigner(keys.PublicKeyString);

            //transfer.Fee = DataConverter.ConvertFrom((ulong)500000);
            //transfer.Deadline = DataConverter.ConvertFrom((ulong)117657395737);

            var result = transfer.SignTransaction(keys, HttpSetUp.genHash);

            //Debug.WriteLine(result.Payload.ToHex());
            //Debug.WriteLine(DataConverter.ConvertTo<ulong>(transfer.Deadline));
            //Debug.WriteLine(DataConverter.ConvertTo<ulong>(transfer.Fee));

            //var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);
            //
            //var a = await client.Announce(result);
            //
            //var status = await client.GetTransactionStatus(result.Hash);
            //
            //Assert.AreEqual(status.ComposedResponse.Code, "Success");

            //Assert.That(result.Payload.ToHex(), Is.EqualTo(""));
        }

        private void produceAccounts()
        {
            var a = Account.GenerateNewAccount(NetworkType.Types.TEST_NET);
            var b = Account.GenerateNewAccount(NetworkType.Types.TEST_NET);
            var c = Account.GenerateNewAccount(NetworkType.Types.TEST_NET);
            var d = Account.GenerateNewAccount(NetworkType.Types.TEST_NET);

            Debug.WriteLine(a.KeyPair.PrivateKeyString);
            Debug.WriteLine(b.KeyPair.PrivateKeyString);
            Debug.WriteLine(c.KeyPair.PrivateKeyString);
            Debug.WriteLine(d.KeyPair.PrivateKeyString);

            Debug.WriteLine(a.KeyPair.PublicKeyString);
            Debug.WriteLine(b.KeyPair.PublicKeyString);
            Debug.WriteLine(c.KeyPair.PublicKeyString);
            Debug.WriteLine(d.KeyPair.PublicKeyString);
        }

        [Test, Timeout(20000)]
        public async Task CreateAccountKeyLinkTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateKeyLinkTransaction(
                    TransactionTypes.Types.ACCOUNT_KEY_LINK,
                    "",
                    0x1,
                    1000000,
                    false
                );

            transfer.SetSigner(keys.PublicKeyString);

            //transfer.Fee = DataConverter.ConvertFrom((ulong)500000);
            //transfer.Deadline = DataConverter.ConvertFrom((ulong)117657395737);

            var result = transfer.SignTransaction(keys, HttpSetUp.genHash);

            Debug.WriteLine(result.Payload.ToHex());
            Debug.WriteLine(DataConverter.ConvertTo<ulong>(transfer.Deadline));
            Debug.WriteLine(DataConverter.ConvertTo<ulong>(transfer.Fee));

            //var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);
            //
            //var a = await client.Announce(result);
            //
            //var status = await client.GetTransactionStatus(result.Hash);
            //
            //Assert.AreEqual(status.ComposedResponse.Code, "Success");

            //Assert.That(result.Payload.ToHex(), Is.EqualTo(""));
        }

        [Test, Timeout(20000)]
        public async Task CreateNodeKeyLinkTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateKeyLinkTransaction(
                    TransactionTypes.Types.NODE_KEY_LINK,
                    "",
                    0x1,
                    1000000,
                    false
                );

            transfer.SetSigner(keys.PublicKeyString);

            //transfer.Fee = DataConverter.ConvertFrom((ulong)500000);
            //transfer.Deadline = DataConverter.ConvertFrom((ulong)117657395737);

            var result = transfer.SignTransaction(keys, HttpSetUp.genHash);

            Debug.WriteLine(result.Payload.ToHex());
            Debug.WriteLine(DataConverter.ConvertTo<ulong>(transfer.Deadline));
            Debug.WriteLine(DataConverter.ConvertTo<ulong>(transfer.Fee));

            //var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);
            //
            //var a = await client.Announce(result);
            //
            //var status = await client.GetTransactionStatus(result.Hash);
            //
            //Assert.AreEqual(status.ComposedResponse.Code, "Success");

            //Assert.That(result.Payload.ToHex(), Is.EqualTo(""));
        }

        [Test, Timeout(20000)]
        public async Task CreateVRFKeyLinkTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateKeyLinkTransaction(
                    TransactionTypes.Types.VRF_KEY_LINK,
                    "",
                    0x1,
                    1000000,
                    false
                );

            transfer.SetSigner(keys.PublicKeyString);

            //transfer.Fee = DataConverter.ConvertFrom((ulong)500000);
            //transfer.Deadline = DataConverter.ConvertFrom((ulong)117657395737);

            var result = transfer.SignTransaction(keys, HttpSetUp.genHash);

            Debug.WriteLine(result.Payload.ToHex());
            Debug.WriteLine(DataConverter.ConvertTo<ulong>(transfer.Deadline));
            Debug.WriteLine(DataConverter.ConvertTo<ulong>(transfer.Fee));

            //var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);
            //
            //var a = await client.Announce(result);
            //
            //var status = await client.GetTransactionStatus(result.Hash);
            //
            //Assert.AreEqual(status.ComposedResponse.Code, "Success");

            //Assert.That(result.Payload.ToHex(), Is.EqualTo(""));
        }

        [Test, Timeout(20000)]
        public async Task CreateVotingKeyLinkTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateVotingKeyLinkTransaction(
                    TransactionTypes.Types.VOTING_KEY_LINK,
                    0,
                    0,
                    "",
                    0x1,
                    1000000,
                    false
                );

            transfer.SetSigner(keys.PublicKeyString);

            //transfer.Fee = DataConverter.ConvertFrom((ulong)500000);
            //transfer.Deadline = DataConverter.ConvertFrom((ulong)117657395737);

            var result = transfer.SignTransaction(keys, HttpSetUp.genHash);

            Debug.WriteLine(result.Payload.ToHex());
            Debug.WriteLine(DataConverter.ConvertTo<ulong>(transfer.Deadline));
            Debug.WriteLine(DataConverter.ConvertTo<ulong>(transfer.Fee));

            //var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);
            //
            //var a = await client.Announce(result);
            //
            //var status = await client.GetTransactionStatus(result.Hash);
            //
            //Assert.AreEqual(status.ComposedResponse.Code, "Success");

            //Assert.That(result.Payload.ToHex(), Is.EqualTo(""));
        }
    }
}
