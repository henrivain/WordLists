namespace WordValidationLibrary;
public struct MatchingString
{
	public MatchingString(string str, bool isMatch)
	{
		String = str;
		IsMatch = isMatch;
	}

	public string String { get; }
	public bool IsMatch { get; }

	public override string ToString()
	{
		return 
			$$"""
			{
				{{nameof(String)}} : {{String}},
				{{nameof(IsMatch)}} : {{IsMatch}}
			}
			""";
	}
}
