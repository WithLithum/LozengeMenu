namespace LozengeMenu.Submenus;

using LemonUI.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MiscMenu : ISubMenu
{
    private readonly NativeMenu _menu = new("Lozenge", "Misc Options");
    private readonly NativeCheckboxItem _blackOut = new("Blackout Mode");
    private readonly NativeCheckboxItem _blackOutVehicles = new("Blackout Vehicles");
    private readonly NativeCheckboxItem _riotMode = new("Riot Mode");

    public NativeMenu Create()
    {
        _blackOut.CheckboxChanged += BlackoutChanged;
        _blackOutVehicles.CheckboxChanged += BlackoutVehiclesChanged;
        _riotMode.CheckboxChanged += RiotChanged;

        _menu.Add(_blackOut);
        _menu.Add(_blackOutVehicles);
        _menu.Add(_riotMode);

        return _menu;
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
