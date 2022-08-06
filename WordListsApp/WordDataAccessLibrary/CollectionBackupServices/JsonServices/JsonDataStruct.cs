using WordDataAccessLibrary.Helpers;

namespace WordDataAccessLibrary.CollectionBackupServices.JsonServices;
/// <summary>
/// Store collections with metadata
/// </summary>
public struct JsonBackupStruct
{
    public JsonBackupStruct(IExportWordCollection[] collections)
    {
        Collections = collections;
    }
    public string ParserVersion { get; set; } = AssemblyHelper.CurrentAssembly.VersionString;
    public string ParserAppName { get; set; } = AssemblyHelper.EntryAssembly.Name;
    public DateTime DateCreated { get; set; } = DateTime.Now;
    public IExportWordCollection[] Collections { get; set; }
}
