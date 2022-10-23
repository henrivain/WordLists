namespace WordListsViewModels.Events;

public class DeleteWantedEventArgs : EventArgs
{
	public DeleteWantedEventArgs() { }
	public DeleteWantedEventArgs(WordCollectionOwner owner)
	{
		WordCollectionOwner = owner;
	}

    public WordCollectionOwner? WordCollectionOwner { get; set; }
}