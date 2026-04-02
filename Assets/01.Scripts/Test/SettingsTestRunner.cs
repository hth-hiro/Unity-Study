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

    [SerializeField] private CrosshairSystem m_crosshairSystem;
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
        if (SettingsManager.Instance == null)
        {
            Debug.LogWarning("SettingsTestRunner: SettingsManager.Instance is null.");
            return;
        }

        SettingsManager.Instance.Load();
        SettingsManager.Instance.ApplyGraphics();
        PlayerController.Instance?.SetMouseSensitivity(SettingsManager.Instance.Play.MouseSensitivity);
        LogQualityLevels();
        LogCurrentSettings("Loaded");
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (SettingsManager.Instance == null || keyboard == null || !keyboard.f3Key.isPressed)
        {
            return;
        }

        bool isShiftPressed = keyboard.leftShiftKey.isPressed || keyboard.rightShiftKey.isPressed;

        if (keyboard.sKey.wasPressedThisFrame)
        {
            SettingsManager.Instance.Save();
            LogCurrentSettings("Settings Saved");
            return;
        }

        if (keyboard.lKey.wasPressedThisFrame)
        {
            SettingsManager.Instance.Load();
            SettingsManager.Instance.ApplyGraphics();
            PlayerController.Instance?.SetMouseSensitivity(SettingsManager.Instance.Play.MouseSensitivity);
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
            bool nextVSync = !SettingsManager.Instance.Graphics.VSyncEnabled;
            SettingsManager.Instance.SetVSyncEnabled(nextVSync);
            SettingsManager.Instance.ApplyGraphics();
            LogCurrentSettings("VSync Toggled");
            return;
        }

        if (keyboard.digit1Key.wasPressedThisFrame)
        {
            SettingsManager.Instance.SetResolution(1280, 720);
            SettingsManager.Instance.ApplyGraphics();
            LogCurrentSettings("Resolution Changed To 1280x720");
            return;
        }

        if (keyboard.digit2Key.wasPressedThisFrame)
        {
            SettingsManager.Instance.SetResolution(1920, 1080);
            SettingsManager.Instance.ApplyGraphics();
            LogCurrentSettings("Resolution Changed To 1920x1080");
            return;
        }

        if (keyboard.digit6Key.wasPressedThisFrame)
        {
            SettingsManager.Instance.SetFullscreenMode(FullScreenMode.Windowed);
            SettingsManager.Instance.ApplyGraphics();
            LogCurrentSettings("Display Mode Changed To Windowed");
            return;
        }

        if (keyboard.digit7Key.wasPressedThisFrame)
        {
            SettingsManager.Instance.SetFullscreenMode(FullScreenMode.FullScreenWindow);
            SettingsManager.Instance.ApplyGraphics();
            LogCurrentSettings("Display Mode Changed To FullScreenWindow");
            return;
        }

        if (keyboard.digit8Key.wasPressedThisFrame)
        {
            SettingsManager.Instance.SetFullscreenMode(FullScreenMode.ExclusiveFullScreen);
            SettingsManager.Instance.ApplyGraphics();
            LogCurrentSettings("Display Mode Changed To ExclusiveFullScreen");
            return;
        }

        if (keyboard.numpad1Key.wasPressedThisFrame)
        {
            AdjustAudioValue(SettingsManager.Instance.Audio.MasterVolume, isShiftPressed, SettingsManager.Instance.SetMasterVolume, "Master Volume Changed");
            return;
        }

        if (keyboard.numpad2Key.wasPressedThisFrame)
        {
            AdjustAudioValue(SettingsManager.Instance.Audio.BgmVolume, isShiftPressed, SettingsManager.Instance.SetBgmVolume, "BGM Volume Changed");
            return;
        }

        if (keyboard.numpad3Key.wasPressedThisFrame)
        {
            AdjustAudioValue(SettingsManager.Instance.Audio.SfxVolume, isShiftPressed, SettingsManager.Instance.SetSfxVolume, "SFX Volume Changed");
            return;
        }

        if (keyboard.numpad4Key.wasPressedThisFrame)
        {
            AdjustAudioValue(SettingsManager.Instance.Audio.VoiceVolume, isShiftPressed, SettingsManager.Instance.SetVoiceVolume, "Voice Volume Changed");
            return;
        }

        if (keyboard.numpad5Key.wasPressedThisFrame)
        {
            AdjustAudioValue(SettingsManager.Instance.Audio.UiVolume, isShiftPressed, SettingsManager.Instance.SetUiVolume, "UI Volume Changed");
            return;
        }

        if (keyboard.minusKey.wasPressedThisFrame)
        {
            float nextValue = Mathf.Clamp(
                SettingsManager.Instance.Play.MouseSensitivity - SENSITIVITY_STEP,
                MIN_MOUSE_SENSITIVITY,
                MAX_MOUSE_SENSITIVITY);
            SettingsManager.Instance.SetMouseSensitivity(nextValue);
            SettingsManager.Instance.ApplyPlay();
            PlayerController.Instance?.SetMouseSensitivity(nextValue);
            LogCurrentSettings($"Mouse Sensitivity Decreased : {SettingsManager.Instance.Play.MouseSensitivity:0.00}");
            return;
        }

        if (keyboard.equalsKey.wasPressedThisFrame)
        {
            float nextValue = Mathf.Clamp(
                SettingsManager.Instance.Play.MouseSensitivity + SENSITIVITY_STEP,
                MIN_MOUSE_SENSITIVITY,
                MAX_MOUSE_SENSITIVITY);
            SettingsManager.Instance.SetMouseSensitivity(nextValue);
            SettingsManager.Instance.ApplyPlay();
            PlayerController.Instance?.SetMouseSensitivity(nextValue);
            LogCurrentSettings($"Mouse Sensitivity Increased : {SettingsManager.Instance.Play.MouseSensitivity:0.00}");
            return;
        }

        if (keyboard.leftBracketKey.isPressed)
        {
            float nextValue = Mathf.Max(0.0f, SettingsManager.Instance.Play.CrosshairThickness - CROSSHAIR_SIZE_STEP);
            SettingsManager.Instance.SetCrosshairThickness(nextValue);
            SettingsManager.Instance.ApplyPlay();
            LogCurrentSettings("Crosshair Thickness Decreased");
            return;
        }

        if (keyboard.rightBracketKey.isPressed)
        {
            float nextValue = SettingsManager.Instance.Play.CrosshairThickness + CROSSHAIR_SIZE_STEP;
            SettingsManager.Instance.SetCrosshairThickness(nextValue);
            SettingsManager.Instance.ApplyPlay();
            LogCurrentSettings("Crosshair Thickness Increased");
            return;
        }

        if (keyboard.semicolonKey.isPressed)
        {
            float nextValue = Mathf.Max(0.0f, SettingsManager.Instance.Play.CrosshairLength - CROSSHAIR_LENGTH_STEP);
            SettingsManager.Instance.SetCrosshairLength(nextValue);
            SettingsManager.Instance.ApplyPlay();
            LogCurrentSettings("Crosshair Length Decreased");
            return;
        }

        if (keyboard.quoteKey.isPressed)
        {
            float nextValue = SettingsManager.Instance.Play.CrosshairLength + CROSSHAIR_LENGTH_STEP;
            SettingsManager.Instance.SetCrosshairLength(nextValue);
            SettingsManager.Instance.ApplyPlay();
            LogCurrentSettings("Crosshair Length Increased");
            return;
        }

        if (keyboard.commaKey.wasPressedThisFrame)
        {
            float nextValue = Mathf.Clamp01(SettingsManager.Instance.Play.CrosshairOpacity - CROSSHAIR_OPACITY_STEP);
            SettingsManager.Instance.SetCrosshairOpacity(nextValue);
            SettingsManager.Instance.ApplyPlay();
            LogCurrentSettings("Crosshair Opacity Decreased");
            return;
        }

        if (keyboard.periodKey.wasPressedThisFrame)
        {
            float nextValue = Mathf.Clamp01(SettingsManager.Instance.Play.CrosshairOpacity + CROSSHAIR_OPACITY_STEP);
            SettingsManager.Instance.SetCrosshairOpacity(nextValue);
            SettingsManager.Instance.ApplyPlay();
            LogCurrentSettings("Crosshair Opacity Increased");
            return;
        }

        if (keyboard.slashKey.wasPressedThisFrame)
        {
            int typeCount = System.Enum.GetValues(typeof(PlaySettingsData.CrosshairTypeOption)).Length;
            int currentTypeIndex = (int)SettingsManager.Instance.Play.CrosshairType;
            int nextTypeIndex = (currentTypeIndex + 1) % typeCount;

            SettingsManager.Instance.SetCrosshairType((PlaySettingsData.CrosshairTypeOption)nextTypeIndex);
            SettingsManager.Instance.ApplyPlay();
            LogCurrentSettings("Crosshair Type Changed");
            return;
        }

        if (keyboard.backslashKey.wasPressedThisFrame)
        {
            int currentColorIndex = GetClosestCrosshairColorIndex(SettingsManager.Instance.Play.CrosshairColor);
            int nextColorIndex = (currentColorIndex + 1) % m_crosshairColorPresets.Length;

            SettingsManager.Instance.SetCrosshairColor(m_crosshairColorPresets[nextColorIndex]);
            SettingsManager.Instance.ApplyPlay();
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

        int currentQualityLevel = SettingsManager.Instance.Graphics.QualityLevel;
        int nextQualityLevel = (currentQualityLevel + 1) % qualityCount;

        SettingsManager.Instance.SetQualityLevel(nextQualityLevel);
        SettingsManager.Instance.ApplyGraphics();
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
        if (SettingsManager.Instance == null)
        {
            Debug.LogWarning("SettingsTestRunner: SettingsManager.Instance is null.");
            return;
        }

        GraphicsSettingsData graphics = SettingsManager.Instance.Graphics;
        AudioSettingsData audio = SettingsManager.Instance.Audio;
        PlaySettingsData play = SettingsManager.Instance.Play;

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
            $"MouseSensitivity: {play.MouseSensitivity:0.00}, " +
            $"CrosshairType: {play.CrosshairType}, " +
            $"CrosshairColor: {play.CrosshairColor}, " +
            $"CrosshairThickness: {play.CrosshairThickness:0.0}, " +
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
