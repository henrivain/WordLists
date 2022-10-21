using System.Collections.ObjectModel;
using System.Diagnostics;
using WordDataAccessLibrary.DataBaseActions.Interfaces;
using WordDataAccessLibrary.Generators;
using WordListsMauiHelpers.DeviceAccess;
using WordListsViewModels.Events;
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
    ObservableCollection<string> words = new();

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
            Words = new(StringParserActions[UseParser](text));
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

    public IRelayCommand FlipSides => new RelayCommand(() => Words = new(Words.ToList().FlipPairs()));

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

    public IRelayCommand<string> Delete => new RelayCommand<string>(value =>
    {
        if (value is null) throw new NotImplementedException($"{nameof(value)} should not be null");

        Words.Remove(value);
        OnPropertyChanged(nameof(Words));
    });

    public IRelayCommand<string> Edit => new RelayCommand<string>(value =>
    {
        if (value is null) throw new NotImplementedException($"{nameof(value)} should not be null");
        int index = Words.IndexOf(value);
        EditWantedEvent?.Invoke(this, new(value, index));
    });

    public IRelayCommand New => new RelayCommand(() => AddWantedEvent?.Invoke(this, EventArgs.Empty));

    public enum Parser
    {
        Otava
    }
    
    readonly Dictionary<Parser, Func<string, List<string>>> StringParserActions = new()
    {
        [Parser.Otava] = (pairs) => { return new OtavaWordPairParser(pairs).ToStringList(); }
    };

    /// <summary>
    /// Try set string value of specific index in Words ObservableCollection
    /// </summary>
    /// <param name="indexInList"></param>
    /// <param name="value"></param>
    /// <returns>boolean value reprcenting if action was success</returns>
    public bool SetWordValueWithIndex(int indexInList, string value)
    {
        if (string.IsNullOrEmpty(value)) return false;
        try
        {
            Words[indexInList] = value;
            return true;
        }
        catch (ArgumentOutOfRangeException)
        {
            Debug.WriteLine($"Attempt in {nameof(ListGeneratorViewModel)} to set word value to '{value}' in index '{indexInList}' " +
                $"failed because of {nameof(ArgumentOutOfRangeException)}, only '{Words.Count}' indexes exist");
            return false;
        }
    }

    public void AddWord(string result)
    {
        if (string.IsNullOrEmpty(result)) return;
        Words.Add(result);
    }

    public event CollectionAddedEventHandler? CollectionAddedEvent;

    public event EditWantedEventHandler? EditWantedEvent;

    public event AddWantedEventHandler? AddWantedEvent;
}

