using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PamposTools.JobsProcessor
{
    /// <summary>
    /// In-memory implementation of <see cref="IJobsProcessor{T}"/>. Uses a <see cref="BlockingCollection{T}"/> with a <see cref="ConcurrentQueue{T}"/> as the underlying collection to handle queueing/dequeing of jobs.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InMemoryJobsProcessor : IJobsProcessor
    {
        private readonly BlockingCollection<IJob> _jobs;
        private readonly Func<IJob, bool> _onExecuteCallback;
        private readonly Thread _thread;
        private readonly int _enqueueTimeoutMs = -1; //infinite

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processCallback">The action to be called when executing the job</param>
        /// <param name="maxCapacity">The max number of jobs that can be enqueued</param>
        public InMemoryJobsProcessor(Func<IJob, bool> processCallback, int maxCapacity)
        {
            _onExecuteCallback = processCallback;
            _jobs = _jobs ?? new BlockingCollection<IJob>(maxCapacity);
            _thread = new Thread(ProcessJobs) { IsBackground = true };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processCallback"></param>
        /// <param name="maxCapacity"></param>
        /// <param name="insertTimeout">The insert timeout in ms. If the job cannot be inserted in the specified amount of time, it will be ignored</param>
        public InMemoryJobsProcessor(Func<IJob, bool> processCallback, int maxCapacity, int insertTimeout) :this(processCallback, maxCapacity)
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
        public virtual bool Process(IJob job) => _onExecuteCallback(job);

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
