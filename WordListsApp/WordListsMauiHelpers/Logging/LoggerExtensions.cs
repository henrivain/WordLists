using Serilog;

namespace WordListsMauiHelpers.Logging;
public static class LoggerExtensions
{
    /// <summary>
    /// Get serilogger as Microsoft ILogger
    /// </summary>
    /// <param name="logger"></param>
    /// <returns>serilogger wrapped in microsoft ilogger</returns>
    public static Microsoft.Extensions.Logging.ILogger AsMicrosoftLogger(this ILogger logger)
    {
        return new SerilogWrapper(logger);
    }
}
