using UnityEngine;

public class EnemyDeadState : EnemyState
{
    public EnemyDeadState(EnemyBase enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log($"{m_enemy.name} -> Dead");
        m_enemy.gameObject.SetActive(false);
    }
}
