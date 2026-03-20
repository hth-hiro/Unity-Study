using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    public float Skill1CoolDown = 5f;
    public float Skill1Remain { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (Skill1Remain > 0f)
        {
            Skill1Remain -= Time.deltaTime;
            if (Skill1Remain < 0f) Skill1Remain = 0f;

            HUDManager.Instance?.RefreshHUD();
        }
    }

    public void UseSkill1()
    {
        if (Skill1Remain > 0f) return;
        Skill1Remain = Skill1CoolDown;
        HUDManager.Instance?.RefreshHUD();
    }
}
