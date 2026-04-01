using UnityEngine;
using UnityEngine.InputSystem;

public class SettingsTestRunner : MonoBehaviour
{
    private const float AUDIO_STEP = 0.1f;
    private const float SENSITIVITY_STEP = 0.1f;
    private const float MIN_MOUSE_SENSITIVITY = 0.1f;
    private const float MAX_MOUSE_SENSITIVITY = 100.0f;
    private const float CROSSHAIR_SIZE_STEP = 1.0f;
    private const float CROSSHAIR_LENGTH_STEP = 1.0f;
    private const float CROSSHAIR_OPACITY_STEP = 0.1f;

    private SettingsManager m_settingsManager;
    private readonly Color[] m_crosshairColorPresets =
    {
        Color.white,
        Color.red,
        Color.green,
        Color.cyan,
        Color.yellow
    };

    private void Start()
    {
        m_settingsManager = new SettingsManager();
        m_settingsManager.Load();
        m_settingsManager.ApplyGraphics();
        PlayerController.Instance?.SetMouseSensitivity(m_settingsManager.Play.MouseSensitivity);
        LogQualityLevels();
        LogCurrentSettings("Loaded");
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (m_settingsManager == null || keyboard == null || !keyboard.f3Key.isPressed)
        {
            return;
        }

        bool isShiftPressed = keyboard.leftShiftKey.isPressed || keyboard.rightShiftKey.isPressed;

        if (keyboard.sKey.wasPressedThisFrame)
        {
            m_settingsManager.Save();
            LogCurrentSettings("Settings Saved");
            return;
        }

        if (keyboard.lKey.wasPressedThisFrame)
        {
            m_settingsManager.Load();
            m_settingsManager.ApplyGraphics();
            PlayerController.Instance?.SetMouseSensitivity(m_settingsManager.Play.MouseSensitivity);
            LogCurrentSettings("Settings Loaded");
            return;
        }

        if (keyboard.qKey.wasPressedThisFrame)
        {
            CycleQualityLevel();
            return;
        }

        if (keyboard.vKey.wasPressedThisFrame)
        {
            bool nextVSync = !m_settingsManager.Graphics.VSyncEnabled;
            m_settingsManager.SetVSyncEnabled(nextVSync);
            m_settingsManager.ApplyGraphics();
            LogCurrentSettings("VSync Toggled");
            return;
        }

        if (keyboard.digit1Key.wasPressedThisFrame)
        {
            m_settingsManager.SetResolution(1280, 720);
            m_settingsManager.ApplyGraphics();
            LogCurrentSettings("Resolution Changed To 1280x720");
            return;
        }

        if (keyboard.digit2Key.wasPressedThisFrame)
        {
            m_settingsManager.SetResolution(1920, 1080);
            m_settingsManager.ApplyGraphics();
            LogCurrentSettings("Resolution Changed To 1920x1080");
            return;
        }

        if (keyboard.digit6Key.wasPressedThisFrame)
        {
            m_settingsManager.SetFullscreenMode(FullScreenMode.Windowed);
            m_settingsManager.ApplyGraphics();
            LogCurrentSettings("Display Mode Changed To Windowed");
            return;
        }

        if (keyboard.digit7Key.wasPressedThisFrame)
        {
            m_settingsManager.SetFullscreenMode(FullScreenMode.FullScreenWindow);
            m_settingsManager.ApplyGraphics();
            LogCurrentSettings("Display Mode Changed To FullScreenWindow");
            return;
        }

        if (keyboard.digit8Key.wasPressedThisFrame)
        {
            m_settingsManager.SetFullscreenMode(FullScreenMode.ExclusiveFullScreen);
            m_settingsManager.ApplyGraphics();
            LogCurrentSettings("Display Mode Changed To ExclusiveFullScreen");
            return;
        }

        if (keyboard.numpad1Key.wasPressedThisFrame)
        {
            AdjustAudioValue(m_settingsManager.Audio.MasterVolume, isShiftPressed, m_settingsManager.SetMasterVolume, "Master Volume Changed");
            return;
        }

        if (keyboard.numpad2Key.wasPressedThisFrame)
        {
            AdjustAudioValue(m_settingsManager.Audio.BgmVolume, isShiftPressed, m_settingsManager.SetBgmVolume, "BGM Volume Changed");
            return;
        }

        if (keyboard.numpad3Key.wasPressedThisFrame)
        {
            AdjustAudioValue(m_settingsManager.Audio.SfxVolume, isShiftPressed, m_settingsManager.SetSfxVolume, "SFX Volume Changed");
            return;
        }

        if (keyboard.numpad4Key.wasPressedThisFrame)
        {
            AdjustAudioValue(m_settingsManager.Audio.VoiceVolume, isShiftPressed, m_settingsManager.SetVoiceVolume, "Voice Volume Changed");
            return;
        }

        if (keyboard.numpad5Key.wasPressedThisFrame)
        {
            AdjustAudioValue(m_settingsManager.Audio.UiVolume, isShiftPressed, m_settingsManager.SetUiVolume, "UI Volume Changed");
            return;
        }

        if (keyboard.minusKey.wasPressedThisFrame)
        {
            float nextValue = Mathf.Clamp(
                m_settingsManager.Play.MouseSensitivity - SENSITIVITY_STEP,
                MIN_MOUSE_SENSITIVITY,
                MAX_MOUSE_SENSITIVITY);
            m_settingsManager.SetMouseSensitivity(nextValue);
            PlayerController.Instance?.SetMouseSensitivity(nextValue);
            LogCurrentSettings("Mouse Sensitivity Decreased");
            return;
        }

        if (keyboard.equalsKey.wasPressedThisFrame)
        {
            float nextValue = Mathf.Clamp(
                m_settingsManager.Play.MouseSensitivity + SENSITIVITY_STEP,
                MIN_MOUSE_SENSITIVITY,
                MAX_MOUSE_SENSITIVITY);
            m_settingsManager.SetMouseSensitivity(nextValue);
            PlayerController.Instance?.SetMouseSensitivity(nextValue);
            LogCurrentSettings("Mouse Sensitivity Increased");
            return;
        }

        if (keyboard.leftBracketKey.wasPressedThisFrame)
        {
            float nextValue = Mathf.Max(0.0f, m_settingsManager.Play.CrosshairSize - CROSSHAIR_SIZE_STEP);
            m_settingsManager.SetCrosshairSize(nextValue);
            LogCurrentSettings("Crosshair Size Decreased");
            return;
        }

        if (keyboard.rightBracketKey.wasPressedThisFrame)
        {
            float nextValue = m_settingsManager.Play.CrosshairSize + CROSSHAIR_SIZE_STEP;
            m_settingsManager.SetCrosshairSize(nextValue);
            LogCurrentSettings("Crosshair Size Increased");
            return;
        }

        if (keyboard.semicolonKey.wasPressedThisFrame)
        {
            float nextValue = Mathf.Max(0.0f, m_settingsManager.Play.CrosshairLength - CROSSHAIR_LENGTH_STEP);
            m_settingsManager.SetCrosshairLength(nextValue);
            LogCurrentSettings("Crosshair Length Decreased");
            return;
        }

        if (keyboard.quoteKey.wasPressedThisFrame)
        {
            float nextValue = m_settingsManager.Play.CrosshairLength + CROSSHAIR_LENGTH_STEP;
            m_settingsManager.SetCrosshairLength(nextValue);
            LogCurrentSettings("Crosshair Length Increased");
            return;
        }

        if (keyboard.commaKey.wasPressedThisFrame)
        {
            float nextValue = Mathf.Clamp01(m_settingsManager.Play.CrosshairOpacity - CROSSHAIR_OPACITY_STEP);
            m_settingsManager.SetCrosshairOpacity(nextValue);
            LogCurrentSettings("Crosshair Opacity Decreased");
            return;
        }

        if (keyboard.periodKey.wasPressedThisFrame)
        {
            float nextValue = Mathf.Clamp01(m_settingsManager.Play.CrosshairOpacity + CROSSHAIR_OPACITY_STEP);
            m_settingsManager.SetCrosshairOpacity(nextValue);
            LogCurrentSettings("Crosshair Opacity Increased");
            return;
        }

        if (keyboard.slashKey.wasPressedThisFrame)
        {
            int typeCount = System.Enum.GetValues(typeof(PlaySettingsData.CrosshairTypeOption)).Length;
            int currentTypeIndex = (int)m_settingsManager.Play.CrosshairType;
            int nextTypeIndex = (currentTypeIndex + 1) % typeCount;

            m_settingsManager.SetCrosshairType((PlaySettingsData.CrosshairTypeOption)nextTypeIndex);
            LogCurrentSettings("Crosshair Type Changed");
            return;
        }

        if (keyboard.backslashKey.wasPressedThisFrame)
        {
            int currentColorIndex = GetClosestCrosshairColorIndex(m_settingsManager.Play.CrosshairColor);
            int nextColorIndex = (currentColorIndex + 1) % m_crosshairColorPresets.Length;

            m_settingsManager.SetCrosshairColor(m_crosshairColorPresets[nextColorIndex]);
            LogCurrentSettings("Crosshair Color Changed");
        }
    }

