using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordDataAccessLibrary.DataBaseActions.Interfaces;
public interface IWordCollectionService
{
    Task<int> AddWordCollection(WordCollection collection);
    Task SaveProgression(WordCollection wordCollection);
    Task<List<WordCollection>> GetWordCollections();
    Task<WordCollection> GetWordCollection(int id);
    Task<List<WordCollection>> GetWordCollectionsById(int[] ids);
    Task DeleteWordCollection(int OwnerId);
    Task<int> DeleteAll(string verifyByTrue);
}
