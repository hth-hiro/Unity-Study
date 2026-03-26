using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuickSlotSlotView : MonoBehaviour
{
    [SerializeField] private Image m_iconImage;
    [SerializeField] private TextMeshProUGUI m_amountText;
    [SerializeField] private GameObject m_emptySlotIndicator;
    [SerializeField] private TextMeshProUGUI m_slotNumberText;

    private QuickSlotViewData m_currentData;
    public QuickSlotViewData CurrentData => m_currentData;

    public void SetData(QuickSlotViewData data)
    {
        m_currentData = data;

        if (m_currentData.IsEmpty)
        {
            SetEmpty();
            return;
        }

        if (m_emptySlotIndicator != null)
        {
            m_emptySlotIndicator.SetActive(false);
        }

        if (m_iconImage != null)
        {
            m_iconImage.gameObject.SetActive(true);
            m_iconImage.sprite = data.Item.Icon;
        }

        if (m_amountText != null)
        {
            if (data.Amount > 1)
            {
                m_amountText.gameObject.SetActive(true);
                m_amountText.text = data.Amount.ToString();
            }
            else
            {
                m_amountText.gameObject.SetActive(false);
                m_amountText.text = string.Empty;
            }
        }
    }

    public void SetEmpty()
    {
        if (m_iconImage != null)
        {
            m_iconImage.gameObject.SetActive(false);
            m_iconImage.sprite = null;
        }

        if (m_amountText != null)
        {
            m_amountText.gameObject.SetActive(false);
            m_amountText.text = string.Empty;
        }

        if (m_emptySlotIndicator != null)
        {
            m_emptySlotIndicator.SetActive(true);
        }
    }

    public void Clear()
    {
        m_currentData = QuickSlotViewData.Empty;
        SetEmpty();
    }
}
