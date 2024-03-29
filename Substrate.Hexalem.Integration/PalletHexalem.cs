﻿using Serilog;
using Substrate.Hexalem.Integration.Model;
using Substrate.Hexalem.NET.NetApiExt.Generated.Model.pallet_hexalem.types.board.resource;
using Substrate.Hexalem.NET.NetApiExt.Generated.Model.sp_core.crypto;
using Substrate.Hexalem.NET.NetApiExt.Generated.Storage;
using Substrate.Hexalem.NET.NetApiExt.Generated.Types.Base;
using Substrate.Integration.Client;
using Substrate.Integration.Helper;
using Substrate.NetApi;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Substrate.Integration
{
    /// <summary>
    /// Substrate network
    /// </summary>
    public partial class SubstrateNetwork : BaseClient
    {
        #region storage

        /// <summary>
        /// Get game
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="blockHash"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<MatchmakingStateSharp?> GetMatchmakingStateAsync(string playerAddress, string? blockHash, CancellationToken token)
        {
            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            var key = new AccountId32();
            key.Create(Utils.GetPublicKeyFrom(playerAddress));

            var result = await SubstrateClient.HexalemModuleStorage.MatchmakingStateStorage(key, blockHash, token);

            if (result == null) return null;

            return new MatchmakingStateSharp(result);
        }

        /// <summary>
        /// Get game
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="blockHash"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<GameSharp?> GetGameAsync(byte[] gameId, string? blockHash, CancellationToken token)
        {
            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            var key = new Hexalem.NET.NetApiExt.Generated.Types.Base.Arr32U8();
            key.Create(gameId);

            var result = await SubstrateClient.HexalemModuleStorage.GameStorage(key, blockHash, token);

            if (result == null) return null;

            return new GameSharp(gameId, result);
        }

        /// <summary>
        /// Get board
        /// </summary>
        /// <param name="playerAddress"></param>
        /// <param name="blockHash"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<BoardSharp?> GetBoardAsync(string playerAddress, string? blockHash, CancellationToken token)
        {
            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            var key = new AccountId32();
            key.Create(Utils.GetPublicKeyFrom(playerAddress));

            var result = await SubstrateClient.HexalemModuleStorage.HexBoardStorage(key, blockHash, token);

            if (result == null) return null;

            return new BoardSharp(result);
        }

        #endregion storage

        #region call

        /// <summary>
        /// Create game
        /// </summary>
        /// <param name="account"></param>
        /// <param name="players"></param>
        /// <param name="gridSize"></param>
        /// <param name="concurrentTasks"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string?> CreateGameAsync(Account account, List<Account> players, byte gridSize, int concurrentTasks, CancellationToken token)
        {
            var extrinsicType = $"Hexalem.CreateGame";

            var extrinsic = HexalemModuleCalls.CreateGame(new BaseVec<AccountId32>(players.Select(p => p.ToAccountId32()).ToArray()), new U8(gridSize));

            return await GenericExtrinsicAsync(account, extrinsicType, extrinsic, concurrentTasks, token);
        }

        /// <summary>
        /// Queue
        /// </summary>
        /// <param name="account"></param>
        /// <param name="concurrentTasks"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string?> QueueAsync(Account account, int concurrentTasks, CancellationToken token)
        {
            var extrinsicType = $"Hexalem.Queue";

            var extrinsic = HexalemModuleCalls.Queue();

            return await GenericExtrinsicAsync(account, extrinsicType, extrinsic, concurrentTasks, token);
        }

        /// <summary>
        /// Play
        /// </summary>
        /// <param name="account"></param>
        /// <param name="placeIndex"></param>
        /// <param name="buyIndex"></param>
        /// <param name="payType"></param>
        /// <param name="concurrentTasks"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string?> PlayAsync(Account account, byte placeIndex, byte buyIndex, int concurrentTasks, CancellationToken token)
        {
            var extrinsicType = $"Hexalem.Play";

            var moveStruct = new Move
            {
                PlaceIndex = new U8(placeIndex),
                BuyIndex = new U8(buyIndex),
            };

            var extrinsic = HexalemModuleCalls.Play(moveStruct);

            return await GenericExtrinsicAsync(account, extrinsicType, extrinsic, concurrentTasks, token);
        }

        /// <summary>
        /// Upgrade a tile
        /// </summary>
        /// <param name="account"></param>
        /// <param name="placeIndex"></param>
        /// <param name="buyIndex"></param>
        /// <param name="concurrentTasks"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string?> UpgradeAsync(Account account, byte placeIndex, int concurrentTasks, CancellationToken token)
        {
            var extrinsicType = $"Hexalem.Upgrade";

            var extrinsic = HexalemModuleCalls.Upgrade(new U8(placeIndex));

            return await GenericExtrinsicAsync(account, extrinsicType, extrinsic, concurrentTasks, token);
        }

        /// <summary>
        /// Finish turn
        /// </summary>
        /// <param name="account"></param>
        /// <param name="concurrentTasks"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string?> FinishTurnAsync(Account account, int concurrentTasks, CancellationToken token)
        {
            var extrinsicType = $"Hexalem.FinishTurn";

            var extrinsic = HexalemModuleCalls.FinishTurn();

            return await GenericExtrinsicAsync(account, extrinsicType, extrinsic, concurrentTasks, token);
        }

        /// <summary>
        /// Claim rewards
        /// </summary>
        /// <param name="account"></param>
        /// <param name="concurrentTasks"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string?> ClaimAsync(Account account, int concurrentTasks, CancellationToken token)
        {
            var extrinsicType = $"Hexalem.ClaimRewards";

            var extrinsic = HexalemModuleCalls.ClaimRewards();

            return await GenericExtrinsicAsync(account, extrinsicType, extrinsic, concurrentTasks, token);
        }

        /// <summary>
        /// Accept match
        /// </summary>
        /// <param name="account"></param>
        /// <param name="concurrentTasks"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string?> AcceptAsync(Account account, int concurrentTasks, CancellationToken token)
        {
            var extrinsicType = $"Hexalem.AcceptMatch";

            var extrinsic = HexalemModuleCalls.AcceptMatch();

            return await GenericExtrinsicAsync(account, extrinsicType, extrinsic, concurrentTasks, token);
        }

        /// <summary>
        /// Root delete game, needs to be called by Sudo.
        /// </summary>
        /// <param name="account">Sudo account</param>
        /// <param name="GameIdBytes">Game Id</param>
        /// <param name="concurrentTasks">Concurrant tasks of this type allowed</param>
        /// <param name="token">Cancellation token</param>
        /// <returns></returns>
        public async Task<string?> SudoRootDeleteGameAsync(Account account, byte[] GameIdBytes, int concurrentTasks, CancellationToken token)
        {
            var extrinsicType = $"Hexalem.RootDeleteGame";

            Arr32U8 gameId = new Arr32U8();
            gameId.Create(GameIdBytes.Select(p => new U8(p)).ToArray());

            var rootDeleteGame = Call.PalletHexalem.HexalemRootDeleteGame(GameIdBytes);

            var extrinsic = SudoCalls.Sudo(rootDeleteGame);

            return await GenericExtrinsicAsync(account, extrinsicType, extrinsic, concurrentTasks, token);
        }

        #endregion call
    }
}