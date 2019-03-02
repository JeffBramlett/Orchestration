using System;
using System.Collections.Concurrent;

namespace Common.Orchestration
{
    /// <summary>
    /// Scheduler interface
    /// </summary>
    /// <typeparam name="T">The type of object to schedule</typeparam>
    public interface IOrchestrator<T> : IOrchestrateBase, IDisposable
    {
        #region Properties
        #endregion

        #region Events
        /// <summary>
        /// Event raised when on every interval, every scheduled item is checked on this interval
        /// </summary>
        event EventHandler ScheduledTimeReached;

        /// <summary>
        /// Event raised when scheduled item is complete (end datetime reached)
        /// </summary>
        event EventHandler ScheduledItemCompleted;

        /// <summary>
        /// Event raised when the Scheduler ends (duration reached, EndDateTime reached)
        /// </summary>
        event EventHandler OrchestratorEnded;
        #endregion

        #region Methods
        /// <summary>
        /// Add and Item to the Scheduler
        /// </summary>
        /// <param name="item">the item to add to the schedule</param>
        /// <param name="start">the start datetime of the item</param>
        /// <param name="interval">when to raise this item again</param>
        /// <returns>the id (incremented integer) of the scheduled item</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the start occurs before the schedulers start</exception>
        int ScheduleItem(T item, DateTime start, TimeSpan interval, TimeSpan duration, string variableTrigger = "");

        /// <summary>
        /// Add a Item to the Scheduler
        /// </summary>
        /// <param name="item">the item to add to the schedule</param>
        /// <param name="offset">the timespan offset from the scheduler start datetime</param>
        /// <param name="interval">when to raise this item again</param>
        /// <returns>the id (incremented integer) of the scheduled item</returns>
        int ScheduleItem(T item, TimeSpan offset, TimeSpan interval, TimeSpan duration, string variableTrigger = "");

        /// <summary>
        /// Deletes the Scheduled Item from the scheduler
        /// </summary>
        /// <param name="id">the id of the existing scheduled item</param>
        void DeleteScheduledItem(int id);

        #endregion
    }
}