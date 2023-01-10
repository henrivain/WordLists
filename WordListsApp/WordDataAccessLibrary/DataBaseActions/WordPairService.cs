using Microsoft.Extensions.Logging;
using SQLite;
using System.Linq.Expressions;
using WordDataAccessLibrary.DataBaseActions.Interfaces;

namespace WordDataAccessLibrary.DataBaseActions;

public class WordPairService : IWordPairService
{
    public WordPairService(ILogger<IWordPairService> logger)
    {
        Logger = logger;
    }

    SQLiteAsyncConnection _db;

    ILogger<IWordPairService> Logger { get; }

    private async Task Init()
    {
        if (_db is not null) return;

        string databasePath = Path.Combine(FileSystem.AppDataDirectory, DataBaseInfo.GetDataBaseName());
        Logger.LogInformation("Initialize new {wordPair} database at {path}.", nameof(WordPairService), databasePath);


        _db = new SQLiteAsyncConnection(databasePath);

        await _db.CreateTableAsync<WordPair>();
    }
    public async Task<List<WordPair>> GetAll()
    {
        Logger.LogInformation("Get all {wordPair}s.", nameof(WordPair));

        await Init();
        return await _db.Table<WordPair>().ToListAsync();
    }
    public async Task<List<WordPair>> GetByOwnerId(int ownerId)
    {
        Logger.LogInformation("Get {wordPair}s by owner id {id}.", nameof(WordPair), ownerId);

        await Init();
        return await _db.Table<WordPair>()
            .Where(x => x.OwnerId == ownerId)
                .ToListAsync();
    }
    public async Task<List<WordPair>> GetByOwner(WordCollectionOwner owner) => await GetByOwnerId(owner.Id);
    public async Task<List<WordPair>> GetByExpression(Expression<Func<WordPair, bool>> expression)
    {
        await Init();
        Logger.LogInformation("Get {wordPair}s that match given expression.", nameof(WordPair));
        return await _db.Table<WordPair>().Where(expression).ToListAsync();
    }

    public async Task UpdatePairsAsync(WordCollection collection)
    {
        await Init();

        Logger.LogInformation("Update all {wordPair}s from {collection}.", nameof(WordPair), nameof(WordCollection));

        foreach (WordPair pair in collection.WordPairs)
        {
            pair.OwnerId = collection.Owner.Id;
            await _db.UpdateAsync(pair);
        }
    }
    public async Task UpdatePairAsync(WordPair pair)
    {
        await Init();
        Logger.LogInformation("Update single {wordPair}.", nameof(WordPair));
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
            Logger.LogWarning("Failed to get {wordPair} by its primary key '{key}' " +
                "because of exception '{exType}': '{msg}'",
                nameof(WordPair), key, ex.GetType().Name, ex.Message);
            return null;
        }
    }

    public async Task InsertPairsAsync(WordCollection collection)
    {
        await Init();

        Logger.LogInformation("Insert all {wordPair}s of {collection}", nameof(WordPair), nameof(WordCollection));

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
