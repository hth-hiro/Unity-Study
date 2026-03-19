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
            Debug.Log($"구매 성공! 남은 골드 : {gold}");
            return true;
        }

        Debug.Log($"골드가 부족합니다.");
        return false;
    }

    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log($"판매 성공! 현재 골드 : {gold}");
    }
}
