namespace LozengeMenu.Core;

using GTA;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static class Util
{
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
