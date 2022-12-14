namespace WordListsViewModels.Events;

public class InvalidDataEventArgs<T>
{
	public InvalidDataEventArgs() { }

	public InvalidDataEventArgs(string? message, T? data)
	{
        Message = message;
        Data = data;
    }

	public string? Message { get; set; } = string.Empty;
	public T? Data { get; set; }
}