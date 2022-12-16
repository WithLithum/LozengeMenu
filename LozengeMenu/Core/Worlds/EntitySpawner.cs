namespace LozengeMenu.Core.Worlds;

using GTA;
using LemonUI.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static class EntitySpawner
{
    public static void SpawnVehicleInteractive()
    {
        Model model;

        while (true)
        {
            var input = Game.GetUserInput(WindowTitle.EnterMessage20, string.Empty, 20);

            var hash = Natives.GetHashKey(input);

            if (!Natives.IsModelValid(hash) || !Natives.IsModelAVehicle(hash))
            {
                continue;
            }

            model = new Model(input);
            break;
        }

        SpawnVehicle(model);
    }

    public static void SpawnVehicle(Model model)
    {
        if (!model.IsValid || !model.IsVehicle) return;
        var p = Game.Player.Character;

        World.CreateVehicle(model, p.FrontPosition, p.Heading);
    }
}
