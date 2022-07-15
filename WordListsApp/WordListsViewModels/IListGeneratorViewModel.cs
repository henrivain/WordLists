using System.ComponentModel;
using WordDataAccessLibrary;

namespace WordListsViewModels;
public interface IListGeneratorViewModel
{
    event PropertyChangedEventHandler? PropertyChanged;

    WordCollection GetDataAsWordCollection();

    void SaveToDatabase();

    List<WordPair> WordPairs { get; set; }

    string CollectionName { get; set; }

    string Description { get; set; }

    string LanguageHeaders { get; set; }

}
