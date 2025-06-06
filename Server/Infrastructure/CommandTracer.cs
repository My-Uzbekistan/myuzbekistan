﻿using ActualLab.CommandR.Commands;
using ActualLab.CommandR.Configuration;
using ActualLab.CommandR.Internal;
using ActualLab.CommandR;
using ActualLab.DependencyInjection;
using ActualLab.Diagnostics;
using System.Diagnostics;
using OpenTelemetry.Trace;
using ActualLab.CommandR.Diagnostics;
using System.Xml.Linq;

namespace Server.Infrastructure;

public class CommandTracer(IServiceProvider services) : ICommandHandler<ICommand>
{
    private ILogger? _log;

    protected IServiceProvider Services { get; } = services;

    protected ILogger Log
    {
        get => _log ??= Services.LogFor(GetType());
        init => _log = value;
    }

    public LogLevel ErrorLogLevel { get; init; } = LogLevel.Error;

    [CommandFilter(Priority = CommanderCommandHandlerPriority.CommandTracer)]
    public async Task OnCommand(ICommand command, CommandContext context, CancellationToken cancellationToken)
    {
        if (!ShouldTrace(command, context))
        {
            await context.InvokeRemainingHandlers(cancellationToken).ConfigureAwait(false);
            return;
        }

        using var activity = StartActivity(command, context);
        try
        {
            await context.InvokeRemainingHandlers(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            if (activity == null)
                throw;

            activity.RecordException(e);
            activity.SetStatus(Status.Error);
            activity.Finalize(e, cancellationToken);
            activity?.SetParentId(ActivityTraceId.CreateRandom().ToString());
            var message = context.IsOutermost ?
                "Outermost command failed: {Command}" :
                "Nested command failed: {Command}";
            // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
            Log.IfEnabled(LogLevel.Error)?.Log(ErrorLogLevel, e, message, command);
            throw;
        }
    }

    // Protected methods

    protected virtual bool ShouldTrace(ICommand command, CommandContext context)
    {
        // Always trace top-level commands
        if (context.IsOutermost)
            return true;

        // Do not trace system commands & any nested command they run
        for (var c = context; c != null; c = c.OuterContext)
            if (c.UntypedCommand is ISystemCommand)
                return false;

        // Trace the rest
        return true;
    }

    protected virtual Activity? StartActivity(ICommand command, CommandContext context)
    {
        var operationName = command.GetOperationName();
        using var activitySource = CommanderInstruments.ActivitySource.StartActivity(operationName); 
       

        if (activitySource != null)
        {
            var tags = new ActivityTagsCollection { { "command", command.ToString() } };
            var activityEvent = new ActivityEvent(operationName, tags: tags);
            activitySource.AddEvent(activityEvent);
        }
        return activitySource;
    }
}
