using System;
using System.Collections.Generic;
using System.Threading;
using EquationSolver.Dto;
using Xunit;

namespace Common.Orchestration.Unit.Tests
{
    public class OrchestrationTests
    {
        [Fact]
        public void SetOrchestratorTest()
        {
            IOrchestration orchestration = new Orchestration();

            bool stringOrchestratorEnded = false;
            bool intOrchestratorEnded = false;

            IOrchestrator<string> stringOrchestrator = new Orchestrator<string>("ByString");
            stringOrchestrator.OrchestratorEnded += delegate(object sender, EventArgs args)
            {
                stringOrchestratorEnded = true;
            };

            IOrchestrator<int> intOrchestrator = new Orchestrator<int>("ByInt");
            intOrchestrator.OrchestratorEnded += delegate (object sender, EventArgs args)
            {
                intOrchestratorEnded = true;
            };

            orchestration.SetOrchestrator(stringOrchestrator);
            orchestration.SetOrchestrator(intOrchestrator);

            orchestration.StopOrchestrator<string>();
            orchestration.StopOrchestrator<int>();

            Thread.Sleep(1000);

            Assert.True(stringOrchestratorEnded);
            Assert.True(intOrchestratorEnded);
        }

        [Fact]
        public void OrchestrationProjectSolveTest()
        {
            string byString = "ByString";
            string byInt = "ByInt";

            bool stringOrchestratorEnded = false;
            bool intOrchestratorEnded = false;

            EquationProject project = new EquationProject()
            {
                Equations = new List<Equation>()
                {
                     new Equation()
                     {
                         Expression = "false",
                         Target = byString,
                         UseExpression = "true"
                     },
                     new Equation()
                     {
                         Expression = "false",
                         Target = byInt,
                         UseExpression = "true"
                     }
                }
            };

            IOrchestration orchestration = new Orchestration(project);

            IOrchestrator<string> stringOrchestrator = new Orchestrator<string>(byString);
            stringOrchestrator.OrchestratorEnded += delegate (object sender, EventArgs args)
            {
                stringOrchestratorEnded = true;
            };

            IOrchestrator<int> intOrchestrator = new Orchestrator<int>(byInt);
            intOrchestrator.OrchestratorEnded += delegate (object sender, EventArgs args)
            {
                intOrchestratorEnded = true;
            };

            orchestration.SetOrchestrator(stringOrchestrator);
            orchestration.SetOrchestrator(intOrchestrator);

            orchestration.SolveEquations();

            Thread.Sleep(1000);

            Assert.True(stringOrchestratorEnded);
            Assert.True(intOrchestratorEnded);
        }
    }
}
