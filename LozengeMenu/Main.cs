// Copyright (C) WithLithum 2022.
// Licensed under GNU General Public License, either version 3 or any later
// version of your choice.

namespace LozengeMenu;

using GTA;
using GTA.UI;
using LozengeMenu.Config;
using LozengeMenu.Core;
using System;
using System.Reflection;
using System.Windows.Forms;

public class Main : Script
{
    private Keys _openMenuKey = Keys.F7;
    private Ticker _ticker;
    private readonly MenuController _controller = new();
    private bool _ok;

    public Main()
    {
        Tick += Main_Tick;
        Aborted += Main_Aborted;
        KeyDown += Main_KeyDown;

        ConfigManager.ConfigReload += ConfigManager_ConfigReload;
    }

    private void ConfigManager_ConfigReload(object sender, ConfigManager.ConfigEventArgs e)
    {
        _openMenuKey = e.Config.OpenMenuKey;
        Notification.Show($"Press {_openMenuKey} to open menu.");
    }

    private void Main_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
    {
        if (_ok && e.KeyCode == _openMenuKey)
        {
            _controller.OpenMenu();
        }
    }

    private void Main_Aborted(object sender, EventArgs e)
    {
        _ticker?.Abort();
    }

    private void Main_Tick(object sender, EventArgs e)
    {
        while (Game.IsLoading)
        {
            Yield();
        }

        if (!_ok)
        {
            _controller.Init();
            _ok = true;


            _ticker = InstantiateScript<Ticker>();
            ConfigManager.LoadConfig();
            Notification.Show($"~g~Lozenge Menu ~b~{Assembly.GetExecutingAssembly().GetName().Version} ~s~ready.");
        }

        _controller.Process();
    }
}
