using System.ComponentModel;

namespace WordListsViewModels.Interfaces;
public interface IListGeneratorViewModel
{
    event PropertyChangedEventHandler? PropertyChanged;

    List<string> Words { get; set; }

    string CollectionName { get; set; }

    string Description { get; set; }

    string LanguageHeaders { get; set; }

    bool CanSave { get; }

    IAsyncRelayCommand GeneratePairsCommand { get; }

    IAsyncRelayCommand SaveCollection { get; }

    IRelayCommand FlipSides { get; }

    IRelayCommand<string> Remove { get; }

    WordCollection GetData();

    event CollectionAddedEventHandler? CollectionAddedEvent;
}
