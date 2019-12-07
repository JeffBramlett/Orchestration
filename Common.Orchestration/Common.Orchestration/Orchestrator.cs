using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EquationSolver;

namespace Common.Orchestration
{
    /// <summary>
    /// Executing class for Scheduling items
    /// </summary>
    /// <typeparam name="T">The object type to schedule</typeparam>
    public sealed class Orchestrator<T> : OrchestratorBase<T>, IOrchestrator<T>
    {
        #region Constants
        #endregion

        #region Fields

        private bool IsInited { get; set; }
        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtors
        /// <summary>
        /// Minimal Ctor
        /// </summary>
        /// <param name="name">Name of Orchestrator</param>
        /// <param name="solver">the equation solver to use for orchestrating schedule items</param>
        public Orchestrator(string name, IOrchestratorRepository<T> repository, IEquationSolver solver = null) :
            this(name, TimeSpan.FromSeconds(1), repository, solver)
        {
        }

        /// <summary>
        /// Ctor for defining interval and name of orchestrator
        /// </summary>
        /// <param name="name">Name of the Orchestrator</param>
        /// <param name="interval">the interval to use for schedule items</param>
        /// <param name="solver">the equation solver to use for orchestrating schedule items</param>
        public Orchestrator(string name, TimeSpan interval, IOrchestratorRepository<T> repository, IEquationSolver solver = null) :
            this(name, interval, TimeSpan.FromDays(365 * 10), repository, solver)
        {
        }

        /// <summary>
        /// Ctor using DateTime.Now as starting DateTime
        /// </summary>
        /// <param name="interval">Timespan interval of execution on eligible ScheduleItems</param>
        /// <param name="duration">How long for this scheduler to remain active</param>
        public Orchestrator(string name, TimeSpan interval, TimeSpan duration, IOrchestratorRepository<T> repository, IEquationSolver solver = null) :
            this(name, DateTime.Now, interval, duration, repository, solver)
        {
        }

        /// <summary>
        /// Ctor using start DateTime for when this Scheduler becomes active
        /// </summary>
        /// <param name="start">the starting DateTime</param>
        /// <param name="interval">How often to check the scheduled items</param>
        /// <param name="duration">How long will this Scheduler be active</param>
        public Orchestrator(string name, DateTime start, TimeSpan interval, TimeSpan duration, IOrchestratorRepository<T> repository, IEquationSolver solver = null)
        {
            StartDateTime = start;
            Interval = interval;
            EndDateTime = DateTime.Now + duration;
            Name = name;
            Repository = repository;

            if (solver != null)
            {
                Solver = solver;
            }
        }
        #endregion

        #region Publics

        /// <summary>
        /// Initialize the Orchestrator
        /// </summary>
        public void Initialize()
        {
            if (!IsInited)
            {

                IsInited = true;
            }
        }

        /// <summary>
        /// Get all items in the Orchestrator
        /// </summary>
        /// <returns>Enumeration of all the items in the Orchestrator</returns>
        public IEnumerable<IScheduleItem<T>> GetItems()
        {
            return Repository.AllItems();
        }

        /// <summary>
        /// Build and schedule your own item
        /// </summary>
        /// <param name="scheduleItem">the item to schedule</param>
        /// <returns>the id for the schedule item</returns>
        public int ScheduleItem(IScheduleItem<T> scheduleItem)
        {
            if (scheduleItem == null)
                throw new ArgumentOutOfRangeException(nameof(scheduleItem));

            scheduleItem.Timestamp = scheduleItem.StartDateTime;

            if (scheduleItem.MaxOccurrances == 1)
            {
                scheduleItem.EndDateTime = EndDateTime;
            }
            int id = Repository.SaveOrchestratorItem(scheduleItem);

            return id;
        }

        /// <summary>
        /// Update a schedule item
        /// </summary>
        /// <param name="item">the changed scheduled item</param>
        /// <returns>the id of the item</returns>
        public int UpdateScheduledItem(ScheduleItem<T> item)
        {
            return Repository.UpdateOrchestratorItem(item);
        }

        /// <summary>
        /// Remove the Orchestrated item 
        /// </summary>
        /// <param name="id"></param>
        public bool DeleteScheduledItem(int id)
        {
            return Repository.RemoveOrchestratorItem(id);
        }

        /// <summary>
        /// Find all scheduled items with the variable target named
        /// </summary>
        /// <param name="variableTargetName">the name of the variable target</param>
        /// <returns>collection (may be empty) of scheduled items</returns>
        public IEnumerable<IScheduleItem<T>> Find(Func<IScheduleItem<T>, object[], bool> filterDelegate, object[] args)
        {
            return Repository.Find(filterDelegate, args);
        }

        #endregion;

        #region Privates
        #endregion
    }
}
