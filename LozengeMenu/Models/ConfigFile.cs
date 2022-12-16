namespace LozengeMenu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public struct ConfigFile
{
    public ConfigFile()
    {
        // C# compiler says this need to be here
    }

    public Keys OpenMenuKey { get; set; } = Keys.F7;
}
