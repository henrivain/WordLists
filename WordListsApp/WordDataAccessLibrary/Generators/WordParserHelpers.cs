namespace WordDataAccessLibrary.Generators;
internal static class WordParserHelpers
{
    /// <summary>
    ///  Removes pronunciation between [] -brackets, trims and then removes empty lines.
    /// </summary>
    /// <param name="lines"></param>
    /// <returns>IEnumerable of string</returns>
    internal static IEnumerable<string> CleanLines(this IEnumerable<string> lines)
    {
        foreach (string line in lines)
        {
            string cleanLine = RemovePronunciation(line).Trim();
            if (string.IsNullOrWhiteSpace(cleanLine) is false)
            {
                yield return cleanLine;
            }
        }
    }

    /// <summary>
    /// Remove pronunciation between [] -brackets
    /// </summary>
    /// <param name="line"></param>
    /// <returns>string with pronunciation removed.</returns>
    internal static string RemovePronunciation(string line)
    {
        while (true)
        {
            int start = line.IndexOf('[');
            if (start < 0) return line;

            int end = line.IndexOf("]", start);
            if (end < 0) return line;

            line = $"{line[..start]}{line[(end + 1)..]}";
        }
    }

}
