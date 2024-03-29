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


namespace Substrate.Hexalem.NET.NetApiExt.Generated.Model.pallet_sudo.pallet
{
    
    
    /// <summary>
    /// >> Event
    /// The `Event` enum of this pallet
    /// </summary>
    public enum Event
    {
        
        /// <summary>
        /// >> Sudid
        /// A sudo just took place. \[result\]
        /// </summary>
        Sudid = 0,
        
        /// <summary>
        /// >> KeyChanged
        /// The \[sudoer\] just switched identity; the old key is supplied if one existed.
        /// </summary>
        KeyChanged = 1,
        
        /// <summary>
        /// >> SudoAsDone
        /// A sudo just took place. \[result\]
        /// </summary>
        SudoAsDone = 2,
    }
    
    /// <summary>
    /// >> 30 - Variant[pallet_sudo.pallet.Event]
    /// The `Event` enum of this pallet
    /// </summary>
    public sealed class EnumEvent : BaseEnumExt<Event, Substrate.Hexalem.NET.NetApiExt.Generated.Types.Base.EnumResult, Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.Hexalem.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32>, Substrate.Hexalem.NET.NetApiExt.Generated.Types.Base.EnumResult>
    {
    }
}
