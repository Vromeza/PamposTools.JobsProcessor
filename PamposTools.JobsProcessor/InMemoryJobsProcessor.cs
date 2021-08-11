using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PamposTools.JobsProcessor
{
    /// <summary>
    /// In-memory implementation of <see cref="IJobsProcessor{T}"/> which uses a <see cref="BlockingCollection{T}"/> with a <see cref="ConcurrentQueue{T}"/> as the underlying collection to handle queueing/dequeing of jobs.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InMemoryJobsProcessor : IJobsProcessor
    {
        private readonly BlockingCollection<IJob> _jobs;
        private readonly Func<IJob, bool> _processDelegate;
        private readonly Thread _thread;
        private readonly int _enqueueTimeoutMs = -1; //infinite

        /// <summary>
        /// Initializes an instance of <see cref="InMemoryJobsProcessor"/> with the specified process delegate and has the specified max capacity
        /// </summary>
        /// <param name="processDelegate">The action to be called when executing the job</param>
        public InMemoryJobsProcessor(Func<IJob, bool> processDelegate)
        {
            _processDelegate = processDelegate;
            _thread = new Thread(ProcessJobs) { IsBackground = true };
            _jobs = _jobs ?? new BlockingCollection<IJob>();
        }

        /// <summary>
        /// Initializes an instance of <see cref="InMemoryJobsProcessor"/> with the specified process delegate and has the specified max capacity
        /// </summary>
        /// <param name="processDelegate">The action to be called when executing the job</param>
        /// <param name="maxCapacity">The bounded capacity for number of jobs that can be enqueued</param>
        public InMemoryJobsProcessor(Func<IJob, bool> processDelegate, int maxCapacity) : this(processDelegate)
        {
            _jobs = new BlockingCollection<IJob>(maxCapacity);
        }

        /// <summary>
        /// Initializes an instance of <see cref="InMemoryJobsProcessor"/> with the specified process delegate, max capacity and insertion timeout in miliseconds
        /// </summary>
        /// <param name="processDelegate"></param>
        /// <param name="maxCapacity"></param>
        /// <param name="insertTimeout">The insert timeout in ms. If the job cannot be inserted in the specified amount of time, it will be ignored</param>
        public InMemoryJobsProcessor(Func<IJob, bool> processDelegate, int maxCapacity, int insertTimeout):this(processDelegate, maxCapacity)
        {
            _enqueueTimeoutMs = insertTimeout;
        }

        /// <summary>
        /// Adds a job into the <see cref="BlockingCollection{T}"/>
        /// </summary>
        /// <param name="job"></param>
        public bool Insert(IJob job) => _jobs.TryAdd(job, _enqueueTimeoutMs);

        /// <summary>
        /// Calls the action to run the job
        /// </summary>
        /// <param name="job"></param>
        public virtual bool Process(IJob job) => _processDelegate(job);

        /// <summary>
        /// Starts jobs processing
        /// </summary>
        public void StartProcessing()
        {
            _thread.Start();
        }

        /// <summary>
        /// Stops job processing
        /// </summary>
        public void StopProcessing()
        {
            _jobs.CompleteAdding();
        }

        /// <summary>
        /// Retrieves the jobs pending to be processed
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IJob> GetPendingJobs()
        {
            return _jobs.AsEnumerable();
        }

        private void ProcessJobs()
        {
            foreach (var job in _jobs.GetConsumingEnumerable(CancellationToken.None)) {
                Process(job);
            }
        }
    }
}
