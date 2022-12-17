﻿// Copyright (C) WithLithum 2022.
// Licensed under GNU General Public License, either version 3 or any later
// version of your choice.

namespace LozengeMenu.Core;

using GTA;
using LozengeMenu.Core.Worlds;
using System;

[ScriptAttributes(Author = "WithLithum", NoDefaultInstance = true)]
public class Ticker : Script
{
    #region Player Options
    public static bool Invincible { get; set; }

    public static bool NeverWanted { get; set; }
    public static int MaxWantedLevel { get; set; }
    public static bool LockMaxWantedLevel { get; set; }
    public static int WantedLevel { get; set; }
    public static bool LockWantedLevel { get; set; }

    private static int LastPlayer { get; set; }

    public static event EventHandler WantedLevelUpdate;

    public static void UpdateInvincible(bool enable)
    {
        Invincible = enable;

        if (!enable && FastUtil.IsEntityValid(LastPlayer))
        {
            FastUtil.SetInvincible(LastPlayer, enable);
        }
    }

    private void ProcessPlayer(Ped player)
    {
        LastPlayer = player.Handle;

        if (Invincible)
        {
            player.IsInvincible = true;
        }

        if (!NeverWanted)
        {
            if (LockMaxWantedLevel)
            {
                Util.CheckLockMaxWantedLevel(MaxWantedLevel);
            }
            else
            {
                MaxWantedLevel = Game.MaxWantedLevel;
            }

            if (!LockWantedLevel)
            {
                if (WantedLevel != Game.Player.WantedLevel)
                {
                    WantedLevel = Game.Player.WantedLevel;
                    WantedLevelUpdate?.Invoke(null, EventArgs.Empty);
                }
            }
            else
            {
                Game.Player.WantedLevel = WantedLevel;
            }
        }
        else
        {
            Util.CheckLockMaxWantedLevel(0);
            Game.Player.WantedLevel = 0;
        }
    }
    #endregion
    #region Time Options
    public static int Hour { get; set; }
    public static int Minute { get; set; }
    public static bool LockTime { get; set; }

    public Ticker()
    {
        WantedLevel = Game.Player.WantedLevel;
        MaxWantedLevel = Game.MaxWantedLevel;

        Tick += Ticker_Tick;
    }

    public static event EventHandler ClockUpdate;

    public static void UpdateGameTime()
    {
        if (!LockTime)
        {
            FastUtil.SetGameTime(Hour, Minute);
        }
    }

    private void ProcessTime()
    {
        if (LockTime)
        {
            FastUtil.SetGameTime(Hour, Minute);
        }
        else
        {
            Hour = Natives.GetClockHours();
            Minute = Natives.GetClockMinutes();
            ClockUpdate?.Invoke(null, EventArgs.Empty);
        }
    }
    #endregion

    #region Vehicle Options
    private bool _wasInVehicle;
    private int _editorCounter;

    public static event EventHandler<bool> InVehicleStatusUpdate;

    private void ProcessVehicle(Ped player)
    {
        var inVehicle = Natives.IsPedInAnyVehicle(player.Handle, false);
        if (inVehicle != _wasInVehicle)
        {
            _wasInVehicle = inVehicle;
            InVehicleStatusUpdate?.Invoke(null, inVehicle);
        }

        VehicleEditor.Update();
    }
    #endregion

    private void Ticker_Tick(object sender, EventArgs e)
    {
        var player = Game.Player.Character;

        ProcessPlayer(player);
        ProcessTime();

        if (_editorCounter < 10)
        {
            _editorCounter++;
        }
        else
        {
            _editorCounter = 0;
            ProcessVehicle(player);
        }
    }
}
