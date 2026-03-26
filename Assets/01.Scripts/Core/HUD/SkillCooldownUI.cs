using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillCooldownUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image m_cooldownFill;
    [SerializeField] private TextMeshProUGUI m_cooldownText;
    [SerializeField] private GameObject m_readyEffect;

    [Header("Options")]
    [SerializeField] private bool m_showText = true;

    public void SetCooldown(float remain, float max)
    {
        remain = Mathf.Max(0f, remain);
        float ratio = (max <= 0f) ? 0f : Mathf.Clamp01(remain / max);

        if (m_cooldownFill != null)
            m_cooldownFill.fillAmount = ratio;

        if (m_cooldownText != null && m_showText)
        {
            m_cooldownText.text = remain > 0f ? Mathf.CeilToInt(remain).ToString() : "";
        }

        SetReady(remain <= 0f);
    }

    public void SetReady(bool isReady)
    {
        if (m_readyEffect != null)
            m_readyEffect.SetActive(isReady);
    }

    public void Clear()
    {
        if (m_cooldownFill != null)
            m_cooldownFill.fillAmount = 0f;

        if (m_cooldownText != null)
            m_cooldownText.text = string.Empty;

        if (m_readyEffect != null)
            m_readyEffect.SetActive(false);
    }
}
