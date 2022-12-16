namespace LozengeMenu.Core.Worlds;

using GTA;
using LemonUI.Menus;
using LozengeMenu.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class VehicleEditor
{
    #region Primitive UI
    private static readonly NativeMenu _menu = new("Lozenge", "Vehicle Editor");
    private static Vehicle _current;

    private static readonly NativeItem _itemRepair = new("~r~Repair Vehicle");
    private static readonly LozengeCheckboxItem _itemEngine = new("~g~Engine Active");
    private static readonly LozengeCheckboxItem _itemSiren = new("~y~Siren Active");
    private static readonly NativeCheckboxItem _itemSirenSilent = new("~y~Siren Silent");

    static VehicleEditor()
    {
        _itemSiren.CheckboxChanged += SirenCheckChanged;
        _itemRepair.Activated += RepairActivated;
        _itemSirenSilent.CheckboxChanged += SirenSilentChanged;
        _menu.Closed += MenuClosed;
    }

    private static void SirenSilentChanged(object sender, EventArgs e)
    {
        if (_current?.Exists() != true)
        {
            return;
        }

        _current.IsSirenSilent = _itemSiren.Checked;
    }

    private static void RepairActivated(object sender, EventArgs e)
    {
        if (_current?.Exists() != true)
        {
            return;
        }

        _current.Repair();
    }

    private static void MenuClosed(object sender, EventArgs e)
    {
        if (_current?.Exists() != true)
        {
            return;
        }

        _current = null;
    }

    private static void SirenCheckChanged(object sender, EventArgs e)
    {
        if (_current?.Exists() != true)
        {
            return;
        }

        _current.IsSirenActive = _itemSiren.Checked;
    }
    #endregion

    public static void Update()
    {
        if (!_menu.Visible || _current?.Exists() != true)
        {
            return;
        }

        _itemEngine.SetCheckedSilent(_current.IsEngineRunning);
        _itemSiren.SetCheckedSilent(_current.IsSirenActive);
    }

    public static void Show(NativeMenu parent, Vehicle vehicle)
    {
        if (_menu.Visible || vehicle?.Exists() != true)
        {
            return;
        }

        _menu.Parent = parent;
        parent.Visible = false;
        _menu.Visible = true;
    }

    public static void Populate(Vehicle vehicle)
    {
        _menu.Clear();

        if (vehicle?.Exists() != true)
        {
            return;
        }

        _menu.Add(_itemEngine);

        CheckSirenItems(vehicle);
    }

    public static void CheckSirenItems(Vehicle vehicle)
    {
        if (vehicle?.Exists() != true || !vehicle.HasSiren) return;

        _menu.Add(_itemSiren);
        _menu.Add(_itemSirenSilent);
    }
}
