using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PamposTools.JobsProcessor
{
    /// <summary>
    /// Abstraction on a job
    /// </summary>
    public interface IJob
    {
        public string Name { get; }
        JobPriority Priority { get; }
    }
}
