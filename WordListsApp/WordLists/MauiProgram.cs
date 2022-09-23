﻿using WordDataAccessLibrary.CollectionBackupServices;
using WordDataAccessLibrary.CollectionBackupServices.JsonServices;
using WordDataAccessLibrary.DataBaseActions;
using WordDataAccessLibrary.DataBaseActions.Interfaces;
using WordListsMauiHelpers.Factories;
using WordListsUI.AppInfoPage;
using WordListsUI.WordDataPages;
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

		builder.Services.AddTransient<FlipCardTrainingPage>();
		builder.Services.AddTransient<StartTrainingPage>();
		builder.Services.AddTransient<WordCollectionEditPage>();
		builder.Services.AddTransient<ListGeneratorPage>();
		builder.Services.AddTransient<WordDataPage>();
		builder.Services.AddTransient<AppInfoPage>();
		builder.Services.AddSingleton<WritingTestPage>();
		builder.Services.AddSingleton<WritingTestConfigurationPage>();
		builder.Services.AddSingleton<IWordTrainingViewModel, WordTrainingViewModel>();
		builder.Services.AddTransient<IStartTrainingViewModel, StartTrainingViewModel>();
		builder.Services.AddTransient<IWordCollectionHandlingViewModel, WordCollectionHandlingViewModel>();
		builder.Services.AddAbstractFactory<IListGeneratorViewModel, ListGeneratorViewModel>();
        builder.Services.AddTransient<IWordDataViewModel, WordDataViewModel>();
        builder.Services.AddTransient<IAppInfoViewModel, AppInfoViewModel>();
		builder.Services.AddTransient<IWriteWordViewModel, WriteWordViewModel>();
		builder.Services.AddAbstractFactory<IWritingTestConfigurationViewModel, WritingTestConfigurationViewModel>();

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

        return builder.Build();
	}
}
