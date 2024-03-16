//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Substrate.NetApi.Attributes;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Metadata.V14;
using System.Collections.Generic;


namespace Substrate.Hexalem.NET.NetApiExt.Generated.Model.pallet_hexalem.types.board.resource
{
    
    
    /// <summary>
    /// >> 143 - Composite[pallet_hexalem.types.board.resource.ResourceProductions]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class ResourceProductions : BaseType
    {
        
        /// <summary>
        /// >> produces
        /// </summary>
        public Substrate.Hexalem.NET.NetApiExt.Generated.Types.Base.Arr7U8 Produces { get; set; }
        /// <summary>
        /// >> human_requirements
        /// </summary>
        public Substrate.Hexalem.NET.NetApiExt.Generated.Types.Base.Arr7U8 HumanRequirements { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "ResourceProductions";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Produces.Encode());
            result.AddRange(HumanRequirements.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Produces = new Substrate.Hexalem.NET.NetApiExt.Generated.Types.Base.Arr7U8();
            Produces.Decode(byteArray, ref p);
            HumanRequirements = new Substrate.Hexalem.NET.NetApiExt.Generated.Types.Base.Arr7U8();
            HumanRequirements.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}