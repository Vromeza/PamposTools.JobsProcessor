using System;

namespace PamposTools.JobsProcessor
{
    /// <summary>
    /// An abstraction for a job object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Job : IJob
    {
        /// <summary>
        /// Auto-generated Guid of the job
        /// </summary>
        public Guid Id => Guid.NewGuid();
        public string Name { get; private set; }
        public JobPriority Priority { get; private set; }

        /// <summary>
        /// Initializes an instance of a <see cref="Job"/> specifying the name and optionally specifying the <see cref="JobPriority"/>
        /// </summary>
        /// <param name="name">Name of the job</param>
        /// <param name="priority">Priority of the job</param>
        public Job(string name, JobPriority priority = JobPriority.Medium)
        {
            Name = name;
            Priority = priority;
        }
    }
}
