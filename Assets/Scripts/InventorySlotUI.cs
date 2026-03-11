using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text amountText;

    private InventoryUI inventoryUI;
    private int slotIndex;

    public void Initialize(InventoryUI ui, int index)
    {
        inventoryUI = ui;
        slotIndex = index;
    }

    public void SetEmpty()
    {
        icon.enabled = false;
        amountText.text = "";
    }

    public void SetItem(ItemData item, int amount)
    {
        if (item == null)
        {
            SetEmpty();
            return;
        }

        icon.enabled = true;
        icon.sprite = item.icon;
        amountText.text = amount > 1 ? amount.ToString() : "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 이 함수는 좌클릭 우클릭 휠클릭 모두 감지됨. 분기 나눠야 함.
        // 1. 좌클릭일 때
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            inventoryUI.OnClickSlot(slotIndex);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryUI.OnHoverSlot(slotIndex);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryUI.OnExitSlot(slotIndex);
    }
}
