// Copyright (C) WithLithum 2022.
// Licensed under GNU General Public License, either version 3 or any later
// version of your choice.

namespace LozengeMenu.Core;

using GTA;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;

internal static class Util
{
    internal static bool TryAskQuestion(WindowTitle title, int max, Predicate<string> check, out string result)
    {
        while (true)
        {
            var input = Game.GetUserInput(title, string.Empty, max);

            if (input?.Length == 0)
            {
                result = null;
                break;
            }

            try
            {
                if (check?.Invoke(input) == false)
                {
                    continue;
                }
            }
            catch
            {
                result = null;
                break;
            }

            result = input;
            return true;
        }

        return false;
    }

    internal static JsonSerializer Serializer { get; } = new()
    {
        ContractResolver = new DefaultContractResolver()
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        },
        Formatting = Formatting.Indented,
    };

    internal static void CheckLockMaxWantedLevel(int target)
    {
        if (Game.MaxWantedLevel != target)
        {
            Game.MaxWantedLevel = target;
        }
    }
}
