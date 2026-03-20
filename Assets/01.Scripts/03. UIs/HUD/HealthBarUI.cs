using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider m_slider;
    [SerializeField] private TextMeshProUGUI m_hpText;

    public void SetValue(float current, float max)
    {
        if (m_slider != null)
            m_slider.value = max <= 0 ? 0 : current / max;

        if (m_hpText != null)
            m_hpText.text = $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";
    }
}
