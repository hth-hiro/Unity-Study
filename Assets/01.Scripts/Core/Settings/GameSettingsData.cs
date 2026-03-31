using System;
using UnityEngine;

[Serializable]
public class GameSettingsData
{
    [SerializeField] private GraphicsSettingsData m_graphics = new GraphicsSettingsData();

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
}
