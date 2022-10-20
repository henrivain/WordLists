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

    IAsyncRelayCommand GeneratePairsCommand { get; }

    IAsyncRelayCommand SaveCollection { get; }

    IRelayCommand FlipSides { get; }

    IRelayCommand<string> Remove { get; }

    IRelayCommand<string> Edit { get; }

    IRelayCommand New { get; }

    WordCollection GetData();

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

    event CollectionAddedEventHandler? CollectionAddedEvent;

    event EditWantedEventHandler? EditWantedEvent;

    event AddWantedEventHandler? AddWantedEvent;
}
