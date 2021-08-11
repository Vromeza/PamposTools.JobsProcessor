using System;

namespace PamposTools.JobsProcessor
{
    /// <summary>
    /// Abstraction of a job
    /// </summary>
    public interface IJob
    {
        Guid Id { get; }
        JobPriority Priority { get; }
    }
}
