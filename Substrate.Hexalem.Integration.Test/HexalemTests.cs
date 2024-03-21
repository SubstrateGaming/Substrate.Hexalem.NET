using Substrate.Integration;
using Substrate.Integration.Client;
using Substrate.NET.Schnorrkel.Keys;
using Substrate.NET.Wallet.Keyring;
using Substrate.NetApi;
using Substrate.NetApi.Model.Types;

namespace Substrate.Hexalem.Integration.Test
{
    public class HexalemTest : IntegrationCommonTests
    {
        [Test]
        public async Task CreateGameTestAsync()
        {
            Assert.That(_client, Is.Not.Null);
            Assert.That(_client.IsConnected, Is.False);

            Assert.That(await _client.ConnectAsync(true, true, CancellationToken.None), Is.True);
            Assert.That(_client.IsConnected, Is.True);

            var tcs = new TaskCompletionSource<ExtrinsicInfo>();

            _client.ExtrinsicManager.ExtrinsicUpdated += (subscriptionId, queueInfo) =>
            {
                if (queueInfo.HasEvents)
                {
                    tcs.SetResult(queueInfo);
                }
            };

            var subscriptionId = await _client.CreateGameAsync(Alice, new List<Account>() { Alice, Bob }, 25, 1, CancellationToken.None);
            if (subscriptionId == null)
            {
                Assert.Fail();
                return;
            }

            await Task.WhenAny(tcs.Task, Task.Delay(TimeSpan.FromMinutes(1)));

            Assert.That(tcs.Task.IsCompleted, Is.True);

            Assert.That(await _client.DisconnectAsync(), Is.True);
            Assert.That(_client.IsConnected, Is.False);
        }

        [Test]
        public async Task GetEloRatingTestAsync()
        {
            var elo = await _client.GetRatingStorageAsync(Alice.ToString(), CancellationToken.None);
            Assert.That(elo, Is.GreaterThan(0));
        }
    }
}
