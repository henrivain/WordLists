using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Media;
#if WINDOWS
using WordListsUI.Platforms.Windows;
#endif

namespace WordListsUI.WordDataPages.ImageRecognisionPage;

public partial class ImageRecognisionPage : ContentPage
{
	public ImageRecognisionPage(IImageRecognisionViewModel model)
	{
		Model = model;
        BindingContext = Model;
        InitializeComponent();
	}

	public IImageRecognisionViewModel Model { get; }

	private async void Button_Clicked(object sender, EventArgs e)
	{
		string filePath = null;

#if WINDOWS
        var picker = new WordListsUI.Platforms.Windows.WindowsMediaPicker(Application.Current.Windows[0]);
        var file = await picker.CaptureFileAsync();
		filePath = file.Path;
#else
        if (MediaPicker.Default.IsCaptureSupported)
		{
			FileResult photo = await MediaPicker.Default.CapturePhotoAsync();
			if (photo is not null)
			{
				filePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
				using Stream sourceStream = await photo.OpenReadAsync();
				using FileStream localFileStream = File.OpenWrite(filePath);
				await sourceStream.CopyToAsync(localFileStream);
			}
		}
#endif
		if (string.IsNullOrEmpty(filePath))
		{

		}
	}
}