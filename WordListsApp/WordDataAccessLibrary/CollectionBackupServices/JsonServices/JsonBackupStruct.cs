﻿using WordDataAccessLibrary.Helpers;
#nullable enable

namespace WordDataAccessLibrary.CollectionBackupServices.JsonServices;
/// <summary>
/// Store collections with metadata
/// </summary>
public struct JsonBackupStruct
{
    public JsonBackupStruct(DefaultExportWordCollection[] collections)
    {
        Collections = collections;
    }
    public string ParserVersion { get; set; } = AssemblyHelper.CurrentAssembly.VersionString ?? "Unknown";
    public string ParserAppName { get; set; } = AssemblyHelper.EntryAssembly.Name ?? string.Empty;
    public DateTime DateCreated { get; set; } = DateTime.Now;
    public DefaultExportWordCollection[] Collections { get; set; }
}
