namespace LozengeMenu.Submenus;

using GTA;
using LemonUI.Menus;
using LozengeMenu.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class PlayerMenu : ISubMenu
{
    private readonly NativeMenu _menu = new("Lozenge", "Player Options");
    private readonly NativeCheckboxItem _invincible = new("Invinciblity");
    private readonly NativeCheckboxItem _neverWanted = new("Never Wanted", "This option overrides all maximum wanted level and wanted level lock options.");
    private readonly NativeListItem<int> _wantedLevel = new("Wanted Level", "This option may ignore maximum wanted level.", 0, 1, 2, 3, 4, 5);
    private readonly NativeCheckboxItem _lockWantedLevel = new("Lock Wanted Level");
    private readonly NativeListItem<int> _maxWantedLevel = new("Max Wanted Level", 0, 1, 2, 3, 4, 5);
    private readonly NativeCheckboxItem _lockMaxWantedLevel = new("Lock Max Wanted Level");

    public NativeMenu Create()
    {
        _wantedLevel.SelectedItem = Game.Player.WantedLevel;
        _maxWantedLevel.SelectedItem = Game.MaxWantedLevel;

        Ticker.WantedLevelUpdate += Ticker_WantedLevelUpdate;

        _invincible.CheckboxChanged += InvincibleChanged;
        _neverWanted.CheckboxChanged += NeverWantedChanged;
        _maxWantedLevel.ItemChanged += MaxWantedLevelChanged;
        _wantedLevel.ItemChanged += WantedLevelChanged;
        _lockMaxWantedLevel.CheckboxChanged += _lockMaxWantedLevel_CheckboxChanged;
        _lockWantedLevel.CheckboxChanged += LockWantedLevelChanged;

        _menu.Add(_invincible);
        _menu.Add(_neverWanted);
        _menu.Add(_wantedLevel);
        _menu.Add(_lockWantedLevel);
        _menu.Add(_maxWantedLevel);
        _menu.Add(_lockMaxWantedLevel);

        _menu.Shown += MenuShown;

        return _menu;
    }

    private void Ticker_WantedLevelUpdate(object sender, EventArgs e)
    {
        _wantedLevel.SelectedItem = Ticker.WantedLevel;
    }

    private void LockWantedLevelChanged(object sender, EventArgs e)
    {
        Ticker.LockWantedLevel = _lockWantedLevel.Checked;
    }

    private void WantedLevelChanged(object sender, ItemChangedEventArgs<int> e)
    {
        Game.Player.WantedLevel = _wantedLevel.SelectedItem;
        Ticker.WantedLevel = _wantedLevel.SelectedItem;
    }

    private void _lockMaxWantedLevel_CheckboxChanged(object sender, EventArgs e)
    {
        Ticker.LockMaxWantedLevel = _lockMaxWantedLevel.Checked;
    }

    private void MenuShown(object sender, EventArgs e)
    {
        _maxWantedLevel.SelectedItem = Ticker.MaxWantedLevel;
    }

    private void MaxWantedLevelChanged(object sender, ItemChangedEventArgs<int> e)
    {
        Game.MaxWantedLevel = _maxWantedLevel.SelectedItem;
        _maxWantedLevel.SelectedItem = Ticker.MaxWantedLevel;
    }

    private void NeverWantedChanged(object sender, EventArgs e)
    {
        var c = _neverWanted.Checked;

        _maxWantedLevel.Enabled = !c;
        _lockMaxWantedLevel.Enabled = !c;
        _wantedLevel.Enabled = !c;
        _lockWantedLevel.Enabled = !c;
        Ticker.NeverWanted = c;
    }

    private void InvincibleChanged(object sender, EventArgs e)
    {
        Ticker.UpdateInvincible(_invincible.Checked);
    }
}
