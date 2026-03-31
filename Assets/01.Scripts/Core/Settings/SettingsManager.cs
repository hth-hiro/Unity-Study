using UnityEngine;

/// <summary>
/// Central manager for loading, storing, changing, applying, and saving game settings.
/// </summary>
public class SettingsManager
{
    private GameSettingsData m_currentSettings;
    private SettingsSaveService m_settingsSaveService;

    public SettingsManager()
        : this(null)
    {
    }

    public SettingsManager(SettingsSaveService settingsSaveService)
    {
        m_settingsSaveService = settingsSaveService ?? new SettingsSaveService();
        m_currentSettings = new GameSettingsData();
    }

    public GameSettingsData CurrentSettings
    {
        get
        {
            if (m_currentSettings == null)
            {
                m_currentSettings = new GameSettingsData();
            }

            return m_currentSettings;
        }
    }

    public GraphicsSettingsData Graphics
    {
        get { return CurrentSettings.Graphics; }
    }

    public void Load()
    {
        if (m_settingsSaveService == null)
        {
            m_settingsSaveService = new SettingsSaveService();
        }

        m_currentSettings = m_settingsSaveService.Load();
        if (m_currentSettings == null)
        {
            m_currentSettings = new GameSettingsData();
        }

        ApplyGraphics();
    }

    public void Save()
    {
        if (m_settingsSaveService == null)
        {
            m_settingsSaveService = new SettingsSaveService();
        }

        m_settingsSaveService.Save(CurrentSettings);
    }

    public void ApplyGraphics()
    {
        GraphicsSettingsData graphics = Graphics;
        if (graphics == null)
        {
            return;
        }

        Screen.SetResolution(graphics.ResolutionWidth, graphics.ResolutionHeight, graphics.FullscreenMode);
        QualitySettings.SetQualityLevel(graphics.QualityLevel, true);

        if (graphics.VSyncEnabled)
        {
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = -1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = graphics.TargetFrameRate;
        }
    }

    public void ResetToDefault()
    {
        m_currentSettings = new GameSettingsData();
        ApplyGraphics();
        Save();
    }

    public void SetResolution(int width, int height)
    {
        Graphics.ResolutionWidth = width;
        Graphics.ResolutionHeight = height;
    }

    public void SetFullscreenMode(FullScreenMode fullscreenMode)
    {
        Graphics.FullscreenMode = fullscreenMode;
    }

    public void SetQualityLevel(int qualityLevel)
    {
        Graphics.QualityLevel = qualityLevel;
    }

    public void SetVSyncEnabled(bool enabled)
    {
        Graphics.VSyncEnabled = enabled;
    }

    public void SetTargetFrameRate(int targetFrameRate)
    {
        Graphics.TargetFrameRate = targetFrameRate;
    }
}
