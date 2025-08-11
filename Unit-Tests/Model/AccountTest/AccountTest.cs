//
// Copyright 2018 NEM
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 

using Coppery;
using io.nem2.sdk.src.Model;
using io.nem2.sdk.src.Model.Accounts;
using System.Diagnostics;

namespace test.Model.AccountTest
{

    public class AccountTest
    {
        [Test]
        public void CreateNewTestNetAccount()
        {
            var newAcc = Account.GenerateNewAccount(NetworkType.Types.TEST_NET);

            Assert.That(newAcc.PublicAccount.PublicKey.ToHex().Length, Is.EqualTo(64));
            Assert.That(newAcc.Address.Plain.IsBase32(), Is.True);
        }
    }
}
