using Substrate.Hexalem.NET.NetApiExt.Generated.Model.pallet_hexalem.types;
using Substrate.Hexalem.NET.NetApiExt.Generated.Types.Base;
using Substrate.NetApi.Model.Types;
using System.Linq;

namespace Substrate.Hexalem.Integration.Model
{
    /// <summary>
    /// Matchmaking State C# Wrapper
    /// </summary>
    public class MatchmakingStateSharp
    {
        /// <summary>
        /// Matchmaking State Constructor
        /// </summary>
        /// <param name="matchmakingState"></param>
        public MatchmakingStateSharp(EnumMatchmakingState matchmakingState)
        {
            MatchmakingState = matchmakingState.Value;
            GameId = null;
            if (matchmakingState.Value == MatchmakingState.Joined)
            {
                GameId = ((Arr32U8)matchmakingState.Value2).Value.Select(p => p.Value).ToArray();
            }
        }

        /// <summary>
        /// Matchmaking State
        /// </summary>
        public MatchmakingState MatchmakingState { get; private set; }

        /// <summary>
        /// Game Id
        /// </summary>
        public byte[]? GameId { get; private set; }

        /// <summary>
        /// Return true if the game is created
        /// </summary>
        public bool IsGameFound => GameId != null;
    }
}