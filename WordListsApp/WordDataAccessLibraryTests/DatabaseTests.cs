using WordDataAccessLibrary.Generators;
using WordDataAccessLibrary;
using WordDataAccessLibrary.DataBaseActions;


namespace WordDataAccessLibraryTests;

[TestClass]
public partial class DatabaseTests
{
    [TestMethod]
    public async Task AddWordCollection_ThrowsException_WhenCollectionNull()
    {
        await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
        {
            await WordCollectionService.AddWordCollection(null);
        });
    }

    [TestMethod]
    public async Task AddGet_NoWordPairsWillBeLost()
    {
        int id = await WordCollectionService.AddWordCollection(WordCollection_NormalIds);
        WordCollection collection = await WordCollectionService.GetWordCollection(id);
        Assert.AreEqual(WordCollection_NormalIds.WordPairs.Count, collection.WordPairs.Count);
    }

    [TestMethod]
    public async Task AddGet_NoWordPairsWillBeLost_StartWithRandomIds()
    {
        int id = await WordCollectionService.AddWordCollection(WordCollection_RandomIds);
        WordCollection collection = await WordCollectionService.GetWordCollection(id);
        Assert.AreEqual(WordCollection_NormalIds.WordPairs.Count, collection.WordPairs.Count);
    }
    

}