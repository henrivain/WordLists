using System.ComponentModel;

namespace WordListsViewModels.Interfaces;
public interface IListGeneratorViewModel
{
    event PropertyChangedEventHandler? PropertyChanged;

    List<WordPair> WordPairs { get; set; }

    string CollectionName { get; set; }

    string Description { get; set; }

    string LanguageHeaders { get; set; }

    bool CanSave { get; }

    bool LableVisible { get; }

    IAsyncRelayCommand GeneratePairsCommand { get; }

    IAsyncRelayCommand SaveCollection { get; }

    IRelayCommand FlipSides { get; }

    WordCollection GetData();

    event CollectionAddedEventHandler? CollectionAddedEvent;
}
