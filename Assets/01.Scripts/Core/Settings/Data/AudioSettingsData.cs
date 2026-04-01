using System;
using UnityEngine;

/// <summary>
/// Stores normalized audio setting values.
/// </summary>
[Serializable]
public class AudioSettingsData
{
    [SerializeField] private float m_masterVolume = 1.0f;
    [SerializeField] private float m_sfxVolume = 1.0f;
    [SerializeField] private float m_bgmVolume = 1.0f;
    [SerializeField] private float m_voiceVolume = 1.0f;
    [SerializeField] private float m_uiVolume = 1.0f;

    public float MasterVolume
    {
        get { return Mathf.Clamp01(m_masterVolume); }
        set { m_masterVolume = Mathf.Clamp01(value); }
    }

    public float SfxVolume
    {
        get { return Mathf.Clamp01(m_sfxVolume); }
        set { m_sfxVolume = Mathf.Clamp01(value); }
    }

    public float BgmVolume
    {
        get { return Mathf.Clamp01(m_bgmVolume); }
        set { m_bgmVolume = Mathf.Clamp01(value); }
    }

    public float VoiceVolume
    {
        get { return Mathf.Clamp01(m_voiceVolume); }
        set { m_voiceVolume = Mathf.Clamp01(value); }
    }

    public float UiVolume
    {
        get { return Mathf.Clamp01(m_uiVolume); }
        set { m_uiVolume = Mathf.Clamp01(value); }
    }
}
