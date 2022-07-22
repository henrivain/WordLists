using WordDataAccessLibrary;

namespace WordListsViewModels;
public interface IWordCollectionHandlingViewModel
{
    WordCollectionOwner? Selected { get; set; }

    List<WordCollectionOwner> AvailableCollections { get; set; }
}
