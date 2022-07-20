﻿using CommunityToolkit.Mvvm.ComponentModel;
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
        StartNew(TestData_Length4);
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
    WordPair? visibleWordPair;

    [ObservableProperty]
    int maxWordIndex = 0;

    [AlsoNotifyChangeFor(nameof(UIVisibleIndex))]
    [AlsoNotifyChangeFor(nameof(CanFlipNext))]
    [AlsoNotifyChangeFor(nameof(CanFlipPrevious))]
    int RealIndex = 0;

    public bool CanFlipPrevious => IsFirstWordPair() is false;

    public bool CanFlipNext => RealIndex <= MaxWordIndex;

    public int UIVisibleIndex => GetUIIndex();

    public void Next()
    {
        if (CanFlipNext)
        {
            RealIndex++;
            ShowCurrentWord();
        }
    }
    public void Previous()
    {
        if (CanFlipPrevious)
        {
            RealIndex--;
            ShowCurrentWord();
        }
    }
    public void StartNew(WordCollection collection)
    {
        MaxWordIndex = collection.WordPairs.Count - 1;
        WordCollection = collection;
        RealIndex = 0;
        ShowCurrentWord();
    }
    public void StartNew(WordCollection collection, int fromIndex)
    {
        MaxWordIndex = collection.WordPairs.Count - 1;

        if (fromIndex > MaxWordIndex || fromIndex < 0)
        {
            throw new ArgumentException(
                $"{nameof(fromIndex)} can't be bigger than max index, or smaller than 0. Was given: {fromIndex}, max: {MaxWordIndex}");
        }

        WordCollection = collection;
        RealIndex = fromIndex;
        ShowCurrentWord();
    }



    private void ShowCurrentWord()
    {
        if (IsOverMaxIndex())
        {
            VisibleWordPair = CompletedView;
            return;
        }
        VisibleWordPair = WordCollection.WordPairs[UIVisibleIndex];
    }
    private int GetUIIndex()
    {
        return (RealIndex > MaxWordIndex) ? MaxWordIndex : RealIndex;
    }
    private bool IsFirstWordPair()
    {
        if (RealIndex <= 0)
        {
            RealIndex = 0;
            return true;
        }
        return false;
    }
    private bool IsOverMaxIndex()
    {
        return RealIndex > MaxWordIndex;
    }

    private readonly WordPair CompletedView = new()
    {
        NativeLanguageWord = "Word list completed!",
        ForeignLanguageWord = "Word list completed!"
    };

    private static readonly WordCollection TestData_Length4 = new()
    {
        Owner = new()
        {
            Name = "WordCollection1",
            Description = "this is WordCollection1",
            Id = 1
        },
        WordPairs = new()
        {
            new()
            {
                NativeLanguageWord = "a Tree",
                ForeignLanguageWord = "puu",
                IndexInVocalbulary = 0,
                LearnState = WordLearnState.MightKnow,
                OwnerId = 1
            },
            new()
            {
                NativeLanguageWord = "a flower",
                ForeignLanguageWord = "kukka",
                IndexInVocalbulary = 1,
                LearnState = WordLearnState.NeverHeard,
                OwnerId = 1
            },
            new()
            {
                NativeLanguageWord = "a pig",
                ForeignLanguageWord = "sika",
                IndexInVocalbulary = 2,
                LearnState = WordLearnState.Learned,
                OwnerId = 1
            },
            new()
            {
                NativeLanguageWord = "a car",
                ForeignLanguageWord = "auto",
                IndexInVocalbulary = 3,
                LearnState = WordLearnState.NeverHeard,
                OwnerId = 1
            }

        }
    };
}
