using UnityEngine;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    public List<SkillData> Skills = new List<SkillData>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        bool isAnySkillCooling = false;

        foreach (var skill in Skills)
        {
            skill.UpdateCooldown(Time.deltaTime);
            isAnySkillCooling = true;
        }

        if (isAnySkillCooling)
        {
            HUDManager.Instance?.RefreshHUD();
        }
    }

    public void UseSkill(int index)
    {
        if (index < 0 || index >= Skills.Count) return;

        SkillData skill = Skills[index];

        if (skill.IsReady)
        {
            skill.Remain = skill.CoolDown;
            Debug.Log($"{skill.Name} !");

            skill.Use(PlayerController.Instance.gameObject);
        }

        HUDManager.Instance?.RefreshHUD();
    }
}
