using UnityEngine;

public class LobbyMainPanel : MonoBehaviour
{
    [SerializeField] private LobbyMenuManager m_lobbyMenuManager;

    public void OnClickReward()
    {
        if (m_lobbyMenuManager != null)
        {
            m_lobbyMenuManager.OnClickReward();
        }
    }

    public void OnClickShop()
    {
        if (m_lobbyMenuManager != null)
        {
            m_lobbyMenuManager.OnClickShop();
        }
    }

    public void OnClickStageSelect()
    {
        if (m_lobbyMenuManager != null)
        {
            m_lobbyMenuManager.OnClickStageSelect();
        }
    }

    public void OnClickClose()
    {
        if (m_lobbyMenuManager != null)
        {
            m_lobbyMenuManager.CloseLobbyMenu();
        }
    }
}
