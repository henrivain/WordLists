using SQLite;
using System.Diagnostics;

namespace WordDataAccessLibrary.DataBaseActions;

/// <summary>
/// This class implements only getters, because WordCollectionOwners should be removed only as a part of WordCollection
/// <para/> See => WordCollectionService
/// </summary>
public static class WordCollectionOwnerService
{
    static SQLiteAsyncConnection db;

    private static async Task Init()
    {
        if (db is not null) return;

        Debug.WriteLine($"{nameof(WordCollectionOwnerService)}: Initialize new");

        string databasePath = Path.Combine(FileSystem.AppDataDirectory, DataBaseInfo.GetDataBaseName());

        Debug.WriteLine($"{nameof(WordCollectionOwnerService)}: Database at path: {databasePath}");

        db = new SQLiteAsyncConnection(databasePath);

        await db.CreateTableAsync<WordCollectionOwner>();

        Debug.WriteLine($"{nameof(WordCollectionOwnerService)}: Table created");

    }
    public static async Task<List<WordCollectionOwner>> GetAll()
    {
        await Init();

        Debug.WriteLine($"{nameof(WordCollectionOwnerService)}: Get all {nameof(WordCollectionOwner)}s");

        return await db.Table<WordCollectionOwner>().ToListAsync();
    }
    public static async Task<List<WordCollectionOwner>> GetByLanguage(string languageHeaders)
    {
        await Init();

        Debug.WriteLine($"{nameof(WordCollectionOwnerService)}: Get all containing language headers: {languageHeaders}");

        return await db.Table<WordCollectionOwner>()
            .Where(x => x.LanguageHeaders.Contains(languageHeaders.Trim()))
                .ToListAsync();
    }
    public static async Task<List<WordCollectionOwner>> GetByName(string name)
    {
        await Init();
     
        Debug.WriteLine($"{nameof(WordCollectionOwnerService)}: Get all containing name: {name}");

        return await db.Table<WordCollectionOwner>()
            .Where(x => x.Name.Contains(name))
                .ToListAsync();
    }
    /// <summary>
    /// Get by WordCollectionOwner id (primary key)
    /// </summary>
    /// <param name="id"></param>
    /// <returns>WordCollectionOwner if found, else null</returns>
    public static async Task<WordCollectionOwner> GetById(int id)
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

}
