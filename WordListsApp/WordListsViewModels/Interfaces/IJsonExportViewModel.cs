using WordListsViewModels.Helpers;
using static WordDataAccessLibrary.CollectionBackupServices.BackupDelegates;

namespace WordListsViewModels.Interfaces;
public interface IJsonExportViewModel
{
    List<WordCollectionInfo> VisibleCollections { get; }
    List<WordCollectionInfo> AvailableCollections { get; }
    List<object> SelectedCollections { get; }


    string NameParameter { get; set; }
    string LanguageHeadersParameter { get; set; }
    string ExportPath { get; set; }

    bool CanExportAllVisible { get; }
    bool CanExportSelected { get; }
    bool RemoveUserDataFromWordPairs { get; set; }

    IAsyncRelayCommand ExportSelectionsCommand { get; }
    IAsyncRelayCommand ExportAllVisibleCommand { get; }
    IAsyncRelayCommand ChooseExportLocationCommand { get; }
    IAsyncRelayCommand CopyPathToClipBoardCommand { get; }
    IRelayCommand SelectionChangedCommand { get; }

    event ExportFailEventHandler? EmptyExportAttempted;

    event ExportSuccessfullEventHandler? ExportCompleted;
}
