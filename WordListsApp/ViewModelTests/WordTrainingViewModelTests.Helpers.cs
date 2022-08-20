namespace ViewModelTests;
public partial class WordTrainingViewModelTests
{
    internal static WordPair GetFirstPair(IWordTrainingViewModel viewModel)
    {
        return viewModel.WordCollection.WordPairs.First();
    }


    internal static WordPair[] GetWordPairArray(int length)
    {
        WordPair[] array = new WordPair[length];
        for (int i = 0; i < length; i++)
        {
            array[i] = new WordPair()
            {
                NativeLanguageWord = $"this is pair number {i}"
            };
        }
        return array;
    }
}
