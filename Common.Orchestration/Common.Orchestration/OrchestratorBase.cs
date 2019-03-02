using EquationSolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EquationSolver.Dto;
using System.Collections.Concurrent;
using System.Threading;

namespace Common.Orchestration
{
    /// <summary>
    /// Executing class for base schedule
    /// </summary>
    public class OrchestratorBase <T>: IOrchestrateBase
    {
        #region Fields
        DateTime _start;
        DateTime _end;
        TimeSpan _duration;
        TimeSpan _interval;

        ConcurrentDictionary<int, ScheduleItem<T>> _scheduledItemDictionary;

        private VariableProvider _variables;
        private IEquationSolver _solver;

        Timer _timer;
        #endregion

        #region Properties
        /// <summary>
        /// The logical identifier of this Orchestrator
        public string Name { get; set; }

        /// <summary>
        /// When to start executing
        /// </summary>
        public DateTime StartDateTime
        {
            get
            {
                if (_start == DateTime.MinValue)
                {
                    _start = DateTime.Now;
                }

                return _start;
            }
            set
            {
                IsStartingSet = true;
                _start = value;
            }
        }

        /// <summary>
        /// How long to execute
        /// </summary>
        public TimeSpan Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        /// <summary>
        /// How often to query the scheduled items
        /// </summary>
        public TimeSpan Interval
        {
            get
            {
                if (_interval == TimeSpan.MinValue)
                {
                    _interval = new TimeSpan(0, 0, 1);
                }
                return _interval;
            }
            set
            {
                _interval = value;
            }
        }

        /// <summary>
        /// Ending Datetime
        /// </summary>
        public DateTime EndDateTime
        {
            get { return _end; }
            protected set { _end = value; }
        }

        protected VariableProvider Variables
        {
            get
            {
                if (_variables == null)
                {
                    _variables = new VariableProvider();
                    _variables.VariableValueChanged += Variables_VariableValueChanged;
                }

                return _variables;
            }
        }

        protected IEquationSolver Solver
        {
            get
            {
                if (_solver == null)
                {
                    EquationProject proj = new EquationProject()
                    {
                        Equations = new List<Equation>(),
                        Functions = new List<Function>(),
                        Tables = new List<Table>(),
                        Variables = new List<Variable>()
                    };

                    _solver = EquationSolverFactory.Instance.CreateEquationSolver(proj, Variables);
                }

                return _solver;
            }
            set { _solver = value; }
        }

        /// <summary>
        /// Orchestrated Items in a dictionary
        /// </summary>
        protected ConcurrentDictionary<int, ScheduleItem<T>> ScheduledItemDictionary
        {
            get
            {
                _scheduledItemDictionary = _scheduledItemDictionary ?? new ConcurrentDictionary<int, ScheduleItem<T>>();
                return _scheduledItemDictionary;
            }
        }

        protected Timer HeartbeatTimer
        {
            get { return _timer; }
            set { _timer = value; }
        }

        #endregion

        #region Auto Properties
        /// <summary>
        /// Sorting Ordinal
        /// </summary>
        public int Ordinal { get; set; }

        /// <summary>
        /// is the scheudle sequential
        /// </summary>
        public bool IsSequential { get; set; }

        /// <summary>
        /// Is the StartDateTime set?
        /// </summary>
        public bool IsStartingSet { get; set; }

        /// <summary>
        /// Is the Duration set?
        /// </summary>
        public bool IsDurationSet { get; set; }

        /// <summary>
        /// Is the Ending set?
        /// </summary>
        public bool IsEndingSet { get; set; }
        #endregion

        #region Delegates and Events
        /// <summary>
        /// Event raised when on every interval, every Orchestrated item is checked on this interval
        /// </summary>
        public event EventHandler ScheduledTimeReached;

        /// <summary>
        /// Event raised when Orchestrated item is complete (end datetime reached)
        /// </summary>
        public event EventHandler ScheduledItemCompleted;

        /// <summary>
        /// Event raised when the Orchestrator ends (duration reached, EndDateTime reached)
        /// </summary>
        public event EventHandler OrchestratorEnded;
        #endregion

