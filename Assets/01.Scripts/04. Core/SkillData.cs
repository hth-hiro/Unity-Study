using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[System.Serializable]
public class SkillData
{
    public string Name;
    public float CoolDown;
    public float Remain;
    public Sprite Icon;

    public bool IsReady => Remain <= 0;

    public void UpdateCooldown(float deltaTime)
    {
        if (Remain > 0f)
        {
            Remain -= deltaTime;
            if (Remain < 0f) Remain = 0f;
        }
    }

    public void Use()
    {
        Remain = CoolDown;

        // SkillLogic
    }
}
