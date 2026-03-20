using UnityEngine;
using UnityEngine.LowLevelPhysics2D;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [SerializeField] private PlayerHUD m_playerHUD;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        RefreshHUD();
    }

    public void RefreshHUD()
    {
        m_playerHUD?.Refresh();
    }
}
