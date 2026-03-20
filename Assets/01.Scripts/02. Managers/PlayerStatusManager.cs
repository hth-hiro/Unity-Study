using UnityEngine;

public class PlayerStatusManager : MonoBehaviour
{
    public static PlayerStatusManager Instance { get; private set; }

    public float CurrentHP { get; private set; } = 100f;
    public float MaxHP { get; private set; } = 100f;

    public int CurrentAmmo { get; private set; } = 30;
    public int MaxAmmo { get; private set; } = 30;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetHP(float value)
    {
        CurrentHP = Mathf.Clamp(value, 0, MaxHP);
        HUDManager.Instance?.RefreshHUD();
    }

    public void SetAmmo(int value)
    {
        CurrentAmmo = Mathf.Clamp(value, 0, MaxAmmo);
        HUDManager.Instance?.RefreshHUD();
    }
}
