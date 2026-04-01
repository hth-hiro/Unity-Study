using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankConditionRowUI : MonoBehaviour
{
    [SerializeField] private Image m_iconImage;
    [SerializeField] private TextMeshProUGUI m_titleText;
    [SerializeField] private TextMeshProUGUI m_statusText;
    [SerializeField] private Image m_backgroundImage;
    [SerializeField] private Sprite m_successSprite;
    [SerializeField] private Sprite m_failSprite;
    [SerializeField] private Color m_successColor = Color.white;
    [SerializeField] private Color m_failColor = Color.gray;
    [SerializeField] private Color m_defaultColor = Color.white;

    public void SetCondition(string title, bool isSuccess)
    {
        if (m_titleText != null)
        {
            m_titleText.text = title ?? string.Empty;
        }

        if (m_statusText != null)
        {
            m_statusText.text = isSuccess ? "SUCCESS" : "FAIL";
        }

        if (m_iconImage != null)
        {
            m_iconImage.sprite = isSuccess ? m_successSprite : m_failSprite;
            m_iconImage.color = isSuccess ? m_successColor : m_failColor;
        }

        if (m_backgroundImage != null)
        {
            m_backgroundImage.color = isSuccess ? m_successColor : m_failColor;
        }
    }

    public void Clear()
    {
        if (m_titleText != null)
        {
            m_titleText.text = string.Empty;
        }

        if (m_statusText != null)
        {
            m_statusText.text = string.Empty;
        }

        if (m_iconImage != null)
        {
            m_iconImage.sprite = null;
            m_iconImage.color = m_defaultColor;
        }

        if (m_backgroundImage != null)
        {
            m_backgroundImage.color = m_defaultColor;
        }
    }
}
