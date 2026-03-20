using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private HealthBarUI m_healthBarUI;
    [SerializeField] private AmmoUI m_ammoUI;
    [SerializeField] private SkillCooldownUI[] m_skillUIs;
    // 수집품
    // 보스 체력

    public void Refresh()
    {
        if (PlayerStatusManager.Instance != null)
        {
            m_healthBarUI?.SetValue(
                PlayerStatusManager.Instance.CurrentHP, 
                PlayerStatusManager.Instance.MaxHP
                );

            m_ammoUI?.SetValue(
                PlayerStatusManager.Instance.CurrentAmmo,
                PlayerStatusManager.Instance.MaxAmmo
                );
        }

        if (SkillManager.Instance != null && m_skillUIs.Length > 0)
        {
            m_skillUIs[0]?.SetCooldown(
                SkillManager.Instance.Skill1Remain,
                SkillManager.Instance.Skill1CoolDown
                );
        }
    }
}
