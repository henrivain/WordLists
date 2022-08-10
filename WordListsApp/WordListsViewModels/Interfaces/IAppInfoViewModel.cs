namespace WordListsViewModels.Interfaces;
public interface IAppInfoViewModel
{
    string AppVersion { get; }

    string AppEnvironment { get; }

    string DotNetVersion { get; }
}
