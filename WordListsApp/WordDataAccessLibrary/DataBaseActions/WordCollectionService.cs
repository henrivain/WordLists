using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SQLite;
using System.Diagnostics;
using WordDataAccessLibrary.DataBaseActions.Interfaces;

namespace WordDataAccessLibrary.DataBaseActions;


public class WordCollectionService : IWordCollectionService
{
    public WordCollectionService(
        IWordCollectionOwnerService ownerService,
        IWordPairService pairService,
        ILogger<IWordCollectionService> logger)
    {
        OwnerService = ownerService;
        PairService = pairService;
        Logger = logger;
    }

    IWordCollectionOwnerService OwnerService { get; }
    IWordPairService PairService { get; }
    ILogger<IWordCollectionService> Logger { get; }

    SQLiteAsyncConnection _db;

    private async Task Init()
    {
        if (_db is not null) return;
        Logger.LogInformation("Initialize new {type}", nameof(WordCollectionService));

        string databasePath = Path.Combine(FileSystem.AppDataDirectory, DataBaseInfo.GetDataBaseName());

        Logger.LogInformation("Database is stored at {path}", databasePath);

        _db = new SQLiteAsyncConnection(databasePath);
        await _db.CreateTableAsync<WordCollectionOwner>();
        await _db.CreateTableAsync<WordPair>();

        Logger.LogInformation("Created tables for {type1} and {type2}", nameof(WordCollectionOwner), nameof(WordPair));
    }

    /// <summary>
    /// Add word collection to database
    /// </summary>
    /// <param name="collection"></param>
    /// <returns>database id of inserted collection</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<int> AddWordCollection(WordCollection collection)
    {
        if (collection is null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        await Init();
        await _db.InsertAsync(collection.Owner);
        await PairService.InsertPairsAsync(collection);

        Logger.LogInformation("Added new {type} with id '{id}'.", nameof(WordCollection), collection.Owner.Id);

        return collection.Owner.Id;
    }


    public async Task SaveProgression(WordCollection wordCollection)
    {
        await Init();

        Logger.LogInformation("Update {type} in database", nameof(WordCollection));

        await _db.UpdateAsync(wordCollection.Owner);

        await PairService.UpdatePairsAsync(wordCollection);
    }

    /// <summary>
    /// Get all WordCollections from database
    /// </summary>
    /// <returns>array of WordCollections</returns>
    public async Task<List<WordCollection>> GetWordCollections()
    {
        await Init();
        Logger.LogInformation("Get all word {type}s", nameof(WordCollection));

        List<WordCollection> result = new();

        List<WordCollectionOwner> owners = await OwnerService.GetAll();
        foreach (WordCollectionOwner owner in owners)
        {
            result.Add(
                new WordCollection()
                {
                    Owner = owner,
                    WordPairs = await PairService.GetByOwner(owner)
                });
        }
        return result;
    }

    /// <summary>
    /// Get one word collection from database with matching id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>first appearance of WordCollection 
    /// with id or null if does not exist</returns>
    public async Task<WordCollection> GetWordCollection(int id)
    {
        await Init();
        Logger.LogInformation("Get {type} with id '{id}'", nameof(WordCollection), id);


        WordCollectionOwner owner = await OwnerService.GetById(id);
        
        if (owner is null)
        {
            Logger.LogWarning("{type} with id '{id}' was not found.", nameof(WordCollection), id);
            return null;
        }

        List<WordPair> pairs = await PairService.GetByOwner(owner);
        return new(owner, pairs);
    }

    public async Task<List<WordCollection>> GetWordCollectionsById(int[] ids)
    {
        Logger.LogInformation("Get {type}s with ids: '{ids}'", 
            nameof(WordCollection), string.Join(", ", ids));
        List<WordCollection> result = new();
        foreach (int id in ids)
        {
            result.Add(await GetWordCollection(id));
        }
        return result.Where(x => x is not null).ToList();
    }

    /// <summary>
    /// Remove WordCollection with matching id (remove owner and all child word pairs with same id)
    /// </summary>
    /// <param name="ownerId"></param>
    /// <returns>number of objects deleted</returns>
    public async Task<int> DeleteWordCollection(int ownerId)
    {
        Logger.LogInformation("Delete {name} with id: {id}", nameof(WordCollection), ownerId);

        await Init();
        int countDeleted = await _db.DeleteAsync<WordCollectionOwner>(ownerId);
        countDeleted += await _db.Table<WordPair>()
                          .Where(x => x.OwnerId == ownerId)
                          .DeleteAsync();
        Logger.LogInformation("Deleted {count} objects from data base", countDeleted);

        return countDeleted;
    }

    /// <summary>
    /// WARNING!!!  DELETES ALL OBJECTS FROM DATABASE! execute by passing "true" as parameter, otherwise will not run
    /// </summary>
    /// <param name="verifyByTrue"></param>
    /// <returns>number of object deleted</returns>
    public async Task<int> DeleteAll(string verifyByTrue)
    {
        if (verifyByTrue.ToLower() != "true")
        {
            throw new InvalidOperationException("\"true\" must be passed. !WARNING! DELETES ALL OBJECTS FROM DATABASE");
        }

        await Init();
        Logger.LogInformation("Delete all {owner}s and {wordPair}s", nameof(WordCollectionOwner), nameof(WordPair));
        int removedObjects = await _db.DeleteAllAsync<WordCollectionOwner>();
        removedObjects += await _db.DeleteAllAsync<WordPair>();
        Logger.LogInformation("{service} deleted all {count} word colelctions successfully.", nameof(WordCollectionService), removedObjects);

        return removedObjects;
    }

    public async Task<int> CountItems()
    {
       return await OwnerService.CountItems();
    }

    
}
