using Substrate.Integration;
using Substrate.NET.Wallet.Keyring;
using Substrate.NetApi.Model.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Substrate.Hexalem.Integration.Test
{
    public abstract class IntegrationCommonTests
    {
        protected readonly string _nodeUrl = "ws://127.0.0.1:9944";
        protected SubstrateNetwork _client;

        protected  Keyring _keyring = new Keyring();

        private Account? _alice;
        public Account Alice
        {
            get
            {
                if (_alice is null)
                {
                    _alice = _keyring.AddFromUri("//Alice", new Meta() { Name = "Alice" }, KeyType.Sr25519).Account;
                }

                return _alice;
            }
        }

        private Account? _bob;
        public Account Bob
        {
            get
            {
                if (_bob is null)
                    _bob = _keyring.AddFromUri("//Bob", new Meta() { Name = "Bob" }, KeyType.Sr25519).Account;

                return _bob;
            }
        }

        [SetUp]
        public async Task Setup()
        {
            // create client
            _client = new SubstrateNetwork(Alice, Substrate.Integration.Helper.NetworkType.Live, _nodeUrl);

            Assert.That(_client, Is.Not.Null);
            Assert.That(_client.IsConnected, Is.False);

            Assert.That(await _client.ConnectAsync(true, true, CancellationToken.None), Is.True);
            Assert.That(_client.IsConnected, Is.True);
        }

        [TearDown]
        public void TearDown()
        {
            // dispose client
            _client.SubstrateClient.Dispose();
        }

    }
}
