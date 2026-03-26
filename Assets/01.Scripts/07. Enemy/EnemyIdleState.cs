using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(EnemyBase enemy, EnemyStateMachine stateMachine)
        : base(enemy, stateMachine)
    { }

    public override void Enter()
    {
        Debug.Log($"{m_enemy.name} -> Idle");
    }
}
