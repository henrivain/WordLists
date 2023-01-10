using System.Xml;
using WordListsMauiHelpers.Logging;
using WordListsServices.FileSystemServices;
using WordListsServices.FileSystemServices.ActionResults;
using WordListsViewModels.Events;
namespace WordListsUI.AppInfoPage;

public partial class AppInfoPage : ContentPage
{
    public AppInfoPage(IAppInfoViewModel model, IFileHandler fileHandler, ILoggingInfoProvider loggingInfo)
    {
        BindingContext = model;
        InitializeComponent();
        model.LogsCopied += Model_LogsCopied;
        model.ShowLogFileFailed += Model_ShowLogFileFailed;
        FileHandler = fileHandler;
        LoggingInfo = loggingInfo;
    }

    private async void Model_ShowLogFileFailed(object sender, InvalidDataEventArgs<string[]> e)
    {
        await DisplayAlert("Avaaminen epäonnistui!", $"Lokitiedostokansion avaaminen epäonnistui. Syy: '{e.Message}'.", "OK");
    }

    private async void Model_LogsCopied(object sender, LogsCopiedEventArgs e)
    {
        if (e.Success)
        {
            await DisplayAlert("Lokitiedostot kopioitu!",
                $"{e.FilesValid} lokitieodostoa kopioitu onnistuneesti polkuun: \n {e.OutputDirectory}",
                "OK");
            return;
        }
        int filesTotal = e.FilesValid + e.FilesFailed;
        await DisplayAlert("Tiedostojen kopiointi epäonnistui!",
            $"{e.FilesFailed}/{filesTotal} lokitiedostoa ei pystytty kopioimaan. \nSyy:\n {e.Message}",
            "OK");
    }

    public IAppInfoViewModel Model => (IAppInfoViewModel)BindingContext;

    public IFileHandler FileHandler { get; }
    public ILoggingInfoProvider LoggingInfo { get; }

    private async void DeleteLogs_Clicked(object sender, EventArgs e)
    {
        List<IFileSystemResult> filesDeleted = new();
        foreach (var path in LoggingInfo.LoggingFilePaths)
        {
            filesDeleted.Add(await FileHandler.DeleteAsync(path));
        }
        var failed = filesDeleted.Where(x => x.Success is false).ToList();
        if (failed.Count > 0)
        {
            await DisplayAlert("Log deletion failed!", $"Failed to delete '{failed.Count}' logs. " +
                $"Reasons:\n'{string.Join("\n", failed.Select(x => x.Message))}'", "OK");
            return;
        }
        await DisplayAlert($"Deleted log files!", $"Deleted '{filesDeleted.Count}' log files successfully! " +
            $"You might need to restart the app.", "OK");
    }
}