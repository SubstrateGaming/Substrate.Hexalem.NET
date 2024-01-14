using Substrate.Hexalem.NET.NetApiExt.Generated.Model.pallet_hexalem.pallet;
using Substrate.Hexalem.NET.NetApiExt.Generated.Model.sp_core.crypto;
using Substrate.Integration.Helper;
using Substrate.NetApi.Model.Types.Primitive;
using System.Linq;

namespace Substrate.Hexalem.Integration.Model
{
    /// <summary>
    /// Game C# Wrapper
    /// </summary>
    public class GameSharp
    {
        /// <summary>
        /// Game Constructor
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="game"></param>
        public GameSharp(byte[] gameId, NET.NetApiExt.Generated.Model.pallet_hexalem.pallet.Game game)
        {
            GameId = gameId;
            State = game.State.Value;
            Round = game.Round.Value;
            PlayerTurn = (byte)(game.PlayerTurnAndPlayed.Value & 0x7F);
            Played = ((game.PlayerTurnAndPlayed.Value & 0x80) >> 7) == 1;
            Players = ((AccountId32[])game.Players.Value).Select(p => p.ToAddress()).ToArray();
            Selection = ((U8[])game.Selection.Value).Select(p => p.Value).ToArray();
            SelectionSize = game.SelectionSize.Value;
            LastBlock = game.LastPlayedBlock.Value;
        }

        /// <summary>
        /// Game Id
        /// </summary>
        public byte[] GameId { get; private set; }
        
        /// <summary>
        /// Game State
        /// </summary>
        public GameState State { get; private set; }
        
        /// <summary>
        /// Max Rounds
        /// </summary>
        public byte MaxRounds { get; private set; }
        
        /// <summary>
        /// Round
        /// </summary>
        public byte Round { get; private set; }
        
        /// <summary>
        /// Player Turn
        /// </summary>
        public byte PlayerTurn { get; private set; }
        
        /// <summary>
        /// Played
        /// </summary>
        public bool Played { get; private set; }
        
        /// <summary>
        /// Players
        /// </summary>
        public string[] Players { get; private set; }
        
        /// <summary>
        /// Selection
        /// </summary>
        public byte[] Selection { get; private set; }
        
        /// <summary>
        /// Selection Size
        /// </summary>
        public byte SelectionSize { get; private set; }
        
        /// <summary>
        /// Last Block
        /// </summary>
        public uint LastBlock { get; private set; }
    }
}