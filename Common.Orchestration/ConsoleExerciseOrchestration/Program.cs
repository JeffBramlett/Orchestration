using Common.Orchestration;
using System;
using System.Collections.Generic;
using System.Threading;
using EquationSolver;
using EquationSolver.Dto;

namespace ConsoleExerciseOrchestration
{
    class Program
    {
        static void Main(string[] args)
        {
            OrchestratorTest();
            OrchestrationTest();

            Console.WriteLine("Press any key to quit");
            Console.ReadKey();
        }

        #region Orchestrator testing

        private static void OrchestratorTest()
        {
            Console.WriteLine("Orchestrator Tests");

            //int event1Count = 0;
            //int eventCount2 = 0;
            //int eventCount3 = 0;

            //IOrchestrator<string> orch = new Orchestrator<string>("", TimeSpan.FromSeconds(1));
            //orch.ScheduledTimeReached += delegate(object sender, EventArgs args) { event1Count++;};
            //orch.ScheduledItemCompleted += delegate(object sender, EventArgs args) { eventCount2++; };
            //orch.OrchestratorEnded += delegate(object sender, EventArgs args) { eventCount3++;};
            //orch.Start();

            //orch.ScheduleItem("Test", DateTime.Now);

            //BusyWait(4 * 1000);

            //orch.Dispose();

            //Console.WriteLine(string.Format("Reached:{0}\tCompleted: {1}\tEnded:{2}", event1Count, eventCount2, eventCount3));
            //Console.WriteLine();
            //Console.WriteLine("Orchestrator Tests Ended");
            //Console.WriteLine("---------------------------------------------------");
            //Console.WriteLine();
        }
        #endregion

        #region Orchestration testing
        private static void OrchestrationTest()
        {
            Console.WriteLine("Orchestration Tests");

            //    VariableProvider variables = new VariableProvider()
            //    {

            //    };

            //    variables.SetVariable("test", 1);

            //    var project = MakeDemoProject();
            //    var solver = EquationSolverFactory.Instance.CreateEquationSolver(project, variables);

            //    Orchestrator<string> orchestrator = new Orchestrator<string>("Demo", TimeSpan.FromSeconds(1), TimeSpan.FromMinutes(4.5), solver);

            //    orchestrator.ScheduledTimeReached += Orchestrator_ScheduledItemReturned;
            //    orchestrator.ScheduledItemCompleted += Orchestrator_ScheduledItemCompleted;
            //    orchestrator.OrchestratorEnded += Orchestrator_OrchestratorEnded;
            //    orchestrator.Start();

            //    ScheduleItem<string> triggeredItem = new ScheduleItem<string>()
            //    {
            //        TriggerVariableName = "test",
            //        Item = "Triggered",
            //        Interval = TimeSpan.MaxValue
            //    };
            //    orchestrator.ScheduleItem("Scheduled Item 1 (triggered)", TimeSpan.MinValue, TimeSpan.FromSeconds(15), TimeSpan.FromMinutes(.8), "test");
            //    orchestrator.ScheduleItem("Scheduled Item 2", TimeSpan.FromSeconds(45), TimeSpan.FromSeconds(15), TimeSpan.FromMinutes(1.5));

            //    solver.SolveEquations();

            //    BusyWait(60 * 1000);

            //    orchestrator.Dispose();

            //    Console.WriteLine();
            //    Console.WriteLine("Orchestration Tests Ended");
            //    Console.WriteLine("---------------------------------------------------");
            //    Console.WriteLine();
        }

        //private static void Orchestrator_OrchestratorEnded(object sender, EventArgs e)
        //{
        //    Console.WriteLine("Orchestrator ended");
        //}

        //private static void Orchestrator_ScheduledItemCompleted(object sender, EventArgs e)
        //{
        //    var si = e as ScheduledItemCompletedEventArgs<string>;
        //    Console.WriteLine("{0} completed at {1}", si.ScheduleItem.Item, si.Timestamp);
        //}

        //private static void Orchestrator_ScheduledItemReturned(object sender, EventArgs e)
        //{
        //    if (e is ScheduledItemTimeReachedEventArgs<string>)
        //    {
        //        var sitrea = e as ScheduledItemTimeReachedEventArgs<string>;
        //        Console.WriteLine("{0} Scheduled Item returned, name = {1}", sitrea.Timestamp, sitrea.OrchestratedItem);
        //    }
        //}
        #endregion

        #region privates

        //private static void BusyWait(int milliseconds)
        //{
        //    DateTime end = DateTime.Now + TimeSpan.FromMilliseconds(milliseconds);
        //    while (end > DateTime.Now)
        //    {
        //        Thread.Sleep(500);
        //    }
        //}
        //private static EquationProject MakeDemoProject()
        //{
        //    EquationProject proj = new EquationProject()
        //    {
        //        Equations = new List<Equation>()
        //        {
        //            new Equation()
        //            {
        //                Name = "Test Setting",
        //                Expression = "1",
        //                Target = "test",
        //                UseExpression = "true"
        //            }
        //        },
        //        Variables = new List<Variable>(),
        //        Functions = new List<Function>(),
        //        Tables = new List<Table>(),
        //        Audit = new AuditInfo()
        //        {
        //            CreatedBy = "Demo",
        //            CreatedOn = DateTime.Now,
        //            ModifiedBy = "Demo",
        //            ModifiedOn = DateTime.Now
        //        },
        //        Settings = new SolverSettings()
        //        {
        //            CalculationMethod = CalculationMethods.Decimal
        //        },
        //        Title = "Demo project"
        //    };

        //    return proj;
        //}
        #endregion
    }
}
