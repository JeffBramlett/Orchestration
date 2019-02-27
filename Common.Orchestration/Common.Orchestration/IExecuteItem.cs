using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Orchestration
{
    public interface IExecuteItem<T> : IComparable<IScheduleItem<T>>, IComparer<IScheduleItem<T>>
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
        /// The item to raise on the interval
        /// </summary>
        T Item { get; set; }

    }
}
