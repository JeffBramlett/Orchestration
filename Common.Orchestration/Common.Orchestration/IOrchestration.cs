using System;

namespace Common.Orchestration
{
    public interface IOrchestration
    {
        IOrchestration RemoveOrchestrator<T>();
        IOrchestration SetOrchestrator<T>(IOrchestrator<T> orchestrator);
        IOrchestration SetVariable(string variableName, string value);
        IOrchestration SolveEquations();
        IOrchestration StartOrchestrator<T>();
        IOrchestration StartOrchestratorByName(string name);
        IOrchestration StartOrchestratorsByType(params Type[] types);
        IOrchestration StopOrchestrator<T>();
        IOrchestration StopOrchestratorByName(string name);
        IOrchestration StopOrchestratorsByType(params Type[] types);
    }
}