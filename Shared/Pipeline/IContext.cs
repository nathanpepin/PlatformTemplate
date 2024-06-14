
using FluentResults;

namespace Shared;

public interface IContext<TContext> where TContext: new()
{
    public List<ContextBehavior<TContext>> ContextBehaviors { get; }

    static abstract bool IsAcceptedFormat<TInput>(TInput input);

    static abstract Task<(TContext context, string inputName)> Import<TInput>(TInput input, CancellationToken cancellationToken);

    static abstract Task<TInput> Export<TInput>(TInput input, CancellationToken cancellationToken);

    static abstract Task Setup<TInput>(CancellationToken cancellation = default);

    static abstract Task Cleanup<TInput>(CancellationToken cancellation = default);

    Exception? Exception { get; set; }
    public bool IsSuccess { get; set; }

    public List<StatusMessage> StatusMessages { get; }

    public string Messages => GetFromStatus();
    public string Successes => GetFromStatus(Status.Success);
    public string Warnings => GetFromStatus(Status.Warning);
    public string Failures => GetFromStatus(Status.Failure);

    public string GetFromStatus(Status? status = null)
    {
        var query = (status is null
                ? StatusMessages
                : StatusMessages
                    .Where(x => x.Status == status.Value))
            .OrderBy(x => x.Logged)
            .Select(x => x.ToString());

        return string.Join(Environment.NewLine, query);
    }
}