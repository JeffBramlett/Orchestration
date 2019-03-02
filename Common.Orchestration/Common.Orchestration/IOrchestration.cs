using System;

namespace Common.Orchestration
{
    public interface IOrchestration
    {
        void RemoveOrchestrator<T>();
        void SetOrchestrator<T>(IOrchestrator<T> orchestrator);
        void SetVariable(string variableName, string value);
        void SolveEquations();
        void StartOrchestrator<T>();
        void StartOrchestratorByName(string name);
        void StartOrchestratorsByType(params Type[] types);
        void StopOrchestrator<T>();
        void StopOrchestratorByName(string name);
        void StopOrchestratorsByType(params Type[] types);
    }
}