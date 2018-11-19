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
        /// <summary>
        /// The schudule dictionary for finding scheduled items
        /// </summary>
        ConcurrentDictionary<int, OrchestrateItem<T>> ScheduledDictionary { get; }
        #endregion

        #region Events
        /// <summary>
        /// Event raised when on every interval, every scheduled item is checked on this interval
        /// </summary>
        event EventHandler ScheduledTimeReached;

        /// <summary>
        /// Event raised when scheduled item is complete (end datetime reached)
        /// </summary>
        event EventHandler ScheduleItemCompleted;

        /// <summary>
        /// Event raised when the Scheduler ends (duration reached, EndDateTime reached)
        /// </summary>
        event EventHandler SchedulerEnded;
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
        int AddScheduleItem(T item, DateTime start, TimeSpan interval, TimeSpan duration);

        /// <summary>
        /// Add a Item to the Scheduler
        /// </summary>
        /// <param name="item">the item to add to the schedule</param>
        /// <param name="offset">the timespan offset from the scheduler start datetime</param>
        /// <param name="interval">when to raise this item again</param>
        /// <returns>the id (incremented integer) of the scheduled item</returns>
        int AddScheduleItem(T item, TimeSpan offset, TimeSpan interval, TimeSpan duration);

        /// <summary>
        /// Deletes the Scheduled Item from the scheduler
        /// </summary>
        /// <param name="id">the id of the existing scheduled item</param>
        void DeleteScheduleItem(int id);

        /// <summary>
        /// Start the Scheduler
        /// </summary>
        void Start();

        /// <summary>
        /// Stop the Scheduler
        /// </summary>
        void Stop();
        #endregion
    }
}