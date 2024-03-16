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


namespace Substrate.Hexalem.NET.NetApiExt.Generated.Model.sp_weights
{
    
    
    /// <summary>
    /// >> 70 - Composite[sp_weights.RuntimeDbWeight]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class RuntimeDbWeight : BaseType
    {
        
        /// <summary>
        /// >> read
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U64 Read { get; set; }
        /// <summary>
        /// >> write
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U64 Write { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "RuntimeDbWeight";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Read.Encode());
            result.AddRange(Write.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Read = new Substrate.NetApi.Model.Types.Primitive.U64();
            Read.Decode(byteArray, ref p);
            Write = new Substrate.NetApi.Model.Types.Primitive.U64();
            Write.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