    private void LogQualityLevels()
    {
        string[] qualityNames = QualitySettings.names;
        if (qualityNames == null || qualityNames.Length == 0)
        {
            Debug.LogWarning("SettingsTestRunner: No quality levels found.");
            return;
        }

        string qualityLog = "SettingsTestRunner: Quality Levels";
        for (int i = 0; i < qualityNames.Length; i++)
        {
            qualityLog += $" | [{i}] {qualityNames[i]}";
        }

        Debug.Log(qualityLog);
    }

    private void CycleQualityLevel()
    {
        int qualityCount = QualitySettings.names.Length;
        if (qualityCount <= 1)
        {
            Debug.LogWarning("SettingsTestRunner: Quality Level count is 1 or less.");
            LogCurrentSettings("Quality Level Change Skipped");
            return;
        }

        int currentQualityLevel = m_settingsManager.Graphics.QualityLevel;
        int nextQualityLevel = (currentQualityLevel + 1) % qualityCount;

        m_settingsManager.SetQualityLevel(nextQualityLevel);
        m_settingsManager.ApplyGraphics();
        LogCurrentSettings("Quality Level Changed");
    }

    private void AdjustAudioValue(float currentValue, bool isShiftPressed, System.Action<float> setter, string prefix)
    {
        float delta = isShiftPressed ? -AUDIO_STEP : AUDIO_STEP;
        float nextValue = Mathf.Clamp01(currentValue + delta);
        setter(nextValue);
        LogCurrentSettings(prefix);
    }

