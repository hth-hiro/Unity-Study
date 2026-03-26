using UnityEngine;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI m_ammoText;

    public void SetValue(int current, int max)
    {
        if (m_ammoText == null)
            return;

        current = Mathf.Max(0, current);
        max = Mathf.Max(0, max);

        m_ammoText.text = ($"{current} / {max}");
    }

    public void Clear()
    {
        if (m_ammoText != null)
        {
            m_ammoText.text = string.Empty;
        }
    }
}
