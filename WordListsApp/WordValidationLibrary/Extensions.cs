namespace WordValidationLibrary;
public static class Extensions
{
    public static string ToJsonString(this List<MatchingString> strings)
    {
        string result = string.Empty;
        foreach (var str in strings)
        {
            result += str.ToString() + ",\n";
        }
        string[] lines = result.Split('\n');
        result = string.Empty;
        foreach (var line in lines)
        {
            result += $"\t{line}";
        }

        result =
            $$"""
            {
            {{result}}
            }
            """;
        return result;
    }
}
