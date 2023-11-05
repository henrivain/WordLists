using Microsoft.Extensions.Logging;
using Serilog.Events;
using ISerilogger = Serilog.ILogger;


namespace WordListsMauiHelpers.Logging;

internal class SerilogWrapper : ILogger
{
    private readonly ISerilogger _logger;
    public SerilogWrapper(ISerilogger logger)
    {
        _logger = logger;
    }

#nullable disable
    public IDisposable BeginScope<TState>(TState state) => default;
#nullable enable

    public bool IsEnabled(LogLevel logLevel)
    {
        return _logger.IsEnabled(LogLevelToEventLevel(logLevel));
    }

    public void Log<TState>(
        LogLevel logLevel, EventId eventId,
        TState state, Exception? exception,
        Func<TState, Exception, string> formatter)
    {
        if (IsEnabled(logLevel) is false) return;
        _logger.Write(LogLevelToEventLevel(logLevel), exception, state?.ToString() ?? string.Empty);
    }

    private static LogEventLevel LogLevelToEventLevel(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Debug => LogEventLevel.Debug,
            LogLevel.Information => LogEventLevel.Information,
            LogLevel.Warning => LogEventLevel.Warning,
            LogLevel.Error => LogEventLevel.Error,
            LogLevel.Critical => LogEventLevel.Fatal,
            LogLevel.None => LogEventLevel.Verbose,
            LogLevel.Trace => LogEventLevel.Verbose,
            _ => LogEventLevel.Verbose
        };
    }

}
