namespace WordDataAccessLibrary.DataBaseActions;
internal static class DataBaseInfo
{
    internal static string GetDataBaseName()
    {
#if DEBUG
        return "WordListsDataDebug.db";
#else
        return "WordListsData.db";
#endif
    }
}
