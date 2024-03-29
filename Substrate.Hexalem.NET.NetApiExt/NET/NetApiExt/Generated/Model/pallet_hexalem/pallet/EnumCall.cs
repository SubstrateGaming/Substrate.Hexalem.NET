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


namespace Substrate.Hexalem.NET.NetApiExt.Generated.Model.pallet_hexalem.pallet
{
    
    
    /// <summary>
    /// >> Call
    /// Contains a variant per dispatchable extrinsic that this pallet has.
    /// </summary>
    public enum Call
    {
        
        /// <summary>
        /// >> create_game
        /// See [`Pallet::create_game`].
        /// </summary>
        create_game = 0,
        
        /// <summary>
        /// >> queue
        /// See [`Pallet::queue`].
        /// </summary>
        queue = 100,
        
        /// <summary>
        /// >> accept_match
        /// See [`Pallet::accept_match`].
        /// </summary>
        accept_match = 101,
        
        /// <summary>
        /// >> force_accept_match
        /// See [`Pallet::force_accept_match`].
        /// </summary>
        force_accept_match = 102,
        
        /// <summary>
        /// >> play
        /// See [`Pallet::play`].
        /// </summary>
        play = 1,
        
        /// <summary>
        /// >> upgrade
        /// See [`Pallet::upgrade`].
        /// </summary>
        upgrade = 2,
        
        /// <summary>
        /// >> finish_turn
        /// See [`Pallet::finish_turn`].
        /// </summary>
        finish_turn = 3,
        
        /// <summary>
        /// >> force_finish_turn
        /// See [`Pallet::force_finish_turn`].
        /// </summary>
        force_finish_turn = 4,
        
        /// <summary>
        /// >> claim_rewards
        /// See [`Pallet::claim_rewards`].
        /// </summary>
        claim_rewards = 5,
        
        /// <summary>
        /// >> root_delete_game
        /// See [`Pallet::root_delete_game`].
        /// </summary>
        root_delete_game = 6,
    }
    
    /// <summary>
    /// >> 90 - Variant[pallet_hexalem.pallet.Call]
    /// Contains a variant per dispatchable extrinsic that this pallet has.
    /// </summary>
    public sealed class EnumCall : BaseEnumExt<Call, BaseTuple<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.Hexalem.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32>, Substrate.NetApi.Model.Types.Primitive.U8>, Substrate.Hexalem.NET.NetApiExt.Generated.Model.pallet_hexalem.types.board.resource.Move, Substrate.NetApi.Model.Types.Primitive.U8, BaseVoid, Substrate.Hexalem.NET.NetApiExt.Generated.Types.Base.Arr32U8, BaseVoid, Substrate.Hexalem.NET.NetApiExt.Generated.Types.Base.Arr32U8, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid, BaseVoid>
    {
    }
}
