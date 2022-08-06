using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class JsonImportViewModel : IJsonImportViewModel
{
    [ObservableProperty]
    string importPath = string.Empty;



    public IAsyncRelayCommand SelectFile => new AsyncRelayCommand(async () =>
    {

    });

    public IAsyncRelayCommand Import => new AsyncRelayCommand(async () =>
    {
        if (ImportPath is null || File.Exists(importPath) is false)
        {


            return;
        }
    });


    public event 

}
