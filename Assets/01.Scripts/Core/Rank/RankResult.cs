public enum RankGrade
{
    S,
    A,
    B,
    C
}

public class RankResult
{
    // RankCalculator가 계산한 최종 결과 데이터를 담는 컨테이너
    public int TotalScore { get; private set; }
    public RankGrade Grade { get; private set; }
    public int RewardCurrency { get; private set; }
    public RankRecord OriginalRecord { get; private set; }

    public RankResult(int totalScore, RankGrade grade, int rewardCurrency, RankRecord originalRecord)
    {
        TotalScore = totalScore;
        Grade = grade;
        RewardCurrency = rewardCurrency;
        OriginalRecord = originalRecord;
    }
}
