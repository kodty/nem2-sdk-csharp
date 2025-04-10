// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-31-2018
// ***********************************************************************

using System.Runtime.CompilerServices;
using io.nem2.sdk.Model.Accounts;


[assembly: InternalsVisibleTo("test")]
[assembly: InternalsVisibleTo("integration-test")]
namespace io.nem2.sdk.Model.Transactions
{

    public static class TransactionExtensions
    {
        internal static byte[] SignHash(SecretKeyPair account, byte[] hash)
        {
            return account.Sign(hash);
        }
    }
}
