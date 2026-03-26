using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI m_goldText;
    
    public void SetValue(int value)
    {
        if (m_goldText == null)
            return;

        m_goldText.text = string.Format(value.ToString("N0"));
    }

    public void Clear()
    {
        if (m_goldText != null)
            m_goldText.text = string.Empty;
    }
}
