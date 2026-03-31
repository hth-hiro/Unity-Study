using UnityEngine;
using TMPro;

public class PauseSettingsPanel : MonoBehaviour
{
    [SerializeField] private GameObject m_graphicsPanel;
    [SerializeField] private GameObject m_audioPanel;
    [SerializeField] private GameObject m_gameplayPanel;
    [SerializeField] private GameObject m_controlPanel;
    [SerializeField] private PauseMenuManager m_pauseMenuManager;
    [SerializeField] private TextMeshProUGUI m_graphicsLabel;
    [SerializeField] private TextMeshProUGUI m_audioLabel;
    [SerializeField] private TextMeshProUGUI m_gameplayLabel;
    [SerializeField] private TextMeshProUGUI m_controlLabel;
    [SerializeField] private Color m_selectedColor = Color.white;
    [SerializeField] private Color m_unselectedColor = Color.gray;

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
        UpdateLabelState(m_graphicsLabel);
    }

    public void ShowAudio()
    {
        SetPanelState(m_graphicsPanel, false);
        SetPanelState(m_audioPanel, true);
        SetPanelState(m_gameplayPanel, false);
        SetPanelState(m_controlPanel, false);
        UpdateLabelState(m_audioLabel);
    }

    public void ShowGameplay()
    {
        SetPanelState(m_graphicsPanel, false);
        SetPanelState(m_audioPanel, false);
        SetPanelState(m_gameplayPanel, true);
        SetPanelState(m_controlPanel, false);
        UpdateLabelState(m_gameplayLabel);
    }

    public void ShowControl()
    {
        SetPanelState(m_graphicsPanel, false);
        SetPanelState(m_audioPanel, false);
        SetPanelState(m_gameplayPanel, false);
        SetPanelState(m_controlPanel, true);
        UpdateLabelState(m_controlLabel);
    }

    public void OnGraphicsChanged(bool isOn)
    {
        if (!isOn)
        {
            return;
        }

        ShowGraphics();
    }

    public void OnAudioChanged(bool isOn)
    {
        if (!isOn)
        {
            return;
        }

        ShowAudio();
    }

    public void OnGameplayChanged(bool isOn)
    {
        if (!isOn)
        {
            return;
        }

        ShowGameplay();
    }

    public void OnControlChanged(bool isOn)
    {
        if (!isOn)
        {
            return;
        }

        ShowControl();
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
        SetLabelColor(m_graphicsLabel, m_unselectedColor);
        SetLabelColor(m_audioLabel, m_unselectedColor);
        SetLabelColor(m_gameplayLabel, m_unselectedColor);
        SetLabelColor(m_controlLabel, m_unselectedColor);
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
