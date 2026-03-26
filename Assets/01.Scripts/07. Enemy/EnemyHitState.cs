using UnityEngine;

public class EnemyHitState : EnemyState
{
    private float m_hitDuration = 0.2f;
    private float m_timer;

    public EnemyHitState(EnemyBase enemy, EnemyStateMachine stateMachine)
        : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        m_timer = m_hitDuration;
        Debug.Log($"{m_enemy.name} -> Hit");
    }

    public override void Update()
    {
        m_timer -= Time.deltaTime;

        if (m_timer <= 0f)
        {
            m_stateMachine.ChangeState(m_enemy.IdleState);
        }
    }
}
