using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordDataAccessLibrary.DataBaseActions;

namespace WordDataAccessLibrary.ExportServices;
public class ExportDelegates
{
    public delegate void ExportFailEventHandler(object sender, ExportActionResult e);
    public delegate void ExportSuccessfullEventHandler(object sender, ExportActionResult e);
}
