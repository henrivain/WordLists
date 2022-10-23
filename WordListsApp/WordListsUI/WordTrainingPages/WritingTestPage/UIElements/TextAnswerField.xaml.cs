using WordListsViewModels.Helpers;

namespace WordListsUI.WordTrainingPages.WritingTestPage.UIElements;

public partial class TextAnswerField : Grid
{
	public TextAnswerField()
	{
		InitializeComponent();
        BindingContext = WordPairQuestion;
	}

    public WordPairQuestion WordPairQuestion
    {
        get => (WordPairQuestion)GetValue(WordPairQuestionProperty);
        set => SetValue(WordPairQuestionProperty, value);
    }

    public static readonly BindableProperty WordPairQuestionProperty =
        BindableProperty.Create(nameof(WordPairQuestion), typeof(WordPairQuestion), typeof(TextAnswerField));
}