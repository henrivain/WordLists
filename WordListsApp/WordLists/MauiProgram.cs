using WordDataAccessLibrary.DataBaseActions;
using WordDataAccessLibrary.DataBaseActions.Interfaces;
using WordListsMauiHelpers.DeviceAccess;
using WordListsViewModels.Helpers;
using WordListsMauiHelpers.Factories;
using WordListsUI.JsonExportPage;
using WordListsUI.ListGeneratorPage;
using WordListsUI.StartTrainingPage;
using WordListsUI.WordCollectionHandlingPage;
using WordListsUI.WordTrainingPage;
using WordListsViewModels;
using WordListsViewModels.Interfaces;
using WordDataAccessLibrary.ExportServices;
using WordDataAccessLibrary.BackupServices.JsonServices;

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
		builder.Services.AddTransient<ListGeneratorPage>();
		builder.Services.AddTransient<WordTrainingPage>();
		builder.Services.AddTransient<StartTrainingPage>();
		builder.Services.AddTransient<WordCollectionHandlingPage>();
		builder.Services.AddTransient<JsonExportPage>();
		builder.Services.AddSingleton<IWordTrainingViewModel, WordTrainingViewModel>();
		builder.Services.AddTransient<IStartTrainingViewModel, StartTrainingViewModel>();
		builder.Services.AddTransient<IWordCollectionHandlingViewModel, WordCollectionHandlingViewModel>();
		builder.Services.AddAbstractFactory<IListGeneratorViewModel, ListGeneratorViewModel>();
		builder.Services.AddAbstractFactory<IJsonExportViewModel, JsonExportViewModel>();

		builder.Services.AddSingleton<IWordCollectionOwnerService, WordCollectionOwnerService>();
		builder.Services.AddSingleton<IWordPairService, WordPairService>();
		builder.Services.AddSingleton<IWordCollectionService, WordCollectionService>();
		builder.Services.AddSingleton<ICollectionExportService, JsonWordCollectionExportService>();
		builder.Services.AddSingleton<IWordCollectionInfoService, WordCollectionInfoService>();



        return builder.Build();
	}
}
