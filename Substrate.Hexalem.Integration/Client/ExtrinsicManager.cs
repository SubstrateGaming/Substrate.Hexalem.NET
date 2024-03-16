using Serilog;
using Substrate.Hexalem.NET.NetApiExt.Generated;
using Substrate.Hexalem.NET.NetApiExt.Generated.Model.frame_system;
using Substrate.NetApi.Model.Rpc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Substrate.Integration.Client
{
    /// <summary>
    /// Extrinsic Update Event
    /// </summary>
    /// <param name="subscriptionId"></param>
    /// <param name="queueInfo"></param>
    public delegate void ExtrinsicUpdateEvent(string subscriptionId, ExtrinsicInfo queueInfo);

    /// <summary>
    /// Extrinsic Manager
    /// </summary>
    public class ExtrinsicManager
    {
        /// <summary>
        /// Reteintation time in seconds
        /// </summary>
        public const int RetentationTimeSec = 60;

        /// <summary>
        /// Extrinsic Update Event
        /// </summary>
        public event ExtrinsicUpdateEvent ExtrinsicUpdated;

        /// <summary>
        /// Running extrinsics
        /// </summary>
        public IEnumerable<ExtrinsicInfo> Running => _data.Values.Where(p => !p.IsCompleted);

        /// <summary>
        /// Pre in block extrinsics
        /// </summary>
        public IEnumerable<ExtrinsicInfo> PreInblock => _data.Values.Where(p => !p.IsInBlock && !p.IsCompleted);

        private readonly ConcurrentDictionary<string, ExtrinsicInfo> _data;

        /// <summary>
        /// Extrinisic Manager
        /// </summary>
        /// <param name="client"></param>
        public ExtrinsicManager(SubstrateClientExt client)
        {
            _data = new ConcurrentDictionary<string, ExtrinsicInfo>();

            ExtrinsicUpdated += OnExtrinsicUpdated;
        }

        /// <summary>
        /// Add a subscription of a certain extrinsic type
        /// </summary>
        /// <param name="subscription"></param>
        /// <param name="extrinsicType"></param>
        public void Add(string subscription, string extrinsicType)
        {
            _data.TryAdd(subscription, new ExtrinsicInfo(extrinsicType));
        }

        /// <summary>
        /// Get extrinsic info
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ExtrinsicInfo Get(string id)
        {
            if (!_data.TryGetValue(id, out ExtrinsicInfo queueInfo))
            {
                Log.Debug("QueueInfo not available for subscriptionId {id}", id);
                return null;
            }

            return queueInfo;
        }

        /// <summary>
        /// Update extrinsic info
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="extrinsicUpdate"></param>
        public void UpdateExtrinsicInfo(string subscriptionId, TransactionEventInfo extrinsicUpdate)
        {
            if (!_data.TryGetValue(subscriptionId, out ExtrinsicInfo queueInfo) || queueInfo == null)
            {
                queueInfo = new ExtrinsicInfo("Unknown");
            }

            queueInfo.Update(extrinsicUpdate);

            /// Possible transaction status events.
            ///
            /// The status events can be grouped based on their kinds as:
            ///
            /// 1. Runtime validated the transaction:
            /// 		- `Validated`
            ///
            /// 2. Inside the `Ready` queue:
            /// 		- `Broadcast`
            ///
            /// 3. Leaving the pool:
            /// 		- `BestChainBlockIncluded`
            /// 		- `Invalid`
            ///
            /// 4. Block finalized:
            /// 		- `Finalized`
            ///
            /// 5. At any time:
            /// 		- `Dropped`
            /// 		- `Error`
            ///
            /// The subscription's stream is considered finished whenever the following events are
            /// received: `Finalized`, `Error`, `Invalid` or `Dropped`. However, the user is allowed
            /// to unsubscribe at any moment.

            ExtrinsicUpdated?.Invoke(subscriptionId, queueInfo);

            if (!queueInfo.HasEvents && queueInfo.Hash != null && queueInfo.Index != null)
            {
                Log.Debug("Extrinsic {id} completed with {state}", subscriptionId, queueInfo.TransactionEvent);
            }

            var removeKeys = _data.Where(p => p.Value.TimeElapsed > RetentationTimeSec && p.Value.IsCompleted)
                              .Select(p => p.Key).ToList(); // Materialize the list of keys to remove
            Log.Debug("Remove {count} completed extrinsics, after {time}", removeKeys.Count, RetentationTimeSec);
            foreach (var key in removeKeys)
            {
                _data.TryRemove(key, out _); // Use TryRemove for thread-safe removal
            }
        }

        /// <summary>
        /// Update extrinsic events
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="allExtrinsicEvents"></param>
        internal void UpdateExtrinsicEvents(string subscriptionId, IEnumerable<EventRecord> allExtrinsicEvents)
        {
            if (!_data.TryGetValue(subscriptionId, out ExtrinsicInfo queueInfo))
            {
                return;
            }

            queueInfo.EventRecords = allExtrinsicEvents.ToList();

            ExtrinsicUpdated?.Invoke(subscriptionId, queueInfo);
        }

        /// <summary>
        /// Update extrinsic error
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="errorMsg"></param>
        internal void UpdateExtrinsicError(string subscriptionId, string errorMsg)
        {
            if (!_data.TryGetValue(subscriptionId, out ExtrinsicInfo queueInfo))
            {
                return;
            }

            queueInfo.Error = errorMsg;

            ExtrinsicUpdated?.Invoke(subscriptionId, queueInfo);
        }

        /// <summary>
        /// Simple extrinsic tester
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="queueInfo"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void OnExtrinsicUpdated(string subscriptionId, ExtrinsicInfo queueInfo)
        {
            Log.Debug("{name}[{id}] updated {state}",
                queueInfo.ExtrinsicType,
                subscriptionId,
                queueInfo.TransactionEvent);
        }
    }
}