// Copyright 2022 Henri Vainio 
using SQLite;
using System.Diagnostics;

namespace WordDataAccessLibrary.DataBaseActions;


public static class WordCollectionService
{

    /// <summary>
    /// Add word collection to database
    /// </summary>
    /// <param name="collection"></param>
    /// <returns>database id of inserted collection</returns>
    public static async Task<int> AddWordCollection(WordCollection collection)
    {
        if (collection is null) throw new ArgumentNullException(nameof(collection));
        await Init();
        await db.InsertAsync(collection.Owner);
        await InsertAllWordPairsAsync(collection);
        return collection.Owner.Id;
    }

    /// <summary>
    /// Get all WordCollections from database
    /// </summary>
    /// <returns>array of WordCollections</returns>
    public static async Task<List<WordCollection>> GetWordCollections()
    {
        await Init();
        List<WordCollection> result = new();

        List<WordCollectionOwner> owners = await db.Table<WordCollectionOwner>().ToListAsync();
        foreach (WordCollectionOwner owner in owners)
        {
            result.Add(
                new WordCollection()
                {
                    Owner = owner,
                    WordPairs = await GetChildWordPairsAsync(owner)
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
    public static async Task<WordCollection> GetWordCollection(int id)
    {
        await Init();
        
        WordCollectionOwner owner = await db.Table<WordCollectionOwner>()
                                            .FirstOrDefaultAsync(o => o.Id == id);

        List<WordPair> pairs = await GetChildWordPairsAsync(owner);

        return new()
        {
            Owner = owner,
            WordPairs = pairs
        };
    }

    /// <summary>
    /// Remove WordCollection with matching id (remove owner and all child word pairs with same id)
    /// </summary>
    /// <param name="OwnerId"></param>
    /// <returns>awaitable task</returns>
    public static async Task DeleteWordCollection(int OwnerId)
    {
        await Init();
        await db.DeleteAsync<WordCollectionOwner>(OwnerId);
        await db.Table<WordPair>()
                .Where(x => x.OwnerId == OwnerId)
                .DeleteAsync();
    }

    /// <summary>
    /// WARNING!!!  DELETES ALL OBJECTS FROM DATABASE! execute by passing "true" as parameter, otherwise will not run
    /// </summary>
    /// <param name="verifyByTrue"></param>
    /// <returns>number of object deleted</returns>
    public static async Task<int> DeleteAll(string verifyByTrue)
    {
        if (verifyByTrue.ToLower() != "true") throw new InvalidOperationException("\"true\" must be passed. !WARNING! DELETES ALL OBJECTS FROM DATABASE");
        await Init();

        int removedObjects = 0;
        removedObjects += await db.DeleteAllAsync<WordCollectionOwner>();
        removedObjects += await db.DeleteAllAsync<WordPair>();
        return removedObjects;
    }


    #region PrivateMembers
    static SQLiteAsyncConnection db;

    private static async Task Init()
    {
        if (db is not null) return;

        string databasePath = Path.Combine(FileSystem.AppDataDirectory, GetDataBaseName());
        Debug.WriteLine($"Database is stored at {databasePath}");

        db = new SQLiteAsyncConnection(databasePath);

        await db.CreateTableAsync<WordCollectionOwner>();
        await db.CreateTableAsync<WordPair>();
    }

    private static async Task InsertAllWordPairsAsync(WordCollection collection)
    {
        foreach (WordPair pair in collection.WordPairs)
        {
            pair.OwnerId = collection.Owner.Id;
            await db.InsertAsync(pair);
        }
    }

    private static async Task<List<WordPair>> GetChildWordPairsAsync(WordCollectionOwner owner)
    {
        return await db.Table<WordPair>()
                       .Where(p => p.OwnerId == owner.Id)
                       .ToListAsync();
    }

    private static string GetDataBaseName()
    {
    #if DEBUG
        return "WordListsDataDebug.db";
    #else
        return "WordListsData.db";
    #endif
    }
    #endregion
}
