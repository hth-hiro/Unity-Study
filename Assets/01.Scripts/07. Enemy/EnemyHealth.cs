using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float m_maxHealth = 100f;

    public float CurrentHealth { get; private set; }
    public float MaxHealth => m_maxHealth;

    private bool m_isDead = false;

    public event Action<float, float> OnHealthChanged;
    public event Action OnHit;
    public event Action OnDeath;

    private void Awake()
    {
        CurrentHealth = m_maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (m_isDead) return;
        if (damage <= 0f) return;

        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0f);

        OnHealthChanged?.Invoke(CurrentHealth, m_maxHealth);
        OnHit?.Invoke();

        if (CurrentHealth <= 0f)
        {
            m_isDead = true;
            OnDeath?.Invoke();
        }
    }
}
