using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [SerializeField] private int gold = 1000;

    public int Gold => gold;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool ConsumeGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            return true;
        }

        return false;
    }

    public void AddGold(int amount)
    {
        gold += amount;
    }
}
