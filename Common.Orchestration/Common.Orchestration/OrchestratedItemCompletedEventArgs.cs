using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Orchestration
{
    /// <summary>
    /// Event args for ScheduleItem completion
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OrchestratedItemCompletedEventArgs<T>: EventArgs
    {
        /// <summary>
        /// When did this time get reached?
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// The ScheduleItem that completed
        /// </summary>
        public IOrchestrateItem<T> ScheduleItem { get; set; }

        /// <summary>
        /// Default Ctor
        /// </summary>
        /// <param name="scheduledItem"></param>
        public OrchestratedItemCompletedEventArgs(IOrchestrateItem<T> scheduledItem)
        {
            ScheduleItem = scheduledItem;
            Timestamp = DateTimeOffset.Now;
        }
    }
}
