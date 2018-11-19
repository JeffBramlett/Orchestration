using System;
using System.Collections.Generic;

namespace Common.Orchestration
{
    /// <summary>
    /// Executing contract for base scheduling
    /// </summary>
    public interface IOrchestrateBase: IComparable<IOrchestrateBase>, IComparer<IOrchestrateBase>
    {
        /// <summary>
        /// How long does the Schedule execute
        /// </summary>
        TimeSpan Duration { get; set; }

        /// <summary>
        /// What is the DateTime the scedule ends
        /// </summary>
        DateTime EndDateTime { get; }

        /// <summary>
        /// How often is the schedule repeated
        /// </summary>
        TimeSpan Interval { get; }

        /// <summary>
        /// is the scheudle sequential
        /// </summary>
        bool IsSequential { get; set; }

        /// <summary>
        /// Sorting ordinal
        /// </summary>
        int Ordinal { get; set; }

        /// <summary>
        /// Is the StartDateTime set?
        /// </summary>
        bool IsStartingSet { get; set; }

        /// <summary>
        /// Is the Duration set?
        /// </summary>
        bool IsDurationSet { get; set; }

        /// <summary>
        /// Is the Ending set?
        /// </summary>
        bool IsEndingSet { get; set; }

        /// <summary>
        /// When to start the schedule.
        /// </summary>
        DateTime StartDateTime { get; set; }
    }
}