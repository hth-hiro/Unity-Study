using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject m_shopUI;
    private bool m_isShopOpen;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (m_shopUI != null)
        {
            m_shopUI.SetActive(false);
        }
    }

    public void ToggleShop()
    {
        m_isShopOpen = !m_isShopOpen;

        if (m_shopUI != null)
        {
            m_shopUI.SetActive(m_isShopOpen);
        }

        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.SetInputBlock(m_isShopOpen);
        }
    }
}
