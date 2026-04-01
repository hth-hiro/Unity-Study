using UnityEngine;

public class LobbyMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject m_lobbyMenuRoot;
    [SerializeField] private GameObject m_mainPanel;
    [SerializeField] private LobbyRewardPanel m_rewardPanel;
    [SerializeField] private LobbyShopPanel m_shopPanel;
    [SerializeField] private LobbyStageSelectPanel m_stageSelectPanel;

    private bool m_isOpen;

    private void Start()
    {
        if (m_lobbyMenuRoot != null)
        {
            m_lobbyMenuRoot.SetActive(false);
        }

        if (m_mainPanel != null)
        {
            m_mainPanel.SetActive(false);
        }

        if (m_rewardPanel != null)
        {
            m_rewardPanel.gameObject.SetActive(false);
        }

        if (m_shopPanel != null)
        {
            m_shopPanel.gameObject.SetActive(false);
        }

        if (m_stageSelectPanel != null)
        {
            m_stageSelectPanel.gameObject.SetActive(false);
        }

        m_isOpen = false;
    }

    public void ToggleLobbyMenu()
    {
        if (!m_isOpen)
        {
            OpenLobbyMenu();
        }
        else
        {
            if (IsMainPanelActive())
            {
                CloseLobbyMenu();
            }
            else
            {
                ShowMainPanel();
            }
        }
    }

    public void OpenLobbyMenu()
    {
        if (m_lobbyMenuRoot == null)
        {
            return;
        }

        

        m_lobbyMenuRoot.SetActive(true);
        ShowMainPanel();

        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.SetInputBlock(true);
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        m_isOpen = true;
    }

    public void CloseLobbyMenu()
    {
        if (m_lobbyMenuRoot != null)
        {
            m_lobbyMenuRoot.SetActive(false);
        }

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

        if (m_rewardPanel != null)
        {
            m_rewardPanel.gameObject.SetActive(false);
        }

        if (m_shopPanel != null)
        {
            m_shopPanel.gameObject.SetActive(false);
        }

        if (m_stageSelectPanel != null)
        {
            m_stageSelectPanel.gameObject.SetActive(false);
        }
    }

    public void ShowRewardPanel()
    {
        if (m_mainPanel != null)
        {
            m_mainPanel.SetActive(false);
        }

        if (m_rewardPanel != null)
        {
            m_rewardPanel.gameObject.SetActive(true);
            m_rewardPanel.ResetToDefault();
        }

        if (m_shopPanel != null)
        {
            m_shopPanel.gameObject.SetActive(false);
        }

        if (m_stageSelectPanel != null)
        {
            m_stageSelectPanel.gameObject.SetActive(false);
        }
    }

    public void ShowShopPanel()
    {
        if (m_mainPanel != null)
        {
            m_mainPanel.SetActive(false);
        }

        if (m_rewardPanel != null)
        {
            m_rewardPanel.gameObject.SetActive(false);
        }

        if (m_shopPanel != null)
        {
            m_shopPanel.gameObject.SetActive(true);
            m_shopPanel.ResetToDefault();
        }

        if (m_stageSelectPanel != null)
        {
            m_stageSelectPanel.gameObject.SetActive(false);
        }
    }

    public void ShowStageSelectPanel()
    {
        if (m_mainPanel != null)
        {
            m_mainPanel.SetActive(false);
        }

        if (m_rewardPanel != null)
        {
            m_rewardPanel.gameObject.SetActive(false);
        }

        if (m_shopPanel != null)
        {
            m_shopPanel.gameObject.SetActive(false);
        }

        if (m_stageSelectPanel != null)
        {
            m_stageSelectPanel.gameObject.SetActive(true);
            m_stageSelectPanel.ResetToDefault();
        }
    }

    public void OnClickReward()
    {
        ShowRewardPanel();
    }

    public void OnClickShop()
    {
        ShowShopPanel();
    }

    public void OnClickStageSelect()
    {
        ShowStageSelectPanel();
    }

    private bool IsMainPanelActive()
    {
        return m_mainPanel != null && m_mainPanel.activeSelf;
    }
}
