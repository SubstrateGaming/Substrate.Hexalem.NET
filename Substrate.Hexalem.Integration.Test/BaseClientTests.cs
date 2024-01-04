using Schnorrkel.Keys;
using Substrate.Integration.Client;
using Substrate.NetApi;
using Substrate.NetApi.Model.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Substrate.Hexalem.Integration.Test
{
    internal class BaseClientTests
    {
        public MiniSecret MiniSecretAlice => new MiniSecret(Utils.HexToByteArray("0xe5be9a5092b81bca64be81d212e7f2f9eba183bb7a90954f7b76361f6edb5c0a"), ExpandMode.Ed25519);
        public Account Alice => Account.Build(KeyType.Sr25519, MiniSecretAlice.ExpandToSecret().ToBytes(), MiniSecretAlice.GetPair().Public.Key);

        [Test]
        public void DeriveAliceAccount_WithValidDerivation_ShouldNotReturnAlice()
        {
            var derivedAccount = BaseClient.DeriveAccount(Alice, "//Hexalem");

            Assert.That(Alice.Bytes, Is.Not.EquivalentTo(derivedAccount.Bytes));
            Assert.That(Alice.PrivateKey, Is.Not.EquivalentTo(derivedAccount.PrivateKey));
        }

        [Test]
        public void DeriveAliceAccount_WithValidDerivation_ShouldBeDeterministic()
        {
            var derivedAccount1 = BaseClient.DeriveAccount(Alice, "//Hexalem");
            var derivedAccount2 = BaseClient.DeriveAccount(Alice, "//Hexalem");
            var derivedAccount3 = BaseClient.DeriveAccount(Alice, "//Hexalem2");

            Assert.That(derivedAccount1.Bytes, Is.EquivalentTo(derivedAccount2.Bytes));
            Assert.That(derivedAccount1.Bytes, Is.Not.EquivalentTo(derivedAccount3.Bytes));
        }
    }
}
