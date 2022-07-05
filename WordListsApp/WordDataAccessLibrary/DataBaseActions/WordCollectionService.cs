// Copyright 2022 Henri Vainio 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace WordDataAccessLibrary.DataBaseActions;


public class WordCollectionService
{
    static SQLiteAsyncConnection DataBase;

    /// <summary>
    /// Initialize new database if not already initialized
    /// </summary>
    /// <param name="databaseName"></param>
    /// <returns>awaitable task</returns>
    public static async Task Init(string databaseName = "WordListsData.db")
    {
        if (DataBase is null) return;

        string databasePath = Path.Combine(FileSystem.AppDataDirectory, databaseName);

        DataBase = new SQLiteAsyncConnection(databasePath);

        await DataBase.CreateTableAsync<WordCollection>();
    }

    /// <summary>
    /// Add word collection to database
    /// </summary>
    /// <param name="collection"></param>
    /// <returns>database id of inserted collection</returns>
    public static async Task<int> AddWordCollection(WordCollection collection)
    {
        await Init();
        return await DataBase.InsertAsync(collection);
    }

    /// <summary>
    /// Remove WordCollection with matching id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>awaitable task</returns>
    public static async Task RemoveWordCollection(int id)
    {
        await Init();
        await DataBase.DeleteAsync<WordCollection>(id);
    }

    /// <summary>
    /// Get all WordCollections from database
    /// </summary>
    /// <returns>array of WordCollections</returns>
    public static async Task<WordCollection[]> GetWordCollections()
    {
        await Init();

        return await DataBase.Table<WordCollection>().ToArrayAsync();
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

        return await DataBase.Table<WordCollection>()
            .FirstOrDefaultAsync(w => w.Id == id);
    }
}
