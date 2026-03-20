using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillCooldownUI : MonoBehaviour
{
    [SerializeField] private Image m_cooldownFill;
    [SerializeField] private TextMeshProUGUI m_cooldownText;
    [SerializeField] private GameObject m_readyEffect;

    public void SetCooldown(float remain, float max)
    {
        float ratio = (max <= 0f) ? 0f : Mathf.Clamp01(remain / max);

        if (m_cooldownFill != null)
            m_cooldownFill.fillAmount = ratio;

        if (m_cooldownText != null)
        {
            m_cooldownText.text = remain > 0f ? Mathf.CeilToInt(remain).ToString() : "";
        }

        if (m_readyEffect != null)
        {
            m_readyEffect.SetActive(remain <= 0f);
        }
    }
}
