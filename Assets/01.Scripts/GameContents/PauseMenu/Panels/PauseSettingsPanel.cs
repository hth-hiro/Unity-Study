using UnityEngine;

public class PauseSettingsPanel : MonoBehaviour
{
    [SerializeField] private GameObject m_graphicsPanel;
    [SerializeField] private GameObject m_audioPanel;
    [SerializeField] private GameObject m_gameplayPanel;
    [SerializeField] private GameObject m_controlPanel;
    [SerializeField] private PauseMenuManager m_pauseMenuManager;

    private void OnEnable()
    {
        ResetToDefault();
    }

    public void ResetToDefault()
    {
        ShowGraphics();
    }

    public void ShowGraphics()
    {
        SetPanelState(m_graphicsPanel, true);
        SetPanelState(m_audioPanel, false);
        SetPanelState(m_gameplayPanel, false);
        SetPanelState(m_controlPanel, false);
    }

    public void ShowAudio()
    {
        SetPanelState(m_graphicsPanel, false);
        SetPanelState(m_audioPanel, true);
        SetPanelState(m_gameplayPanel, false);
        SetPanelState(m_controlPanel, false);
    }

    public void ShowGameplay()
    {
        SetPanelState(m_graphicsPanel, false);
        SetPanelState(m_audioPanel, false);
        SetPanelState(m_gameplayPanel, true);
        SetPanelState(m_controlPanel, false);
    }

    public void ShowControl()
    {
        SetPanelState(m_graphicsPanel, false);
        SetPanelState(m_audioPanel, false);
        SetPanelState(m_gameplayPanel, false);
        SetPanelState(m_controlPanel, true);
    }

    public void OnClickBack()
    {
        if (m_pauseMenuManager != null)
        {
            m_pauseMenuManager.ShowMainPanel();
        }
    }

    private void SetPanelState(GameObject panelObject, bool isActive)
    {
        if (panelObject != null)
        {
            panelObject.SetActive(isActive);
        }
    }
}
