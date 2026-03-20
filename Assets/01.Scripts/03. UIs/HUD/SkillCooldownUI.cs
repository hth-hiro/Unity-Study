using UnityEngine;

public class SkillCooldownUI : MonoBehaviour
{
    private float m_remain;
    private float m_cooldown;

    public void SetCooldown(float remain, float cooldown)
    {
        m_remain = remain;
        m_cooldown = cooldown;
    }
}
