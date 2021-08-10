using System;
using System.Threading;

namespace PamposTools.JobsProcessor.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var jobProcessor = new InMemoryJobsProcessor(DoSomeWork, 10);
            jobProcessor.StartProcessing();
            for (int i = 0; i < 100; i++)
                jobProcessor.Insert(new Job($"Job {i+1}"));

            System.Console.ReadLine();
        }

        private static bool DoSomeWork(IJob job)
        {
            System.Console.Write($"Processing {job.Name} - ");
            Thread.Sleep(100);
            System.Console.WriteLine($"Processed {job.Name}");
            return true;
        }
    }
}
