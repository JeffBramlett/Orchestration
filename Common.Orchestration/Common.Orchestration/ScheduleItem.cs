﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Orchestration
{
    /// <summary>
    /// Data class for a scheduled item
    /// </summary>
    /// <typeparam name="T">the type of the scheduled item</typeparam>
    public class ScheduleItem<T> : IScheduleItem<T>
    {
        #region Auto Properties
        /// <summary>
        /// Assigned id of the schedule item
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The scheduled item 
        /// </summary>
        public T Item { get; set; }

        /// <summary>
        /// MaximumOccurances that can be scheduled (0 is unbounded)
        /// </summary>
        public int MaxOccurrances { get; set; }

        /// <summary>
        /// Datetime when the item will be raised again
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Count of how many time this schedule item has been raised
        /// </summary>
        public int Count { get; set; }

        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// The interval to raise the item
        /// </summary>
        public TimeSpan Interval { get; set; }

        /// <summary>
        /// The DateTime when the scheduled item is to be no longer raised
        /// </summary>
        public DateTime EndDateTime { get; set; }

        /// <summary>
        /// The name of the variable that is a trigger for this item
        /// </summary>
        public string TriggerVariableName { get; set; }

        /// <summary>
        /// The expression that if it evaluates to true the item is sent
        /// </summary>
        public string EvaluateExpression { get; set; }
        #endregion

        #region Comparison Implementation
        /// <summary>
        /// Compare two Schedule items
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>int of the comparison</returns>
        public int Compare(IScheduleItem<T> x, IScheduleItem<T> y)
        {
            return x.CompareTo(y);
        }

        /// <summary>
        /// Compare this to another Scheduled item
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(IScheduleItem<T> other)
        {
            return Timestamp.CompareTo(other.Timestamp);
        }
        #endregion
    }
}
