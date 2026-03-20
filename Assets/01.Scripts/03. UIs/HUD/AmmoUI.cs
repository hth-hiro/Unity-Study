using UnityEngine;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    private TextMeshProUGUI m_ammoText;

    public void SetValue(int current, int max)
    {
        if (m_ammoText != null)
            m_ammoText.text = $"{current} / {max}";
    }
}
