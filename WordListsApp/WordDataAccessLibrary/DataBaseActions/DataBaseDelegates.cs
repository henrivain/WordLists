using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordDataAccessLibrary.DataBaseActions;
public class DataBaseDelegates
{
    public delegate void CollectionAddedEventHandler(object sender, DataBaseActionArgs e);
    
    public delegate void CollectionUpdatedEventHandler(object sender, DataBaseActionArgs e);
    
    public delegate void CollectionDeletedEventHandler(object sender, DataBaseActionArgs e);

    public delegate void ReadCollectionEventHandler(object sender, DataBaseActionArgs e);

}
