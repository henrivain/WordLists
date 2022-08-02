using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordDataAccessLibrary;
using static WordDataAccessLibrary.DataBaseActions.DataBaseDelegates;

namespace WordListsViewModels.Interfaces;


public interface IWordTrainingViewModel
{
    string Title { get; set; }
    string Description { get; set; }
    string LanguageHeaders { get; set; }
    WordCollection WordCollection { get; set; }
    WordPair VisibleWordPair { get; set; }

    int MaxWordIndex { get; set; }
    int UIVisibleIndex { get; }
    int LearnStateAsInt { get; set; }

    bool CanGoNext { get; set; }
    bool CanGoPrevious { get; set; }

    IAsyncRelayCommand SaveProgression { get; }

    IRelayCommand WordStateNotSetCommand { get; }
    IRelayCommand WordLearnedCommand { get; }
    IRelayCommand MightKnowWordCommand { get; }
    IRelayCommand WordNeverHeardCommand { get; }

    void Next();
    void Previous();

    void StartNew(WordCollection collection);
    void StartNew(WordCollection collection, int fromIndex);
    Task StartNewAsync(int collectionId);

    public event CollectionUpdatedEventHandler? CollectionUpdated;

}
