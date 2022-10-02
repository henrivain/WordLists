using WordListsViewModels.Helpers;

namespace WordListsUI.WordTrainingPages.WritingTestPage.UIElements;

public partial class WordResultViewer : Grid
{
	public WordResultViewer()
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
		BindableProperty.Create(nameof(WordPairQuestion), typeof(WordPairQuestion), typeof(WordResultViewer));
}