using SQLite;
using System.Diagnostics;
using WordDataAccessLibrary.DataBaseActions.Interfaces;

namespace WordDataAccessLibrary.DataBaseActions;


public class WordPairService : IWordPairService
{
    SQLiteAsyncConnection db;

    private async Task Init()
    {
        if (db is not null) return;

        string databasePath = Path.Combine(FileSystem.AppDataDirectory, DataBaseInfo.GetDataBaseName());
        Debug.WriteLine($"{nameof(WordPairService)}: Initialize new");


        db = new SQLiteAsyncConnection(databasePath);

        await db.CreateTableAsync<WordPair>();
    }
    public async Task<List<WordPair>> GetAll()
    {
        Debug.WriteLine($"{nameof(WordPairService)}: Get all {nameof(WordPair)}s");

        await Init();
        return await db.Table<WordPair>().ToListAsync();
    }
    public async Task<List<WordPair>> GetByOwnerId(int ownerId)
    {
        Debug.WriteLine($"{nameof(WordPairService)}: Get by owner id: {ownerId}");

        await Init();
        return await db.Table<WordPair>()
            .Where(x => x.OwnerId == ownerId)
                .ToListAsync();
    }
    public async Task<List<WordPair>> GetByOwner(WordCollectionOwner owner)
    {
        Debug.WriteLine($"{nameof(WordPairService)}: Get by {nameof(WordCollectionOwner)}");

        return await GetByOwnerId(owner.Id);
    }
    public async Task<List<WordPair>> GetByIdAndLearnState(int ownerId, WordLearnState learnState)
    {
        await Init();

        Debug.WriteLine($"{nameof(WordPairService)}: Get all by {nameof(ownerId)} and {nameof(WordLearnState)}.{learnState}");

        return await db.Table<WordPair>()
            .Where(x => x.OwnerId == ownerId)
            .Where(x => x.LearnState == learnState)
            .ToListAsync();
    }
    public async Task UpdatePairsAsync(WordCollection collection)
    {
        await Init();

        Debug.WriteLine($"{nameof(WordPairService)}: Insert or update all word pairs of {nameof(WordCollection)}");

        foreach (WordPair pair in collection.WordPairs)
        {
            pair.OwnerId = collection.Owner.Id;
            await db.UpdateAsync(pair);
        }
    }
    public async Task InsertPairsAsync(WordCollection collection)
    {
        await Init();

        Debug.WriteLine($"{nameof(WordPairService)}: Insert all word pairs of {nameof(WordCollection)}");

        foreach (WordPair pair in collection.WordPairs)
        {
            pair.OwnerId = collection.Owner.Id;
            await db.InsertAsync(pair);
        }
    }
}
