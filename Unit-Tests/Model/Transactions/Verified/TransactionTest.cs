using Coppery;
using Integration_Tests;
using io.nem2.sdk.Infrastructure.HttpClients;
using io.nem2.sdk.Model;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Articles;
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
                    Address.CreateFromEncoded(HttpSetUp.Recipient),
                    EmptyMessage.Create(),
                    Mosaic.CreateFromHexIdentifier("72C0212E67A08BCE", 1000000),
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
                    Address.CreateFromEncoded("TA3GCBHJBTRCEHVYVHCNUCULY2NB76W7MVECFUY"), // dummy address
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
        public async Task CreateHashLockTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateHashLockTransaction(
                    "72C0212E67A08BCE",
                    10000000,
                    2880,
                    "F8D6857FBE59B1E30C6EF73C208E3082AB0102352C8B67175E24B83D371DF3F7", // dummy hash
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

        [Test, Timeout(20000)]
        public async Task CreateSecretLockTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateSecretLockTransaction(
                    "72C0212E67A08BCE",
                    10000000,
                    "F8D6857FBE59B1E30C6EF73C208E3082AB0102352C8B67175E24B83D371DF3F7", // dummy secret
                    HashType.Types.SHA3_512,
                    "TA3GCBHJBTRCEHVYVHCNUCULY2NB76W7MVECFUY", // dummy recipient
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
        public async Task CreateSecretProofTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateSecretProofTransaction(
                    "TA3GCBHJBTRCEHVYVHCNUCULY2NB76W7MVECFUY",
                    "F8D6857FBE59B1E30C6EF73C208E3082AB0102352C8B67175E24B83D371DF3F7",
                    HashType.Types.SHA3_512,
                    "F8D6857FBE59B1E30C6EF73C208E3082AB0102352C8B67175E24B83D371DF3F7",
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
                    "F8D6857FBE59B1E30C6EF73C208E3082AB0102352C8B67175E24B83D371DF3F7", // dummy key
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
                    "F8D6857FBE59B1E30C6EF73C208E3082AB0102352C8B67175E24B83D371DF3F7",
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
                    "F8D6857FBE59B1E30C6EF73C208E3082AB0102352C8B67175E24B83D371DF3F7", // dummy key
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
                    "F8D6857FBE59B1E30C6EF73C208E3082AB0102352C8B67175E24B83D371DF3F7", // dummy key
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
