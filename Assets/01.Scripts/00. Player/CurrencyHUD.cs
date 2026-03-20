using TMPro;
using UnityEngine;

public class CurrencyHUD : MonoBehaviour
{
    private TextMeshProUGUI m_goldText;

    void Start()
    {
        UpdateGoldDisplay();
    }

    private void OnEnable()
    {
        UpdateGoldDisplay();
    }

    public void UpdateGoldDisplay()
    {
        if (CurrencyManager.Instance != null && m_goldText != null) 
        {
            m_goldText.text = $"{CurrencyManager.Instance.Gold.ToString("N0")} G";
        }
    }
}
