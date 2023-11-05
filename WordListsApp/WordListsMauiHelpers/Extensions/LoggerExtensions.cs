namespace WordListsMauiHelpers.Extensions;
public static class LoggerExtensions
{
    /// <summary>
    /// Log exception using template "{msg}, '{ex}': '{exMsg}'.".
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="message"></param>
    /// <param name="ex"></param>
    /// <param name="mode"></param>
    public static void LogException(this ILogger logger, string message, Exception ex, LogMode mode = LogMode.Warning)
    {
        switch (mode)
        {
            case LogMode.Warning:
                logger.LogWarning("{msg}, '{ex}': '{exMsg}'.", message, ex?.GetType().Name, ex?.Message);

                break;
            case LogMode.Error:
                logger.LogError("{msg}, '{ex}': '{exMsg}'.", message, ex?.GetType().Name, ex?.Message);

                break;
            case LogMode.Information:
                logger.LogInformation("{msg}, '{ex}': '{exMsg}'.", message, ex?.GetType().Name, ex?.Message);
                break;
            default:
                logger.LogWarning("{msg}, '{ex}': '{exMsg}'.", message, ex?.GetType().Name, ex?.Message);
                break;
        }
    }
}
