using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject m_pauseMenuRoot;
    [SerializeField] private GameObject m_mainPanel;
    [SerializeField] private PauseSettingsPanel m_settingsPanel;
    [SerializeField] private PauseCharacterInfoPanel m_characterInfoPanel;

    private bool m_isOpen;

    private void Start()
    {
        if (m_pauseMenuRoot != null)
        {
            m_pauseMenuRoot.SetActive(false);
        }

        if (m_mainPanel != null)
        {
            m_mainPanel.SetActive(false);
        }

        if (m_settingsPanel != null)
        {
            m_settingsPanel.gameObject.SetActive(false);
        }

        if (m_characterInfoPanel != null)
        {
            m_characterInfoPanel.gameObject.SetActive(false);
        }

        m_isOpen = false;
    }

    private void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        if (!m_isOpen)
        {
            OpenPauseMenu();
        }
        else
        {
            if (IsMainPanelActive())
            {
                ClosePauseMenu();
            }
            else
            {
                ShowMainPanel();
            }
        }
    }

    public void OpenPauseMenu()
    {
        if (m_pauseMenuRoot == null)
        {
            return;
        }

        m_pauseMenuRoot.SetActive(true);
        ShowMainPanel();

        Time.timeScale = 0f;

        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.SetInputBlock(true);
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        m_isOpen = true;
    }

    public void ClosePauseMenu()
    {
        if (m_pauseMenuRoot != null)
        {
            m_pauseMenuRoot.SetActive(false);
        }

        Time.timeScale = 1f;

        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.SetInputBlock(false);
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        m_isOpen = false;
    }

    public void ShowMainPanel()
    {
        if (m_mainPanel != null)
        {
            m_mainPanel.SetActive(true);
        }

        if (m_settingsPanel != null)
        {
            m_settingsPanel.gameObject.SetActive(false);
        }

        if (m_characterInfoPanel != null)
        {
            m_characterInfoPanel.gameObject.SetActive(false);
        }
    }

    public void ShowSettingsPanel()
    {
        if (m_mainPanel != null)
        {
            m_mainPanel.SetActive(false);
        }

        if (m_settingsPanel != null)
        {
            m_settingsPanel.gameObject.SetActive(true);
            m_settingsPanel.ResetToDefault();
        }

        if (m_characterInfoPanel != null)
        {
            m_characterInfoPanel.gameObject.SetActive(false);
        }
    }

    public void ShowCharacterInfoPanel()
    {
        if (m_mainPanel != null)
        {
            m_mainPanel.SetActive(false);
        }

        if (m_settingsPanel != null)
        {
            m_settingsPanel.gameObject.SetActive(false);
        }

        if (m_characterInfoPanel != null)
        {
            m_characterInfoPanel.gameObject.SetActive(true);
            m_characterInfoPanel.ResetToDefault();
        }
    }

    public void ResumeGame()
    {
        ClosePauseMenu();
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnClickSettings()
    {
        ShowSettingsPanel();
    }

    public void OnClickCharacterInfo()
    {
        ShowCharacterInfoPanel();
    }

    private bool IsMainPanelActive()
    {
        return m_mainPanel != null && m_mainPanel.activeSelf;
    }
}
