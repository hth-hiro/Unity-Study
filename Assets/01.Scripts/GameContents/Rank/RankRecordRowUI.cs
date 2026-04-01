using TMPro;
using UnityEngine;

public class RankRecordRowUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_labelText;
    [SerializeField] private TextMeshProUGUI m_valueText;
    [SerializeField] private TextMeshProUGUI m_scoreText;

    public void SetRow(string label, string value, string scoreText)
    {
        if (m_labelText != null)
        {
            m_labelText.text = label ?? string.Empty;
        }

        if (m_valueText != null)
        {
            m_valueText.text = value ?? string.Empty;
        }

        if (m_scoreText != null)
        {
            m_scoreText.text = scoreText ?? string.Empty;
        }
    }

    public void Clear()
    {
        SetRow(string.Empty, string.Empty, string.Empty);
    }
}
