//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Substrate.NetApi.Model.Types.Base;
using System.Collections.Generic;


namespace Substrate.Hexalem.NET.NetApiExt.Generated.Model.pallet_grandpa.pallet
{
    
    
    /// <summary>
    /// >> Event
    /// The `Event` enum of this pallet
    /// </summary>
    public enum Event
    {
        
        /// <summary>
        /// >> NewAuthorities
        /// New authority set has been applied.
        /// </summary>
        NewAuthorities = 0,
        
        /// <summary>
        /// >> Paused
        /// Current authority set has been paused.
        /// </summary>
        Paused = 1,
        
        /// <summary>
        /// >> Resumed
        /// Current authority set has been resumed.
        /// </summary>
        Resumed = 2,
    }
    
    /// <summary>
    /// >> 44 - Variant[pallet_grandpa.pallet.Event]
    /// The `Event` enum of this pallet
    /// </summary>
    public sealed class EnumEvent : BaseEnumExt<Event, Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.Hexalem.NET.NetApiExt.Generated.Model.sp_consensus_grandpa.app.Public, Substrate.NetApi.Model.Types.Primitive.U64>>, BaseVoid, BaseVoid>
    {
    }
}
