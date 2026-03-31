using System;
using System.IO;
using UnityEngine;

/// <summary>
/// Saves and loads game settings data as a JSON file.
/// </summary>
public class SettingsSaveService
{
    private readonly string m_fileName = "settings.json";

    public void Save(GameSettingsData settingsData)
    {
        GameSettingsData targetData = settingsData ?? CreateDefaultData();

        try
        {
            string json = JsonUtility.ToJson(targetData, true);
            File.WriteAllText(GetFilePath(), json);
        }
        catch (Exception exception)
        {
            Debug.LogError($"SettingsSaveService: Failed to save settings. {exception.Message}");
        }
    }

    public GameSettingsData Load()
    {
        if (!Exists())
        {
            return CreateDefaultData();
        }

        try
        {
            string json = File.ReadAllText(GetFilePath());
            if (string.IsNullOrWhiteSpace(json))
            {
                return CreateDefaultData();
            }

            GameSettingsData settingsData = JsonUtility.FromJson<GameSettingsData>(json);
            if (settingsData == null)
            {
                return CreateDefaultData();
            }

            if (settingsData.Graphics == null)
            {
                settingsData.Graphics = new GraphicsSettingsData();
            }

            return settingsData;
        }
        catch (Exception exception)
        {
            Debug.LogWarning($"SettingsSaveService: Failed to load settings. {exception.Message}");
            return CreateDefaultData();
        }
    }

    public bool Exists()
    {
        return File.Exists(GetFilePath());
    }

    public void Delete()
    {
        if (!Exists())
        {
            return;
        }

        try
        {
            File.Delete(GetFilePath());
        }
        catch (Exception exception)
        {
            Debug.LogWarning($"SettingsSaveService: Failed to delete settings file. {exception.Message}");
        }
    }

    private string GetFilePath()
    {
        return Path.Combine(Application.persistentDataPath, m_fileName);
    }

    private GameSettingsData CreateDefaultData()
    {
        return new GameSettingsData();
    }
}
