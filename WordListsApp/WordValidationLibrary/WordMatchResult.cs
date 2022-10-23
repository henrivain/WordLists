namespace WordValidationLibrary;

public class WordMatchResult
{
	public WordMatchResult(bool isFullMatch)
	{
		IsFullMatch = isFullMatch;
		if (isFullMatch)
		{
			CharMatchPercentage = 100;
		}
	}

    ushort _charMatchPercentage = 0;

    public bool IsFullMatch { get; }

	public bool HasMatch => CharMatchPercentage is not 0;

    public ushort CharMatchPercentage 
	{ 
		get => _charMatchPercentage;
		internal set 
		{
			if (value < 0 || value > 100) 
				throw new ArgumentException($"{nameof(CharMatchPercentage)} must me between 0 and 100");
			_charMatchPercentage = value;
		} 
	}

	public List<MatchingString> ValidatedStringSpans { get; internal set; } = Enumerable.Empty<MatchingString>().ToList();
}
