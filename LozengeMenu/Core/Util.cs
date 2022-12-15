namespace LozengeMenu.Core;

using GTA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static class Util
{
    internal static void CheckLockMaxWantedLevel(int target)
    {
        if (Game.MaxWantedLevel != target)
        {
            Game.MaxWantedLevel = target;
        }
    }
}
