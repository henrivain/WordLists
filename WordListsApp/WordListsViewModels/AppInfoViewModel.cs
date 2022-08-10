using System.Reflection;

namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class AppInfoViewModel : IAppInfoViewModel
{
    [ObservableProperty]
    string appVersion = WordDataAccessLibrary.Helpers.AssemblyHelper.EntryAssembly.VersionString ?? "Ei saatavilla";

    [ObservableProperty]
    string appEnvironment = GetAppEnvironment();

    [ObservableProperty]
    string dotNetVersion = GetDotnetVersion();

    private static string GetDotnetVersion()
    {
        return Environment.Version.Major.ToString() + " MAUI";
    }

    private static string GetAppEnvironment()
    {
        return DeviceInfo.Current.Platform.ToString();
    }
}
