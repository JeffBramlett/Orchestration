using System;
using System.Collections.Generic;

namespace Common.Orchestration
{
    /// <summary>
    /// Interface to a OrchestrateItem
    /// </summary>
    /// <typeparam name="T">the type to use for this</typeparam>
    public interface IScheduleItem<T>: IComparable<IScheduleItem<T>>, IComparer<IScheduleItem<T>>
    {
        /// <summary>
        /// Id of the Schedule Item (incremented)
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// How many times has this item been raised
        /// </summary>
        int Count { get; set; }

        /// <summary>
        /// When to raise the item
        /// </summary>
        TimeSpan Interval { get; set; }

        /// <summary>
        /// The item to raise on the interval
        /// </summary>
        T Item { get; set; }

        /// <summary>
        /// The next DateTime to raise the item
        /// </summary>
        DateTime Timestamp { get; set; }

        /// <summary>
        /// the DateTime when the scheduled item is to be no longer raised
        /// </summary>
        DateTime EndDateTime { get; set; }
    }
}