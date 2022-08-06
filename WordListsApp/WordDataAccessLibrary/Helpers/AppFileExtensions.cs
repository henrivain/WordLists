namespace WordDataAccessLibrary.Helpers;
public static class AppFileExtension
{
    public static string GetExtension(FileExtension extension)
    {
        return extension switch
        {
            FileExtension.Json => ".json",
            FileExtension.Zip => ".zip",
            FileExtension.Wordlist => ".wordlist",
            _ => throw new NotImplementedException($"{nameof(extension)} {extension} is not implemented")
        };
    }
}
