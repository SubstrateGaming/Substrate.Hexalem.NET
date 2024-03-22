using Substrate.Hexalem.NET.NetApiExt.Generated.Model.hexalem_runtime;
using Substrate.Hexalem.NET.NetApiExt.Generated.Model.pallet_hexalem.types;
using Substrate.Hexalem.NET.NetApiExt.Generated.Model.pallet_hexalem.types.game;
using Substrate.Hexalem.NET.NetApiExt.Generated.Model.sp_runtime.multiaddress;
using Substrate.Integration;
using Substrate.Integration.Call;
using Substrate.Integration.Helper;
using Substrate.NET.Wallet.Extensions;
using Substrate.NET.Wallet.Keyring;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Substrate.Hexalem.Integration.Test
{
    public class MatchmakingTests : IntegrationCommonTests
    {
        private readonly string _nodeUrl = "ws://127.0.0.1:9944";

        private SubstrateNetwork _client;

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

        private async Task ShouldBeInQueueAsync(Account account)
        {
            var isInQueue = await _client.IsPlayerInMatchmakingAsync(account, CancellationToken.None);
            var bobMatchmakingState = await _client.GetMatchmakingStateAsync(account, null, CancellationToken.None);

            Assert.That(isInQueue, Is.True);

            Assert.That(bobMatchmakingState, Is.Not.Null);
            Assert.That(bobMatchmakingState.MatchmakingState, Is.EqualTo(MatchmakingState.Matchmaking));
        }

        private async Task ShouldNotBeInQueueAsync(Account account)
        {
            var isInQueue = await _client.IsPlayerInMatchmakingAsync(account, CancellationToken.None);
            var bobMatchmakingState = await _client.GetMatchmakingStateAsync(account, null, CancellationToken.None);

            Assert.That(isInQueue, Is.False);

            Assert.That(bobMatchmakingState, Is.Not.Null);
            Assert.That(bobMatchmakingState.MatchmakingState, Is.EqualTo(MatchmakingState.None));
        }

        private async Task PlayersAcceptMathAsync(int concurrentTasksAllowed, List<Account> players)
        {
            foreach (var player in players)
            {
                await _client.AcceptAsync(player, concurrentTasksAllowed, CancellationToken.None);
                Thread.Sleep(500);
            }
        }

        private async Task<List<byte[]>> EnsureGamesAreCreatedButNobodyJoinedAsync(List<Account> players)
        {
            List<byte[]> gamesId = new List<byte[]>();
            // Matchmaking is not done, game has been create but players did not accept yet
            foreach (var player in players)
            {
                var playerMatchmakingState = await _client.GetMatchmakingStateAsync(player, null, CancellationToken.None);
                Assert.That(playerMatchmakingState.MatchmakingState, Is.EqualTo(MatchmakingState.Joined));

                Assert.That(playerMatchmakingState.GameId, Is.Not.Null);
                if (!gamesId.Contains(playerMatchmakingState.GameId))
                    gamesId.Add(playerMatchmakingState.GameId);

                var game = await _client.GetGameAsync(playerMatchmakingState.GameId, null, CancellationToken.None);

                Assert.That(game, Is.Not.Null);
                Assert.That(game.PlayerAccepted, Is.Not.Null);
                Assert.Multiple(() =>
                {
                    Assert.That(game.PlayerAccepted.All(x => x), Is.False);
                    Assert.That(game.State, Is.EqualTo(GameState.Accepting));
                });
            }

            return gamesId;
        }

        [Test]
        public async Task PlayerJoinQueueTestAsync()
        {
            await ShouldNotBeInQueueAsync(Bob);

            _ = await _client.QueueAsync(Bob, 1, CancellationToken.None);

            Thread.Sleep(6_000);

            await ShouldBeInQueueAsync(Bob);
        }

        [Test]
        public async Task Matchmaking_WhenEnoughPlayerHaveJoin_ShouldMatchAndCreateGameAsync()
        {
            int concurrentTasksAllowed = 20;
            var players = new List<Account>();
            var balanceTransfer = new List<EnumRuntimeCall>();

            for (int i = 0; i < 10; i++)
            {
                var player = _keyring.AddFromUri($"MatchmakingTestEnoughPlayer_{i}", new Meta() { Name = $"TestPlayer{i}" }, KeyType.Sr25519);
                Assert.That(player, Is.Not.Null);

                balanceTransfer.Add(PalletBalances.BalancesTransferKeepAlive(
                    player.Account.ToAccountId32(), 
                    new BigInteger(1000 * SubstrateNetwork.DECIMALS)));

                players.Add(player.Account);
            }

            _ = await _client.BatchAllAsync(balanceTransfer, concurrentTasksAllowed, CancellationToken.None);

            Thread.Sleep(15_000);

            // Now each player queue
            foreach (var player in players.Take(9))
            {
                await ShouldNotBeInQueueAsync(player);

                _ = await _client.QueueAsync(player, concurrentTasksAllowed, CancellationToken.None);
                Thread.Sleep(1_500); // To simulate asynchronous queing
            }

            Thread.Sleep(15_000);

            foreach (var player in players.Take(9))
            {
                var playerMatchmakingState = await _client.GetMatchmakingStateAsync(player, null, CancellationToken.None);

                Assert.That(playerMatchmakingState, Is.Not.Null);
                Assert.That(playerMatchmakingState.MatchmakingState, Is.EqualTo(MatchmakingState.Matchmaking));
                Assert.That(playerMatchmakingState.GameId, Is.Null);
            }

            // Now the last one queue
            _ = await _client.QueueAsync(players.Last(), concurrentTasksAllowed, CancellationToken.None);

            Thread.Sleep(15_000);

            List<byte[]> gamesId = await EnsureGamesAreCreatedButNobodyJoinedAsync(players);
            await PlayersAcceptMathAsync(concurrentTasksAllowed, players);

            Thread.Sleep(15_000);

            foreach (var gameId in gamesId)
            {
                var game = await _client.GetGameAsync(gameId, null, CancellationToken.None);

                Assert.That(game, Is.Not.Null);
                Assert.That(game.State, Is.EqualTo(GameState.Playing));
            }
        }

        [Test]
        public async Task Matchmaking_WhenNotEnoughPlayerHaveJoin_ButExceedTenBlocks_ShouldMatchAndCreateGameAsync()
        {
            int concurrentTasksAllowed = 20;
            var players = new List<Account>();
            for (int i = 0; i < 4; i++)
            {
                var player = _keyring.AddFromUri($"MatchmakingTestNotEnoughPlayer_{i}", new Meta() { Name = $"TestPlayer{i}" }, KeyType.Sr25519);
                Assert.That(player, Is.Not.Null);

                // Alice send them some token
                _ = await _client.TransferKeepAliveAsync(
                    Alice,
                    player.Account.ToAccountId32(),
                    new BigInteger(1000 * SubstrateNetwork.DECIMALS),
                    concurrentTasksAllowed, CancellationToken.None);

                players.Add(player.Account);
            }

            Thread.Sleep(15_000);

            // Now each player queue
            foreach (var player in players)
            {
                await ShouldNotBeInQueueAsync(player);

                _ = await _client.QueueAsync(player, concurrentTasksAllowed, CancellationToken.None);
                Thread.Sleep(1_500); // To simulate asynchronous queing
            }

            // Now wait > 10 blocks to trigger the matchmaking even if there is not enough player
            Thread.Sleep(11 * 6_000);

            List<byte[]> gamesId = await EnsureGamesAreCreatedButNobodyJoinedAsync(players);
            await PlayersAcceptMathAsync(concurrentTasksAllowed, players);

            Thread.Sleep(15_000);

            foreach (var gameId in gamesId)
            {
                var game = await _client.GetGameAsync(gameId, null, CancellationToken.None);

                Assert.That(game, Is.Not.Null);
                Assert.That(game.State, Is.EqualTo(GameState.Playing));
            }
        }
    }
}
