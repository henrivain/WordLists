using WordDataAccessLibrary.CollectionBackupServices;
using WordDataAccessLibrary.CollectionBackupServices.JsonServices;
using WordDataAccessLibrary.DataBaseActions;
using WordDataAccessLibrary.DataBaseActions.Interfaces;
using WordListsMauiHelpers.Factories;
using WordListsUI.JsonExportPage;
using WordListsUI.JsonImportPage;
using WordListsUI.ListGeneratorPage;
using WordListsUI.StartTrainingPage;
using WordListsUI.WordCollectionHandlingPage;
using WordListsUI.WordTrainingPage;
using WordListsViewModels;
using WordListsViewModels.Helpers;
using WordListsViewModels.Interfaces;

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

		builder.Services.AddTransient<WordTrainingPage>();
		builder.Services.AddTransient<StartTrainingPage>();
		builder.Services.AddTransient<WordCollectionHandlingPage>();
		builder.Services.AddTransient<ListGeneratorPage>();
		builder.Services.AddSingleton<IWordTrainingViewModel, WordTrainingViewModel>();
		builder.Services.AddTransient<IStartTrainingViewModel, StartTrainingViewModel>();
		builder.Services.AddTransient<IWordCollectionHandlingViewModel, WordCollectionHandlingViewModel>();
		builder.Services.AddAbstractFactory<IListGeneratorViewModel, ListGeneratorViewModel>();

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

        return builder.Build();
	}
}
