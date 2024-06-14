using System.Collections.Immutable;
using Shared.Pipeline;

namespace Shared;

public sealed class _834PipelineOptions : IPipelineExecutorOptions
{
    public bool CleanupEnabled { get; init; }
}

public sealed class _834Pipeline(IServiceProvider serviceProvider)
    : PipelineExecutor<_834Context, _834PipelineOptions>(serviceProvider)
{
    protected override ImmutableArray<ContextBehavior<_834Context>> GetBehaviors()
    {
        throw new NotImplementedException();
    }
}