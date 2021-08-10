namespace PamposTools.JobsProcessor
{
    /// <summary>
    /// Abstraction on a job
    /// </summary>
    public interface IJob
    {
        string Name { get; }
        JobPriority Priority { get; }
    }
}
