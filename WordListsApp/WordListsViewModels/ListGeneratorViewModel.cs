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
    bool CanOverWriteDatabase = false;
    

    [ObservableProperty]
    List<WordPair> wordPairs = new();

    [ObservableProperty]
    string collectionName = "My word collection";

    [ObservableProperty]
    string description = "Description";

    [ObservableProperty]
    string languageHeaders = "fi-en";

    public IAsyncRelayCommand GeneratePairsCommand => new AsyncRelayCommand(
        async () =>
        {
            string text = await ClipBoardAccess.GetStringAsync();
            SetWordPairsFromString(text);
        });

    public IAsyncRelayCommand SaveCollection => new AsyncRelayCommand(
        async () =>
        {
            // implement save on top of old instance if that saved
            if (WordPairs.Count is 0)
            {
                Debug.WriteLine($"{nameof(SaveCollection)}: Can't add empty word collection");
                return;
            }
            await WordCollectionService.AddWordCollection(GetDataAsWordCollection());
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
                Name = CollectionName ?? string.Empty,
                Description = Description ?? string.Empty,
                LanguageHeaders = LanguageHeaders ?? string.Empty
            }
        };
    }

    public void SaveToDatabase()
    {
        throw new NotImplementedException();
    }
}

