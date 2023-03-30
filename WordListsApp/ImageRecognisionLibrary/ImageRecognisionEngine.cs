using Tesseract;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Security;

namespace ImageRecognisionLibrary;
public class ImageRecognisionEngine : IImageRecognisionEngine
{

    public ImageRecognisionEngine()
    {

    }

    private const string ImagePath = @"C:\1 Henri\github\WordLists\WordListsApp\ImageRecognisionLibrary\ImageContainer\camera_image.png";

    public string Result { get; private set; } = "String not read yet!";

    private static async Task<(bool success, string Path)> MakeSureThatTrainedDataExist()
    {
        string appDataPath = Path.Combine(FileSystem.Current.CacheDirectory, "TesseracData", "fin_4.1.0.traineddata");
        string directoryPath = Path.GetDirectoryName(appDataPath);
        if (await FileSystem.Current.AppPackageFileExistsAsync("fin_4.1.0.traineddata") is false)
        {
            return (false, appDataPath);
        }
       
        using var stream = await FileSystem.Current.OpenAppPackageFileAsync("fin_4.1.0.traineddata");

#pragma warning disable CS0168 // Variable is declared but never used
        try
        {
            File.Delete(appDataPath);
        }
        catch (Exception ex)
        {

        }
        try
        {
            Directory.Delete(appDataPath, true);
        }
        catch (Exception ex)
        {

        }
        try
        {
            Directory.CreateDirectory(directoryPath);
            using var fileStream = File.Create(appDataPath);

            stream.CopyTo(fileStream);
        }
        catch (Exception ex)
        {

        }
#pragma warning restore CS0168 // Variable is declared but never used

        return (true, appDataPath);
    }


    public async Task Read()
    {
        var (success, path) = await MakeSureThatTrainedDataExist();
        if (success && File.Exists(path))
        {
            string dirName = Path.GetDirectoryName(path);
            try
            {
                using TesseractEngine engine = new(dirName, "fin_4.1.0");
                using var image = Pix.LoadFromFile(ImagePath);
                using var page = engine.Process(image);
                Result = page.GetText();
            }
            catch (Exception)
            {

            }
        }
        else
        {
            Result = "Action failed";
        }
    }
}
