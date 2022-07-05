using WordDataAccessLibrary;

namespace WordListsUI.WordTrainingPage.FlipCardControl;
public class WordPairFlipCard : AnimatedFlipCard
{
    public WordPairFlipCard() : base() => WordPair = new(string.Empty, string.Empty);

    public WordPairFlipCard(WordPair pair, bool showNativeWord = true) : base()
    {
        WordPair = pair;
        if (showNativeWord is false) _ = ShowBottomSide();
    }

    WordPair _wordPair;

    public WordPair WordPair
    {
        get => _wordPair;
        set
        {
            _wordPair = value;
            ResetSideTextsFromWordPair(value);
        }
    }

    public static readonly BindableProperty WordPairProperty =
        BindableProperty.Create(nameof(TopSideText), typeof(WordPair), typeof(WordPairFlipCard));

    private void ResetSideTextsFromWordPair(WordPair pair)
    {
        if (pair is null) return;
        TopSideText = pair.NativeLanguageWord;
        BottomSideText = pair.ForeignLanguageWord;
        UpdateText();
    }


}
