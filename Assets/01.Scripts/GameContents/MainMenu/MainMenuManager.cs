using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject m_creditPanel;
    [SerializeField] private string m_lobbySceneName = "Lobby";
    [SerializeField] private bool m_hideCreditPanelOnStart = true;

    private void Start()
    {
        if (!m_hideCreditPanelOnStart)
        {
            return;
        }

        if (m_creditPanel == null)
        {
            return;
        }

        m_creditPanel.SetActive(false);
    }

    public void OpenCreditPanel()
    {
        if (m_creditPanel == null)
        {
            return;
        }

        m_creditPanel.SetActive(true);
    }

    public void CloseCreditPanel()
    {
        if (m_creditPanel == null)
        {
            return;
        }

        m_creditPanel.SetActive(false);
    }

    public void StartGame()
    {
        if (string.IsNullOrWhiteSpace(m_lobbySceneName))
        {
            return;
        }

        SceneManager.LoadScene(m_lobbySceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
