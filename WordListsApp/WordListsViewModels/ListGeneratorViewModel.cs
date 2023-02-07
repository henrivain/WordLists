using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.InteropServices;
using WordDataAccessLibrary.DataBaseActions.Interfaces;
using WordDataAccessLibrary.Generators;
using WordListsMauiHelpers.DeviceAccess;
using WordListsMauiHelpers.Settings;
using WordListsViewModels.Events;
using WordListsViewModels.Helpers;

namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class ListGeneratorViewModel : IListGeneratorViewModel
{
    public ListGeneratorViewModel(
        IWordCollectionService collectionService,
        ILogger<IListGeneratorViewModel> logger,
        IEnumerable<IWordPairParser> parsers, 
        ISettings settings)
    {
        CollectionService = collectionService;
        Logger = logger;
        Settings = settings;
        Parsers = parsers.Select(x => new ParserInfo { Name = GetParserName(x), Parser = x }).ToList();
        if (Parsers.Count < 1)
        {
            throw new ArgumentException("At least on parser must be defined.", nameof(parsers));
        }
        _selectedParser = Parsers.FirstOrGivenDefault(x => x.Name == (settings.DefaultParserName ?? ""), Parsers[0]);
        _languageHeaders = settings.DefaultWordCollectionLanguage ?? "fi-en";
        PropertyChanged += UpdateSettingValue;
    }


    IWordCollectionService CollectionService { get; }
    ILogger<IListGeneratorViewModel> Logger { get; }
    ISettings Settings { get; }
    public List<ParserInfo> Parsers { get; }

    [ObservableProperty]
    object _selectedParser;

    [ObservableProperty]
    ObservableCollection<string> _words = new();

    [ObservableProperty]
    string _collectionName = "My word collection";

    [ObservableProperty]
    string _description = "Description";

    [ObservableProperty]
    string _languageHeaders;

    [ObservableProperty]
    bool _isEditMode = false;

    [ObservableProperty]
    bool _isBusy = false;

    public bool ShowUnEvenWordCountWarning => Words.Count < 10 || Words.Count % 2 is 0;

    public bool CanSave => Words.Count / 2 > 0;

    private WordCollection? OldWordCollectionValue { get; set; }

    public IAsyncRelayCommand GeneratePairsCommand => new AsyncRelayCommand(
        async () =>
        {
            IsBusy = true;
            bool isAndroid = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

            IWordPairParser parser;
            if (SelectedParser is ParserInfo info)
            {
                parser = info.Parser;
            }
            else
            {
                parser = Parsers[0].Parser;
            }

            var words = await ParseFromClipBoard(parser);
            Words.Clear();
            foreach (var word in words)
            {
                Words.Add(word);
            }
            OnPropertyChanged(nameof(CanSave), nameof(ShowUnEvenWordCountWarning));
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

            Settings.DefaultWordCollectionLanguage = LanguageHeaders;
            CollectionAddedEvent?.Invoke(this, new DataBaseActionArgs
            {
                Text = "Added wordCollection successfully",
                RefIds = new[] { id },
                CollectionNames = new[] { CollectionName }
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

    public IRelayCommand<string> Delete => new RelayCommand<string>(value =>
    {
        if (value is null)
        {
            Logger.LogError("{vm}: Cannot delete word, because the word value is not defined.", nameof(ListGeneratorViewModel));
            throw new NotImplementedException($"{nameof(Delete)}: Parameter {nameof(value)} should not be null");
        }

        Words.Remove(value);
        OnPropertyChanged(nameof(CanSave), nameof(ShowUnEvenWordCountWarning));
    });

    public IRelayCommand<string> Edit => new RelayCommand<string>(value =>
    {
        if (value is null)
        {
            Logger.LogError("{vm}: Cannot edit word, because the word value is not defined.", nameof(ListGeneratorViewModel));
            throw new NotImplementedException($"{nameof(Edit)}: Parameter {nameof(value)} should not be null");
        }

        int index = Words.IndexOf(value);
        EditWantedEvent?.Invoke(this, new(value, index));
    });

    public IRelayCommand New => new RelayCommand(() => AddWantedEvent?.Invoke(this, EventArgs.Empty));

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

        Settings.DefaultWordCollectionLanguage = LanguageHeaders;
        EditFinished?.Invoke(this, new()
        {
            RefIds = new[] { newId },
            CollectionNames = new[] { newData.Owner.Name },
            Text = "Successfully removed old word collection and added new edited one."
        });
        IsBusy = false;
    });





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
        OnPropertyChanged(nameof(CanSave), nameof(ShowUnEvenWordCountWarning));
    }
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
        OnPropertyChanged(nameof(CanSave), nameof(ShowUnEvenWordCountWarning));
    }
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


    public event CollectionAddedEventHandler? CollectionAddedEvent;
    public event CollectionAddedEventHandler? FailedToSaveEvent;
    public event CollectionEditEventHandler? EditWantedEvent;
    public event AddWantedEventHandler? AddWantedEvent;
    public event ParserErrorEventHandler? ParserError;
    public event CollectionAddedEventHandler? EditFinished;



    private static string GetParserName(IWordPairParser parser)
    {
        return parser switch
        {
            NewOtavaWordPairParser => "Otava uusi",
            OtavaWordPairParser => "Otava vanha",
            _ => parser.GetType().Name,
        };
    }
    private async Task<string[]> ParseFromClipBoard(IWordPairParser parser)
    {
        try
        {
            string text = await ClipboardAccess.GetStringAsync();
            return await Task.Run(parser.ToStringList(text).ToArray);
        }
        catch (Exception ex)
        {
            Logger.LogInformation("Exception whilst trying to parse word list with '{parser}'. '{ex}': '{msg}'",
                parser.GetType().Name, ex.GetType().Name, ex.Message);

            ParserError?.Invoke(this, "Invalid parser error");
            return Array.Empty<string>();
        }
    }
    private void OnPropertyChanged(params string[] propertyNames)
    {
        if (propertyNames is null)
        {
            return;
        }
        foreach (var name in propertyNames)
        {
            OnPropertyChanged(name);
        }
    }
    private void UpdateSettingValue(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(SelectedParser):
                if (SelectedParser is ParserInfo info)
                {
                    Settings.DefaultParserName = info.Name;
                }
                return;
        }
    }
}

