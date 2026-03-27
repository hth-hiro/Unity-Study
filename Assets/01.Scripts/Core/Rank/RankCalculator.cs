using UnityEngine;

// 기록을 입력받아 최종 결과를 산출하는 순수 계산기 클래스
public class RankCalculator
{
    // 내부적으로 관리되며 나중에 기획 시 조정하기 쉬운 계산 기준들
    private int m_headshotScore = 150;
    private int m_skillKillScore = 100;
    private int m_aerialKillScore = 200;
    private int m_comboMultiplier = 10;
    
    private int m_hitPenalty = 50;
    private int m_deathPenalty = 500;
    
    private float m_targetClearTime = 120f; // 목표 클리어 타임 기준 (초)
    private int m_timeBonusPerSecond = 10;

    public RankResult Calculate(RankRecord record)
    {
        int score = 0;

        // 가점 요소 계산
        score += record.HeadshotKills * m_headshotScore;
        score += record.SkillKills * m_skillKillScore;
        score += record.AerialKills * m_aerialKillScore;
        score += record.MaxCombo * m_comboMultiplier;

        // 클리어 타임 보너스 연산 (빠를수록 가점 부여)
        float timeDiff = m_targetClearTime - record.ClearTime;
        if (timeDiff > 0)
        {
            score += Mathf.FloorToInt(timeDiff) * m_timeBonusPerSecond;
        }

        // 감점 (패널티) 요소 연산
        score -= record.HitCount * m_hitPenalty;
        score -= record.DeathCount * m_deathPenalty;
        
        // 점수가 음수로 떨어지는 것을 방지
        score = Mathf.Max(0, score);

        // 결과 산정
        RankGrade finalGrade = DetermineGrade(score);
        int rewardCurrency = CalculateReward(finalGrade);

        return new RankResult(score, finalGrade, rewardCurrency, record);
    }

    private RankGrade DetermineGrade(int totalScore)
    {
        if (totalScore >= 5000) return RankGrade.S;
        if (totalScore >= 3000) return RankGrade.A;
        if (totalScore >= 1000) return RankGrade.B;
        return RankGrade.C;
    }

    private int CalculateReward(RankGrade grade)
    {
        switch (grade)
        {
            case RankGrade.S: return 1000;
            case RankGrade.A: return 500;
            case RankGrade.B: return 200;
            case RankGrade.C: return 50;
            default: return 0;
        }
    }
}
