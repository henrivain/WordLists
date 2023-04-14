using System.Collections.ObjectModel;
using System.ComponentModel;
using WordListsViewModels.Events;
using WordListsViewModels.Helpers;

namespace WordListsViewModels.Interfaces;
public interface IListGeneratorViewModel
{
    event PropertyChangedEventHandler? PropertyChanged;

    ObservableCollection<string> Words { get; set; }

    List<ParserInfo> Parsers { get; }

    object SelectedParser { get; set; }

    string CollectionName { get; set; }

    string Description { get; set; }

    string LanguageHeaders { get; set; }

    bool CanSave { get; }

    bool IsEditMode { get; }

    bool IsBusy { get; }

    bool ShowUnEvenWordCountWarning { get; }
    

    // Database commands
    IAsyncRelayCommand GeneratePairsCommand { get; }
    IAsyncRelayCommand Save { get; }

    // Word list commands
    IAsyncRelayCommand FlipSides { get; }

    // Word commands
    IRelayCommand<string> Delete { get; }
    IRelayCommand<string> Edit { get; }
    IRelayCommand New { get; }

    
    /// <summary>
    /// Get all data from this object as wordcollection.
    /// </summary>
    /// <returns>Instance's data as wordcollection</returns>
    WordCollection ParseToWordCollection();

    /// <summary>
    /// Set value of word in given index.
    /// </summary>
    /// <param name="indexInList"></param>
    /// <param name="value"></param>
    /// <returns>True if success, otherwise false.</returns>
    bool SetWordValueWithIndex(int indexInList, string value);

    /// <summary>
    /// Add new word to Words -list.
    /// </summary>
    /// <param name="word"></param>
    void AddWord(string result);

    /// <summary>
    /// Replace all old words with new ones.
    /// </summary>
    /// <param name="words"></param>
    void ResetWordPairs(string[] words);

    /// <summary>
    /// Open already existing collection for user to edit.
    /// </summary>
    /// <param name="value"></param>
    void OpenInEditMode(WordCollection value);

    
    event CollectionAddedEventHandler? CollectionAddedEvent;
    event AddWantedEventHandler? AddWantedEvent;
    event CollectionEditEventHandler? EditWantedEvent;
    event CollectionAddedEventHandler? EditFinished;
    event CollectionAddedEventHandler? FailedToSaveEvent;
    event ParserErrorEventHandler? ParserError;
}
