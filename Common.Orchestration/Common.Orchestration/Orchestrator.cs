using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Orchestration
{
    /// <summary>
    /// Executing class for Scheduling items
    /// </summary>
    /// <typeparam name="T">The object type to schedule</typeparam>
    public sealed class Orchestrator<T>: OrchestrateBase, IOrchestrator<T>
    {
        #region Fields
        Timer _timer;
        ConcurrentDictionary<int, OrchestrateItem<T>> _orchestratedItemDictionary;
        #endregion

        #region Delegates and Events
        /// <summary>
        /// Event raised when on every interval, every scheduled item is checked on this interval
        /// </summary>
        public event EventHandler ScheduledTimeReached;

        /// <summary>
        /// Event raised when scheduled item is complete (end datetime reached)
        /// </summary>
        public event EventHandler ScheduleItemCompleted;

        /// <summary>
        /// Event raised when the Scheduler ends (duration reached, EndDateTime reached)
        /// </summary>
        public event EventHandler SchedulerEnded;
        #endregion

        #region Properties
        private Timer HeartbeatTimer
        {
            get { return _timer; }
            set { _timer = value; }
        }

        /// <summary>
        /// Scheduled Items in a dictionary
        /// </summary>
        public ConcurrentDictionary<int, OrchestrateItem<T>> ScheduledDictionary
        {
            get
            {
                _orchestratedItemDictionary = _orchestratedItemDictionary ?? new ConcurrentDictionary<int, OrchestrateItem<T>>();
                return _orchestratedItemDictionary;
            }
        }
        #endregion

        #region Ctors and Dtors
        /// <summary>
        /// Ctor using DateTime.Now as starting DateTime
        /// </summary>
        /// <param name="interval">Timespan interval of execution on eligible ScheduleItems</param>
        /// <param name="duration">How long for this scheduler to remain active</param>
        public Orchestrator(TimeSpan interval, TimeSpan duration)
        {
            StartDateTime = DateTime.Now;
            Interval = interval;
            EndDateTime = StartDateTime + duration;
        }

        /// <summary>
        /// Ctor using start DateTime for when this Scheduler becomes active
        /// </summary>
        /// <param name="start">the starting DateTime</param>
        /// <param name="interval">How often to check the scheduled items</param>
        /// <param name="duration">How long will this Scheduler be active</param>
        public Orchestrator(DateTime start, TimeSpan interval, TimeSpan duration)
        {
            StartDateTime = start;
            Interval = interval;
            EndDateTime = DateTime.Now + duration;
        }
        #endregion

        #region Publics
        /// <summary>
        /// Start the Scheduler
        /// </summary>
        public void Start()
        {
            if (!IsStartingSet)
                throw new ArgumentOutOfRangeException("StartDateTime", "StartDateTime must be set before calling Start()");

            while(DateTime.Now < StartDateTime)
            {
                Thread.Sleep(Interval);
            }

            TimerCallback callback = new TimerCallback(HeartbeatIntervalReached);
            TimeSpan startDuration = StartDateTime > DateTime.Now? StartDateTime - DateTime.Now: new TimeSpan();
            HeartbeatTimer = new Timer(callback, null, startDuration, Interval);
        }

        /// <summary>
        /// Add a schedule item to the Scheduler
        /// </summary>
        /// <param name="item">the Item to add</param>
        /// <param name="offset">when to raise item after the scheduler startdatetime</param>
        /// <param name="interval">when to raise the item</param>
        /// <param name="duration">how much time from Offset to end this item</param>
        /// <returns>the id (incremented integer) of the scheduled item</returns>
        public int AddScheduleItem(T item, TimeSpan offset, TimeSpan interval, TimeSpan duration)
        {
            int id = ScheduledDictionary.Count;
            DateTime next = offset == TimeSpan.MinValue? DateTime.Now: DateTime.Now + offset;

            OrchestrateItem<T> scheduleItem = new OrchestrateItem<T>()
            {
                Id = id,
                Item = item, 
                Interval = interval,
                Timestamp = next,
                EndDateTime = next + duration
            };

            ScheduledDictionary.TryAdd(id, scheduleItem);

            return id;
        }

        /// <summary>
        /// Add and Item to the Scheduler
        /// </summary>
        /// <param name="item">the item to add to the schedule</param>
        /// <param name="start">the start datetime of the item</param>
        /// <param name="interval">when to raise this item again</param>
        /// <param name="duration">how much time from start to end this item</param>
        /// <returns>the id (incremented integer) of the scheduled item</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the start occurs before the schedulers start</exception>
        public int AddScheduleItem(T item, DateTime start, TimeSpan interval, TimeSpan duration)
        {
            if (start < StartDateTime)
                throw new  ArgumentOutOfRangeException(nameof(start));

            int id = ScheduledDictionary.Count;

            OrchestrateItem<T> scheduleItem = new OrchestrateItem<T>()
            {
                Id = id,
                Item = item,
                Interval = interval,
                Timestamp = start,
                EndDateTime = start + duration
            };

            ScheduledDictionary.TryAdd(id, scheduleItem);

            return id;
        }

        /// <summary>
        /// Remove the Schedule item 
        /// </summary>
        /// <param name="id"></param>
        public void DeleteScheduleItem(int id)
        {
            OrchestrateItem<T> notUsedItem;
            ScheduledDictionary.TryRemove(id, out notUsedItem);
        }

        /// <summary>
        /// Stop the scheduler
        /// </summary>
        public void Stop()
        {
            HeartbeatTimer.Change(Timeout.Infinite, Timeout.Infinite);

            Task.Run(() => {
                OrchestratorEndedEventArgs ended = new OrchestratorEndedEventArgs();
                SchedulerEnded?.Invoke(this, ended);
            });
        }
        #endregion

        #region Event handling
        /// <summary>
        /// Raise the relevant Schedule items
        /// </summary>
        /// <param name="stateInfo"></param>
        private void HeartbeatIntervalReached(object stateInfo)
        {
            DateTime now = DateTime.Now;

            List<int> listToRemove = new List<int>();

            foreach(var key in ScheduledDictionary.Keys)
            {
                if(ScheduledDictionary[key].Timestamp <= now)
                {
                    RaiseScheduledTimeReached(ScheduledDictionary[key].Item);
                    ScheduledDictionary[key].Count++;
                    ScheduledDictionary[key].Timestamp = now + ScheduledDictionary[key].Interval;

                    if(now > ScheduledDictionary[key].EndDateTime)
                    {
                        listToRemove.Add(key);
                    }
                }
            }

            foreach(var key in listToRemove)
            {
                OrchestrateItem<T> raiseIt;
                ScheduledDictionary.TryRemove(key, out raiseIt);
                RaiseScheduleItemCompleted(raiseIt);
            }

            if(now > EndDateTime)
            {
                foreach (var key in ScheduledDictionary.Keys)
                {
                    RaiseScheduleItemCompleted(ScheduledDictionary[key]);
                }

                Stop();
            }
        }

        private void RaiseScheduleItemCompleted(IOrchestrateItem<T> scheduleItem)
        {
            Task.Run(() => {
                OrchestratedItemCompletedEventArgs<T> reached = new OrchestratedItemCompletedEventArgs<T>(scheduleItem);
                ScheduleItemCompleted?.Invoke(this, reached);
            });
        }

        private void RaiseScheduledTimeReached(T itemToPush)
        {
            Task.Run(() => {
                OrchestratedTimeReachedEventArgs<T> reached = new OrchestratedTimeReachedEventArgs<T>(itemToPush);
                ScheduledTimeReached?.Invoke(this, reached);
            });
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    Stop();
                    HeartbeatTimer.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~Orchestrator()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        /// <summary>
        /// Stop all threads and release resources
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);

            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
