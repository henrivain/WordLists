﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordDataAccessLibrary.DataBaseActions;


public static class WordPairService
{
    static SQLiteAsyncConnection db;

    private static async Task Init()
    {
        if (db is not null) return;

        string databasePath = Path.Combine(FileSystem.AppDataDirectory, DataBaseInfo.GetDataBaseName());
        Debug.WriteLine($"{nameof(WordPairService)}: Initialize new");


        db = new SQLiteAsyncConnection(databasePath);

        await db.CreateTableAsync<WordPair>();
    }
    public static async Task<List<WordPair>> GetAll()
    {
        Debug.WriteLine($"{nameof(WordPairService)}: Get all {nameof(WordPair)}s");

        await Init();
        return await db.Table<WordPair>().ToListAsync();
    }
    public static async Task<List<WordPair>> GetByOwnerId(int ownerId)
    {
        Debug.WriteLine($"{nameof(WordPairService)}: Get by owner id: {ownerId}");

        await Init();
        return await db.Table<WordPair>()
            .Where(x => x.OwnerId == ownerId)
                .ToListAsync();
    }
    public static async Task<List<WordPair>> GetByOwner(WordCollectionOwner owner)
    {
        Debug.WriteLine($"{nameof(WordPairService)}: Get by {nameof(WordCollectionOwner)}");

        return await GetByOwnerId(owner.Id);
    }
    public static async Task<List<WordPair>> GetByIdAndLearnState(int ownerId, WordLearnState learnState)
    {
        await Init();

        Debug.WriteLine($"{nameof(WordPairService)}: Get all by {nameof(ownerId)} and {nameof(WordLearnState)}.{learnState}");

        return await db.Table<WordPair>()
            .Where(x => x.OwnerId == ownerId)
            .Where(x => x.LearnState == learnState)
            .ToListAsync();
    }


    public static async Task InsertPairsAsync(WordCollection collection)
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