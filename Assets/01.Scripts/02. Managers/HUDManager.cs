using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [SerializeField] private PlayerHUD m_playerHUD;

    void Awake()
    {
        
    }

    public void RefreshHUD()
    {

    }
}
