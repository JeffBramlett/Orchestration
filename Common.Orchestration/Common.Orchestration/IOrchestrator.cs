using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

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
        /// Initialize the Orchestrator
        /// </summary>
        void Initialize();

        /// <summary>
        /// Get all items in the Orchestrator
        /// </summary>
        /// <returns>Enumeration of all the items in the Orchestrator</returns>
        IEnumerable<IScheduleItem<T>> GetItems();

        /// <summary>
        /// Build and schedule your own item
        /// </summary>
        /// <param name="scheduleItem">the item to schedule</param>
        /// <returns>the id (incremented integer) of the scheduled item</returns>
        int ScheduleItem(IScheduleItem<T> scheduleItem);

        /// <summary>
        /// Find all scheduled items with the variable target named
        /// </summary>
        /// <param name="filterDelegate">delegate to filter results</param>
        /// <returns>collection (may be empty) of scheduled items</returns>
        IEnumerable<IScheduleItem<T>> Find(Func<IScheduleItem<T>, object[], bool> filterDelegate, object[] args);

        /// <summary>
        /// Deletes the Scheduled Item from the scheduler
        /// </summary>
        /// <param name="id">the id of the existing scheduled item</param>
        bool DeleteScheduledItem(int id);

        #endregion
    }
}