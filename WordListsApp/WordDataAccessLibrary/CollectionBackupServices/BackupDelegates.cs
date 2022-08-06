namespace WordDataAccessLibrary.CollectionBackupServices;
public class BackupDelegates
{
    public delegate void ExportFailEventHandler(object sender, ExportActionResult e);
    public delegate void ExportSuccessfullEventHandler(object sender, ExportActionResult e);
}
