using static WordDataAccessLibrary.CollectionBackupServices.BackupDelegates;

namespace WordListsViewModels.Interfaces;
public interface IJsonImportViewModel
{
    string AcceptableFileExtensions { get; }
    string ImportPath { get; }
    bool CanImport { get; }
    bool IsBusy { get; }
    IAsyncRelayCommand SelectFile { get; }
    IAsyncRelayCommand Import { get; }

    event ImportFailEventHandler? EmptyImportAttempted;

    event ImportFailEventHandler? ImportActionFailed;

    event FileAccessEventHandler? SelectFileAttempted;

    event ImportSuccessfullEventHandler? ImportSuccessfull;
    void SetDefaultImportPath(string value);
}
