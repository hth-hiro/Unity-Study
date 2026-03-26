using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HealthBarUI m_healthBarUI;
    [SerializeField] private AmmoUI m_ammoUI;
    [SerializeField] private SkillCooldownUI[] m_skillUI;
    [SerializeField] private CurrencyUI m_currencyUI;
    // ¼öÁýÇ°

    public void SetHealth(float current, float max)
    {
        m_healthBarUI?.SetValue(current, max);
    }

    public void SetAmmo(int current, int max)
    {
        m_ammoUI?.SetValue(current, max);
    }

    public void SetSkillCooldown(int index, float remain, float cooldown)
    {
        if (m_skillUI == null)
            return;

        if (index < 0 || index >= m_skillUI.Length)
            return;

        m_skillUI[index]?.SetCooldown(remain, cooldown);
    }

    public void ClearAll()
    {
        m_healthBarUI?.Clear();
        m_ammoUI?.Clear();
        
        if (m_skillUI != null)
        {
            for (int i = 0; i < m_skillUI.Length; i++)
            {
                m_skillUI[i]?.Clear();
            }
        }

        m_currencyUI.Clear();
    }
}
