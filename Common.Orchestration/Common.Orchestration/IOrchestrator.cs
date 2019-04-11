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
        /// <param name="variableTargetName">the name of the variable target</param>
        /// <returns>collection (may be empty) of scheduled items</returns>
        IEnumerable<IScheduleItem<T>> FindByVariableTargetName(string variableTargetName);

        /// <summary>
        /// Deletes the Scheduled Item from the scheduler
        /// </summary>
        /// <param name="id">the id of the existing scheduled item</param>
        bool DeleteScheduledItem(int id);

        #endregion
    }
}