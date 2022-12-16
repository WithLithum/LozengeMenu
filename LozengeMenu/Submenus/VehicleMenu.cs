namespace LozengeMenu.Submenus;

using LemonUI.Menus;
using LozengeMenu.Core;
using LozengeMenu.Core.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class VehicleMenu : ISubMenu
{
    private readonly NativeMenu _menu = new("Lozenge", "Vehicle Options");

    private readonly NativeItem _spawnVehicle = new("Spawn New Vehicle");
    private readonly NativeItem _itemEditor = new("~r~Edit Current Vehicle");

    public NativeMenu Create()
    {
        Ticker.InVehicleStatusUpdate += Ticker_InVehicleStatusUpdate;
        _spawnVehicle.Activated += SpawnVehicleActivated;

        _menu.Add(_spawnVehicle);
        return _menu;
    }

    private void Ticker_InVehicleStatusUpdate(object sender, bool e)
    {
        _itemEditor.Enabled = e;
    }

    private void SpawnVehicleActivated(object sender, EventArgs e)
    {
        EntitySpawner.SpawnVehicleInteractive();
        _menu.Visible = false;
    }
}
