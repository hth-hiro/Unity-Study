using UnityEngine;

public class StageRankResultController : MonoBehaviour
{
    [SerializeField] private RankScoreSettings m_rankSettings;
    [SerializeField] private RankTracker m_rankTracker;
    [SerializeField] private StageRankResultPanelUI m_rankResultPanel;
    [SerializeField] private bool m_showOnStartForTest;
    [SerializeField] private bool m_useConditionDisplay = true;

    private RankCalculator m_rankCalculator;

    private void Awake()
    {
        m_rankCalculator = new RankCalculator(m_rankSettings);
    }

    private void Start()
    {
        if (!m_showOnStartForTest)
        {
            return;
        }

        if (m_useConditionDisplay)
        {
            ShowCurrentResultWithConditions();
        }
        else
        {
            ShowCurrentResult();
        }
    }

    public void ShowCurrentResult()
    {
        if (m_rankTracker == null || m_rankResultPanel == null || m_rankCalculator == null)
        {
            return;
        }

        RankRecord record = m_rankTracker.GetCurrentRecord();
        if (record == null)
        {
            return;
        }

        RankResult result = m_rankCalculator.Calculate(record);
        if (result == null)
        {
            return;
        }

        if (m_useConditionDisplay)
        {
            string[] conditionTitles = BuildConditionTitles(record);
            bool[] conditionResults = BuildConditionResults(record);
            m_rankResultPanel.Show(result, conditionTitles, conditionResults);
            return;
        }

        m_rankResultPanel.Show(result);
    }

    public void ShowResult(RankResult result)
    {
        if (result == null || m_rankResultPanel == null)
        {
            return;
        }

        if (m_useConditionDisplay)
        {
            RankRecord record = result.OriginalRecord;
            string[] conditionTitles = BuildConditionTitles(record);
            bool[] conditionResults = BuildConditionResults(record);
            m_rankResultPanel.Show(result, conditionTitles, conditionResults);
            return;
        }

        m_rankResultPanel.Show(result);
    }

    public void HideResult()
    {
        if (m_rankResultPanel == null)
        {
            return;
        }

        m_rankResultPanel.Hide();
    }

    public string[] BuildConditionTitles(RankRecord record)
    {
        return new string[]
        {
            "헤드샷 10 이상",
            "피격 3 이하",
            "2분 이내 클리어"
        };
    }

    public bool[] BuildConditionResults(RankRecord record)
    {
        if (record == null)
        {
            return new bool[0];
        }

        return new bool[]
        {
            record.HeadshotKills >= 10,
            record.HitCount <= 3,
            record.ClearTime <= 120f
        };
    }

    public void ShowCurrentResultWithConditions()
    {
        if (m_rankTracker == null || m_rankResultPanel == null || m_rankCalculator == null)
        {
            return;
        }

        RankRecord record = m_rankTracker.GetCurrentRecord();
        if (record == null)
        {
            return;
        }

        RankResult result = m_rankCalculator.Calculate(record);
        if (result == null)
        {
            return;
        }

        string[] conditionTitles = BuildConditionTitles(record);
        bool[] conditionResults = BuildConditionResults(record);
        m_rankResultPanel.Show(result, conditionTitles, conditionResults);
    }
}