    private void LogCurrentSettings(string prefix)
    {
        GraphicsSettingsData graphics = m_settingsManager.Graphics;
        AudioSettingsData audio = m_settingsManager.Audio;
        PlaySettingsData play = m_settingsManager.Play;

        if (graphics == null || audio == null || play == null)
        {
            Debug.LogWarning("SettingsTestRunner: Settings data is null.");
            return;
        }

        Resolution currentResolution = Screen.currentResolution;
        int actualVSyncCount = QualitySettings.vSyncCount;
        int actualQualityLevel = QualitySettings.GetQualityLevel();
        int actualTargetFrameRate = Application.targetFrameRate;
        FullScreenMode actualFullscreenMode = Screen.fullScreenMode;

        Debug.Log(
            $"{prefix}\n" +
            $"Saved Settings | " +
            $"Resolution: {graphics.ResolutionWidth}x{graphics.ResolutionHeight}, " +
            $"FullscreenMode: {graphics.FullscreenMode}, " +
            $"QualityLevel: {graphics.QualityLevel}, " +
            $"VSync: {graphics.VSyncEnabled}, " +
            $"TargetFrameRate: {graphics.TargetFrameRate}\n" +
            $"Audio | " +
            $"Master: {audio.MasterVolume:0.0}, " +
            $"BGM: {audio.BgmVolume:0.0}, " +
            $"SFX: {audio.SfxVolume:0.0}, " +
            $"Voice: {audio.VoiceVolume:0.0}, " +
            $"UI: {audio.UiVolume:0.0}\n" +
            $"Play | " +
            $"MouseSensitivity: {play.MouseSensitivity:0.0}, " +
            $"CrosshairType: {play.CrosshairType}, " +
            $"CrosshairColor: {play.CrosshairColor}, " +
            $"CrosshairSize: {play.CrosshairSize:0.0}, " +
            $"CrosshairLength: {play.CrosshairLength:0.0}, " +
            $"CrosshairOpacity: {play.CrosshairOpacity:0.0}\n" +
            $"Actual Applied State | " +
            $"Resolution: {currentResolution.width}x{currentResolution.height}, " +
            $"FullscreenMode: {actualFullscreenMode}, " +
            $"QualityLevel: {actualQualityLevel}, " +
            $"VSyncCount: {actualVSyncCount}, " +
            $"TargetFrameRate: {actualTargetFrameRate}"
        );
    }

    private int GetClosestCrosshairColorIndex(Color targetColor)
    {
        int closestIndex = 0;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < m_crosshairColorPresets.Length; i++)
        {
            float distance = Vector4.Distance(m_crosshairColorPresets[i], targetColor);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }
}
