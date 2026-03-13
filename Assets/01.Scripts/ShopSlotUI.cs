using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopSlotUI : BaseSlot
{
    [Header("Shop Specific")]
    [SerializeField] private TextMeshProUGUI itemPriceText;
    [SerializeField] private Button purchaseButton;

    private ItemData shopItem;
    private ShopPanel shopPanel;

    public void SetShopItem(ItemData item, ShopPanel panel)
    {
        shopItem = item;
        shopPanel = panel;

        SetItem(item, 1);

        if (itemPriceText !=null)
        {
            itemPriceText.text = $"{item.buyPrice} G";
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (shopItem == null) return;

        if (CurrencyManager.Instance.ConsumeGold(shopItem.buyPrice))
        {
            InventoryPanel.Instance.AddItem(shopItem, 1);
        }
    }
}
