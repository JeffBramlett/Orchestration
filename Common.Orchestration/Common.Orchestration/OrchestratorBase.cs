using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Orchestration
{
    /// <summary>
    /// Executing class for base schedule
    /// </summary>
    public class OrchestratorBase : IOrchestrateBase
    {
        #region Fields
        DateTime _start;
        DateTime _end;
        TimeSpan _duration;
        TimeSpan _interval;
        #endregion

        #region Properties

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
    }

}
