namespace WordDataAccessLibrary;

public class WordCollection
{
    /// <summary>
    /// Initialize new WordCollection "Vocabulary"
    /// </summary>
    public WordCollection() { }
    /// <summary>
    /// Initialize new WordCollection "Vocabulary"
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="pairs"></param>
    /// <exception cref="ArgumentNullException">thrown when one of parameters is null</exception>
    public WordCollection(WordCollectionOwner owner, List<WordPair> pairs) 
    { 
        Owner = owner;
        WordPairs = pairs;
    }


    public WordCollectionOwner Owner 
    {
        get => _owner;
        set
        {
            if (value is null)
                throw new ArgumentNullException($"{nameof(Owner)} can't be null");
            _owner = value;
        }
    }


    List<WordPair> _wordPairs = new();
    WordCollectionOwner _owner = new();

    public List<WordPair> WordPairs 
    { 
        get => _wordPairs; 
        set 
        {
            if (value is null)
                throw new ArgumentNullException($"{nameof(WordPairs)} can't be null");
            _wordPairs = value;
        } 
    }
}
