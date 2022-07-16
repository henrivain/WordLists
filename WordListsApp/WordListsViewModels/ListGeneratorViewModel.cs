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

    public IAsyncRelayCommand GeneratePairsCommand => new AsyncRelayCommand(
        async () =>
        {
            string text = await ClipBoardAccess.GetStringAsync();
            ParseAndSetWordPairsFromString(text);
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
            await WordCollectionService.AddWordCollection(GetData());
        });

    public IRelayCommand FlipSides => new RelayCommand(() =>
    {
        WordPairs = WordListFlipper.FlipWordPair(WordPairs);
    });

    public WordCollection GetData()
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

    public Parser UseParser { get; set; } = Parser.Otava;

    public enum Parser
    {
        Otava
    }

    private void ParseAndSetWordPairsFromString(string pairs)
    {
        WordPairs = UseParser switch
        {
            Parser.Otava => new OtavaWordPairParser(pairs).GetList(),
            _ => throw new NotImplementedException($"Parser {UseParser} is not implemented")
        };
    }
}

