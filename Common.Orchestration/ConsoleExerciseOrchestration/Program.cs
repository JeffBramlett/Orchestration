using Common.Orchestration;
using System;
using System.Collections.Generic;
using EquationSolver;
using EquationSolver.Dto;

namespace ConsoleExerciseOrchestration
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Orchestrator Integration Tests");

            VariableProvider variables = new VariableProvider()
            {
                
            };

            variables.SetVariable("test", 1);

            var project = MakeDemoProject();
            var solver = EquationSolverFactory.Instance.CreateEquationSolver(project, variables);

            Orchestrator<string> orchestrator = new Orchestrator<string>("Demo", TimeSpan.FromSeconds(1), TimeSpan.FromMinutes(4.5), solver);

            orchestrator.ScheduledTimeReached += Orchestrator_ScheduledItemReturned;
            orchestrator.ScheduledItemCompleted += Orchestrator_ScheduledItemCompleted;
            orchestrator.OrchestratorEnded += Orchestrator_OrchestratorEnded;
            orchestrator.Start();

            ScheduleItem<string> triggeredItem = new ScheduleItem<string>()
            {
                TriggerVariableName = "test",
                Item = "Triggered",
                Interval = TimeSpan.MaxValue
            };
            orchestrator.ScheduleItem("Scheduled Item 1 (triggered)", TimeSpan.MinValue, TimeSpan.FromSeconds(15), TimeSpan.FromMinutes(.8), "test");
            orchestrator.ScheduleItem("Scheduled Item 2", TimeSpan.FromSeconds(45), TimeSpan.FromSeconds(15), TimeSpan.FromMinutes(1.5));

            solver.SolveEquations();

            Console.WriteLine("Press any key to quit");
            Console.ReadKey();
        }

        private static void Orchestrator_OrchestratorEnded(object sender, EventArgs e)
        {
            Console.WriteLine("Orchestrator ended");
        }

        private static void Orchestrator_ScheduledItemCompleted(object sender, EventArgs e)
        {
            var si = e as ScheduledItemCompletedEventArgs<string>;
            Console.WriteLine("{0} completed at {1}", si.ScheduleItem.Item, si.Timestamp);
        }

        private static void Orchestrator_ScheduledItemReturned(object sender, EventArgs e)
        {
            if (e is ScheduledItemTimeReachedEventArgs<string>)
            {
                var sitrea = e as ScheduledItemTimeReachedEventArgs<string>;
                Console.WriteLine("{0} Scheduled Item returned, name = {1}", sitrea.Timestamp, sitrea.OrchestratedItem);
            }
        }

        private static EquationProject MakeDemoProject()
        {
            EquationProject proj = new EquationProject()
            {
                Equations = new List<Equation>()
                {
                    new Equation()
                    {
                        Name = "Test Setting",
                        Expression = "1",
                        Target = "test",
                        UseExpression = "true"
                    }
                },
                Variables = new List<Variable>(),
                Functions = new List<Function>(),
                Tables = new List<Table>(),
                Audit = new AuditInfo()
                {
                    CreatedBy = "Demo",
                    CreatedOn = DateTime.Now,
                    ModifiedBy = "Demo",
                    ModifiedOn = DateTime.Now
                },
                Settings = new SolverSettings()
                {
                    CalculationMethod = CalculationMethods.Decimal
                },
                Title = "Demo project"
            };

            return proj;
        }
    }
}
