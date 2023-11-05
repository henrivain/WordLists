using System.Text.RegularExpressions;

namespace WordDataAccessLibrary.Generators;
public partial class NewOtavaWordPairParser : OtavaWordPairParser
{
    public override List<string> ToStringList(string vocabulary)
    {
        if (string.IsNullOrEmpty(vocabulary))
        {
            return new();
        }

        string[] lines = vocabulary
            .Replace("\r", string.Empty)
            .Replace("\n\n\n", "\n\n")
            .Split("\n\n")
            .ToArray();

        List<string> result = new();

        string cache = string.Empty;
        foreach (var line in lines)
        {
            string[] splitted = line.Split('\n');
            foreach (var split in splitted.Select(x => x.Trim()))
            {
                if (split.EndsWith("Play"))
                {
                    result.Add($"{cache}{split}");
                    cache = string.Empty;
                    continue;
                }
                if (split.EndsWith('/') || split.EndsWith(','))
                {
                    cache += split + " ";
                    continue;
                }
                result.Add($"{cache}{split}");
                cache = string.Empty;
                continue;
            }
        }
        return result.Select(TidyUpLine)
                     .Where(x => string.IsNullOrWhiteSpace(x) is false)
                     .ToList();
    }

    private static string TidyUpLine(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            return string.Empty;
        }
        line = RemovePronunciation(line);
        line = line.Trim();
        if (line.EndsWith("Play"))
        {
            line = line[..^"Play".Length].Trim();
        }
        if (line is "/")
        {
            return string.Empty;
        }
        return EmptySlashSpan().Replace(line, "/");    
    }

    [GeneratedRegex("\\/ *\\/")]    // Matches  "/ /" with any number of spaces between
    private static partial Regex EmptySlashSpan();
}
