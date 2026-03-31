using UnityEngine;

public class PauseMainPanel : MonoBehaviour
{
    [SerializeField] private PauseMenuManager m_pauseMenuManager;

    public void OnClickResume()
    {
        if (m_pauseMenuManager != null)
        {
            m_pauseMenuManager.ResumeGame();
        }
    }

    public void OnClickSettings()
    {
        if (m_pauseMenuManager != null)
        {
            m_pauseMenuManager.OnClickSettings();
        }
    }

    public void OnClickCharacterInfo()
    {
        if (m_pauseMenuManager != null)
        {
            m_pauseMenuManager.OnClickCharacterInfo();
        }
    }

    public void OnClickQuit()
    {
        if (m_pauseMenuManager != null)
        {
            m_pauseMenuManager.QuitGame();
        }
    }
}
