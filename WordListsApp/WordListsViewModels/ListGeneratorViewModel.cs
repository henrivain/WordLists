using System.Diagnostics;
using WordDataAccessLibrary.Generators;
using WordDataAccessLibrary.Helpers;
using WordListsMauiHelpers.DeviceAccess;
using WordDataAccessLibrary.DataBaseActions.Interfaces;
using Microsoft.Maui.Animations;
using WordListsViewModels.Extensions;

namespace WordListsViewModels;

[INotifyPropertyChanged]
[QueryProperty(nameof(Owner), nameof(WordCollectionOwner))]
public partial class ListGeneratorViewModel : IListGeneratorViewModel
{
    public ListGeneratorViewModel(IWordCollectionService collectionService)
    {
        CollectionService = collectionService;
    }

    [AlsoNotifyChangeFor(nameof(CanSave))]
    [ObservableProperty]
    List<string> words = new();

    [ObservableProperty]
    string collectionName = "My word collection";

    [ObservableProperty]
    string description = "Description";

    [ObservableProperty]
    string languageHeaders = "fi-en";

    public bool CanSave => Words.Count / 2 > 0;

    public IAsyncRelayCommand GeneratePairsCommand => new AsyncRelayCommand(
        async () =>
        {
            string text = await ClipboardAccess.GetStringAsync();
            Words = StringParserActions[UseParser](text);
        });

    public IAsyncRelayCommand SaveCollection => new AsyncRelayCommand(
        async () =>
        {
            if (Words.Count < 2)
            {
                Debug.WriteLine($"{nameof(SaveCollection)}: Can't add empty word collection");
                throw new InvalidOperationException();
            }
            int id = await CollectionService.AddWordCollection(GetData());

            CollectionAddedEvent?.Invoke(this, new("Added wordCollection successfully", id));
        });


    public event CollectionAddedEventHandler? CollectionAddedEvent;

    public IRelayCommand FlipSides => new RelayCommand(() => Words = Words.FlipPairs());

    public WordCollection GetData()
    {
        return new()
        {
            WordPairs = WordParser.PairWords(Words.ToArray()),
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

    public IWordCollectionService CollectionService { get; }

    public IRelayCommand<string> Remove => new RelayCommand<string>(value => RemoveINstance(value));

    private void RemoveINstance(string? value)
    {
        if (value is null) throw new NotImplementedException();
        Words.Remove(value);
        OnPropertyChanged(nameof(Words));
    }

    public enum Parser
    {
        Otava
    }
    
    readonly Dictionary<Parser, Func<string, List<string>>> StringParserActions = new()
    {
        [Parser.Otava] = (pairs) => { return new OtavaWordPairParser(pairs).ToStringList(); }
    };
}

