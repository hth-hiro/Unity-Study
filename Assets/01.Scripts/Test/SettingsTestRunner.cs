using UnityEngine;

public class SettingsTestRunner : MonoBehaviour
{
    private SettingsManager m_settingsManager;

    private void Start()
    {
        m_settingsManager = new SettingsManager();
        m_settingsManager.Load();
        m_settingsManager.ApplyGraphics();
        LogQualityLevels();
        LogCurrentSettings("Loaded");
    }

    private void Update()
    {
        if (m_settingsManager == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            m_settingsManager.SetResolution(1280, 720);
            m_settingsManager.ApplyGraphics();
            LogCurrentSettings("Set Resolution 1280x720");
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            m_settingsManager.SetResolution(1920, 1080);
            m_settingsManager.ApplyGraphics();
            LogCurrentSettings("Set Resolution 1920x1080");
        }

        if (Input.GetKeyDown(KeyCode.F3))
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
            LogCurrentSettings("Changed Quality Level");
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            bool nextVSync = !m_settingsManager.Graphics.VSyncEnabled;
            m_settingsManager.SetVSyncEnabled(nextVSync);
            m_settingsManager.ApplyGraphics();
            LogCurrentSettings("Toggled VSync");
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            m_settingsManager.SetFullscreenMode(FullScreenMode.Windowed);
            m_settingsManager.ApplyGraphics();
            LogCurrentSettings("Set Display Mode Windowed");
        }

        if (Input.GetKeyDown(KeyCode.F7))
        {
            m_settingsManager.SetFullscreenMode(FullScreenMode.FullScreenWindow);
            m_settingsManager.ApplyGraphics();
            LogCurrentSettings("Set Display Mode FullScreenWindow");
        }

        if (Input.GetKeyDown(KeyCode.F8))
        {
            m_settingsManager.SetFullscreenMode(FullScreenMode.ExclusiveFullScreen);
            m_settingsManager.ApplyGraphics();
            LogCurrentSettings("Set Display Mode ExclusiveFullScreen");
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            m_settingsManager.Save();
            Debug.Log("Settings Saved");
            LogCurrentSettings("Saved");
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            m_settingsManager.Load();
            m_settingsManager.ApplyGraphics();
            LogCurrentSettings("Reloaded");
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

    private void LogCurrentSettings(string prefix)
    {
        GraphicsSettingsData graphics = m_settingsManager.Graphics;
        if (graphics == null)
        {
            Debug.LogWarning("SettingsTestRunner: Graphics settings are null.");
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
            $"Actual Applied State | " +
            $"Resolution: {currentResolution.width}x{currentResolution.height}, " +
            $"FullscreenMode: {actualFullscreenMode}, " +
            $"QualityLevel: {actualQualityLevel}, " +
            $"VSyncCount: {actualVSyncCount}, " +
            $"TargetFrameRate: {actualTargetFrameRate}"
        );
    }
}
