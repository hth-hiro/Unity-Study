using UnityEngine;

public class LobbyStageSelectPanel : MonoBehaviour
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
