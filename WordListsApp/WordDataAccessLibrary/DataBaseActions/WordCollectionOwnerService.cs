using SQLite;
using System.Diagnostics;
using WordDataAccessLibrary.DataBaseActions.Interfaces;

namespace WordDataAccessLibrary.DataBaseActions;

/// <summary>
/// This class implements only getters, because WordCollectionOwners should be removed only as a part of WordCollection
/// <para/> See => WordCollectionService
/// </summary>
public class WordCollectionOwnerService : IWordCollectionOwnerService
{
    static SQLiteAsyncConnection db;

    private async Task Init()
    {
        if (db is not null) return;

        Debug.WriteLine($"{nameof(WordCollectionOwnerService)}: Initialize new");

        string databasePath = Path.Combine(FileSystem.AppDataDirectory, DataBaseInfo.GetDataBaseName());

        Debug.WriteLine($"{nameof(WordCollectionOwnerService)}: Database at path: {databasePath}");

        db = new SQLiteAsyncConnection(databasePath);

        await db.CreateTableAsync<WordCollectionOwner>();

        Debug.WriteLine($"{nameof(WordCollectionOwnerService)}: Table created");

    }
    public async Task<List<WordCollectionOwner>> GetAll()
    {
        await Init();

        Debug.WriteLine($"{nameof(WordCollectionOwnerService)}: Get all {nameof(WordCollectionOwner)}s");

        return await db.Table<WordCollectionOwner>().ToListAsync();
    }
    public async Task<List<WordCollectionOwner>> GetByLanguage(string languageHeaders)
    {
        await Init();

        languageHeaders = languageHeaders.Trim().ToLower();
        Debug.WriteLine($"{nameof(WordCollectionOwnerService)}: Get all containing language headers: {languageHeaders}");

        return await db.Table<WordCollectionOwner>()
            .Where(x => x.LanguageHeaders
                .ToLower()
                    .Contains(languageHeaders))
                        .ToListAsync();
    }
    public async Task<List<WordCollectionOwner>> GetByName(string name)
    {
        await Init();

        name = name.Trim().ToLower();
        Debug.WriteLine($"{nameof(WordCollectionOwnerService)}: Get all containing name: {name}");

        return await db.Table<WordCollectionOwner>()
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

        Debug.WriteLine($"{nameof(WordCollectionOwnerService)}: Get by id: {id}");


        try
        {
            return await db.GetAsync<WordCollectionOwner>(id);
        }
        catch
        {
            Debug.WriteLine($"{nameof(WordCollectionOwnerService)}: Get by id: Collection Not Found => return null");
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
        return await db.Table<WordCollectionOwner>().CountAsync();
    }
}
