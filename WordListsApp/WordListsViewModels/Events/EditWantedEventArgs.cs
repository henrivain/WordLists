namespace WordListsViewModels.Events;

public class EditEventArgs : EventArgs
{
	public EditEventArgs() { }
	public EditEventArgs(string? currentValue, int indexInList)
	{
		CurrentValue = currentValue;
		IndexInList = indexInList;
	}

	public string? CurrentValue { get; set; }
	public int IndexInList { get; set; }
}