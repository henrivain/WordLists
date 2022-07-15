using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm;
using WordDataAccessLibrary;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using WordDataAccessLibrary.DeviceAccess;
using WordDataAccessLibrary.Generators;
using WordDataAccessLibrary.Helpers;
using WordDataAccessLibrary.DataBaseActions;

namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class ListGeneratorViewModel : IListGeneratorViewModel
{

    [ObservableProperty]
    List<WordPair> wordPairs = new();

    [ObservableProperty]
    string collectionName = "My word collection";

    [ObservableProperty]
    string description = "Description";

    [ObservableProperty]
    string languageHeaders = "fi-en";

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

    public IAsyncRelayCommand GeneratePairsCommand => new AsyncRelayCommand(
        async () =>
        {
            string text = await ClipBoardAccess.GetStringAsync();
            SetWordPairsFromString(text);
        });

    public IAsyncRelayCommand SaveCollection => new AsyncRelayCommand(
        async () =>
        {
            //await WordCollectionService.AddWordCollection(GetDataAsWordCollection());
            await Task.Delay(1);
            Debug.WriteLine($"{nameof(SaveCollection)} is not enabled");
        });

    public IRelayCommand FlipSides => new RelayCommand(() =>
    {
        WordPairs = WordListFlipper.FlipWordPair(WordPairs);
    });






    private void SetWordPairsFromString(string text)
    {
        WordPairs = UseParser switch
        {
            Parser.Otava => new OtavaWordPairParser(text).GetList(),

            _ => throw new NotImplementedException($"Parser {UseParser} is not implemented")
        };
    }


    public Parser UseParser { get; set; } = Parser.Otava;

    public enum Parser
    {
        Otava
    }

    public WordCollection GetDataAsWordCollection()
    {
        return new()
        {
            WordPairs = WordPairs,
            Owner = new()
            {
                Name = CollectionName,
                Description = Description,
                LanguageHeaders = LanguageHeaders
            }
        };
    }

    public void SaveToDatabase()
    {
        throw new NotImplementedException();
    }
}

