using System.Diagnostics;
using WordDataAccessLibrary.DataBaseActions.Interfaces;
using WordListsViewModels.Helpers;

namespace WordListsViewModels.Extensions;
internal static class WordPairQuestionService
{
    internal static async Task SaveLearnStates(this List<WordPairQuestion> questions, IWordPairService dataBaseService)
    {
        foreach (var question in questions)
        {
            WordPair? pairInDb = await dataBaseService.GetByPrimaryKey(question.WordPair.Id);
            if (pairInDb is null) continue;
            pairInDb.LearnState = question.WordPair.LearnState;
            await dataBaseService.UpdatePairAsync(pairInDb);
            Debug.WriteLine($"{nameof(WordPairQuestion)} saved learn states");
        }
    }
}
