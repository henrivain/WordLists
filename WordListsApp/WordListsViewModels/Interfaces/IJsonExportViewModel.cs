using System.Collections.ObjectModel;
using WordListsViewModels.Helpers;
using static WordDataAccessLibrary.CollectionBackupServices.BackupDelegates;

namespace WordListsViewModels.Interfaces;
public interface IJsonExportViewModel
{
    ObservableCollection<WordCollectionInfo> VisibleCollections { get; }
    
    List<object> SelectedCollections { get; }

    string ExportFileName { get; set; }
    string ExportFolderPath { get; set; }
    string ExportFileExtension { get; }
    string ExportPath { get; }


    string NameFilter { get; set; }
    string LanguageFilter { get; set; }

    bool CanExportAllVisible { get; }
    bool CanExportSelected { get; }
    bool RemoveUserDataFromWordPairs { get; set; }

    IAsyncRelayCommand ExportSelectionsCommand { get; }
    IAsyncRelayCommand ExportAllVisibleCommand { get; }
    IAsyncRelayCommand ChooseExportFolderCommand { get; }
    IAsyncRelayCommand CopyPathToClipBoardCommand { get; }
    IRelayCommand SearchParameterChangedCommand { get; }
    IRelayCommand SelectionChangedCommand { get; }

    Task ResetCollections();

    event ExportFailEventHandler? EmptyExportAttempted;

    event ExportSuccessfullEventHandler? ExportCompleted;
}
