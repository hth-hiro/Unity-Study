using UnityEngine;

public interface IAudioSettingsApplier
{
    void Apply(AudioSettingsData settings);
}

public interface IPlaySettingsApplier
{
    void Apply(PlaySettingsData settings);
}

/// <summary>
/// Central manager for loading, storing, changing, applying, and saving game settings.
/// </summary>
public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    private GameSettingsData m_currentSettings;
    private SettingsSaveService m_settingsSaveService;
    private IAudioSettingsApplier m_audioSettingsApplier;
    private IPlaySettingsApplier m_playSettingsApplier;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Initialize();
    }

    private void Initialize()
    {
        if (m_settingsSaveService == null)
        {
            m_settingsSaveService = new SettingsSaveService();
        }

        if (m_currentSettings == null)
        {
            m_currentSettings = new GameSettingsData();
        }
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

    public AudioSettingsData Audio
    {
        get { return CurrentSettings.Audio; }
    }

    public PlaySettingsData Play
    {
        get { return CurrentSettings.Play; }
    }

    public void SetAudioSettingsApplier(IAudioSettingsApplier applier)
    {
        m_audioSettingsApplier = applier;
    }

    public void SetPlaySettingsApplier(IPlaySettingsApplier applier)
    {
        m_playSettingsApplier = applier;
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

        ApplyAll();
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

    public void ApplyAudio()
    {
        AudioSettingsData audio = Audio;
        if (audio == null || m_audioSettingsApplier == null)
        {
            return;
        }

        m_audioSettingsApplier.Apply(audio);
    }

    public void ApplyPlay()
    {
        PlaySettingsData play = Play;
        if (play == null || m_playSettingsApplier == null)
        {
            return;
        }

        m_playSettingsApplier.Apply(play);
    }

    public void ApplyAll()
    {
        ApplyGraphics();
        ApplyAudio();
        ApplyPlay();
    }

    public void ResetToDefault()
    {
        m_currentSettings = new GameSettingsData();
        ApplyAll();
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

    public void SetMasterVolume(float value)
    {
        Audio.MasterVolume = Mathf.Clamp01(value);
    }

    public void SetSfxVolume(float value)
    {
        Audio.SfxVolume = Mathf.Clamp01(value);
    }

    public void SetBgmVolume(float value)
    {
        Audio.BgmVolume = Mathf.Clamp01(value);
    }

    public void SetVoiceVolume(float value)
    {
        Audio.VoiceVolume = Mathf.Clamp01(value);
    }

    public void SetUiVolume(float value)
    {
        Audio.UiVolume = Mathf.Clamp01(value);
    }

    public void SetMouseSensitivity(float value)
    {
        Play.MouseSensitivity = Mathf.Max(0.0f, value);
    }

    public void SetCrosshairType(PlaySettingsData.CrosshairTypeOption value)
    {
        Play.CrosshairType = value;
    }

    public void SetCrosshairColor(Color value)
    {
        Play.CrosshairColor = value;
    }

    public void SetCrosshairThickness(float value)
    {
        Play.CrosshairThickness = Mathf.Max(0.0f, value);
    }

    public void SetCrosshairLength(float value)
    {
        Play.CrosshairLength = Mathf.Max(0.0f, value);
    }

    public void SetCrosshairOpacity(float value)
    {
        Play.CrosshairOpacity = Mathf.Clamp01(value);
    }
}
