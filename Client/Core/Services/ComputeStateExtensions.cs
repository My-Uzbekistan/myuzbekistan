﻿namespace Client.Core.Services;

public static class ComputeStateExtensions
{
    internal static TableResponse<T> GetValue<T>(this IState<TableResponse<T>> state
                                         , UInjector _injector)
    where T : class
    {
        _ = state ?? throw new ArgumentNullException(nameof(state));
        _ = _injector.NavigationManager ?? throw new ArgumentNullException(nameof(_injector.NavigationManager));
        _ = _injector.PageHistoryState ?? throw new ArgumentNullException(nameof(_injector.PageHistoryState));

        if (state.Error is not null)
        {
            state.Error.HandleExceptions(_injector);
            return null!;
        }

        var value = state.LastNonErrorValue;

        if (value == null)
        {
            if (typeof(TableResponse<T>).GetConstructor(Type.EmptyTypes) == null)
                throw new InvalidOperationException("Type T does not have a parameterless constructor.");

            return (TableResponse<T>)Activator.CreateInstance(typeof(TableResponse<T>));
        }

        return value;
    }

    internal static T GetValue<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(this IComputedState<T> state,
                                                      UInjector _injector)
    where T : notnull
    {
        _ = state ?? throw new ArgumentNullException(nameof(state));
        _ = _injector.NavigationManager ?? throw new ArgumentNullException(nameof(_injector.NavigationManager));
        _ = _injector.PageHistoryState ?? throw new ArgumentNullException(nameof(_injector.PageHistoryState));

        if (state.Error is not null)
        {
            state.Error.HandleExceptions(_injector);
            return default!;
        }

        var value = state.LastNonErrorValue;

        if (value == null)
        {
            if (typeof(T).GetConstructor(Type.EmptyTypes) == null)
                throw new InvalidOperationException("Type T does not have a parameterless constructor.");

            return (T)Activator.CreateInstance(typeof(T));
        }

        return value;
    }

    internal static T GetValue<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(this ComputedState<T> state,
                                                      UInjector _injector)
    where T : notnull
    {
        _ = state ?? throw new ArgumentNullException(nameof(state));
        _ = _injector.NavigationManager ?? throw new ArgumentNullException(nameof(_injector.NavigationManager));
        _ = _injector.PageHistoryState ?? throw new ArgumentNullException(nameof(_injector.PageHistoryState));

        if (state.Error is not null)
        {
            state.Error.HandleExceptions(_injector);
            return default!;
        }

        var value = state.LastNonErrorValue;

        if (value == null)
        {
            if (typeof(T).GetConstructor(Type.EmptyTypes) == null)
                throw new InvalidOperationException("Type T does not have a parameterless constructor.");

            return (T)Activator.CreateInstance(typeof(T));
        }

        return value;
    }



    internal static void HandleExceptions(this Exception? exception, UInjector _injector)
    {
        if (exception == null)
        {
            return;
        }

        var previousUrl = string.Join("/", _injector.PageHistoryState.GetPreviousPage().Split('/').Skip(3));
        if (string.IsNullOrEmpty(previousUrl) || previousUrl.Contains("error"))
        {
            previousUrl = "/";
        }

        _injector.Exception = exception;
        _injector.BackUrl = previousUrl;
        try
        {
            _injector.NavigationManager.NavigateTo($"/error/{exception.GetStatusCode()}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }

    public static string GetStatusCode(this Exception exception)
    {
        if (exception is NotFoundException)
        {
            return "404";
        }

        if (exception is BadRequestException)
        {
            return "400";
        }

        if (exception is AccessViolationException)
        {
            return "403";
        }

        return "500";
    }
}