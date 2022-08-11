using SQLite;
using System.Diagnostics;
using WordDataAccessLibrary.DataBaseActions.Interfaces;

namespace WordDataAccessLibrary.DataBaseActions;


public class WordCollectionService : IWordCollectionService
{
    public WordCollectionService(IWordCollectionOwnerService ownerService, IWordPairService pairService)
    {
        OwnerService = ownerService;
        PairService = pairService;
    }

    IWordCollectionOwnerService OwnerService { get; }
    IWordPairService PairService { get; }

    SQLiteAsyncConnection db;

    private async Task Init()
    {
        if (db is not null) return;
        Debug.WriteLine($"Initialize {nameof(WordCollectionService)}");

        string databasePath = Path.Combine(FileSystem.AppDataDirectory, DataBaseInfo.GetDataBaseName());

        Debug.WriteLine($"Database is stored at {databasePath}");

        db = new SQLiteAsyncConnection(databasePath);
        await db.CreateTableAsync<WordCollectionOwner>();
        await db.CreateTableAsync<WordPair>();

        Debug.WriteLine($"{nameof(WordCollectionService)}: Tables created");

    }

    /// <summary>
    /// Add word collection to database
    /// </summary>
    /// <param name="collection"></param>
    /// <returns>database id of inserted collection</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<int> AddWordCollection(WordCollection collection)
    {
        if (collection is null) throw new ArgumentNullException(nameof(collection));
        await Init();
        await db.InsertAsync(collection.Owner);
        await PairService.InsertPairsAsync(collection);

        Debug.WriteLine($"{nameof(WordCollectionService)}: Added new collection with id: {collection.Owner.Id}");

        return collection.Owner.Id;
    }


    public async Task SaveProgression(WordCollection wordCollection)
    {
        await Init();

        Debug.WriteLine($"{nameof(WordCollectionService)}: Update {nameof(WordCollection)}");

        await db.UpdateAsync(wordCollection.Owner);

        await PairService.UpdatePairsAsync(wordCollection);
    }

    /// <summary>
    /// Get all WordCollections from database
    /// </summary>
    /// <returns>array of WordCollections</returns>
    public async Task<List<WordCollection>> GetWordCollections()
    {
        await Init();
        Debug.WriteLine($"{nameof(WordCollectionService)}: Get all word collections");

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
        Debug.WriteLine($"{nameof(WordCollectionService)}: Get collection with id: {id}");


        WordCollectionOwner owner = await OwnerService.GetById(id);
        
        if (owner is null)
        {
            Debug.WriteLine($"{nameof(WordCollectionService)}: {nameof(WordCollectionOwner)} with id {id} was not found");
            return null;
        }

        List<WordPair> pairs = await PairService.GetByOwner(owner);
        return new(owner, pairs);
    }

    public async Task<List<WordCollection>> GetWordCollectionsById(int[] ids)
    {
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
    /// <param name="OwnerId"></param>
    /// <returns>number of objects deleted</returns>
    public async Task<int> DeleteWordCollection(int OwnerId)
    {
        Debug.WriteLine($"{nameof(WordCollectionService)}: Delete collection with id: {OwnerId}");


        await Init();
        int objectsDeleted = await db.DeleteAsync<WordCollectionOwner>(OwnerId);
        objectsDeleted += await db.Table<WordPair>()
                          .Where(x => x.OwnerId == OwnerId)
                          .DeleteAsync();
        return objectsDeleted;
    }

    /// <summary>
    /// WARNING!!!  DELETES ALL OBJECTS FROM DATABASE! execute by passing "true" as parameter, otherwise will not run
    /// </summary>
    /// <param name="verifyByTrue"></param>
    /// <returns>number of object deleted</returns>
    public async Task<int> DeleteAll(string verifyByTrue)
    {
        if (verifyByTrue.ToLower() != "true") throw new InvalidOperationException("\"true\" must be passed. !WARNING! DELETES ALL OBJECTS FROM DATABASE");
        await Init();
        Debug.WriteLine($"{nameof(WordCollectionService)}: Delete all {nameof(WordCollectionOwner)}s and {nameof(WordPair)}s");
        int removedObjects = await db.DeleteAllAsync<WordCollectionOwner>();
        removedObjects += await db.DeleteAllAsync<WordPair>();
        Debug.WriteLine($"{nameof(WordCollectionService)}: Delete completed");

        return removedObjects;
    }

    public async Task<int> CountItems()
    {
       return await OwnerService.CountItems();
    }

    
}
