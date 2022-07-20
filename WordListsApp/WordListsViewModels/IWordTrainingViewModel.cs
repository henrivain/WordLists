using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordDataAccessLibrary;

namespace WordListsViewModels;


public interface IWordTrainingViewModel
{
    string Title { get; set; }
    string Description { get; set; }
    string LanguageHeaders { get; set; }
    WordCollection WordCollection { get; set; }
    WordPair VisibleWordPair { get; set; }

    int MaxWordIndex { get; set; }
    int UIVisibleIndex { get; }

    bool CanGoNext { get; set; }
    bool CanGoPrevious { get; set; }

    void Next();
    void Previous();

    void StartNew(WordCollection collection);
}
