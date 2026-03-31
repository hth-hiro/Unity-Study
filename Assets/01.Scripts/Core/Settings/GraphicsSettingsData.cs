using System;
using UnityEngine;

/// <summary>
/// Stores graphics setting values that can be shared by UI, save/load, and apply systems.
/// </summary>
[Serializable]
public class GraphicsSettingsData
{
    [SerializeField] private int m_resolutionWidth = 1920;
    [SerializeField] private int m_resolutionHeight = 1080;
    [SerializeField] private FullScreenMode m_fullscreenMode = FullScreenMode.FullScreenWindow;
    [SerializeField] private int m_qualityLevel = 2;
    [SerializeField] private bool m_vSyncEnabled = true;
    [SerializeField] private int m_targetFrameRate = -1;

    public int ResolutionWidth
    {
        get { return Mathf.Max(1, m_resolutionWidth); }
        set { m_resolutionWidth = Mathf.Max(1, value); }
    }

    public int ResolutionHeight
    {
        get { return Mathf.Max(1, m_resolutionHeight); }
        set { m_resolutionHeight = Mathf.Max(1, value); }
    }

    public FullScreenMode FullscreenMode
    {
        get { return m_fullscreenMode; }
        set { m_fullscreenMode = value; }
    }

    public int QualityLevel
    {
        get { return Mathf.Max(0, m_qualityLevel); }
        set { m_qualityLevel = Mathf.Max(0, value); }
    }

    public bool VSyncEnabled
    {
        get { return m_vSyncEnabled; }
        set { m_vSyncEnabled = value; }
    }

    public int TargetFrameRate
    {
        get { return m_targetFrameRate < 0 ? -1 : m_targetFrameRate; }
        set { m_targetFrameRate = value < 0 ? -1 : value; }
    }
}
