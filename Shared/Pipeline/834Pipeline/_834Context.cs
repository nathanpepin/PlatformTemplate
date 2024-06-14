
namespace Shared;

public sealed class _834Context : IContext<_834Context>
{
    public List<ContextBehavior<_834Context>> ContextBehaviors { get; } = [];

    public static bool IsAcceptedFormat<TInput>(TInput input)
    {
        return input switch
        {
            FileInfo _ => true,
            string _ => true,
            _ => false
        };
    }

    public static Task<(_834Context context, string inputName)> Import<TInput>(TInput input, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public static Task<TInput> Export<TInput>(TInput fileInfo, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public static Task Setup<TInput>(CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }

    public static Task Cleanup<TInput>(CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }

    public Exception? Exception { get; set; }
    public bool IsSuccess { get; set; }
    public List<StatusMessage> StatusMessages { get; } = [];
}