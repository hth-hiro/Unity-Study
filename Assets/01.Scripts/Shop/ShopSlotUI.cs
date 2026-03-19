using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopSlotUI : BaseSlot
{
    [Header("Shop Specific")]
    [SerializeField] private TextMeshProUGUI m_itemPriceText;
    [SerializeField] private Button m_purchaseButton;

    private ItemData m_shopItem;

    public override void Initialize(ISlotHandler handler, int index)
    {
        base.Initialize(handler, index);

        if (m_purchaseButton != null )
        {
            m_purchaseButton.onClick.RemoveAllListeners();
            m_purchaseButton.onClick.AddListener(() => handler.HandleAction(index));
        }
    }

    public override void SetItem(ItemData item, int amount)
    {
        base.SetItem(item, amount);

        if (item != null)
        {
            if (m_itemPriceText != null) m_itemPriceText.text = $"{item.buyPrice}";
            if (m_purchaseButton != null) m_purchaseButton.gameObject.SetActive(true);

            if (amount <= 0 && m_purchaseButton != null) m_purchaseButton.interactable = false;
            else if (m_purchaseButton != null) m_purchaseButton.interactable = true;
        }
    }

    public override void SetEmpty()
    {
        base.SetEmpty();
        if (m_itemPriceText != null) m_itemPriceText.text = "";
        if (m_purchaseButton != null) m_purchaseButton.gameObject.SetActive(false);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        //
    }
}
