namespace LozengeMenu;

using LemonUI;
using LemonUI.Menus;
using LozengeMenu.Submenus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class MenuController
{
    private readonly ObjectPool _pool = new();
    private readonly NativeMenu _main = new("Lozenge", "Main Menu");

    private readonly ISubMenu[] _subs =
    {
        new PlayerMenu(),
        new TimeMenu()
    };

    internal void Operate(ISubMenu menu)
    {
        var m = menu.Create();
        m.UseMouse = false;
        _pool.Add(m);
        _main.AddSubMenu(m).AltTitle = string.Empty;
    }

    internal void OpenMenu()
    {
        if (!_pool.AreAnyVisible)
        {
            _main.Visible = !_main.Visible;
        }
    }

    internal void Process()
    {
        _pool.Process();
    }

    internal void Init()
    {
        _pool.Add(_main);
        _main.UseMouse = false;

        foreach (var sub in _subs)
        {
            Operate(sub);
        }
    }
}
