using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image m_iconImage;
    [SerializeField] private TextMeshProUGUI m_amountText;
    [SerializeField] private GameObject m_emptyRoot;

    private InventorySlotViewData m_currentData;
    public InventorySlotViewData CurrentData => m_currentData;

    public void SetData(InventorySlotViewData data)
    {
        m_currentData = data;

        if (data.IsEmpty)
        {
            SetEmpty();
            return;
        }

        if (m_iconImage != null)
        {
            m_iconImage.sprite = data.Item.Icon;
            m_iconImage.gameObject.SetActive(data.Item.Icon != null);
        }

        if (m_amountText != null)
        {
            m_amountText.text = data.Amount > 1 ? data.Amount.ToString() : string.Empty;
        }

        if (m_emptyRoot != null)
        {
            m_emptyRoot.SetActive(false);
        }
    }

    public void SetEmpty()
    {
        m_currentData = InventorySlotViewData.Empty;

        if (m_iconImage != null)
        {
            m_iconImage.sprite = null;
            m_iconImage.gameObject.SetActive(false);
        }

        if (m_amountText != null)
        {
            m_amountText.text = string.Empty;
        }

        if (m_emptyRoot != null)
        {
            m_emptyRoot.SetActive(true);
        }
    }

    public void Clear()
    {
        SetEmpty();
    }
}