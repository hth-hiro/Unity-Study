using UnityEngine;

public class CrosshairSystem : MonoBehaviour, IPlaySettingsApplier
{
    [SerializeField] private CrosshairUI m_crosshairUI;
    [SerializeField] private bool m_applyOnStart = true;

    private void Start()
    {
        if (SettingsManager.Instance == null)
        {
            return;
        }

        SettingsManager.Instance.SetPlaySettingsApplier(this);

        if (!m_applyOnStart)
        {
            return;
        }

        ApplyCurrentSettings();
    }

    public void ApplyCurrentSettings()
    {
        if (m_crosshairUI == null || SettingsManager.Instance == null)
        {
            return;
        }

        PlaySettingsData play = SettingsManager.Instance.Play;
        if (play == null)
        {
            return;
        }

        ApplyPlayerSensitivity(play);

        m_crosshairUI.ApplySettings(play);
    }

    public void Apply(PlaySettingsData settings)
    {
        if (settings == null || m_crosshairUI == null)
        {
            return;
        }

        ApplyPlayerSensitivity(settings);

        m_crosshairUI?.ApplySettings(settings);
    }

    public void RefreshCrosshair()
    {
        ApplyCurrentSettings();
    }

    public void ClearCrosshair()
    {
        if (m_crosshairUI == null)
        {
            return;
        }

        m_crosshairUI.Clear();
    }

    private void ApplyPlayerSensitivity(PlaySettingsData settings)
    {
        if (settings == null)
        {
            return;
        }

        PlayerController.Instance?.SetMouseSensitivity(settings.MouseSensitivity);
    }
}
