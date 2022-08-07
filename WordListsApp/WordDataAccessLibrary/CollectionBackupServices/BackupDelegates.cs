using WordDataAccessLibrary.CollectionBackupServices.ActionResults;

namespace WordDataAccessLibrary.CollectionBackupServices;
public class BackupDelegates
{
    public delegate void ExportFailEventHandler(object sender, ExportActionResult e);
    public delegate void ExportSuccessfullEventHandler(object sender, ExportActionResult e);

    public delegate void ImportFailEventHandler(object sender, ImportActionResult e);
    public delegate void ImportSuccessfullEventHandler(object sender, ImportActionResult e);

    public delegate void FileAccessEventHandler(object sender, FileActionResult e);
}
