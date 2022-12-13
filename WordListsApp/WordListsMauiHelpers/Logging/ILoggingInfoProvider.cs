namespace WordListsMauiHelpers.Logging;
public interface ILoggingInfoProvider
{
    /// <summary>
    /// Paths to every log file.
    /// </summary>
    string[] LoggingFilePaths { get; }
}
