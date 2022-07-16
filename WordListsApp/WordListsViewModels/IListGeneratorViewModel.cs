using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using WordDataAccessLibrary;

namespace WordListsViewModels;
public interface IListGeneratorViewModel
{
    event PropertyChangedEventHandler? PropertyChanged;

    List<WordPair> WordPairs { get; set; }

    string CollectionName { get; set; }

    string Description { get; set; }

    string LanguageHeaders { get; set; }

    IAsyncRelayCommand GeneratePairsCommand { get; }
    
    IAsyncRelayCommand SaveCollection { get; }

    IRelayCommand FlipSides { get; }
    
    WordCollection GetData();


}
