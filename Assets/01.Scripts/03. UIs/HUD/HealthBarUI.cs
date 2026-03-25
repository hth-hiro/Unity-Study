using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image m_fillImage;
    [SerializeField] private TextMeshProUGUI m_hpText;

    private Material m_runtimeMaterial;

    private void Awake()
    {
        if (m_fillImage != null)
        {
            m_runtimeMaterial = Instantiate(m_fillImage.material);
            m_fillImage.material = m_runtimeMaterial;
        }
    }

    public void SetValue(float current, float max)
    {
        float fill = max <= 0 ? 0 : current / max;

        Debug.Log($"Fill = {fill}");

        if (m_runtimeMaterial != null)
        {
            m_runtimeMaterial.SetFloat("_FillAmount", fill);
        }

        if (m_hpText != null)
            m_hpText.text = $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";
    }
}
