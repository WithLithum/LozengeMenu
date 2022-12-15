namespace LozengeMenu.Submenus;

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
    private readonly NativeCheckboxItem _neverWanted = new("Never Wanted");
    private readonly NativeListItem<int> _maxWantedLevel = new("Max Wanted Level", 0, 1, 2, 3, 4, 5);

    public NativeMenu Create()
    {
        _invincible.CheckboxChanged += InvincibleChanged;
        _neverWanted.CheckboxChanged += NeverWantedChanged;
        _maxWantedLevel.ItemChanged += MaxWantedLevelChanged;

        _menu.Add(_invincible);
        _menu.Add(_neverWanted);
        _menu.Add(_maxWantedLevel);

        return _menu;
    }

    private void MaxWantedLevelChanged(object sender, ItemChangedEventArgs<int> e)
    {
        Ticker.MaxWantedLevel = _maxWantedLevel.SelectedItem;
    }

    private void NeverWantedChanged(object sender, EventArgs e)
    {
        var c = _neverWanted.Checked;

        _maxWantedLevel.Enabled = !c;
        Ticker.NeverWanted = c;
    }

    private void InvincibleChanged(object sender, EventArgs e)
    {
        Ticker.UpdateInvincible(_invincible.Checked);
    }
}
