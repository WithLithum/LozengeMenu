namespace LozengeMenu.Submenus;

using LemonUI.Menus;
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

    public NativeMenu Create()
    {
        _spawnVehicle.Activated += SpawnVehicleActivated;

        _menu.Add(_spawnVehicle);
        return _menu;
    }

    private void SpawnVehicleActivated(object sender, EventArgs e)
    {
        EntitySpawner.SpawnVehicleInteractive();
        _menu.Visible = false;
    }
}
