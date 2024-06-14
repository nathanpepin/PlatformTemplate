namespace Shared.Pipeline;

public interface IPipelineExecutorOptions
{
    public bool SetupEnabled { get; init; }

    public bool CleanupEnabled { get; init; }
}