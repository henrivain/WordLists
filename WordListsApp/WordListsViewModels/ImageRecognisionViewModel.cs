using ImageRecognisionLibrary;

namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class ImageRecognisionViewModel : IImageRecognisionViewModel
{
	public ImageRecognisionViewModel(IImageRecognisionEngine recognisionEngine)
	{
		RecognisionEngine = recognisionEngine;
		ReadTestText();
	}

	public IImageRecognisionEngine RecognisionEngine { get; }


	
	[ObservableProperty]
	string _text = string.Empty;


	private async void ReadTestText()
	{
        await RecognisionEngine.Read();
		Text = RecognisionEngine.Result;
	}
}
