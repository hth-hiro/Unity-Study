using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private HealthBarUI m_healthBarUI;
    [SerializeField] private AmmoUI m_ammoUI;
    [SerializeField] private SkillCooldownUI[] m_skillUI;
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

        if (SkillManager.Instance != null && m_skillUI.Length > 0)
        {
            for (int i = 0; i < m_skillUI.Length; i++) 
            {
                if (i < SkillManager.Instance.Skills.Count)
                {
                    var skillData = SkillManager.Instance.Skills[i];

                    m_skillUI[i].SetCooldown(skillData.Remain, skillData.CoolDown);
                }
                else
                {
                    m_skillUI[i].SetCooldown(0, 1);
                }
            }
        }
    }
}