        #region Comparison Implementation
        /// <summary>
        /// Compare two OrchestrateBase objects
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(IOrchestrateBase x, IOrchestrateBase y)
        {
            return x.CompareTo(y);
        }

        /// <summary>
        /// Compare this OrchestrateBase with another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(IOrchestrateBase other)
        {
            return Ordinal - other.Ordinal;
        }
        #endregion

        #region Lifecycle
        /// <summary>
        /// Start the Scheduler
        /// </summary>
        public void Start()
        {
            if (!IsStartingSet)
                throw new ArgumentOutOfRangeException("StartDateTime", "StartDateTime must be set before calling Start()");

            while (DateTime.Now < StartDateTime)
            {
                Thread.Sleep(Interval);
            }

            TimerCallback callback = new TimerCallback(HeartbeatIntervalReached);
            TimeSpan startDuration = StartDateTime > DateTime.Now ? StartDateTime - DateTime.Now : TimeSpan.FromTicks(10);
            HeartbeatTimer = new Timer(callback, null, startDuration, Interval);
        }

        /// <summary>
        /// Stop the Orchestrator
        /// </summary>
        public void Stop()
        {
            HeartbeatTimer.Change(Timeout.Infinite, Timeout.Infinite);

            RaiseOrchestrationEnded();
        }

        #endregion

        #region Protected Event Raisers

        protected void RaiseScheduledItemCompleted(IScheduleItem<T> scheduleItem)
        {
            Task.Run(() =>
            {
                ScheduledItemCompletedEventArgs<T> reached = new ScheduledItemCompletedEventArgs<T>(scheduleItem);
                ScheduledItemCompleted?.Invoke(this, reached);
            });
        }

        protected void RaiseScheduledItemTimeReached(T itemToPush)
        {
            Task.Run(() =>
            {
                ScheduledItemTimeReachedEventArgs<T> reached = new ScheduledItemTimeReachedEventArgs<T>(itemToPush);
                ScheduledTimeReached?.Invoke(this, reached);
            });
        }

        protected void RaiseOrchestrationEnded()
        {
            Task.Run(() =>
            {
                OrchestratorEndedEventArgs ended = new OrchestratorEndedEventArgs();
                OrchestratorEnded?.Invoke(this, ended);
            });

        }
        #endregion

        #region Privates

        private void Variables_VariableValueChanged(string variableName)
        {
            foreach (var itemKey in ScheduledItemDictionary.Keys)
            {
                if (ScheduledItemDictionary[itemKey].TriggerVariableName == variableName)
                {
                    RaiseScheduledItemTimeReached(ScheduledItemDictionary[itemKey].Item);
                }
            }
        }
        #endregion

        #region Event handling
        /// <summary>
        /// Raise the relevant Orchestrated items
        /// </summary>
        /// <param name="stateInfo"></param>
        private void HeartbeatIntervalReached(object stateInfo)
        {
            DateTime now = DateTime.Now;

            if (now > EndDateTime)
            {
                foreach (var key in ScheduledItemDictionary.Keys)
                {
                    RaiseScheduledItemCompleted(ScheduledItemDictionary[key]);
                }

                Stop();
            }
            else
            {

                List<int> listToRemove = new List<int>();

                foreach (var key in ScheduledItemDictionary.Keys)
                {
                    if (ScheduledItemDictionary[key].Timestamp <= now)
                    {
                        ScheduledItemDictionary[key].Count++;
                        ScheduledItemDictionary[key].Timestamp = now + ScheduledItemDictionary[key].Interval;

                        if (now > ScheduledItemDictionary[key].EndDateTime)
                        {
                            listToRemove.Add(key);
                        }
                        else
                        {
                            RaiseScheduledItemTimeReached(ScheduledItemDictionary[key].Item);
                        }
                    }
                }

                foreach (var key in listToRemove)
                {
                    ScheduleItem<T> raiseIt;
                    ScheduledItemDictionary.TryRemove(key, out raiseIt);
                    RaiseScheduledItemCompleted(raiseIt);
                }
            }
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~OrchestratorBase()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }

}
