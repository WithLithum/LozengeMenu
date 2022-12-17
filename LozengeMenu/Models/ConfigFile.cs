// Copyright (C) WithLithum 2022.
// Licensed under GNU General Public License, either version 3 or any later
// version of your choice.

namespace LozengeMenu.Models;
using System.Windows.Forms;

public struct ConfigFile
{
    public ConfigFile()
    {
        // C# compiler says this need to be here
    }

    public Keys OpenMenuKey { get; set; } = Keys.F7;
}
