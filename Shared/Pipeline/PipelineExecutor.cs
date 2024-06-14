using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Reflection.Emit;
using FluentResults;
using Microsoft.Extensions.Options;

namespace Shared.Pipeline;

public abstract class PipelineExecutor<TContext, TOptions>(IServiceProvider services)
    where TContext : IContext<TContext>, new()
    where TOptions : class, IPipelineExecutorOptions
{
    protected abstract ImmutableArray<ContextBehavior<TContext>> GetBehaviors();

    public Task<TContext> InvokeAsync<TInput>(TInput input, CancellationToken cancellationToken = default)
    {
        return InvokeAsync(input, GetBehaviors(), cancellationToken);
    }

    public async Task<TContext> InvokeAsync<TInput>(TInput input, ImmutableArray<ContextBehavior<TContext>> behaviors, CancellationToken cancellationToken = default)
    {
        var logger = services.GetRequiredService<ILogger<PipelineExecutor<TContext, TOptions>>>();
        var options = services.GetRequiredService<IOptionsMonitor<TOptions>>().CurrentValue;

        var result = await Result.Try(() => TContext.Import(input, cancellationToken));

        var (context, inputName) = result.IsSuccess
            ? result.Value
            : (new TContext(), null);

        if (!context.IsSuccess || inputName is null)
        {
            var errorInput = input switch
            {
                string s => s,
                FileInfo fi => fi.Name,
                _ => typeof(TInput).Name
            };

            HandleError(context, logger, "Import", "Failed to import input", errorInput);
            return context;
        }

        if (behaviors.Length == 0)
        {
            HandleError(context, logger, "Pre-startup", "There were no behaviors to apply", inputName);
            return context;
        }

        if (options.SetupEnabled)
        {
            try
            {
                await TContext.Setup<TInput>(cancellationToken);
            }
            catch (Exception exception)
            {
                HandleError(context, logger, "Setup", inputName, exception);
                return context;
            }
        }

        logger.LogInformation("Running pipeline for {inputName}", inputName);

        foreach (var behavior in behaviors)
        {
            var stopwatch = Stopwatch.StartNew();
            logger.LogInformation("Applying {BehaviorName} to {inputName}", behavior.BehaviorName, inputName);

            context.ContextBehaviors.Add(behavior);

            try
            {
                await behavior.HandleAsync(context, cancellationToken);
            }
            catch (Exception exception)
            {
                HandleError(context, logger, inputName, behavior, exception);
            }
            finally
            {
                var elapsed = stopwatch.Elapsed;
                stopwatch.Stop();

                logger.LogDebug("Pipeline behavior {BehaviorName} completed in {Time}", behavior.BehaviorName, elapsed);

                if (elapsed.TotalSeconds > 30)
                    logger.LogWarning("Pipeline behavior {BehaviorName} took longer than expected to complete", behavior.BehaviorName);
            }
        }

        if (!options.CleanupEnabled) return context;
        {
            try
            {
                await TContext.Cleanup<TInput>(cancellationToken);
            }
            catch (Exception exception)
            {
                HandleError(context, logger, "Cleanup",  inputName, exception);
            }
        }

        return context;
    }

    private static void HandleError(TContext context, ILogger<PipelineExecutor<TContext, TOptions>> logger, string inputName, ContextBehavior<TContext> behavior, Exception exception)
    {
        context.IsSuccess = false;
        context.Exception = exception;
        context.StatusMessages.Add(new StatusMessage(Status.Failure, exception.Message, DateTime.Now));
        logger.LogError(exception, "Failed to apply {BehaviorName} to {inputName} due to reason: {Message}", behavior.BehaviorName, inputName, exception.Message);
    }

    private static void HandleError(TContext context, ILogger<PipelineExecutor<TContext, TOptions>> logger, string task,
        string inputName, Exception exception)
    {
        context.IsSuccess = false;
        context.Exception = exception;
        context.StatusMessages.Add(new StatusMessage(Status.Failure, exception.Message, DateTime.Now));
        logger.LogError(exception, "Failed to complete task {task} for {inputName} due to reason: {Message}", task, inputName, exception.Message);
    }

    private static void HandleError(TContext context, ILogger<PipelineExecutor<TContext, TOptions>> logger, string task, string inputName, string message)
    {
        context.IsSuccess = false;
        context.StatusMessages.Add(new StatusMessage(Status.Failure, message, DateTime.Now));
        logger.LogError("Failed to complete task {task} for {inputName} due to reason: {message}", task, inputName, message);
    }
}