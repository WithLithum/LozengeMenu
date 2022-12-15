namespace LozengeMenu.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
