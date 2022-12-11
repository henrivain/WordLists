using ImageRecognisionLibrary;
using Serilog;
using WordDataAccessLibrary.CollectionBackupServices;
using WordDataAccessLibrary.CollectionBackupServices.JsonServices;
using WordDataAccessLibrary.DataBaseActions;
using WordDataAccessLibrary.DataBaseActions.Interfaces;
using WordListsMauiHelpers.Factories;
using WordListsMauiHelpers.Logging;
using WordListsServices.FileSystemServices;
using WordListsUI.AppInfoPage;
using WordListsUI.HomePage;
using WordListsUI.WordDataPages;
using WordListsUI.WordDataPages.ImageRecognisionPage;
using WordListsUI.WordDataPages.JsonExportPage;
using WordListsUI.WordDataPages.JsonImportPage;
using WordListsUI.WordDataPages.ListGeneratorPage;
using WordListsUI.WordDataPages.WordCollectionEditPage;
using WordListsUI.WordTrainingPages.FlipCardTrainingPage;
using WordListsUI.WordTrainingPages.StartTrainingPage;
using WordListsUI.WordTrainingPages.WritingTestPage;
using WordListsViewModels;
using WordListsViewModels.Helpers;
using WordListsViewModels.Interfaces;
using WordValidationLibrary;
using ILogger = Microsoft.Extensions.Logging.ILogger;

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
			});
		// injecting appshell will make app buggy and starts to change visual element visibility

		var logger = DefaultLoggingProvider.GetFileLogger();
        new ExceptionHandler(AppDomain.CurrentDomain, logger.AsMicrosoftLogger())
            .AddExceptionHandling();

        builder.Services.AddLogging(logBuilder =>
		{
			logBuilder.AddSerilog(logger, true);
		});
		builder.Services.AddSingleton<ILogger>(logger.AsMicrosoftLogger());
		builder.Services.AddSingleton<HomePage>();
		builder.Services.AddTransient<FlipCardTrainingPage>();
		builder.Services.AddTransient<StartTrainingPage>();
		builder.Services.AddTransient<WordCollectionEditPage>();
		builder.Services.AddTransient<ListGeneratorPage>();
		builder.Services.AddTransient<WordDataPage>();
		builder.Services.AddTransient<AppInfoPage>();
		builder.Services.AddSingleton<WritingTestPage>();
		builder.Services.AddSingleton<WritingTestConfigurationPage>();
		builder.Services.AddTransient<WriteTestResultPage>();
		builder.Services.AddTransient<ImageRecognisionPage>();
		builder.Services.AddSingleton<IWordTrainingViewModel, WordTrainingViewModel>();
		builder.Services.AddTransient<IStartTrainingViewModel, StartTrainingViewModel>();
		builder.Services.AddTransient<IWordCollectionHandlingViewModel, WordCollectionHandlingViewModel>();
		builder.Services.AddAbstractFactory<IListGeneratorViewModel, ListGeneratorViewModel>();
        builder.Services.AddTransient<IWordDataViewModel, WordDataViewModel>();
        builder.Services.AddTransient<IAppInfoViewModel, AppInfoViewModel>();
		builder.Services.AddAbstractFactory<IWriteWordViewModel, WriteWordViewModel>();
		builder.Services.AddAbstractFactory<IWritingTestConfigurationViewModel, WritingTestConfigurationViewModel>();
		builder.Services.AddTransient<ITestResultViewModel, TestResultViewModel>();
		builder.Services.AddTransient<IImageRecognisionViewModel, ImageRecognisionViewModel>();

        builder.Services.AddTransient<JsonExportPage>();
        builder.Services.AddTransient<JsonImportPage>();
        builder.Services.AddAbstractFactory<IJsonExportViewModel, JsonExportViewModel>();
		builder.Services.AddAbstractFactory<IJsonImportViewModel, JsonImportViewModel>();

		builder.Services.AddSingleton<IWordCollectionOwnerService, WordCollectionOwnerService>();
		builder.Services.AddSingleton<IWordPairService, WordPairService>();
		builder.Services.AddSingleton<IWordCollectionService, WordCollectionService>();
		builder.Services.AddSingleton<ICollectionExportService, WordCollectionExportService>();
		builder.Services.AddSingleton<ICollectionImportService, JsonWordCollectionImportService>();
		builder.Services.AddSingleton<IWordCollectionInfoService, WordCollectionInfoService>();
		builder.Services.AddSingleton<IUserInputWordValidator, UserInputWordValidator>();
		builder.Services.AddTransient<IImageRecognisionEngine, ImageRecognisionEngine>();
		builder.Services.AddTransient<IFolderHandler, FolderHandler>();
		builder.Services.AddTransient<IFileHandler, FileHandler>();
        return builder.Build();
	}
}
