namespace WordListsUI.WordTrainingPages.WriteWordTrainingPage;

public partial class WriteWordTrainingPage : ContentPage
{
	public WriteWordTrainingPage(IWriteWordViewModel model)
	{
		BindingContext = model;
        InitializeComponent();
	}

	public IWriteWordViewModel Model => (IWriteWordViewModel)BindingContext;
}