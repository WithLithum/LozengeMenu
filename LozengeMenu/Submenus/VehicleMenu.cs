// Copyright (C) WithLithum 2022.
// Licensed under GNU General Public License, either version 3 or any later
// version of your choice.

namespace LozengeMenu.Submenus;

using GTA;
using LemonUI.Menus;
using LozengeMenu.Core;
using LozengeMenu.Core.Worlds;
using System;

internal class VehicleMenu : ISubMenu
{
    private readonly NativeMenu _menu = new("Lozenge", "Vehicle Options");

    private readonly NativeItem _spawnVehicle = new("Spawn New Vehicle");
    private readonly NativeItem _itemEditor = new("~r~Edit Current Vehicle");

    public NativeMenu Create()
    {
        Ticker.InVehicleStatusUpdate += Ticker_InVehicleStatusUpdate;
        _spawnVehicle.Activated += SpawnVehicleActivated;
        _itemEditor.Activated += EditorActivated;

        _menu.Add(_spawnVehicle);
        _menu.Add(_itemEditor);
        return _menu;
    }

    private void EditorActivated(object sender, EventArgs e)
    {
        if (!Game.Player.Character.IsInVehicle())
        {
            _itemEditor.Enabled = false;
        }

        VehicleEditor.Show(_menu, Game.Player.Character.CurrentVehicle);
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
