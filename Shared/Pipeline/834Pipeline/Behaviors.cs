using System.Collections.Immutable;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Pipeline;

namespace Shared;

public sealed class P1 : ContextBehavior<_834Context>
{
    public override Task HandleAsync(_834Context context, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override string BehaviorName => nameof(P1);
}

public sealed class P2 : ContextBehavior<_834Context>
{
    public override Task HandleAsync(_834Context context, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override string BehaviorName => nameof(P2);

}

public sealed class P3 : ContextBehavior<_834Context>
{
    public override Task HandleAsync(_834Context context, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override string BehaviorName => nameof(P3);
}