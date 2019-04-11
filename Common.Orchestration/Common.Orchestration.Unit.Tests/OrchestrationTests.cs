using System;
using System.Collections.Generic;
using System.Threading;
using EquationSolver.Dto;
using Moq;
using Xunit;

namespace Common.Orchestration.Unit.Tests
{
    public class OrchestrationTests
    {
        [Fact]
        public void SetOrchestratorTest()
        {
            Mock<IOrchestratorRepository<string>> mockStringRepo = new Mock<IOrchestratorRepository<string>>();
            Mock<IOrchestratorRepository<int>> mockIntRepo = new Mock<IOrchestratorRepository<int>>();

            IOrchestration orchestration = new Orchestration();

            bool stringOrchestratorEnded = false;
            bool intOrchestratorEnded = false;

            IOrchestrator<string> stringOrchestrator = new Orchestrator<string>("ByString", mockStringRepo.Object);
            stringOrchestrator.OrchestratorEnded += delegate(object sender, EventArgs args)
            {
                stringOrchestratorEnded = true;
            };

            IOrchestrator<int> intOrchestrator = new Orchestrator<int>("ByInt", mockIntRepo.Object);
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
            Mock<IOrchestratorRepository<string>> mockStringRepo = new Mock<IOrchestratorRepository<string>>();
            Mock<IOrchestratorRepository<int>> mockIntRepo = new Mock<IOrchestratorRepository<int>>();

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

            IOrchestrator<string> stringOrchestrator = new Orchestrator<string>(byString, mockStringRepo.Object);
            stringOrchestrator.OrchestratorEnded += delegate (object sender, EventArgs args)
            {
                stringOrchestratorEnded = true;
            };

            IOrchestrator<int> intOrchestrator = new Orchestrator<int>(byInt, mockIntRepo.Object);
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

        [Fact]
        public void TestOnlyOnceInOrchestrator()
        {
            Mock<IOrchestratorRepository<string>> mockStringRepo = new Mock<IOrchestratorRepository<string>>();
            Mock<IOrchestratorRepository<int>> mockIntRepo = new Mock<IOrchestratorRepository<int>>();

            int count1 = 0;
            int count2 = 0;
            IOrchestrator<string> stringOrchestrator = new Orchestrator<string>("not relevant", mockStringRepo.Object);
            stringOrchestrator.ScheduledTimeReached += delegate(object sender, EventArgs args)
            {
                count1++;
            };
            stringOrchestrator.ScheduledItemCompleted += delegate(object sender, EventArgs args)
            {
                count2++;
            };

            stringOrchestrator.Start();

            ScheduleItem<string> sched = new ScheduleItem<string>()
            {
                Item = "stuff",
                MaxOccurrances = 1,
                StartDateTime = DateTime.Now
            };

            stringOrchestrator.ScheduleItem(sched);

            BusyWait(10000);

            Assert.True(count1 == 1);
            Assert.True(count2 == 1);
        }

        private void BusyWait(int milliseconds)
        {
            DateTime end = DateTime.Now + TimeSpan.FromMilliseconds(milliseconds);
            while (DateTime.Now < end)
            {
                Thread.Sleep(100);
            }
        }
    }
}
