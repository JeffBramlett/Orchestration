using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Orchestration
{
    /// <summary>
    /// Data class for args containing the Item raised
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OrchestratedTimeReachedEventArgs<T>: EventArgs
    {
        /// <summary>
        /// When did this time get reached?
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// The schedule item raised
        /// </summary>
        public T OrchestratedItem { get; set; }

        /// <summary>
        /// Default Ctor
        /// </summary>
        /// <param name="orchestratedItem">the selected item required</param>
        public OrchestratedTimeReachedEventArgs(T orchestratedItem)
        {
            OrchestratedItem = orchestratedItem;
            Timestamp = DateTimeOffset.Now;
        }
    }
}
