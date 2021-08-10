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
        public void JobProcess_Prevents_Processing_IfStopped()
        {
            IJobsProcessor processor = new InMemoryJobsProcessor(DummyProcess, 10);
            Job testJob = new Job("Test");
            processor.StartProcessing();
            for (int i=0; i<=10; i++) {
                if (i == 5)
                    processor.StopProcessing();
            }
            processor.Insert(testJob);
        }

        private bool DummyProcess(IJob job)
        {
            return false;
        }
    }
}
