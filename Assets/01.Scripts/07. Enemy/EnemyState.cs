using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    protected EnemyBase m_enemy;
    protected EnemyStateMachine m_stateMachine;

    protected EnemyState(EnemyBase enemy, EnemyStateMachine stateMachine)
    {
        this.m_enemy = enemy;
        this.m_stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Update() { } 
    public virtual void Exit() { }
}
