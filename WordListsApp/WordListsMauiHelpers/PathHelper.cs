namespace WordListsMauiHelpers;
public class PathHelper
{
    /// <summary>
    /// Returns default export path for used paltform
    /// </summary>
    /// <returns>downloads folder on windows, else path to myDocuments</returns>
    public static string GetDefaultExportFolderPath()
    {
#if WINDOWS
        return Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), "Downloads");
#else
        return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
#endif
    }

    /// <summary>
    /// Returns default export path for used platform with filename WordListsExport.json
    /// </summary>
    /// <returns>downloads folder on windows, else path to myDocuments</returns>
    public static string GetDefaultExportFilePath()
    {
        string date = DateTime.Now.ToString("G")
                                  .Replace(" ", string.Empty)
                                  .Replace(".", string.Empty);
        return Path.Combine(GetDefaultExportFolderPath(), $"WordListsExport{date}.json");
    }

}
