using UnityEngine;

[CreateAssetMenu(menuName ="Skills/Dash")]
public class DashSkill : SkillData
{
    public float DashForce = 50f;

    public override void Use(GameObject caster)
    {
        Rigidbody rb = caster.GetComponent<Rigidbody>();
        rb?.AddForce(caster.transform.forward * DashForce, ForceMode.Impulse);
    }
}
