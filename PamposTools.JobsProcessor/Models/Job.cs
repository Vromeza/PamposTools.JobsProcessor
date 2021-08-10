using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PamposTools.JobsProcessor
{
    /// <summary>
    /// An abstraction for a job object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Job : IJob
    {
        public Guid Id => Guid.NewGuid();
        public string Name { get; private set; }
        public JobPriority Priority { get; private set; }

        public Job(string name, JobPriority priority = JobPriority.Medium)
        {
            Name = name;
            Priority = priority;
        }
    }
}
