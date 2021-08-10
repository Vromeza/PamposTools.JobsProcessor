namespace PamposTools.JobsProcessor
{
    /// <summary>
    /// Abstraction of a job
    /// </summary>
    public interface IJob
    {
        string Name { get; }
        JobPriority Priority { get; }
    }
}
