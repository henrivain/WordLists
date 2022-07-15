using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordDataAccessLibrary.Helpers;
public static class WordListFlipper
{
    public static List<WordPair> FlipWordPair(List<WordPair> pairs)
    {
        return pairs.Select(FlipWords).ToList();
    }

    private static WordPair FlipWords(WordPair pair)
    {
        (pair.ForeignLanguageWord, pair.NativeLanguageWord) = (pair.NativeLanguageWord, pair.ForeignLanguageWord);
        return pair;
    }
}
