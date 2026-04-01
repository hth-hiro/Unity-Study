using System;
using UnityEngine;

[Serializable]
public class GameSettingsData
{
    [SerializeField] private GraphicsSettingsData m_graphics = new GraphicsSettingsData();
    [SerializeField] private AudioSettingsData m_audio = new AudioSettingsData();
    [SerializeField] private PlaySettingsData m_play = new PlaySettingsData();

    public GraphicsSettingsData Graphics
    {
        get
        {
            if (m_graphics == null)
            {
                m_graphics = new GraphicsSettingsData();
            }

            return m_graphics;
        }
        set
        {
            m_graphics = value ?? new GraphicsSettingsData();
        }
    }

    public AudioSettingsData Audio
    {
        get
        {
            if (m_audio == null)
            {
                m_audio = new AudioSettingsData();
            }

            return m_audio;
        }
        set
        {
            m_audio = value ?? new AudioSettingsData();
        }
    }

    public PlaySettingsData Play
    {
        get
        {
            if (m_play == null)
            {
                m_play = new PlaySettingsData();
            }

            return m_play;
        }
        set
        {
            m_play = value ?? new PlaySettingsData();
        }
    }
}
