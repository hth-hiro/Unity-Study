using UnityEngine;

public class LobbyShopPanel : MonoBehaviour
{
    [SerializeField] private GameObject m_buyPanel;
    [SerializeField] private GameObject m_sellPanel;
    [SerializeField] private LobbyMenuManager m_lobbyMenuManager;

    private void OnEnable()
    {
        ResetToDefault();
    }

    public void ResetToDefault()
    {
        ShowBuy();
    }

    public void ShowBuy()
    {
        SetPanelState(m_buyPanel, true);
        SetPanelState(m_sellPanel, false);
    }

    public void ShowSell()
    {
        SetPanelState(m_buyPanel, false);
        SetPanelState(m_sellPanel, true);
    }

    public void OnBuyChanged(bool isOn)
    {
        if (!isOn)
        {
            return;
        }

        ShowBuy();
    }

    public void OnSellChanged(bool isOn)
    {
        if (!isOn)
        {
            return;
        }

        ShowSell();
    }

    public void OnClickBack()
    {
        if (m_lobbyMenuManager != null)
        {
            m_lobbyMenuManager.ShowMainPanel();
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
