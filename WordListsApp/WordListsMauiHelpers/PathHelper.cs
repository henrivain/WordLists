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
        return Path.Combine(GetDefaultExportFolderPath(), GetNewExportFileName());
    }

    public static string GetNewExportFileName()
    {
        string date = DateTime.Now.ToString("G")
                                  .Replace(" ", string.Empty)
                                  .Replace(".", string.Empty);
        return $"WordListsExport{date}.json";
    }

}
