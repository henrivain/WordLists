using System.Collections.ObjectModel;
using System.ComponentModel;
using WordListsViewModels.Events;

namespace WordListsViewModels.Interfaces;
public interface IListGeneratorViewModel
{
    event PropertyChangedEventHandler? PropertyChanged;

    ObservableCollection<string> Words { get; set; }

    string CollectionName { get; set; }

    string Description { get; set; }

    string LanguageHeaders { get; set; }

    bool CanSave { get; }

    bool IsEditMode { get; }

    bool IsBusy { get; }

    IAsyncRelayCommand GeneratePairsCommand { get; }

    IAsyncRelayCommand Save { get; }

    IAsyncRelayCommand FlipSides { get; }

    IRelayCommand<string> Delete { get; }

    IRelayCommand<string> Edit { get; }

    IAsyncRelayCommand FinishEdit { get; }

    IRelayCommand New { get; }

    WordCollection ParseData();

    /// <summary>
    /// Try set string value of specific index in Words ObservableCollection
    /// </summary>
    /// <param name="indexInList"></param>
    /// <param name="value"></param>
    /// <returns>boolean value reprcenting if action was success</returns>
    bool SetWordValueWithIndex(int indexInList, string value);

    /// <summary>
    /// Adds new word to Words ObservableCollection
    /// </summary>
    /// <param name="result"></param>
    void AddWord(string result);
    void StartEditProcess(WordCollection value);

    event CollectionAddedEventHandler? CollectionAddedEvent;
    event AddWantedEventHandler? AddWantedEvent;
    event CollectionEditEventHandler? EditWantedEvent;
    event CollectionAddedEventHandler? EditFinished;
    event CollectionAddedEventHandler? FailedToSaveEvent;
}
