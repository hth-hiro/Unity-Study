using UnityEngine;
using UnityEngine.InputSystem;

public class TestScript : MonoBehaviour
{
    [SerializeField] private HealthBarUI m_hpBar;

    [SerializeField] private float m_currentHP = 100f;
    [SerializeField] private float m_maxHP = 100f;

    private void Start()
    {
        m_hpBar.SetValue(m_currentHP, m_maxHP);
    }

    private void Update()
    {
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            m_currentHP -= 10f;
            m_currentHP = Mathf.Clamp(m_currentHP, 0, m_maxHP);

            m_hpBar.SetValue(m_currentHP, m_maxHP);
        }

        // J 키 누르면 체력 회복 (테스트용)
        if (Keyboard.current.jKey.wasPressedThisFrame)
        {
            m_currentHP += 10f;
            m_currentHP = Mathf.Clamp(m_currentHP, 0, m_maxHP);

            m_hpBar.SetValue(m_currentHP, m_maxHP);
        }
    }
}
