using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit;
using WordDataAccessLibrary;

namespace WordListsUI.ListGeneratorPage;




[INotifyPropertyChanged]
public partial class ViewModel
{
    [ObservableProperty]
    List<WordPair> wordPairs = GetTestData().WordPairs;

    





    private static WordCollection GetTestData() => new()
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
