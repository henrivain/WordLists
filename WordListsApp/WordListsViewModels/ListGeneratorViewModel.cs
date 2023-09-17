using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using WordDataAccessLibrary.DataBaseActions.Interfaces;
using WordDataAccessLibrary.Generators;
using WordListsMauiHelpers.Settings;
using WordListsViewModels.Events;
using WordListsViewModels.Helpers;

namespace WordListsViewModels;

public partial class ListGeneratorViewModel : ObservableObject, IListGeneratorViewModel
{
    public ListGeneratorViewModel(
        IWordCollectionService collectionService,
        ILogger<IListGeneratorViewModel> logger,
        IEnumerable<IWordPairParser> parsers,
        ISettings settings,
        IClipboard clipboard
        )
    {
        CollectionService = collectionService;
        Logger = logger;
        Settings = settings;
        Clipboard = clipboard;
        Parsers = parsers.ToParserInfos().ToList();
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
    IClipboard Clipboard { get; }



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

    // Database commands 
    public IAsyncRelayCommand GeneratePairsCommand => new AsyncRelayCommand(
        async () =>
        {
            IsBusy = true;

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
            await ResetWordPairs(words);
            IsBusy = false;
        });
    public IAsyncRelayCommand Save => new AsyncRelayCommand(async () =>
    {
        if (IsEditMode)
        {
            await SaveEdit();
            return;
        }
        await SaveNew();
    });

    // List commands
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

    // Word commands
    public IRelayCommand<string> Delete => new RelayCommand<string>(value =>
    {
        if (value is null)
        {
            Logger.LogError("{vm}: Cannot delete word, because the word value is not defined.", nameof(ListGeneratorViewModel));
            throw new NotImplementedException($"{nameof(Delete)}: Parameter {nameof(value)} should not be null");
        }

        Words.Remove(value);
        NotifyChanged(nameof(CanSave), nameof(ShowUnEvenWordCountWarning));
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




    // public methods
    public bool SetWordValueWithIndex(int indexInList, string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

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
    public void AddWord(string word)
    {
        if (string.IsNullOrEmpty(word))
        {
            return;
        }

        Words.Add(word);
        NotifyChanged(nameof(CanSave), nameof(ShowUnEvenWordCountWarning));
    }

   
    public async Task ResetWordPairs(IEnumerable<string> words)
    {
        int i = 0;
        Words.Clear();
        foreach (var word in words)
        {
            i++;
            if (i % 20 is 0)
            {
                await Task.Delay(500);
            }
            Words.Add(word);
        }
        NotifyChanged(nameof(CanSave), nameof(ShowUnEvenWordCountWarning));
    }
    public async Task OpenInEditMode(WordCollection collection)
    {
        IsEditMode = true;
        if (collection is null)
        {
            CollectionName = "Jotain meni pieleen...";
            Logger.LogError("Cannot edit WordCollection that is null in [{type}.{method}]",
                nameof(ListGeneratorViewModel), nameof(OpenInEditMode));
            return;
        }
        Logger.LogInformation("Start editing {type} with name {name}",
            nameof(WordCollection), collection.Owner.Name);

        OldWordCollectionValue = collection;
        CollectionName = collection.Owner.Name;
        LanguageHeaders = collection.Owner.LanguageHeaders;
        Description = collection.Owner.Description;


        List<string> words = new();
        foreach (var wordPair in collection.WordPairs)
        {
            words.Add(wordPair.NativeLanguageWord);
            words.Add(wordPair.ForeignLanguageWord);
        }
        await ResetWordPairs(words);

        NotifyChanged(nameof(CanSave), nameof(ShowUnEvenWordCountWarning));
    }
    public WordCollection ParseToWordCollection()
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

    // Events
    public event CollectionAddedEventHandler? CollectionAddedEvent;
    public event CollectionAddedEventHandler? FailedToSaveEvent;
    public event CollectionEditEventHandler? EditWantedEvent;
    public event AddWantedEventHandler? AddWantedEvent;
    public event ParserErrorEventHandler? ParserError;
    public event CollectionAddedEventHandler? EditFinished;


    // private methods
    private async Task<string[]> ParseFromClipBoard(IWordPairParser parser)
    {
        try
        {
            string text = await Clipboard.GetTextAsync() ?? string.Empty;
            return await Task.Run(parser.ToStringList(text).ToArray);
        }
        catch (Exception ex)
        {
            Logger.LogWarning("Exception whilst trying to parse word list with '{parser}'. '{ex}': '{msg}'",
                parser.GetType().Name, ex.GetType().Name, ex.Message);

            ParserError?.Invoke(this, "Invalid parser error");
            return Array.Empty<string>();
        }
    }
    private void NotifyChanged(params string[] propertyNames)
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
                if (SelectedParser is ParserInfo info && Settings.DefaultParserName != info.Name)
                {
                    Settings.DefaultParserName = info.Name;
                }
                return;
        }
    }
    private async Task SaveNew()
    {
        if (IsEditMode)
        {
            Logger.LogWarning("Used {mth} when edit mode was TRUE, " +
                "consider using {mth2} instead to update database", nameof(SaveNew), nameof(SaveEdit));
        }

        IsBusy = true;
        if (Words.Count < 2)
        {
            Logger.LogWarning("{cls}.{method}: Can't add empty word collection",
                nameof(ListGeneratorViewModel), nameof(SaveNew));
            FailedToSaveEvent?.Invoke(this, new DataBaseActionArgs
            {
                Text = "Collection was empty or had less than 2 words."
            });
            IsBusy = false;
            return;
        }
        int id = await CollectionService.AddWordCollection(ParseToWordCollection());

        Settings.DefaultWordCollectionLanguage = LanguageHeaders;
        CollectionAddedEvent?.Invoke(this, new DataBaseActionArgs
        {
            Text = "Added wordCollection successfully",
            RefIds = new[] { id },
            CollectionNames = new[] { CollectionName }
        });
        IsBusy = false;
    }
    private async Task SaveEdit()
    {
        if (IsEditMode is false)
        {
            Logger.LogError("Used {mth} when edit mode was FALSE, cannot update non existent collection.",
                nameof(SaveEdit));
            throw new InvalidOperationException("Cannot update word collection, when edit mode is false");
        }
        IsBusy = true;
        var newData = ParseToWordCollection();
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
    }
}

