using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlotUI : BaseSlot, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private InventoryUI owner;
    private int index;

    public void Initialize(InventoryUI inventory, int slotIndex)
    {
        owner = inventory;
        index = slotIndex;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 이 함수는 좌클릭 우클릭 휠클릭 모두 감지됨. 분기 나눠야 함.
        // 1. 좌클릭일 때
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            owner.OnClickSlot(index);
        }

        // 2. 우클릭일 때
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            owner.OnRightClickSlot(index);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        owner.OnHoverSlot(index);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        owner.OnExitSlot(index);
    }

    public override void SetItem(ItemData item, int amount)
    {
        base.SetItem(item, amount);

        // TODO : Add SlotColor
    }
}
