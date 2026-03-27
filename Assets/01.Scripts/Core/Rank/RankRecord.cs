public class RankRecord
{
    // 순수 데이터 컨테이너 역할, 기본값 초기화 제공
    public int HeadshotKills { get; set; }
    public int SkillKills { get; set; }
    public int AerialKills { get; set; }
    public int MaxCombo { get; set; }
    public int HitCount { get; set; }
    public int DeathCount { get; set; }
    public float ClearTime { get; set; }

    public void Reset()
    {
        HeadshotKills = 0;
        SkillKills = 0;
        AerialKills = 0;
        MaxCombo = 0;
        HitCount = 0;
        DeathCount = 0;
        ClearTime = 0f;
    }
}
