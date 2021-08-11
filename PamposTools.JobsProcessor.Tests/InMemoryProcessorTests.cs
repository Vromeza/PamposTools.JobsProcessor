using System;
using System.Linq;
using System.Threading;
using Xunit;

namespace PamposTools.JobsProcessor.Tests
{
    public class InMemoryProcessorTests
    {
        [Fact]
        public void JobProcessor_Is_Not_Running_OnCreate()
        {
            bool processed = false;
            IJobsProcessor processor = new InMemoryJobsProcessor((job) => processed = true, 10);
            Job testJob = new Job("Test");
            processor.Insert(testJob);
            Assert.False(processed);
        }

        [Fact]
        public void JobProcessor_Is_Running_When_Started()
        {
            bool processed = false;
            IJobsProcessor processor = new InMemoryJobsProcessor((job) => processed = true, 10);
            Job testJob = new Job("Test");
            processor.StartProcessing();
            processor.Insert(testJob);
            Thread.Sleep(1000); //give some time for thread to process the event
            Assert.True(processed);
        }

        [Fact]
        public void JobProcessor_MaxCapacity_Prevents_Enqueue()
        {
            IJobsProcessor processor = new InMemoryJobsProcessor(DummyProcess, 1, 100); //specify enqueue timeout
            Job testJob = new Job("Test");
            processor.Insert(testJob);
            bool insertSuccess = processor.Insert(testJob);
            Assert.False(insertSuccess);
        }

        [Fact]
        public void JobProcessor_Throws_InvalidOperationException_IfStopped()
        {
            IJobsProcessor processor = new InMemoryJobsProcessor(DummyProcess, 10);
            processor.StartProcessing();
            Job testJob = new Job($"Test");
            processor.StopProcessing();
            Assert.Throws<InvalidOperationException>(() => processor.Insert(testJob));
        }

        [Fact] 
        public void JobProcessor_Continues_Processing_Remaining_Jobs_AfterStopped()
        {
            IJobsProcessor processor = new InMemoryJobsProcessor(DummyProcess, 10);
            Job testJob = new Job($"Test");
            processor.StartProcessing();
            for (int i = 0; i < 15; i++)
                processor.Insert(testJob);
            processor.StopProcessing();
            Thread.Sleep(1000); //let processor do its work
            int remainingJobs = processor.GetPendingJobs().Count();
            Assert.Equal(0, remainingJobs);
        }

        private bool DummyProcess(IJob job)
        {
            return false;
        }
    }
}
