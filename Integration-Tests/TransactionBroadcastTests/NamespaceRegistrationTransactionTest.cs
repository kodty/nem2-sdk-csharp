using io.nem2.sdk.src.Model;
using io.nem2.sdk.src.Model.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration_Tests.TransactionBroadcastTests
{
    internal class NamespaceRegistrationTransactionTest
    {
        public async Task CreateRootNamespace()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateNamespaceRegistrationTransaction(
                    1440,
                    null,
                    NamespaceId.Create("symbol"),
                    NamespaceTypes.Types.RootNamespace,
                    "symbol",
                    false
                );

            var st = transfer.WrapVerified(keys);
        }

        public async Task CreateChildNamespace()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateTransferTransaction(
                    "TDMYA6WCKAMY5JL5NCNHEOO7UO2S4FIGUP3R7XA",
                    "",
                    new Tuple<string, ulong>("72C0212E67A08BCE", 100),
                    false
                );

            var st = transfer.WrapVerified(keys);
        }
    }
}
