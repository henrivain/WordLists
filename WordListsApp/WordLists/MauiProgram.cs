// using ImageRecognisionLibrary;
using Serilog;
using TesseractOcrMaui;
using WordListsMauiHelpers.Logging;

namespace WordLists;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .ConfigureEssentials(essentials =>
            {
                essentials.UseVersionTracking();
            });

        var serilogger = DefaultLoggingProvider.GetAppDefaultLogger();
        var logger = serilogger.AsMicrosoftLogger();
        new ExceptionHandler(AppDomain.CurrentDomain, logger).AddExceptionHandling();


        builder.Services.AddLogging(logBuilder =>
        {
            logBuilder.AddSerilog(serilogger, true);
        });
        builder.Services.AddSingleton(logger);
        builder.Services.AddAppPages();
        builder.Services.AddAppServices();
        builder.Services.AddAppViewModels();
        builder.Services.AddTesseractOcr(files =>
        {
            files.AddFile("fin.traineddata");
            files.AddFile("deu.traineddata");
            files.AddFile("eng.traineddata");
            files.AddFile("swe.traineddata");
            files.AddFile("fra.traineddata");
        });

        return builder.Build();
    }
}
