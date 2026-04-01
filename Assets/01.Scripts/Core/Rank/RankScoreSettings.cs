using UnityEngine;

[CreateAssetMenu(fileName = "RankScoreSettings", menuName = "Game/Rank/Rank Score Settings")]
public class RankScoreSettings : ScriptableObject
{
    [Header("Score Bonus")]
    [SerializeField] private int m_headshotScore = 150;
    [SerializeField] private int m_skillKillScore = 100;
    [SerializeField] private int m_aerialKillScore = 200;
    [SerializeField] private int m_comboMultiplier = 10;

    [Header("Penalty")]
    [SerializeField] private int m_hitPenalty = 50;
    [SerializeField] private int m_deathPenalty = 500;

    [Header("Time")]
    [SerializeField] private float m_targetClearTime = 120f;
    [SerializeField] private int m_timeBonusPerSecond = 10;

    [Header("Grade Threshold")]
    [SerializeField] private int m_sGradeThreshold = 5000;
    [SerializeField] private int m_aGradeThreshold = 3000;
    [SerializeField] private int m_bGradeThreshold = 1000;

    [Header("Reward")]
    [SerializeField] private int m_sReward = 1000;
    [SerializeField] private int m_aReward = 500;
    [SerializeField] private int m_bReward = 200;
    [SerializeField] private int m_cReward = 50;

    public int HeadshotScore
    {
        get { return m_headshotScore; }
    }

    public int SkillKillScore
    {
        get { return m_skillKillScore; }
    }

    public int AerialKillScore
    {
        get { return m_aerialKillScore; }
    }

    public int ComboMultiplier
    {
        get { return m_comboMultiplier; }
    }

    public int HitPenalty
    {
        get { return m_hitPenalty; }
    }

    public int DeathPenalty
    {
        get { return m_deathPenalty; }
    }

    public float TargetClearTime
    {
        get { return m_targetClearTime; }
    }

    public int TimeBonusPerSecond
    {
        get { return m_timeBonusPerSecond; }
    }

    public int SGradeThreshold
    {
        get { return m_sGradeThreshold; }
    }

    public int AGradeThreshold
    {
        get { return m_aGradeThreshold; }
    }

    public int BGradeThreshold
    {
        get { return m_bGradeThreshold; }
    }

    public int SReward
    {
        get { return m_sReward; }
    }

    public int AReward
    {
        get { return m_aReward; }
    }

    public int BReward
    {
        get { return m_bReward; }
    }

    public int CReward
    {
        get { return m_cReward; }
    }
}
