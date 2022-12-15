namespace LozengeMenu.Submenus;

using LemonUI.Menus;
using LozengeMenu.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class TimeMenu : ISubMenu
{
    private readonly NativeMenu _menu = new("Lozenge", "Time Options");
    private readonly NativeListItem<int> _itemHour = new("Hour", 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16
        , 17, 18, 19, 20, 21, 22, 23);

    private readonly NativeListItem<int> _itemMinute = new("Minute", 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16
        , 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43,
        44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59);

    private readonly NativeCheckboxItem _itemLock = new("Freeze Time", "Locks the current time to the selected time.");

    public NativeMenu Create()
    {
        _menu.Add(_itemHour);
        _menu.Add(_itemMinute);
        _menu.Add(_itemLock);

        _itemHour.ItemChanged += HourChanged;
        _itemMinute.ItemChanged += MinuteChanged;
        _itemLock.CheckboxChanged += _itemLock_CheckboxChanged;

        Ticker.ClockUpdate += Ticker_ClockUpdate;

        return _menu;
    }

    private void _itemLock_CheckboxChanged(object sender, EventArgs e)
    {
        Ticker.LockTime = _itemLock.Checked;
    }

    private void Ticker_ClockUpdate(object sender, EventArgs e)
    {
        _itemHour.SelectedItem = Ticker.Hour;
        _itemMinute.SelectedItem = Ticker.Minute;
    }

    private void MinuteChanged(object sender, ItemChangedEventArgs<int> e)
    {
        Ticker.Minute = e.Object;
        Ticker.UpdateGameTime();
    }

    private void HourChanged(object sender, ItemChangedEventArgs<int> e)
    {
        Ticker.Hour = e.Object;
        Ticker.UpdateGameTime();
    }
}
