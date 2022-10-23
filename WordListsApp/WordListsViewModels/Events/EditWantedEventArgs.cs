namespace WordListsViewModels.Events;

public class EditWantedEventArgs : EventArgs
{
	public EditWantedEventArgs() { }
	public EditWantedEventArgs(string? currentValue, int indexInList)
	{
		CurrentValue = currentValue;
		IndexInList = indexInList;
	}

	public string? CurrentValue { get; set; }
	public int IndexInList { get; set; }
}