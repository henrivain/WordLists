using SQLite;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace WordDataAccessLibrary.DataBaseActions;


public class TesterCollection
{
    public TesterCollection() { }   
    public TesterCollection(Description description, List<Tester> testers) 
    {
        Description = description;
        Testers = testers;
    }

    public Description Description { get; set; } = new();

    public List<Tester> Testers { get; set; } = new();
}

public class Tester
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Word { get; set; }

    public int Owner { get; set; } = 0;

    private int WordLearnStateId { get; set; } = (int)WordLearnState.NeverHeard;

    [EnumDataType(typeof(WordLearnState))]
    public WordLearnState LearnState 
    { 
        get => (WordLearnState)WordLearnStateId;
        set => WordLearnStateId = (int)value;
    }

}

public class Description
{
    public Description() { }

    public Description(string name) 
    { 
        Name = name;
    }

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Name { get; set; } = "Joe";

    public override string ToString() => Name;
}



public static class TestService
{
    static SQLiteAsyncConnection db;

    /// <summary>
    /// Initialize new database if not already initialized
    /// </summary>
    /// <returns>awaitable task</returns>
    private static async Task Init()
    {
        if (db is not null) return;

        string databasePath = Path.Combine(FileSystem.AppDataDirectory, "WordListsData.db");
        Debug.WriteLine($"Database is stored at {databasePath}");

        db = new SQLiteAsyncConnection(databasePath);

        await db.CreateTableAsync<Tester>();
        await db.CreateTableAsync<Description>();
    }

    /// <summary>
    /// Add word collection to database
    /// </summary>
    /// <param name="tester"></param>
    /// <returns>database id of inserted collection</returns>
    public static async Task<int> Add(Tester tester)
    {
        if (tester is null) throw new ArgumentNullException(nameof(tester));
        await Init();
        await db.InsertAsync(tester);
        return tester.Id;
    }

    /// <summary>
    /// Remove WordCollection with matching id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>awaitable task</returns>
    public static async Task RemoveWordCollection(int id)
    {
        await Init();
        await db.DeleteAsync<Tester>(id);
    }

    /// <summary>
    /// Get all WordCollections from database
    /// </summary>
    /// <returns>array of WordCollections</returns>
    public static async Task<Tester[]> GetWordCollections()
    {
        await Init();
        return await db.Table<Tester>().ToArrayAsync();
    }

    /// <summary>
    /// Get one word collection from database with matching id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>first appearance of WordCollection 
    /// with id or null if does not exist</returns>
    public static async Task<Tester> Get(int id)
    {
        await Init();

        return await db.Table<Tester>()
            .FirstOrDefaultAsync(w => w.Id == id);
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
        return await db.DeleteAllAsync<Tester>();
    }







    public static async Task<int> InsertCollection(TesterCollection collection)
    {
        await Init();
        await db.InsertAsync(collection.Description);
        foreach (Tester tester in collection.Testers)
        {
            tester.Owner = collection.Description.Id;
            await db.InsertAsync(tester);
        }
        return collection.Description.Id;
    }

    public static async Task<TesterCollection> GetByTag(int ownerId)
    {
        await Init();
        Description description = await db.GetAsync<Description>(ownerId);
        List<Tester> testers = await db.Table<Tester>().Where(x => x.Owner == ownerId).ToListAsync();
        return new(description, testers);
    }
}

