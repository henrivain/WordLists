using Newtonsoft.Json;


namespace WordDataAccessLibrary.CollectionBackupServices;

/// <summary>
/// Removes id data and owner data from wordCollection for export
/// </summary>
public struct DefaultExportWordCollection : IExportWordCollection
{
    public DefaultExportWordCollection(){}

    public string Name
    {
        get => _name;
        set => _name = value is null ? string.Empty : value;
    }

    public string Description
    {
        get => _description;
        set => _description = value is null ? string.Empty : value;
    }

    public string LanguageHeaders
    {
        get => _languageHeaders;
        set => _languageHeaders = value is null ? string.Empty : value;
    }

    public ExportWordPair[] WordPairs
    {
        get => _wordPairs;
        set => _wordPairs = value ?? Array.Empty<ExportWordPair>();
    }


    public IExportWordCollection FromWordCollection(WordCollection collection)
    {
        if (collection is null) return null;
        FromWordCollectionOwner(collection.Owner);
        WordPairsFromList(collection.WordPairs);
        return this;
    }

    public IExportWordCollection FromWordCollectionOwner(WordCollectionOwner owner)
    {
        if (owner is null) return null;
        Name = owner.Name;
        Description = owner.Description;
        LanguageHeaders = owner.LanguageHeaders;
        return this;
    }

    public IExportWordCollection WordPairsFromList(List<WordPair> pairs)
    {
        if (pairs is null)
        {
            WordPairs = Array.Empty<ExportWordPair>();
            return null;
        }

        List<ExportWordPair> exportPairs = new();
        foreach (WordPair pair in pairs)
        {
            if (pair is null) continue;
            exportPairs.Add(ExportWordPair.FromWordPair(pair));
        }
        WordPairs = exportPairs.ToArray();
        return this;
    }

    public WordCollection GetAsWordCollection()
    {
        return new()
        {
            Owner = new()
            {
                Name = Name,
                Description = Description,
                LanguageHeaders = LanguageHeaders
            },
            WordPairs = WordPairs.Select(x => x.ToWordPair()).ToList()
        };
    }

    /// <param name="objectAsJson"></param>
    /// <returns>JsonWordCollection matching string, null if argument null or otherwise bad</returns>
    public static IExportWordCollection ParseFromJson(string objectAsJson)
    {
        if (objectAsJson is null) return null;
        try
        {
            return JsonConvert.DeserializeObject<DefaultExportWordCollection>(objectAsJson);
        }
        catch (JsonReaderException)
        {
            return null;
        }
    }



    string _name = string.Empty;
    string _description = string.Empty;
    string _languageHeaders = string.Empty;
    ExportWordPair[] _wordPairs = Array.Empty<ExportWordPair>();

   
}
