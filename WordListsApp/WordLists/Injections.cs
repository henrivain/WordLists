using WordDataAccessLibrary.CollectionBackupServices;
using WordDataAccessLibrary.CollectionBackupServices.JsonServices;
using WordDataAccessLibrary.DataBaseActions;
using WordDataAccessLibrary.DataBaseActions.Interfaces;
using WordListsMauiHelpers.DeviceAccess;
using WordListsMauiHelpers.DependencyInjectionExtensions;
using WordListsMauiHelpers.Logging;
using WordListsServices.FileSystemServices;
using WordListsServices.ProcessServices;
using WordListsViewModels;
using WordListsViewModels.Helpers;
using WordListsViewModels.Interfaces;
using WordValidationLibrary;
using WordDataAccessLibrary.Generators;
using WordListsMauiHelpers.Settings;

namespace WordLists;
internal static class Injections
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddSingleton<IWordPairService, WordPairService>();
        services.AddSingleton<IWordCollectionOwnerService, WordCollectionOwnerService>();
        services.AddSingleton<IWordCollectionService, WordCollectionService>();
        services.AddSingleton<ICollectionExportService, WordCollectionExportService>();
        services.AddSingleton<ICollectionImportService, JsonWordCollectionImportService>();
        services.AddSingleton<IWordCollectionInfoService, WordCollectionInfoService>();
        services.AddSingleton<IUserInputWordValidator, UserInputWordValidator>();
        services.AddTransient<IFolderHandler, FolderHandler>();
        services.AddTransient<IFileHandler, FileHandler>();
        services.AddTransient<IProcessLauncher, ProcessLauncher>();
        services.AddSingleton<ILoggingInfoProvider, DefaultLoggingProvider>();
        services.AddSingleton<IFilePickerService, FilePickerService>();
        services.AddTransient<IWordPairParser, OtavaWordPairParser>();
        services.AddTransient<IWordPairParser, NewOtavaWordPairParser>();
        services.AddSingleton<ISettings, Settings>();

        return services;
    }

    public static IServiceCollection AddAppPages(this IServiceCollection services)
    {
        services.AddSingleton<HomePage>();
        services.AddTransient<FlipCardTrainingPage>();
        services.AddTransient<StartTrainingPage>();
        services.AddTransient<WordCollectionEditPage>();
        services.AddTransient<ListGeneratorPage>();
        services.AddTransient<WordDataPage>();
        services.AddTransient<AppInfoPage>();
        services.AddSingleton<WritingTestPage>();
        services.AddSingleton<WritingTestConfigurationPage>();
        services.AddTransient<WriteTestResultPage>();
        services.AddTransient<JsonExportPage>();
        services.AddTransient<JsonImportPage>();
        services.AddTransient<WordListPage>();
        services.AddTransient<TrainingConfigPage>();

        return services;
    }

    public static IServiceCollection AddAppViewModels(this IServiceCollection services)
    {
        services.AddSingleton<IWordTrainingViewModel, WordTrainingViewModel>();
        services.AddTransient<IStartTrainingViewModel, StartTrainingViewModel>();
        services.AddTransient<IWordCollectionHandlingViewModel, WordCollectionHandlingViewModel>();
        services.AddTransient<IWordDataViewModel, WordDataViewModel>();
        services.AddTransient<IAppInfoViewModel, AppInfoViewModel>();
        services.AddTransient<ITestResultViewModel, TestResultViewModel>();
        services.AddTransient<IWordListViewModel, WordListViewModel>();
        services.AddTransient<ITrainingConfigViewModel, TrainingConfigViewModel>();
        services.AddAbstractFactory<IListGeneratorViewModel, ListGeneratorViewModel>();
        services.AddAbstractFactory<IJsonExportViewModel, JsonExportViewModel>();
        services.AddAbstractFactory<IJsonImportViewModel, JsonImportViewModel>();
        services.AddAbstractFactory<IWriteWordViewModel, WriteWordViewModel>();
        services.AddAbstractFactory<IWritingTestConfigurationViewModel, WritingTestConfigurationViewModel>();

        return services;
    }




}
