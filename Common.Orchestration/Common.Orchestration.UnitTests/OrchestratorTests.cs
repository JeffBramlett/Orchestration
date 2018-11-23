using System;
using System.Threading;
using Xunit;

namespace Common.Orchestration.UnitTests
{
    public class OrchestratorTests
    {
        [Fact]
        public void OrchestrateCompleteTest()
        {
            var now = DateTime.Now;

            int cnt = 0;
            Orchestrator<string> orchestrator = new Orchestrator<string>(TimeSpan.FromSeconds(10));
            orchestrator.ScheduledItemCompleted += delegate (object sender, EventArgs args)
            {
                cnt++;
            };
            orchestrator.ScheduleItem("Something", TimeSpan.MinValue, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10));

            now = DateTime.Now;
            orchestrator.Start();

            DateTime stop = DateTime.Now + TimeSpan.FromSeconds(11);
            while( DateTime.Now < stop)
            {
                Thread.Sleep(100);
            }

            Assert.Equal(1, cnt);
        }

        [Fact]
        public void OrchestrateTimeCompleteTest()
        {
            var now = DateTime.Now;

            int cnt = 0;
            Orchestrator<string> orchestrator = new Orchestrator<string>(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(20));
            orchestrator.ScheduledTimeReached += delegate (object sender, EventArgs args)
            {
                cnt++;
            };
            orchestrator.ScheduleItem("Something", TimeSpan.MinValue, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10));

            now = DateTime.Now;
            orchestrator.Start();

            DateTime stop = DateTime.Now + TimeSpan.FromSeconds(11);
            while (DateTime.Now < stop)
            {
                Thread.Sleep(100);
            }

            orchestrator.Stop();

            Assert.Equal(10, cnt);
        }

    }
}
