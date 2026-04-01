using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankSummaryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_gradeText;
    [SerializeField] private TextMeshProUGUI m_scoreText;
    [SerializeField] private Image m_gradeBackground;
    [SerializeField] private Color m_sColor = new Color(1.0f, 0.84f, 0.0f, 1.0f);
    [SerializeField] private Color m_aColor = new Color(0.75f, 0.75f, 0.75f, 1.0f);
    [SerializeField] private Color m_bColor = new Color(0.35f, 0.55f, 1.0f, 1.0f);
    [SerializeField] private Color m_cColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
    [SerializeField] private Color m_defaultColor = Color.white;

    public void SetSummary(RankGrade grade, int totalScore)
    {
        SetGrade(grade);
        SetScore(totalScore);
    }

    public void SetGrade(RankGrade grade)
    {
        string gradeText = grade.ToString();

        if (m_gradeText != null)
        {
            m_gradeText.text = gradeText;
        }

        if (m_gradeBackground != null)
        {
            m_gradeBackground.color = GetGradeColor(grade);
        }
    }

    public void SetScore(int totalScore)
    {
        if (m_scoreText != null)
        {
            m_scoreText.text = totalScore.ToString("N0");
        }
    }

    public void Clear()
    {
        if (m_gradeText != null)
        {
            m_gradeText.text = string.Empty;
        }

        if (m_scoreText != null)
        {
            m_scoreText.text = string.Empty;
        }

        if (m_gradeBackground != null)
        {
            m_gradeBackground.color = m_defaultColor;
        }
    }

    private Color GetGradeColor(RankGrade grade)
    {
        switch (grade)
        {
            case RankGrade.S:
                return m_sColor;
            case RankGrade.A:
                return m_aColor;
            case RankGrade.B:
                return m_bColor;
            default:
                return m_cColor;
        }
    }
}
