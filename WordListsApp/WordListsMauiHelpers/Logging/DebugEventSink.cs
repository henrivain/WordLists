using Serilog.Core;
using Serilog.Events;
using System.Diagnostics;

namespace WordListsMauiHelpers.Logging;
internal class DebugEventSink : ILogEventSink
{
    public void Emit(LogEvent logEvent)
    {
        var logTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        var offset = logEvent.Timestamp.Offset.ToString(@"hh\:mm");

        var time = $"{logTime} +{offset}";
        var severity = LevelToSeverity(logEvent.Level);
        var text = logEvent.RenderMessage();

        Debug.WriteLine($"{time} {severity} {text}");
    }


    private static string LevelToSeverity(LogEventLevel level)
    {
        return level switch
        {
            LogEventLevel.Debug => "[DBG]",
            LogEventLevel.Error => "[ERR]",
            LogEventLevel.Fatal => "[FTL]",
            LogEventLevel.Verbose => "[VRB]",
            LogEventLevel.Warning => "[WRN]",
            _ => "[INF]"
        };
    }
}
