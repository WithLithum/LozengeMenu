namespace LozengeMenu.Core.Worlds;

using GTA;
using LemonUI;
using LemonUI.Menus;
using LozengeMenu.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

public static class VehicleEditor
{
    private static bool _updateLiveries;

    #region Primitive UI
    private static readonly NativeMenu _menu = new("Lozenge", "Vehicle Editor");
    private static Vehicle _current;

    private static readonly NativeItem _itemRepair = new("~r~Repair Vehicle");
    private static readonly NativeItem _itemWash = new("~r~Clean Vehicle");
    private static readonly LozengeListItem<int> _itemLivery = new("Livery");
    private static readonly NativeItem _itemNumberPlate = new("Number Plate");
    private static readonly LozengeCheckboxItem _itemEngine = new("~g~Engine Active");
    private static readonly LozengeCheckboxItem _itemSiren = new("~y~Siren Active");
    private static readonly NativeCheckboxItem _itemSirenSilent = new("~y~Siren Play Sounds");

    static VehicleEditor()
    {
        _itemSiren.CheckboxChanged += SirenCheckChanged;
        _itemRepair.Activated += RepairActivated;
        _itemWash.Activated += WashActivated;
        _itemSirenSilent.CheckboxChanged += SirenSilentChanged;
        _itemNumberPlate.Activated += NumberPlateActivated;
        _itemLivery.ItemChanged += LiveryChanged;
        _menu.Closed += MenuClosed;
        _menu.Opening += MenuOpening;
    }

    private static void LiveryChanged(object sender, LozengeItemEventArgs<int> e)
    {
        if (_current?.Exists() != true)
        {
            return;
        }

        Natives.SetVehicleLivery(_current.Handle, e.Object);
    }

    private static void NumberPlateActivated(object sender, EventArgs e)
    {
        if (_current?.Exists() != true)
        {
            return;
        }

        if (!Util.TryAskQuestion(WindowTitle.EnterMessage20, 8, null, out var plate))
        {
            return;
        }

        Natives.SetVehicleNumberPlateText(_current.Handle, plate);
        _itemNumberPlate.AltTitle = Natives.GetVehicleNumberPlateText(_current.Handle);
    }

    private static void MenuOpening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        if (_current?.Exists() != true)
        {
            return;
        }

        _itemNumberPlate.AltTitle = Natives.GetVehicleNumberPlateText(_current.Handle);
    }

    private static void WashActivated(object sender, EventArgs e)
    {
        if (_current?.Exists() != true)
        {
            return;
        }

        _current.Wash();
    }

    private static void SirenSilentChanged(object sender, EventArgs e)
    {
        if (_current?.Exists() != true)
        {
            return;
        }

        // BUG: fix inconsistent behaviour
        var wasActive = _current.IsSirenActive;

        if (wasActive) _current.IsSirenActive = false;

        _current.IsSirenSilent = _itemSiren.Checked;

        if (wasActive) _current.IsSirenActive = true;
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

    /// <summary>
    /// Called each 10 tick to update this menu.
    /// </summary>
    public static void Update()
    {
        if (!_menu.Visible || _current?.Exists() != true)
        {
            return;
        }

        _itemEngine.SetCheckedSilent(_current.IsEngineRunning);
        _itemSiren.SetCheckedSilent(_current.IsSirenActive);

        if (_updateLiveries)
        {
            _itemLivery.SelectItemSilent(Natives.GetVehicleLivery(_current.Handle));
        }
    }

    public static void Show(NativeMenu parent, Vehicle vehicle)
    {
        if (_menu.Visible || vehicle?.Exists() != true)
        {
            return;
        }

        Populate(vehicle);

        _menu.Parent = parent;
        parent.Visible = false;
        _menu.Visible = true;
    }

    private static void Populate(Vehicle vehicle)
    {
        _current = vehicle;
        _menu.Clear();

        if (vehicle?.Exists() != true)
        {
            return;
        }

        _menu.Add(_itemRepair);
        _menu.Add(_itemWash);

        CheckLivery(vehicle);

        _menu.Add(_itemNumberPlate);
        _menu.Add(_itemEngine);

        CheckSirenItems(vehicle);
    }

    public static void CheckSirenItems(Vehicle vehicle)
    {
        if (vehicle?.Exists() != true || !vehicle.HasSiren) return;

        _menu.Add(_itemSiren);
        _menu.Add(_itemSirenSilent);
    }

    public static void CheckLivery(Vehicle vehicle)
    {
        if (vehicle?.Exists() != true)
        {
            return;
        }

        // Get liveries count
        var count = Natives.GetVehicleLiveryCount(vehicle.Handle);

        if (count <= 0)
        {
            // No liveries available, or liveries configured but not added into textures
            _updateLiveries = false;
            return;
        }

        _itemLivery.Clear();
        _menu.Add(_itemLivery);

        if (count == 1)
        {
            // Only one livery available, tell user that
            _itemLivery.Enabled = false;
            _updateLiveries = false;
            _itemLivery.Add(1);
            return;
        }

        for (var i = 1; i < count; i++)
        {
            // Add all liveries
            _itemLivery.Enabled = true;
            _updateLiveries = true;
            _itemLivery.Add(i);
        }
    }

    internal static void Init(ObjectPool pool)
    {
        pool.Add(_menu);
    }
}
