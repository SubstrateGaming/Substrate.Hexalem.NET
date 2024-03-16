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
        }
        
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