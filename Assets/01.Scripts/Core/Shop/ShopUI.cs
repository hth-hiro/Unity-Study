using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    [Header("Header")]
    [SerializeField] private TextMeshProUGUI m_npcDialogueText;
    [SerializeField] private TextMeshProUGUI m_currencyText;

    [Header("Detail")]
    [SerializeField] private Image m_detailIconImage;
    [SerializeField] private TextMeshProUGUI m_detailNameText;
    [SerializeField] private TextMeshProUGUI m_detailDescriptionText;
    [SerializeField] private TextMeshProUGUI m_detailPriceText;
    [SerializeField] private TextMeshProUGUI m_detailCooldownText;
    [SerializeField] private TextMeshProUGUI m_detailDurationText;

    [Header("Slots")]
    [SerializeField] private ShopSlotView[] m_charmSlots;
    [SerializeField] private ShopSlotView[] m_grimoireSlots;

    public void SetDialogue(string dialogue)
    {
        if (m_npcDialogueText != null)
        {
            m_npcDialogueText.text = dialogue ?? string.Empty;
        }
    }

    public void SetCurrency(int value)
    {
        if (m_currencyText != null)
        {
            value = Mathf.Max(0, value);
            m_currencyText.text = value.ToString("N0");
        }
    }

    public void SetDetail(ItemData item)
    {
        if (item == null)
        {
            ClearDetail();
            return;
        }

        if (m_detailIconImage != null)
        {
            m_detailIconImage.sprite = item.Icon;
            m_detailIconImage.gameObject.SetActive(item.Icon != null);
        }

        if (m_detailNameText != null)
        {
            m_detailNameText.text = item.ItemName ?? string.Empty;
        }
        
        if (m_detailDescriptionText != null)
        {
            m_detailDescriptionText.text = item.Description ?? string.Empty;
        }

        if (m_detailPriceText != null)
        {
            m_detailPriceText.text = item.BuyPrice.ToString("N0");
        }

        if (m_detailCooldownText != null)
        {
            m_detailCooldownText.text = item.Cooldown > 0f
                ? $"{item.Cooldown:0.##}√ "
                : "-";
        }

        if (m_detailDurationText != null)
        {
            m_detailDurationText.text = item.Duration > 0f
                ? $"{item.Duration:0.##}√ "
                : "-";
        }
    }

    public void SetCharmItems(IReadOnlyList<ShopSlotViewData> items)
    {
        SetSlots(m_charmSlots, items);
    }

    public void SetGrimoireItems(IReadOnlyList<ShopSlotViewData> items)
    {
        SetSlots(m_grimoireSlots, items);
    }

    private void ClearSlots(ShopSlotView[] slots)
    {
        if (slots == null)
            return;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null)
            {
                slots[i].Clear();
            }
        }
    }

    public void Clear()
    {
        SetDialogue(string.Empty);

        if (m_currencyText != null)
        {
            m_currencyText.text = string.Empty;
        }

        ClearDetail();
        ClearSlots(m_charmSlots);
        ClearSlots(m_grimoireSlots);
    }

    private void SetSlots(ShopSlotView[] slots, IReadOnlyList<ShopSlotViewData> items)
    {
        if (slots == null)
            return;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
                continue;

            if (items != null && i < items.Count)
            {
                slots[i].SetData(items[i]);
            }
            else
            {
                slots[i].SetEmpty();
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

        if (m_detailPriceText != null)
        {
            m_detailPriceText.text = string.Empty;
        }

        if (m_detailCooldownText != null)
        {
            m_detailCooldownText.text = string.Empty;
        }

        if (m_detailDurationText != null)
        {
            m_detailDurationText.text = string.Empty;
        }
    }
}
