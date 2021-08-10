using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PamposTools.JobsProcessor
{
    public interface IJobsProcessor
    {
        /// <summary>
        /// Starts job processing
        /// </summary>
        void StartProcessing();

        /// <summary>
        /// Stops job processing
        /// </summary>
        void StopProcessing();

        /// <summary>
        /// Processes a single <see cref="IJob"/>.
        /// </summary>
        /// <param name="job"></param>
        /// <returns>True/false on whether the job was processed succesfully</returns>
        bool Process(IJob job);

        /// <summary>
        /// Inserts a job into the processor
        /// </summary>
        /// <param name="job"></param>
        /// <returns>True/false on whether the job was inserted in the processor succesfully</returns>
        bool Insert(IJob job);

        /// <summary>
        /// Retrieves the jobs in the job pool
        /// </summary>
        /// <returns></returns>
        IEnumerable<IJob> GetPendingJobs();
    }
}
