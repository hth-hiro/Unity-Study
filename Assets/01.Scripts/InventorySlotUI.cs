using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlotUI : BaseSlot, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public override void Initialize(InventoryUI inventory, int slotIndex)
    {
        base.Initialize(inventory, slotIndex);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
    }

    public override void SetItem(ItemData item, int amount)
    {
        base.SetItem(item, amount);

        // TODO : Add SlotColor
    }
}
