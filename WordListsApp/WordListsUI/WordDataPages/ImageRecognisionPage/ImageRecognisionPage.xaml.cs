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
		string? filePath = null;


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
		if (string.IsNullOrEmpty(filePath))
		{

		}
	}
}