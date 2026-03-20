using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[System.Serializable]
public abstract class SkillData : ScriptableObject
{
    public string Name;
    public float CoolDown;
    public Sprite Icon;

    [System.NonSerialized] public float Remain;

    public bool IsReady => Remain <= 0;

    public void UpdateCooldown(float deltaTime)
    {
        if (Remain > 0f)
        {
            Remain -= deltaTime;
            if (Remain < 0f) Remain = 0f;
        }
    }

    public abstract void Use(GameObject caster);
}
