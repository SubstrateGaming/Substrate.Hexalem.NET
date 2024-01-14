using Substrate.Hexalem.NET.NetApiExt.Generated.Model.frame_support.dispatch;
using Substrate.Hexalem.NET.NetApiExt.Generated.Model.frame_system;
using Substrate.Hexalem.NET.NetApiExt.Generated.Model.hexalem_runtime;
using Substrate.Hexalem.NET.NetApiExt.Generated.Model.sp_arithmetic;
using Substrate.Hexalem.NET.NetApiExt.Generated.Model.sp_runtime;
using Substrate.Integration.Helper;
using Substrate.NetApi;
using Substrate.NetApi.Model.Rpc;
using Substrate.NetApi.Model.Types.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Substrate.Integration.Client
{
    /// <summary>
    /// Extrinsic Info, class that holds all information about an extrinsic
    /// </summary>
    public class ExtrinsicInfo
    {
        /// <summary>
        /// Current blockchain transaction event information
        /// </summary>
        public TransactionEvent? TransactionEvent { get; private set; }

        /// <summary>
        /// Extrinsic type
        /// </summary>
        public string ExtrinsicType { get; }

        /// <summary>
        ///  Timestamp when the extrinsic was created
        /// </summary>
        public DateTime Created { get; private set; }

        /// <summary>
        /// Timestamp when the extrinsic was last updated
        /// </summary>
        public DateTime LastUpdated { get; private set; }

        /// <summary>
        /// Transaction hash
        /// </summary>
        public Hash Hash { get; private set; }

        /// <summary>
        /// Transaction index
        /// </summary>
        public uint? Index { get; set; }

        /// <summary>
        /// Is the extrinsic ready to be broadcasted
        /// </summary>
        public bool IsReady { get; private set; }

        /// <summary>
        /// Is the extrinsic in a block
        /// </summary>
        public bool IsInBlock { get; private set; }

        /// <summary>
        /// Is the extrinsic successfull
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// Is the extrinsic completed
        /// </summary>
        public bool IsCompleted { get; private set; }

        /// <summary>
        /// Does the extrinsic have events
        /// </summary>
        public bool HasEvents => EventRecords != null;

        /// <summary>
        /// Error message
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// All event records
        /// </summary>
        public List<EventRecord> EventRecords { get; set; }

        /// <summary>
        /// Time elapsed since the extrinsic was created
        /// </summary>
        public double TimeElapsed => DateTime.UtcNow.Subtract(LastUpdated).TotalSeconds;

        /// <summary>
        /// Extrinsic info constructor
        /// </summary>
        /// <param name="extrinsicType"></param>
        public ExtrinsicInfo(string extrinsicType)
        {
            ExtrinsicType = extrinsicType;
            Created = DateTime.UtcNow;
            LastUpdated = Created;
            TransactionEvent = null;
            Hash = null;
            IsReady = false;
            IsInBlock = false;
            IsSuccess = false;
            IsCompleted = false;

            EventRecords = null;
        }

        internal void Update(TransactionEventInfo transactionEventInfo)
        {
            LastUpdated = DateTime.UtcNow;

            TransactionEvent = transactionEventInfo.TransactionEvent;
            Hash = transactionEventInfo.Hash;
            Index = transactionEventInfo.Index;
            Error = transactionEventInfo.Error;

            switch (TransactionEvent)
            {
                case NetApi.Model.Rpc.TransactionEvent.Validated:
                    IsReady = true;
                    break;

                case NetApi.Model.Rpc.TransactionEvent.Broadcasted:
                    break;

                case NetApi.Model.Rpc.TransactionEvent.BestChainBlockIncluded:
                    IsInBlock = true;
                    break;

                case NetApi.Model.Rpc.TransactionEvent.Finalized:
                    IsSuccess = true;
                    IsCompleted = true;
                    break;

                case NetApi.Model.Rpc.TransactionEvent.Error:
                    IsCompleted = true;
                    break;

                case NetApi.Model.Rpc.TransactionEvent.Invalid:
                    IsCompleted = true;
                    break;

                case NetApi.Model.Rpc.TransactionEvent.Dropped:
                    IsCompleted = true;
                    break;

                default:
                    throw new NotSupportedException($"Unknown TransactionEvent {TransactionEvent}");
            }
        }

        public bool SystemExtrinsicEvent(out Hexalem.NET.NetApiExt.Generated.Model.frame_system.pallet.Event? systemExtrinsicEvent, out string errorMsg)
        {
            systemExtrinsicEvent = null;
            errorMsg = null;

            if (!AllEvents(RuntimeEvent.System, out IEnumerable<Hexalem.NET.NetApiExt.Generated.Model.frame_system.pallet.EnumEvent> allEnumEvents))
            {
                return false;
            }

            var systemEnumEvent = allEnumEvents.LastOrDefault();
            if (systemEnumEvent == null)
            {
                return false;
            }

            switch (systemEnumEvent.Value)
            {
                case Hexalem.NET.NetApiExt.Generated.Model.frame_system.pallet.Event.ExtrinsicSuccess:
                    break;

                case Hexalem.NET.NetApiExt.Generated.Model.frame_system.pallet.Event.ExtrinsicFailed:
                    var systemEnumEventData = (BaseTuple<EnumDispatchError, DispatchInfo>)systemEnumEvent.Value2;
                    var enumDispatchError = (EnumDispatchError)systemEnumEventData.Value[0];
                    errorMsg = MessageFromDispatchError(enumDispatchError);
                    break;

                default:
                    return false;
            }

            systemExtrinsicEvent = systemEnumEvent.Value;
            return true;
        }

        private string MessageFromDispatchError(EnumDispatchError dispatchError)
        {
            switch (dispatchError.Value)
            {
                case DispatchError.Module:
                    var moduleError = (ModuleError)dispatchError.Value2;
                    return dispatchError.Value.ToString() + $"[{(RuntimeEvent)moduleError.Index.Value}{moduleError.Index.Value}][{Utils.Bytes2HexString(moduleError.Error.Value.ToBytes())}]";

                case DispatchError.Token:
                    var enumTokenError = (EnumTokenError)dispatchError.Value2;
                    return dispatchError.Value.ToString() + $"[{enumTokenError.Value}]";

                case DispatchError.Arithmetic:
                    var enumArithmeticError = (EnumArithmeticError)dispatchError.Value2;
                    return dispatchError.Value.ToString() + $"[{enumArithmeticError.Value}]";

                case DispatchError.Transactional:
                    var enumTransactionalError = (EnumTransactionalError)dispatchError.Value2;
                    return dispatchError.Value.ToString() + $"[{enumTransactionalError.Value}]";

                default:
                    return dispatchError.Value.ToString();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="runtimeEvent"></param>
        /// <param name="allEnumEvents"></param>
        /// <returns></returns>
        public bool AllEvents<T>(RuntimeEvent runtimeEvent, out IEnumerable<T> allEnumEvents)
        {
            allEnumEvents = null;

            if (EventRecords == null || !EventRecords.Any())
            {
                return false;
            }

            var allevents = EventRecords.Where(p => p.Event.Value == runtimeEvent);
            if (!allevents.Any())
            {
                return false;
            }

            allEnumEvents = allevents.Select(p => (T)p.Event.Value2);
            return true;
        }
    }
}