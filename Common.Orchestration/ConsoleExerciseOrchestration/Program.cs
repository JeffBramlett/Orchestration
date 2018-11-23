using Common.Orchestration;
using System;

namespace ConsoleExerciseOrchestration
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Orchestrator Integration Tests");


            Orchestrator<string> orchestrator = new Orchestrator<string>(TimeSpan.FromSeconds(1), TimeSpan.FromMinutes(1.5));

            orchestrator.ScheduledTimeReached += Orchestrator_ScheduledItemReturned;
            orchestrator.ScheduledItemCompleted += Orchestrator_ScheduledItemCompleted;
            orchestrator.OrchestratorEnded += Orchestrator_OrchestratorEnded;
            orchestrator.Start();

            orchestrator.ScheduleItem("Scheduled Item", TimeSpan.MinValue, TimeSpan.FromSeconds(15), TimeSpan.FromMinutes(.8));

            Console.ReadKey();
        }

        private static void Orchestrator_OrchestratorEnded(object sender, EventArgs e)
        {
            Console.WriteLine("Orchestrator ended");
        }

        private static void Orchestrator_ScheduledItemCompleted(object sender, EventArgs e)
        {
            Console.WriteLine("Scheduled item completed");
        }

        private static void Orchestrator_ScheduledItemReturned(object sender, EventArgs e)
        {
            if (e is ScheduledItemTimeReachedEventArgs<string>)
            {
                var sitrea = e as ScheduledItemTimeReachedEventArgs<string>;
                Console.WriteLine("{0} Scheduled Item returned, name = {1}", sitrea.Timestamp, sitrea.OrchestratedItem);
            }
        }
    }
}
