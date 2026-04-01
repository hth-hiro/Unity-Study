using UnityEngine;

public class LobbyRewardPanel : MonoBehaviour
{
    [SerializeField] private LobbyMenuManager m_lobbyMenuManager;

    public void ResetToDefault()
    {
    }

    public void OnClickBack()
    {
        if (m_lobbyMenuManager != null)
        {
            m_lobbyMenuManager.ShowMainPanel();
        }
    }
}
