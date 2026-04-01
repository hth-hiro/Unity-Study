using UnityEngine;

public class StageRankResultPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject m_panelRoot;
    [SerializeField] private RankRecordRowUI m_headshotRow;
    [SerializeField] private RankRecordRowUI m_skillRow;
    [SerializeField] private RankRecordRowUI m_comboRow;
    [SerializeField] private RankRecordRowUI m_aerialRow;
    [SerializeField] private RankRecordRowUI m_clearTimeRow;
    [SerializeField] private RankSummaryUI m_rankSummary;
    [SerializeField] private RankConditionRowUI[] m_conditionRows;

    public void Show(RankResult result)
    {
        Clear();

        if (result == null)
        {
            ShowPanelRoot();
            return;
        }

        RankRecord record = result.OriginalRecord;

        if (record != null)
        {
            if (m_headshotRow != null)
            {
                m_headshotRow.SetRow("헤드샷", $"{record.HeadshotKills} Kill", string.Empty);
            }

            if (m_skillRow != null)
            {
                m_skillRow.SetRow("스킬", $"{record.SkillKills} Kill", string.Empty);
            }

            if (m_comboRow != null)
            {
                m_comboRow.SetRow("연속 처치", $"{record.MaxCombo} Combo", string.Empty);
            }

            if (m_aerialRow != null)
            {
                m_aerialRow.SetRow("공중 처치", $"{record.AerialKills} Kill", string.Empty);
            }

            if (m_clearTimeRow != null)
            {
                m_clearTimeRow.SetRow("소요 시간", FormatTime(record.ClearTime), string.Empty);
            }
        }

        if (m_rankSummary != null)
        {
            m_rankSummary.SetSummary(result.Grade, result.TotalScore);
        }

        ClearConditionRows();
        ShowPanelRoot();
    }

    public void Show(RankResult result, string[] conditionTitles, bool[] conditionResults)
    {
        Show(result);

        if (m_conditionRows == null || conditionTitles == null || conditionResults == null)
        {
            ClearConditionRows();
            ShowPanelRoot();
            return;
        }

        int count = Mathf.Min(m_conditionRows.Length, conditionTitles.Length, conditionResults.Length);

        for (int i = 0; i < count; i++)
        {
            RankConditionRowUI conditionRow = m_conditionRows[i];
            if (conditionRow == null)
            {
                continue;
            }

            conditionRow.SetCondition(conditionTitles[i], conditionResults[i]);
        }

        for (int i = count; i < m_conditionRows.Length; i++)
        {
            RankConditionRowUI conditionRow = m_conditionRows[i];
            if (conditionRow != null)
            {
                conditionRow.Clear();
            }
        }
    }

    public void Hide()
    {
        if (m_panelRoot != null)
        {
            m_panelRoot.SetActive(false);
            return;
        }

        gameObject.SetActive(false);
    }

    public void Clear()
    {
        if (m_headshotRow != null)
        {
            m_headshotRow.Clear();
        }

        if (m_skillRow != null)
        {
            m_skillRow.Clear();
        }

        if (m_comboRow != null)
        {
            m_comboRow.Clear();
        }

        if (m_aerialRow != null)
        {
            m_aerialRow.Clear();
        }

        if (m_clearTimeRow != null)
        {
            m_clearTimeRow.Clear();
        }

        if (m_rankSummary != null)
        {
            m_rankSummary.Clear();
        }

        ClearConditionRows();
    }

    public string FormatTime(float seconds)
    {
        int totalSeconds = Mathf.Max(0, Mathf.FloorToInt(seconds));
        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int remainSeconds = totalSeconds % 60;

        return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, remainSeconds);
    }

    private void ShowPanelRoot()
    {
        if (m_panelRoot != null)
        {
            m_panelRoot.SetActive(true);
            return;
        }

        gameObject.SetActive(true);
    }

    private void ClearConditionRows()
    {
        if (m_conditionRows == null)
        {
            return;
        }

        for (int i = 0; i < m_conditionRows.Length; i++)
        {
            RankConditionRowUI conditionRow = m_conditionRows[i];
            if (conditionRow != null)
            {
                conditionRow.Clear();
            }
        }
    }
}
