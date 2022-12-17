// Copyright (C) WithLithum 2022.
// Licensed under GNU General Public License, either version 3 or any later
// version of your choice.

namespace LozengeMenu.Core.Worlds;

using GTA;

internal static class EntitySpawner
{
    public static void SpawnVehicleInteractive()
    {
        Model model;

        while (true)
        {
            var input = Game.GetUserInput(WindowTitle.EnterMessage20, string.Empty, 20);

            if (input?.Length == 0)
            {
                // User cancelled
                // We cannot detect if user actually cancelled or just gave us an empty string
                return;
            }

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
