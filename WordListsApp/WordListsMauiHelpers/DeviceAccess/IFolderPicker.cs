namespace WordListsMauiHelpers.DeviceAccess;

public interface IFolderPicker
{
    Task<string> PickAsync();
}