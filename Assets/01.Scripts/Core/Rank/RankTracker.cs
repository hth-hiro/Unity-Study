using UnityEngine;

// 플레이 중 발생하는 기록을 누적하는 추적기 (계산, UI 등 일체 배제)
public class RankTracker : MonoBehaviour
{
    private RankRecord m_currentRecord;
    private float m_stageStartTime;
    private bool m_isTracking;

    private void Awake()
    {
        m_currentRecord = new RankRecord();
        m_isTracking = false;
    }

    public void InitializeRecord()
    {
        m_currentRecord.Reset();
        m_isTracking = false;
    }

    public void StartStage()
    {
        InitializeRecord();
        m_stageStartTime = Time.time;
        m_isTracking = true;
    }

    public void EndStage()
    {
        if (m_isTracking)
        {
            m_currentRecord.ClearTime = Time.time - m_stageStartTime;
            m_isTracking = false;
        }
    }

    public void AddHeadshot()
    {
        if (!m_isTracking) return;
        m_currentRecord.HeadshotKills++;
    }

    public void AddSkillKill()
    {
        if (!m_isTracking) return;
        m_currentRecord.SkillKills++;
    }

    public void AddAerialKill()
    {
        if (!m_isTracking) return;
        m_currentRecord.AerialKills++;
    }

    public void UpdateCombo(int currentCombo)
    {
        if (!m_isTracking) return;
        if (currentCombo > m_currentRecord.MaxCombo)
        {
            m_currentRecord.MaxCombo = currentCombo;
        }
    }

    public void AddHit()
    {
        if (!m_isTracking) return;
        m_currentRecord.HitCount++;
    }

    public void AddDeath()
    {
        if (!m_isTracking) return;
        m_currentRecord.DeathCount++;
    }

    public RankRecord GetCurrentRecord()
    {
        return m_currentRecord;
    }
}
