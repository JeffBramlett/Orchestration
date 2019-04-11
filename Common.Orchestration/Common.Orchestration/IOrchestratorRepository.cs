using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Orchestration
{
    /// <summary>
    /// Contract for Orchestrator item repository
    /// </summary>
    /// <typeparam name="T">type of the items to store</typeparam>
    public interface IOrchestratorRepository<T> : IDisposable
    {
        /// <summary>
        /// Get all Items in the repository
        /// </summary>
        /// <returns>All the items in the Repository</returns>
        IEnumerable<IScheduleItem<T>> AllItems();

        /// <summary>
        /// Filter all items in the repository and return them
        /// </summary>
        /// <param name="variableName">the matching name of the varable</param>
        /// <returns>collection of matching items</returns>
        IEnumerable<IScheduleItem<T>> FindByVariableTargetName(string variableName);

        /// <summary>
        /// Save the item to the Repository
        /// </summary>
        /// <param name="orchestratorItem">the item to save</param>
        /// <returns>the id of the item in the repository</returns>
        int SaveOrchestratorItem(IScheduleItem<T> scheduleItem);

        /// <summary>
        /// Update the item in the repository
        /// </summary>
        /// <param name="orchestratorItem">the new item to replace the existing item</param>
        /// <param name="isMatch">function to match the item to update</param>
        /// <returns>the id of the item replacing</returns>
        int UpdateOrchestratorItem(IScheduleItem<T> scheduleItem);

        /// <summary>
        /// Remove the first matching item
        /// </summary>
        /// <param name="isMatch">funciton to determine a matching item</param>
        /// <returns>true if successful, false otherwise</returns>
        bool RemoveOrchestratorItem(int id);

        /// <summary>
        /// Remove expired Items and consolidate repository
        /// </summary>
        /// <returns>count of remaining items</returns>
        int Recycle();
    }
}
