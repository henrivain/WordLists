namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class AppInfoViewModel : IAppInfoViewModel
{
    [ObservableProperty]
    string _appVersion = WordDataAccessLibrary.Helpers.AssemblyHelper.EntryAssembly.VersionString ?? "Ei saatavilla";

    [ObservableProperty]
    string _appEnvironment = GetAppEnvironment();

    [ObservableProperty]
    string _dotNetVersion = GetDotnetVersion();




    private static string GetDotnetVersion()
    {
        return Environment.Version.Major.ToString() + " MAUI";
    }

    private static string GetAppEnvironment()
    {
        return DeviceInfo.Current.Platform.ToString();
    }
}
