using Serilog;
using System.Runtime.ExceptionServices;
using Exception = System.Exception;

namespace WordListsMauiHelpers.Logging;
public class ExceptionHandler
{
    public ExceptionHandler(AppDomain domain, ILogger logger)
    {
        Domain = domain;
        Logger = logger;
        logger.Information("Added global exception handler");
    }

    ILogger Logger { get; }
    AppDomain Domain { get; }

    public void AddExceptionHandling()
    {
        Domain.FirstChanceException += FirstChanceException;
        Domain.UnhandledException += UnhandledException;
        HandleAndroidException();
        HandleWindowsExceptions();
    }

#pragma warning disable CA1822 // Mark members as static

    private void HandleWindowsExceptions()
    {
#if WINDOWS
        Microsoft.UI.Xaml.Application.Current.UnhandledException += (sender, args) =>
        {
            Exception ex = args.Exception;
            string exType = ex.GetType().Name;
            string exMessage = ex.Message;
            string trace = ex.StackTrace ?? "NULL";
            Logger.Error("'{sender}' threw exception '{ex}': '{msg}', caught with '{methodName}', see trace \n{trace}",
                sender?.GetType(), exType, exMessage, nameof(HandleAndroidException), trace);
        };
#endif
    }
    private void HandleAndroidException()
    {
#if ANDROID
        Android.Runtime.AndroidEnvironment.UnhandledExceptionRaiser += (sender, args) =>
        {
            Exception ex = args.Exception;
            string exType = ex.GetType().Name;
            string exMessage = ex.Message;
            string trace = ex.StackTrace ?? "NULL";
            Logger.Error("'{sender}' threw exception '{ex}': '{msg}', caught with '{methodName}', see trace \n{trace}",
                sender?.GetType(), exType, exMessage, nameof(HandleAndroidException), trace);
        };
#endif
    }

#pragma warning restore CA1822 // Mark members as static

    private void FirstChanceException(object? sender, FirstChanceExceptionEventArgs e)
    {
        Exception ex = e.Exception;
        string exType = ex.GetType().Name;
        string exMessage = ex.Message;
        string trace = ex.StackTrace ?? "NULL";


        Logger.Error("'{sender}' threw exception '{ex}': '{msg}', caught with '{methodName}', see trace \n{trace}",
            sender?.GetType(), exType, exMessage, nameof(FirstChanceException), trace);
    }
    private void UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        string exType;
        string exMessage;
        string trace;
        if (e.ExceptionObject is Exception ex)
        {
            exType = ex.GetType().Name;
            exMessage = ex.Message;
            trace = ex.StackTrace ?? "NULL";
        }
        else
        {

            exType = e.ExceptionObject.GetType().Name;
            exMessage = "NULL, cant get real exception from ExceptionObject";
            trace = "NULL";
        }
        Logger.Error("'{sender}' threw exception '{ex}': '{msg}', caught with '{methodName}', see trace \n{trace}",
            sender?.GetType(), exType, exMessage, nameof(UnhandledException), trace);
    }
}
