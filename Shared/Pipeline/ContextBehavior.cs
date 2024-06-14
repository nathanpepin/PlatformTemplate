namespace Shared;

public abstract class ContextBehavior<TContext>
{
    public abstract Task HandleAsync(TContext context, CancellationToken cancellationToken = default);

    public abstract string BehaviorName { get; }

}