using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelTests;
public partial class WordTrainingViewModelTests
{
    private static WordPair GetFirstPair(IWordTrainingViewModel viewModel)
    {
        return viewModel.WordCollection.WordPairs.First();
    }


    private static WordPair[] GetWordPairArray(int length)
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
