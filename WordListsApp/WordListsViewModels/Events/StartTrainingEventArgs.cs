namespace WordListsViewModels.Events;

public class StartTrainingEventArgs
{
	public StartTrainingEventArgs() { }

	public StartTrainingEventArgs(WordCollection wordCollection)
	{
		WordCollection = wordCollection;
	}

	public WordCollection? WordCollection { get; set; }
}