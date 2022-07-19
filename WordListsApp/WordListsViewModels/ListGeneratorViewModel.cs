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
[QueryProperty(nameof(Owner), nameof(WordCollectionOwner))]
public partial class ListGeneratorViewModel : IListGeneratorViewModel
{
    [AlsoNotifyChangeFor(nameof(CanSave))]
    [ObservableProperty]
    List<WordPair> wordPairs = new();

    [ObservableProperty]
    string collectionName = "My word collection";

    [ObservableProperty]
    string description = "Description";

    [ObservableProperty]
    string languageHeaders = "fi-en";

    public bool CanSave => WordPairs.Count > 0;

    public IAsyncRelayCommand GeneratePairsCommand => new AsyncRelayCommand(
        async () =>
        {
            string text = await ClipboardAccess.GetStringAsync();
            ParseAndSetWordPairsFromString(text);
        });

    public IAsyncRelayCommand SaveCollection => new AsyncRelayCommand(
        async () =>
        {
            LableVisible = true;
            // implement save on top of old instance if that saved
            if (WordPairs.Count is 0)
            {
                Debug.WriteLine($"{nameof(SaveCollection)}: Can't add empty word collection");
                LableVisible = false;
                throw new InvalidOperationException();
                //return;
            }
            int id = await WordCollectionService.AddWordCollection(GetData());

            LableVisible = false;
            CollectionAddedEvent?.Invoke(this, new("Added wordCollection successfully", id));
        });

    public delegate void CollectionAddedEventHandler(object sender, DataBaseActionArgs e);

    public event CollectionAddedEventHandler? CollectionAddedEvent;

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

    public WordCollectionOwner Owner { get; set; } = new();

    public Parser UseParser { get; set; } = Parser.Otava;


    public bool LableVisible { get; set; } = false;

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

