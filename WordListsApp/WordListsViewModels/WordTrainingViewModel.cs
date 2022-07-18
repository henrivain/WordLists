using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using WordDataAccessLibrary;
using WordDataAccessLibrary.DataBaseActions;
using WordDataAccessLibrary.DeviceAccess;
using WordDataAccessLibrary.Generators;
using WordDataAccessLibrary.Helpers;

namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class WordTrainingViewModel : IWordTrainingViewModel
{
    public WordTrainingViewModel()
    {
        StartNewCollection(new());
    }

    public WordCollection WordCollection { get; set; } = new();

    [ObservableProperty]
    string title = "Unter uns 6 kpl 5 nice!";

    [ObservableProperty]
    string description = "This short story takes 200 symbols. " +
        "Once there was a big and massive house. " +
        "Young deer wondered what was happening inside. " +
        "It took a look insede and saw a woman with gun. " +
        "Deer got shot and died sad..";

    [ObservableProperty]
    string languageHeaders = "fi-en-sw-ge-fif";

    [ObservableProperty]
    int currentWordIndex = 0;

    [ObservableProperty]
    int maxWordIndex = 0;

    [ObservableProperty]
    bool canFlipNext = true;

    [ObservableProperty]
    bool canFlipLast = false;

    [ObservableProperty]
    WordPair? visibleWordPair;

    public void Next()
    {
        CanFlipLast = true;
        
        if (CurrentWordIndex >= MaxWordIndex)
        {
            Completed();
            return;
        }
        CurrentWordIndex++;
        VisibleWordPair = WordCollection.WordPairs[CurrentWordIndex];
    }

    public void Previous()
    {
        CanFlipNext = true;
        CurrentWordIndex--;
        if (CurrentWordIndex <= 0)
        {
            CanFlipLast = false;
            CurrentWordIndex = 0;
        }
        VisibleWordPair = WordCollection.WordPairs[CurrentWordIndex];
    }

    public void StartNewCollection(WordCollection collection)
    {
        MaxWordIndex = collection.WordPairs.Count;
        WordCollection = collection;
        CurrentWordIndex = 0;
        CanFlipLast = false;
        
        if (CurrentWordIndex >= MaxWordIndex) Completed();
    }

    private void Completed()
    {
        CanFlipNext = false;
        VisibleWordPair = new()
        {
            NativeLanguageWord = "Word list completed!",
            ForeignLanguageWord = "Word list completed!"
        };
        CurrentWordIndex = MaxWordIndex;
    }

}
