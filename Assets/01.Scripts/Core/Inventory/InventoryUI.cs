using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("Detail")]
    [SerializeField] private Image m_detailIconImage;
    [SerializeField] private TextMeshProUGUI m_detailNameText;
    [SerializeField] private TextMeshProUGUI m_detailDescriptionText;
    [SerializeField] private TextMeshProUGUI m_detailAmountText;

    [Header("Slots")]
    [SerializeField] private InventorySlotView[] m_slots;

    public void SetItems(IReadOnlyList<InventorySlotViewData> items)
    {
        if (m_slots == null)
            return;

        for (int i = 0; i < m_slots.Length; i++)
        {
            if (m_slots[i] == null)
                continue;

            if (items != null && i < items.Count)
            {
                m_slots[i].SetData(items[i]);
            }
            else
            {
                m_slots[i].SetEmpty();
            }
        }
    }

    public void SetDetail(InventorySlotViewData data)
    {
        if (data.IsEmpty)
        {
            ClearDetail();
            return;
        }

        if (m_detailIconImage != null)
        {
            m_detailIconImage.sprite = data.Item.Icon;
            m_detailIconImage.gameObject.SetActive(data.Item.Icon != null);
        }

        if (m_detailNameText != null)
        {
            m_detailNameText.text = data.Item.ItemName ?? string.Empty;
        }

        if (m_detailDescriptionText != null)
        {
            m_detailDescriptionText.text = data.Item.Description ?? string.Empty;
        }

        if (m_detailAmountText != null)
        {
            m_detailAmountText.text = data.Amount.ToString();
        }
    }

    public void Clear()
    {
        ClearDetail();

        if (m_slots == null)
            return;

        for (int i = 0; i < m_slots.Length; i++)
        {
            if (m_slots[i] != null)
            {
                m_slots[i].Clear();
            }
        }
    }

    private void ClearDetail()
    {
        if (m_detailIconImage != null)
        {
            m_detailIconImage.sprite = null;
            m_detailIconImage.gameObject.SetActive(false);
        }

        if (m_detailNameText != null)
        {
            m_detailNameText.text = string.Empty;
        }

        if (m_detailDescriptionText != null)
        {
            m_detailDescriptionText.text = string.Empty;
        }

        if (m_detailAmountText != null)
        {
            m_detailAmountText.text = string.Empty;
        }
    }
}