// Copyright (C) WithLithum 2022.
// Licensed under GNU General Public License, either version 3 or any later
// version of your choice.

namespace LozengeMenu.Core;
/// <summary>
/// Provides methods to access the game engine natives directly without the need to create
/// objects or use .NET data type instances.
/// </summary>
public static class FastUtil
{
    public static void SetGameTime(int hour, int minute)
    {
        Natives.NetworkOverrideClockTime(hour, minute, 0);
    }

    public static bool IsEntityValid(int entity)
    {
        return Natives.DoesEntityExist(entity);
    }

    public static void SetInvincible(int ped, bool value)
    {
        Natives.SetEntityInvincible(ped, value);
    }
}
