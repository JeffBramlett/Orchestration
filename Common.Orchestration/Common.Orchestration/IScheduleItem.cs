using System;
using System.Collections.Generic;

namespace Common.Orchestration
{
    /// <summary>
    /// Interface to a OrchestrateItem
    /// </summary>
    /// <typeparam name="T">the type to use for this</typeparam>
    public interface IScheduleItem<T> : IExecuteItem<T>
    {
        /// <summary>
        /// The timespan between raising the item
        /// </summary>
        TimeSpan Interval { get; set; }

        /// <summary>
        /// The next DateTime to raise the item
        /// </summary>
        DateTime Timestamp { get; set; }

        /// <summary>
        /// the DateTime when the scheduled item is to be no longer raised
        /// </summary>
        DateTime EndDateTime { get; set; }

        /// <summary>
        /// The name of the variable that is a trigger for this item
        /// </summary>
        string TriggerVariableName { get; set; }

        /// <summary>
        /// The expression that if it evaluates to true the item is sent
        /// </summary>
        string EvaluateExpression { get; set; }
    }
}