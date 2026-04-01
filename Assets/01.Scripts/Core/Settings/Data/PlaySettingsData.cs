using System;
using UnityEngine;

/// <summary>
/// Stores gameplay-related setting values such as sensitivity and crosshair options.
/// </summary>
[Serializable]
public class PlaySettingsData
{
    public enum CrosshairTypeOption
    {
        Default,
        Dot,
        Cross
    }

    [SerializeField] private float m_mouseSensitivity = 1.0f;
    [SerializeField] private CrosshairTypeOption m_crosshairType = CrosshairTypeOption.Default;
    [SerializeField] private Color m_crosshairColor = Color.white;
    [SerializeField] private float m_crosshairSize = 16.0f;
    [SerializeField] private float m_crosshairLength = 8.0f;
    [SerializeField] private float m_crosshairOpacity = 1.0f;

    public float MouseSensitivity
    {
        get { return Mathf.Max(0.0f, m_mouseSensitivity); }
        set { m_mouseSensitivity = Mathf.Max(0.0f, value); }
    }

    public CrosshairTypeOption CrosshairType
    {
        get { return m_crosshairType; }
        set { m_crosshairType = value; }
    }

    public Color CrosshairColor
    {
        get { return m_crosshairColor; }
        set { m_crosshairColor = value; }
    }

    public float CrosshairSize
    {
        get { return Mathf.Max(0.0f, m_crosshairSize); }
        set { m_crosshairSize = Mathf.Max(0.0f, value); }
    }

    public float CrosshairLength
    {
        get { return Mathf.Max(0.0f, m_crosshairLength); }
        set { m_crosshairLength = Mathf.Max(0.0f, value); }
    }

    public float CrosshairOpacity
    {
        get { return Mathf.Clamp01(m_crosshairOpacity); }
        set { m_crosshairOpacity = Mathf.Clamp01(value); }
    }
}
