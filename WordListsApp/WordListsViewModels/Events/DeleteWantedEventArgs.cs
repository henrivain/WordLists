namespace WordListsViewModels.Events;

public class DeleteWantedEventArgs : EventArgs
{
	public DeleteWantedEventArgs() { }
	public DeleteWantedEventArgs(WordCollectionOwner[] owners)
	{
        ItemsToDelete = owners;
	}

    public DeleteWantedEventArgs(WordCollectionOwner[] owners, bool deletesAll) : this(owners)
    {
        DeletesAll = deletesAll;
    }
  

    public WordCollectionOwner[]? ItemsToDelete { get; set; }
    public bool DeletesAll { get; set; }
}