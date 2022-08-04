using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordDataAccessLibrary.DataBaseActions;

namespace WordDataAccessLibrary.JsonServices;
public class JsonExportDelegates
{
    public delegate void ExportFailEventHandler(object sender, JsonActionArgs e);
    public delegate void ExportSuccessfullEventHandler(object sender, JsonActionArgs e);
}
