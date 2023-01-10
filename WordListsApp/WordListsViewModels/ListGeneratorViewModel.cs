using System.Collections.ObjectModel;
using WordDataAccessLibrary.DataBaseActions.Interfaces;
using WordDataAccessLibrary.Generators;
using WordListsMauiHelpers.DeviceAccess;
using WordListsViewModels.Events;

namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class ListGeneratorViewModel : IListGeneratorViewModel
{
    public ListGeneratorViewModel(IWordCollectionService collectionService, ILogger<IListGeneratorViewModel> logger)
    {
        CollectionService = collectionService;
        Logger = logger;
    }

    //[AlsoNotifyChangeFor(nameof(CanSave))]
    [ObservableProperty]
    ObservableCollection<string> _words = new();



    [ObservableProperty]
    string _collectionName = "My word collection";

    [ObservableProperty]
    string _description = "Description";

    [ObservableProperty]
    string _languageHeaders = "fi-en";

    [ObservableProperty]
    bool _isEditMode = false;

    [ObservableProperty]
    bool _isBusy = false;

    public bool CanSave => Words.Count / 2 > 0;

    private WordCollection? OldWordCollectionValue { get; set; }

    public IAsyncRelayCommand GeneratePairsCommand => new AsyncRelayCommand(
        async () =>
        {
            IsBusy = true;
            string text = await ClipboardAccess.GetStringAsync();
            var parser = StringParserActions[UseParser];
            string[] words = await Task.Run(() =>
            {
                return parser(text).ToArray();
            });
            Words.Clear();
            foreach (var word in words)
            {
                Words.Add(word);
            }
            OnPropertyChanged(nameof(CanSave));
            IsBusy = false;
        });
    public IAsyncRelayCommand Save => new AsyncRelayCommand(
        async () =>
        {
            IsBusy = true;
            if (Words.Count < 2)
            {
                Logger.LogWarning("{class}.{method}: Can't add empty word collection",
                    nameof(ListGeneratorViewModel), nameof(Save));
                FailedToSaveEvent?.Invoke(this, new DataBaseActionArgs
                {
                    Text = "Collection was empty or had less than 2 words."
                });
                IsBusy = false;
                return;
            }
            int id = await CollectionService.AddWordCollection(ParseData());

            CollectionAddedEvent?.Invoke(this, new DataBaseActionArgs
            {
                Text = "Added wordCollection successfully",
                RefIds = new[] { id }
            });
            IsBusy = false;
        });
    public IAsyncRelayCommand FlipSides => new AsyncRelayCommand(async () =>
    {
        IsBusy = true;
        List<string> words = await Task.Run(() =>
        {
            return Words.ToList().FlipPairs();
        });
        Words.Clear();
        foreach (var word in words)
        {
            Words.Add(word);
        }
        OnPropertyChanged(nameof(CanSave));
        IsBusy = false;
    });

    public WordCollection ParseData()
    {
        return new()
        {
            WordPairs = WordParser.PairWords(Words.ToArray()),
            Owner = new()
            {
                Name = CollectionName ?? string.Empty,
                Description = Description ?? string.Empty,
                LanguageHeaders = LanguageHeaders ?? string.Empty,
            }
        };
    }

    public Parser UseParser { get; set; } = Parser.Otava;

    public IWordCollectionService CollectionService { get; }
    public ILogger<IListGeneratorViewModel> Logger { get; }

    public IRelayCommand<string> Delete => new RelayCommand<string>(value =>
    {
        if (value is null)
        {
            throw new NotImplementedException($"{nameof(value)} should not be null");
        }

        Words.Remove(value);
        OnPropertyChanged(nameof(CanSave));
    });

    public IRelayCommand<string> Edit => new RelayCommand<string>(value =>
    {
        if (value is null)
        {
            throw new NotImplementedException($"{nameof(value)} should not be null");
        }

        int index = Words.IndexOf(value);
        EditWantedEvent?.Invoke(this, new(value, index));
    });

    public IRelayCommand New => new RelayCommand(() => AddWantedEvent?.Invoke(this, EventArgs.Empty));

    public enum Parser
    {
        Otava
    }

    private Dictionary<Parser, Func<string, List<string>>> StringParserActions { get; } = new()
    {
        [Parser.Otava] = (pairs) => { return new OtavaWordPairParser(pairs).ToStringList(); }
    };

    /// <summary>
    /// Try set string collection of specific index in Words ObservableCollection
    /// </summary>
    /// <param name="indexInList"></param>
    /// <param name="value"></param>
    /// <returns>boolean collection reprcenting if action was success</returns>
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
            Logger.LogWarning("Cannot set value '{value}' for word pair in index '{index}'. " +
                "Max index in this '{vmName}' instance is {maxIndex}.",
                value, indexInList, nameof(ListGeneratorViewModel), Words.Count);
            return false;
        }
    }

    public void AddWord(string result)
    {
        if (string.IsNullOrEmpty(result)) return;
        Words.Add(result);
        OnPropertyChanged(nameof(CanSave));
    }

    public event CollectionAddedEventHandler? CollectionAddedEvent;
    public event CollectionAddedEventHandler? FailedToSaveEvent;
    public event CollectionEditEventHandler? EditWantedEvent;
    public event AddWantedEventHandler? AddWantedEvent;
    public event CollectionAddedEventHandler? EditFinished;

    public void StartEditProcess(WordCollection collection)
    {
        IsEditMode = true;
        if (collection is null)
        {
            CollectionName = "Jotain meni pieleen...";
            Logger.LogError("Cannot edit WordCollection that is null in [{type}.{method}]",
                nameof(ListGeneratorViewModel), nameof(StartEditProcess));
            return;
        }
        Logger.LogInformation("Start editing {type} with name {name}",
            nameof(WordCollection), collection.Owner.Name);

        OldWordCollectionValue = collection;
        CollectionName = collection.Owner.Name;
        LanguageHeaders = collection.Owner.LanguageHeaders;
        Description = collection.Owner.Description;

        foreach (var wordPair in collection.WordPairs)
        {
            Words.Add(wordPair.NativeLanguageWord);
            Words.Add(wordPair.ForeignLanguageWord);
        }
        OnPropertyChanged(nameof(CanSave));
    }

    public IAsyncRelayCommand FinishEdit => new AsyncRelayCommand(async () =>
    {
        if (IsEditMode is false)
        {
            throw new InvalidOperationException("Cannot update word collection, when edit mode is not true");
        }
        IsBusy = true;

        var newData = ParseData();
        int oldId = OldWordCollectionValue?.Owner.Id ?? -1;
        if (oldId is not -1)
        {
            await CollectionService.DeleteWordCollection(oldId);
        }
        int newId = await CollectionService.AddWordCollection(newData);

        EditFinished?.Invoke(this, new()
        {
            RefIds = new[] { newId },
            CollectionNames = new[] { newData.Owner.Name },
            Text = "Successfully removed old word collection and added new edited one."
        });
        IsBusy = false;
    });
}

