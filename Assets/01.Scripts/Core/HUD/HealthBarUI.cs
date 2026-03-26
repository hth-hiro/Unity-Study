using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image m_fillImage;
    [SerializeField] private TextMeshProUGUI m_valueText;

    private Material m_runtimeMaterial;

    private void Awake()
    {
        if (m_fillImage != null)
        {
            m_runtimeMaterial = Instantiate(m_fillImage.material);
            m_fillImage.material = m_runtimeMaterial;
        }
    }

    private void OnDestroy()
    {
        if (m_runtimeMaterial != null)
        {
            Destroy(m_runtimeMaterial);
            m_runtimeMaterial = null;
        }
    }

    public void SetValue(float current, float max)
    {
        float fill = max <= 0 ? 0 : current / max;
        fill = Mathf.Clamp01(fill);

        if (m_runtimeMaterial != null)
        {
            m_runtimeMaterial.SetFloat("_FillAmount", fill);
        }

        if (m_valueText != null)
            m_valueText.text = $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";
    }

    public void Clear()
    {
        if (m_runtimeMaterial != null)
            m_runtimeMaterial.SetFloat("_FillAmount", 0f);

        if (m_valueText != null)
            m_valueText.text = string.Empty;
    }
}
