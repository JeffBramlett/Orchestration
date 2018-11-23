using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Orchestration
{
    /// <summary>
    /// Event args for OrchestrateItem completion
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ScheduledItemCompletedEventArgs<T>: EventArgs
    {
        /// <summary>
        /// When did this time get reached?
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// The OrchestrateItem that completed
        /// </summary>
        public IScheduleItem<T> ScheduleItem { get; set; }

        /// <summary>
        /// Default Ctor
        /// </summary>
        /// <param name="scheduledItem"></param>
        public ScheduledItemCompletedEventArgs(IScheduleItem<T> scheduledItem)
        {
            ScheduleItem = scheduledItem;
            Timestamp = DateTimeOffset.Now;
        }
    }
}
