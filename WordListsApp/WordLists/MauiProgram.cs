// using ImageRecognisionLibrary;
using Serilog;
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

		// injecting appshell will make app buggy and starts to change visual element visibility

		var serilogger = DefaultLoggingProvider.GetAppDefaultLogger();
        new ExceptionHandler(AppDomain.CurrentDomain, serilogger.AsMicrosoftLogger())
            .AddExceptionHandling();

        builder.Services.AddLogging(logBuilder =>
		{
			logBuilder.AddSerilog(serilogger, true);
		});
		builder.Services.AddSingleton(serilogger.AsMicrosoftLogger());
		builder.Services.AddAppPages();
		builder.Services.AddAppServices();
		builder.Services.AddAppViewModels();

        // Add image recognision page with these and adding usings and project references
        // builder.Services.AddTransient<ImageRecognisionPage>();
        // builder.Services.AddTransient<IImageRecognisionViewModel, ImageRecognisionViewModel>();
        // builder.Services.AddTransient<IImageRecognisionEngine, ImageRecognisionEngine>();
        return builder.Build();
	}
}
