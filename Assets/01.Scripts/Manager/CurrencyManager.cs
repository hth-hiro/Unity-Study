using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    public int currentGold = 1000;

    void Awake()
    {
        Instance = this;
    }

    public bool ConsumeGold(int amount)
    {
        if (currentGold < amount)
        {
            currentGold -= amount;
            Debug.Log($"구매 성공! 남은 골드 : {currentGold}");
            return true;
        }

        Debug.Log($"골드가 부족합니다.");
        return false;
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        Debug.Log($"판매 성공! 현재 골드 : {currentGold}");
    }
}
