// Copyright (C) WithLithum 2022.
// Licensed under GNU General Public License, either version 3 or any later
// version of your choice.

namespace LozengeMenu.Submenus;

using LemonUI.Menus;
using System;

public class MiscMenu : ISubMenu
{
    private readonly NativeMenu _menu = new("Lozenge", "Misc Options");
    private readonly NativeCheckboxItem _blackOut = new("Blackout Mode");
    private readonly NativeCheckboxItem _blackOutVehicles = new("Blackout Vehicles", true);
    private readonly NativeCheckboxItem _itemPauseVehicle = new("Pause Vehicle Population");
    private readonly NativeCheckboxItem _itemPausePeds = new("Pause Ped Population");
    private readonly NativeCheckboxItem _riotMode = new("Riot Mode");

    public NativeMenu Create()
    {
        _blackOut.CheckboxChanged += BlackoutChanged;
        _blackOutVehicles.CheckboxChanged += BlackoutVehiclesChanged;
        _blackOutVehicles.Enabled = false;
        _riotMode.CheckboxChanged += RiotChanged;
        _itemPauseVehicle.CheckboxChanged += PauseVehicleChanged;
        _itemPausePeds.CheckboxChanged += PausePedsChanged;

        _menu.Add(_blackOut);
        _menu.Add(_blackOutVehicles);
        _menu.Add(_itemPauseVehicle);
        _menu.Add(_itemPausePeds);
        _menu.Add(_riotMode);

        return _menu;
    }

    private void PausePedsChanged(object sender, EventArgs e)
    {
        Natives.SetPedPopulationBudget(_itemPausePeds.Checked ? 0 : 3);
    }

    private void PauseVehicleChanged(object sender, EventArgs e)
    {
        Natives.SetVehiclePopulationBudget(_itemPauseVehicle.Checked ? 0 : 3);
    }

    private void RiotChanged(object sender, EventArgs e)
    {
        Natives.SetRiotModeEnabled(true);
    }

    private void BlackoutVehiclesChanged(object sender, EventArgs e)
    {
        Natives.SetArtificialVehicleLightsState(_blackOutVehicles.Checked);
    }

    private void BlackoutChanged(object sender, EventArgs e)
    {
        Natives.SetArtificialLightsState(_blackOut.Checked);
        _blackOutVehicles.Enabled = _blackOut.Checked;
    }
}
