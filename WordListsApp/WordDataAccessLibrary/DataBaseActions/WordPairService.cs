using SQLite;
using System.Diagnostics;
using WordDataAccessLibrary.DataBaseActions.Interfaces;
using System.Linq.Expressions;

namespace WordDataAccessLibrary.DataBaseActions;

public class WordPairService : IWordPairService
{
    SQLiteAsyncConnection _db;

    private async Task Init()
    {
        if (_db is not null) return;

        string databasePath = Path.Combine(FileSystem.AppDataDirectory, DataBaseInfo.GetDataBaseName());
        Debug.WriteLine($"{nameof(WordPairService)}: Initialize new");


        _db = new SQLiteAsyncConnection(databasePath);

        await _db.CreateTableAsync<WordPair>();
    }
    public async Task<List<WordPair>> GetAll()
    {
        Debug.WriteLine($"{nameof(WordPairService)}: Get all {nameof(WordPair)}s");

        await Init();
        return await _db.Table<WordPair>().ToListAsync();
    }
    public async Task<List<WordPair>> GetByOwnerId(int ownerId)
    {
        Debug.WriteLine($"{nameof(WordPairService)}: Get by owner id: {ownerId}");

        await Init();
        return await _db.Table<WordPair>()
            .Where(x => x.OwnerId == ownerId)
                .ToListAsync();
    }
    public async Task<List<WordPair>> GetByOwner(WordCollectionOwner owner) => await GetByOwnerId(owner.Id);
    public async Task<List<WordPair>> GetByExpression(Expression<Func<WordPair, bool>> expression)
    {
        await Init();
        Debug.WriteLine($"Get {nameof(WordPair)}s by expression");
        return await _db.Table<WordPair>().Where(expression).ToListAsync();
    }

    public async Task UpdatePairsAsync(WordCollection collection)
    {
        await Init();

        Debug.WriteLine($"{nameof(WordPairService)}: Insert or update all word pairs of {nameof(WordCollection)}");

        foreach (WordPair pair in collection.WordPairs)
        {
            pair.OwnerId = collection.Owner.Id;
            await _db.UpdateAsync(pair);
        }
    }
    public async Task UpdatePairAsync(WordPair pair)
    {
        await Init();
        Debug.WriteLine($"{nameof(WordPairService)}: Insert or update single {nameof(WordPair)}");
        await _db.UpdateAsync(pair);
    }

    /// <summary>
    ///  Get word pair by its primary key
    /// </summary>
    /// <param name="key"></param>
    /// <returns>word pair if has match, else NULL</returns>
    public async Task<WordPair> GetByPrimaryKey(int key)
    {
        await Init();
        try
        {
            return await _db.GetAsync<WordPair>(key);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"{nameof(WordPairService)}: Failed to get database item by its primary key '{key}' because of exception '{ex.GetType()}': '{ex.Message}'");
            return null;
        }
    }

    public async Task InsertPairsAsync(WordCollection collection)
    {
        await Init();

        Debug.WriteLine($"{nameof(WordPairService)}: Insert all word pairs of {nameof(WordCollection)}");

        foreach (WordPair pair in collection.WordPairs)
        {
            pair.OwnerId = collection.Owner.Id;
            await _db.InsertAsync(pair);
        }
    }
    public async Task<int> CountItems()
    {
        await Init();
        return await _db.Table<WordPair>().CountAsync();
    }
    public async Task<int> CountItemsMatching(Expression<Func<WordPair, bool>> expression)
    {
        await Init();
        return await _db.Table<WordPair>().Where(expression).CountAsync();
    }
}
