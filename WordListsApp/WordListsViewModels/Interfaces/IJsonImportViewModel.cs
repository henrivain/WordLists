using static WordDataAccessLibrary.CollectionBackupServices.BackupDelegates;

namespace WordListsViewModels.Interfaces;
public interface IJsonImportViewModel
{
    string ImportPath { get; }
    bool CanImport { get; }
    IAsyncRelayCommand SelectFile { get; }
    IAsyncRelayCommand Import { get; }

    event ImportFailEventHandler? EmptyImportAttempted;

    event ImportFailEventHandler? ImportActionFailed;

    event ImportSuccessfullEventHandler? ImportSuccessfull;
}
