using UnityEngine;

public class RankCalculator
{
    private RankScoreSettings m_rankSettings;

    public RankCalculator()
    {
        EnsureRankSettings();
    }

    public RankCalculator(RankScoreSettings rankSettings)
    {
        m_rankSettings = rankSettings;
        EnsureRankSettings();
    }

    public RankScoreSettings RankSettings
    {
        get
        {
            EnsureRankSettings();
            return m_rankSettings;
        }
    }

    public void SetRankSettings(RankScoreSettings rankSettings)
    {
        m_rankSettings = rankSettings;
        EnsureRankSettings();
    }

    public RankResult Calculate(RankRecord record)
    {
        if (record == null)
        {
            return new RankResult(0, RankGrade.C, 0, null);
        }

        EnsureRankSettings();

        int score = 0;
        score += record.HeadshotKills * m_rankSettings.HeadshotScore;
        score += record.SkillKills * m_rankSettings.SkillKillScore;
        score += record.AerialKills * m_rankSettings.AerialKillScore;
        score += record.MaxCombo * m_rankSettings.ComboMultiplier;

        float timeDiff = m_rankSettings.TargetClearTime - record.ClearTime;
        if (timeDiff > 0)
        {
            score += Mathf.FloorToInt(timeDiff) * m_rankSettings.TimeBonusPerSecond;
        }

        score -= record.HitCount * m_rankSettings.HitPenalty;
        score -= record.DeathCount * m_rankSettings.DeathPenalty;
        score = Mathf.Max(0, score);

        RankGrade finalGrade = DetermineGrade(score);
        int rewardCurrency = CalculateReward(finalGrade);

        return new RankResult(score, finalGrade, rewardCurrency, record);
    }

    private RankGrade DetermineGrade(int totalScore)
    {
        if (totalScore >= m_rankSettings.SGradeThreshold)
        {
            return RankGrade.S;
        }

        if (totalScore >= m_rankSettings.AGradeThreshold)
        {
            return RankGrade.A;
        }

        if (totalScore >= m_rankSettings.BGradeThreshold)
        {
            return RankGrade.B;
        }

        return RankGrade.C;
    }

    private int CalculateReward(RankGrade grade)
    {
        switch (grade)
        {
            case RankGrade.S:
                return m_rankSettings.SReward;
            case RankGrade.A:
                return m_rankSettings.AReward;
            case RankGrade.B:
                return m_rankSettings.BReward;
            default:
                return m_rankSettings.CReward;
        }
    }

    private void EnsureRankSettings()
    {
        if (m_rankSettings == null)
        {
            m_rankSettings = ScriptableObject.CreateInstance<RankScoreSettings>();
        }
    }
}
