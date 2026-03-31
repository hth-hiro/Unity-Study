using UnityEngine;

public class PauseCharacterInfoPanel : MonoBehaviour
{
    [SerializeField] private GameObject m_statsPanel;
    [SerializeField] private GameObject m_attackPanel;
    [SerializeField] private GameObject m_skillPanel;
    [SerializeField] private PauseMenuManager m_pauseMenuManager;

    private void OnEnable()
    {
        ResetToDefault();
    }

    public void ResetToDefault()
    {
        ShowStats();
    }

    public void ShowStats()
    {
        SetPanelState(m_statsPanel, true);
        SetPanelState(m_attackPanel, false);
        SetPanelState(m_skillPanel, false);
    }

    public void ShowAttack()
    {
        SetPanelState(m_statsPanel, false);
        SetPanelState(m_attackPanel, true);
        SetPanelState(m_skillPanel, false);
    }

    public void ShowSkill()
    {
        SetPanelState(m_statsPanel, false);
        SetPanelState(m_attackPanel, false);
        SetPanelState(m_skillPanel, true);
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
