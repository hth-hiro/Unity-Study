using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public class EnemyBase : MonoBehaviour
{
    protected EnemyHealth m_health;
    protected EnemyStateMachine m_stateMachine;

    public bool IsDead { get; protected set; }

    protected EnemyIdleState m_idleState;
    protected EnemyHitState m_hitState;
    protected EnemyDeadState m_deadState;

    public EnemyIdleState IdleState => m_idleState;
    public EnemyHitState HitState => m_hitState;
    public EnemyDeadState DeadState => m_deadState;

    protected virtual void Awake()
    {
        m_health = GetComponent<EnemyHealth>();
        m_stateMachine = new EnemyStateMachine();

        m_idleState = new EnemyIdleState(this, m_stateMachine);
        m_hitState = new EnemyHitState(this, m_stateMachine);
        m_deadState = new EnemyDeadState(this, m_stateMachine);

        m_health.OnHit += HandleHit;
    }

    protected virtual void Start()
    {
        m_stateMachine.Initialize(m_idleState);
    }

    protected virtual void Update()
    {
        m_stateMachine.Update();
    }

    public virtual void TakeDamage(float damage)
    {
        if (IsDead) return;
        m_health.TakeDamage(damage);
    }

    protected virtual void HandleHit()
    {
        if (IsDead) return;
        m_stateMachine.ChangeState(m_hitState);
    }

    protected virtual void HandleDeath()
    {
        if (IsDead) return;
        IsDead = false;
        m_stateMachine?.ChangeState(m_deadState);
    }
}
