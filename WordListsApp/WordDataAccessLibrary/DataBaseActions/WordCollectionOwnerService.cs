using Microsoft.Extensions.Logging;
using SQLite;
using WordDataAccessLibrary.DataBaseActions.Interfaces;

namespace WordDataAccessLibrary.DataBaseActions;

/// <summary>
/// This class implements only getters, because WordCollectionOwners should be removed only as a part of WordCollection
/// <para/> See => WordCollectionService
/// </summary>
public class WordCollectionOwnerService : IWordCollectionOwnerService
{
    public WordCollectionOwnerService(ILogger<IWordCollectionOwnerService> logger)
    {
        Logger = logger;
    }

    static SQLiteAsyncConnection _db;
    private ILogger<IWordCollectionOwnerService> Logger { get; }

    private async Task Init()
    {
        if (_db is not null) return;

        Logger.LogInformation("Initialize new {service}", nameof(WordCollectionOwnerService));

        string databasePath = Path.Combine(FileSystem.AppDataDirectory, DataBaseInfo.GetDataBaseName());

        Logger.LogInformation("Database at path: {path}", databasePath);

        _db = new SQLiteAsyncConnection(databasePath);

        await _db.CreateTableAsync<WordCollectionOwner>();

        Logger.LogInformation("Table created in {service}", nameof(WordCollectionOwnerService));

    }
    public async Task<List<WordCollectionOwner>> GetAll()
    {
        await Init();

        Logger.LogInformation("Get all {data}s", nameof(WordCollectionOwner));

        return await _db.Table<WordCollectionOwner>().ToListAsync();
    }
    public async Task<List<WordCollectionOwner>> GetByLanguage(string languageHeaders)
    {
        await Init();

        languageHeaders = languageHeaders.Trim().ToLower();
        Logger.LogInformation("Get {data}s all containing language headers: {hdrs}", 
            nameof(WordCollectionOwner), languageHeaders);

        return await _db.Table<WordCollectionOwner>()
            .Where(x => x.LanguageHeaders
                .ToLower()
                    .Contains(languageHeaders))
                        .ToListAsync();
    }
    public async Task<List<WordCollectionOwner>> GetByName(string name)
    {
        await Init();

        name = name.Trim().ToLower();
        Logger.LogInformation("Get all {data}s containing name: {name}", nameof(WordCollectionOwner), name);

        return await _db.Table<WordCollectionOwner>()
            .Where(x => x.Name
                .ToLower()
                    .Contains(name))
                        .ToListAsync();
    }

    /// <summary>
    /// Get by WordCollectionOwner id (primary key)
    /// </summary>
    /// <param name="id"></param>
    /// <returns>WordCollectionOwner if found, else null</returns>
    public async Task<WordCollectionOwner> GetById(int id)
    {
        await Init();

        Logger.LogInformation("Get {data}s by id: {id}", nameof(WordCollectionOwner), id);

        try
        {
            return await _db.GetAsync<WordCollectionOwner>(id);
        }
        catch
        {
            Logger.LogError("Get {data}s by id: Collection Not Found => return null", nameof(WordCollectionOwner));
            #if DEBUG
            throw;
            #else
            return null;
            #endif
        }
    }
    public async Task<int> CountItems()
    {
        await Init();
        return await _db.Table<WordCollectionOwner>().CountAsync();
    }
}
