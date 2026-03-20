using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// 상점 화면 갱신

public class ShopSlot : BaseSlot
{
    [Header("Shop Specific")]
    [SerializeField] private TextMeshProUGUI m_itemPriceText;

    public override void Initialize(ISlotHandler handler, int index)
    {
        base.Initialize(handler, index);
    }

    public override void SetItem(ItemData item, int amount)
    {
        base.SetItem(item, amount);

        if (item != null)
        {
            if (m_itemPriceText != null) m_itemPriceText.text = $"{item.buyPrice} G";
        }
        else
        {
            SetEmpty();
        }
    }

    public override void SetEmpty()
    {
        base.SetEmpty();
        if (m_itemPriceText != null) m_itemPriceText.text = "";
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        //
    }
}
