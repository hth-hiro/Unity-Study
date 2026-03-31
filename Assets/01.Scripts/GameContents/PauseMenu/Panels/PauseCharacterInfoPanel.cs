using UnityEngine;
using TMPro;

public class PauseCharacterInfoPanel : MonoBehaviour
{
    [SerializeField] private GameObject m_statsPanel;
    [SerializeField] private GameObject m_attackPanel;
    [SerializeField] private GameObject m_skillPanel;
    [SerializeField] private PauseMenuManager m_pauseMenuManager;
    [SerializeField] private TextMeshProUGUI m_statsLabel;
    [SerializeField] private TextMeshProUGUI m_attackLabel;
    [SerializeField] private TextMeshProUGUI m_skillLabel;
    [SerializeField] private Color m_selectedColor = Color.white;
    [SerializeField] private Color m_unselectedColor = Color.gray;

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
        UpdateLabelState(m_statsLabel);
    }

    public void ShowAttack()
    {
        SetPanelState(m_statsPanel, false);
        SetPanelState(m_attackPanel, true);
        SetPanelState(m_skillPanel, false);
        UpdateLabelState(m_attackLabel);
    }

    public void ShowSkill()
    {
        SetPanelState(m_statsPanel, false);
        SetPanelState(m_attackPanel, false);
        SetPanelState(m_skillPanel, true);
        UpdateLabelState(m_skillLabel);
    }

    public void OnStatsChanged(bool isOn)
    {
        if (!isOn)
        {
            return;
        }

        ShowStats();
    }

    public void OnAttackChanged(bool isOn)
    {
        if (!isOn)
        {
            return;
        }

        ShowAttack();
    }

    public void OnSkillChanged(bool isOn)
    {
        if (!isOn)
        {
            return;
        }

        ShowSkill();
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

    private void UpdateLabelState(TextMeshProUGUI selectedLabel)
    {
        SetLabelColor(m_statsLabel, m_unselectedColor);
        SetLabelColor(m_attackLabel, m_unselectedColor);
        SetLabelColor(m_skillLabel, m_unselectedColor);
        SetLabelColor(selectedLabel, m_selectedColor);
    }

    private void SetLabelColor(TextMeshProUGUI label, Color color)
    {
        if (label == null)
        {
            return;
        }

        label.color = color;
    }
}
