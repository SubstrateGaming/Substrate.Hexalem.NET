using Substrate.Hexalem.NET.NetApiExt.Generated.Model.pallet_hexalem.pallet;
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
            GameId = result.GameId.Value.Select(x => (byte)x).ToArray();
            Resources = result.Resources.Value.Select(x => (byte)x).ToArray();

            HexGrid = result.HexGrid.Value.Value.Select(x => new TileSharp(x)).ToArray();
        }

        /// <summary>
        /// Game Id
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