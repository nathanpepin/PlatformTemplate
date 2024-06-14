using Microsoft.Extensions.DependencyInjection;

namespace Shared;

public static class PipelineConfiguration
{
    public static void AddPipelineBehavior<TContext, TImpl>(this IServiceCollection services) 
        where TContext: IContext<TContext>
        where TImpl : class, ContextBehavior<TContext>
    {
        services.AddScoped<ContextBehavior<TContext>, TImpl>();
    }
}