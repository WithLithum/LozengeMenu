// Copyright (C) WithLithum 2022.
// Licensed under GNU General Public License, either version 3 or any later
// version of your choice.

namespace LozengeMenu.Config;

using GTA.UI;
using LozengeMenu.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;

internal static class ConfigManager
{
    public readonly struct ConfigEventArgs
    {
        public ConfigEventArgs(ConfigFile file)
        {
            Config = file;
        }

        public ConfigFile Config { get; }
    }

    private const string _configFilePath = @".lozenge\config.json";

    public static event EventHandler<ConfigEventArgs> ConfigReload;

    private static readonly JsonSerializer _serializer = new()
    {
        ContractResolver = new DefaultContractResolver()
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        }
    };

    internal static ConfigFile ConfigFile { get; private set; }

    internal static void LoadConfig()
    {
        if (!File.Exists(_configFilePath))
        {
            ConfigFile = new();
            SaveConfig();
            return;
        }

        using var reader = new JsonTextReader(new StreamReader(File.OpenRead(_configFilePath)));
        try
        {
            ConfigFile = _serializer.Deserialize<ConfigFile>(reader);
        }
        catch (Exception ex)
        {
            Notification.Show($"Unable to load config: {ex.Message}");
            ConfigFile = new();
        }

        ConfigReload?.Invoke(null, new ConfigEventArgs(ConfigFile));
    }

    internal static void SaveConfig()
    {
        using var writer = new StreamWriter(File.Create(_configFilePath));
        _serializer.Serialize(writer, ConfigFile);
    }
}
