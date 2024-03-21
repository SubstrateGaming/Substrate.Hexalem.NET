using Substrate.Hexalem.NET.NetApiExt.Generated.Model.pallet_hexalem.types;
using Substrate.Hexalem.NET.NetApiExt.Generated.Model.pallet_hexalem.types.board;
using System.Linq;

namespace Substrate.Hexalem.Integration.Model
{
    /// <summary>
    /// Board C# Wrapper
    /// </summary>
    public class BoardSharp
    {
        /// <summary>
        /// Board Constructor
        /// </summary>
        /// <param name="result"></param>
        public BoardSharp(HexBoard result)
        {
            Resources = result.Resources.Value.Select(x => (byte)x).ToArray();

            HexGrid = result.HexGrid.Value.Value.Select(x => new TileSharp(x)).ToArray();

            GameId = result.GameId.Value.Select(p => p.Value).ToArray();
        }
        
        /// <summary>
        /// The game identifier
        /// </summary>
        public byte[] GameId { get; private set; }

        /// <summary>
        /// Resources
        /// </summary>
        public byte[] Resources { get; private set; }
        
        /// <summary>
        /// Hex Grid
        /// </summary>
        public TileSharp[] HexGrid { get; private set; }
    }
}