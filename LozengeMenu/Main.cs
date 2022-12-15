namespace LozengeMenu;

using GTA;
using LozengeMenu.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Main : Script
{
    private Ticker _ticker;
    private readonly MenuController _controller = new();
    private bool _ok;

    public Main()
    {
        Tick += Main_Tick;
        Aborted += Main_Aborted;
        KeyDown += Main_KeyDown;
    }

    private void Main_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
    {
        // TODO configurable menu key
        if (_ok && e.KeyCode == System.Windows.Forms.Keys.F7)
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
        }

        _controller.Process();
    }
}
