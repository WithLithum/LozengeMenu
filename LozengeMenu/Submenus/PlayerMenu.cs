// Copyright (C) WithLithum 2022.
// Licensed under GNU General Public License, either version 3 or any later
// version of your choice.

namespace LozengeMenu.Submenus;

using GTA;
using LemonUI.Menus;
using LozengeMenu.Core;
using LozengeMenu.Core.UI;
using System;

internal class PlayerMenu : ISubMenu
{
    private readonly NativeMenu _menu = new("Lozenge", "Player Options");
    private readonly NativeCheckboxItem _invincible = new("~g~Invinciblity");
    private readonly NativeCheckboxItem _neverWanted = new("~y~Never Wanted", "This option overrides all maximum wanted level and wanted level lock options.");
    private readonly LozengeListItem<int> _wantedLevel = new("~y~Wanted Level", "This option may ignore maximum wanted level.", 0, 1, 2, 3, 4, 5);
    private readonly NativeCheckboxItem _lockWantedLevel = new("~y~Lock Wanted Level");
    private readonly LozengeListItem<int> _maxWantedLevel = new("~y~Max Wanted Level", 0, 1, 2, 3, 4, 5);
    private readonly NativeCheckboxItem _lockMaxWantedLevel = new("~y~Lock Max Wanted Level");
    private readonly NativeCheckboxItem _dispatchCops = new("~r~Dispatch Cops");
    private readonly NativeCheckboxItem _policeIgnore = new("~r~Ignored by Cops");
    private readonly NativeCheckboxItem _pedsIgnore = new("~r~Ignored by Everyone");
    private readonly NativeItem _replenishPlayer = new("~g~Replenish Player");
    private readonly NativeItem _healPlayer = new("~b~Replenish Health");
    private readonly NativeItem _addArmour = new("~b~Replenish Armour");

    private int lastMax;

    public NativeMenu Create()
    {
        _wantedLevel.SelectItemSilent(Game.Player.WantedLevel);
        _maxWantedLevel.SelectItemSilent(Game.MaxWantedLevel);

        Ticker.WantedLevelUpdate += Ticker_WantedLevelUpdate;

        _invincible.CheckboxChanged += InvincibleChanged;
        _neverWanted.CheckboxChanged += NeverWantedChanged;
        _maxWantedLevel.ItemChanged += MaxWantedLevelChanged;
        _wantedLevel.ItemChanged += WantedLevelChanged;
        _lockMaxWantedLevel.CheckboxChanged += LockMaxWantedLevelChanged;
        _lockWantedLevel.CheckboxChanged += LockWantedLevelChanged;
        _addArmour.Activated += AddArmourActivated;
        _healPlayer.Activated += HealActivated;
        _replenishPlayer.Activated += ReplenishPlayer;
        _dispatchCops.CheckboxChanged += DispatchCopsChanged;
        _policeIgnore.CheckboxChanged += PoliceIgnoreChanged;
        _pedsIgnore.CheckboxChanged += PedsIgnoreChanged;

        _menu.Add(_invincible);
        _menu.Add(_neverWanted);
        _menu.Add(_wantedLevel);
        _menu.Add(_lockWantedLevel);
        _menu.Add(_maxWantedLevel);
        _menu.Add(_lockMaxWantedLevel);
        _menu.Add(_dispatchCops);
        _menu.Add(_policeIgnore);
        _menu.Add(_pedsIgnore);
        _menu.Add(_replenishPlayer);
        _menu.Add(_healPlayer);
        _menu.Add(_addArmour);

        _menu.Shown += MenuShown;

        return _menu;
    }

    private void PedsIgnoreChanged(object sender, EventArgs e)
    {
        Game.Player.IgnoredByEveryone = _pedsIgnore.Checked;
    }

    private void PoliceIgnoreChanged(object sender, EventArgs e)
    {
        Game.Player.IgnoredByPolice = _policeIgnore.Checked;
    }

    private void DispatchCopsChanged(object sender, EventArgs e)
    {
        Game.Player.DispatchsCops = _dispatchCops.Checked;
    }

    private void ReplenishPlayer(object sender, EventArgs e)
    {
        HealActivated(sender, e);
        AddArmourActivated(sender, e);
    }

    private void HealActivated(object sender, EventArgs e)
    {
        Game.Player.Character.Health = Game.Player.Character.MaxHealth;
    }

    private void AddArmourActivated(object sender, EventArgs e)
    {
        Game.Player.Character.Armor = 100;
    }

    private void Ticker_WantedLevelUpdate(object sender, EventArgs e)
    {
        if (_wantedLevel.SelectedItem != Ticker.WantedLevel)
        {
            _wantedLevel.SelectItemSilent(Ticker.WantedLevel);
        }
    }

    private void LockWantedLevelChanged(object sender, EventArgs e)
    {
        Ticker.LockWantedLevel = _lockWantedLevel.Checked;
    }

    private void WantedLevelChanged(object sender, LozengeItemEventArgs<int> e)
    {
        Game.Player.WantedLevel = _wantedLevel.SelectedItem;
        Ticker.WantedLevel = _wantedLevel.SelectedItem;
        Ticker.UpdateWanted(_wantedLevel.SelectedItem);
    }

    private void LockMaxWantedLevelChanged(object sender, EventArgs e)
    {
        Ticker.LockMaxWantedLevel = _lockMaxWantedLevel.Checked;
    }

    private void MenuShown(object sender, EventArgs e)
    {
        _maxWantedLevel.SelectItemSilent(Ticker.MaxWantedLevel);
    }

    private void MaxWantedLevelChanged(object sender, LozengeItemEventArgs<int> e)
    {
        Game.MaxWantedLevel = _maxWantedLevel.SelectedItem;
        _maxWantedLevel.SelectItemSilent(Ticker.MaxWantedLevel);
    }

    private void NeverWantedChanged(object sender, EventArgs e)
    {
        var c = _neverWanted.Checked;

        // Workaround for max wanted level stuck at 0
        if (c)
        {
            lastMax = Game.MaxWantedLevel;
        }
        else
        {
            Game.MaxWantedLevel = lastMax;
        }

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
