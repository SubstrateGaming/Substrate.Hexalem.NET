using Substrate.Hexalem.NET.NetApiExt.Generated.Model.hexalem_runtime;

namespace Substrate.Hexalem.Integration.Model
{
    /// <summary>
    /// Tile C# Wrapper
    /// </summary>
    public class TileSharp
    {
        /// <summary>
        /// Value
        /// </summary>
        public byte Value { get; private set; }

        /// <summary>
        /// Tile Constructor
        /// </summary>
        /// <param name="tile"></param>
        public TileSharp(HexalemTile tile)
        {
            TileType = (byte)((tile.Value.Value >> 3) & 0x7);
            TileLevel = (byte)((tile.Value.Value >> 6) & 0x3);
            Pattern = (byte)(tile.Value.Value & 0x7);
            Value = tile.Value.Value;
        }

        /// <summary>
        /// Tile Type
        /// </summary>
        public byte TileType { get; private set; }
        
        /// <summary>
        /// Tile Level
        /// </summary>
        public byte TileLevel { get; private set; }
        
        /// <summary>
        /// Pattern
        /// </summary>
        public byte Pattern { get; private set; }
    }
}