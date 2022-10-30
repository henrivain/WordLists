namespace ImageRecognisionLibrary;
public interface IImageRecognisionEngine
{
    Task Read();
    string Result { get; }
}
