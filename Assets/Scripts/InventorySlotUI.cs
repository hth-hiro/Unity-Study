using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private Button button;

    private InventoryUI inventoryUI;
    private int slotIndex;

    public void Initialize(InventoryUI ui, int index)
    {
        inventoryUI = ui;
        slotIndex = index;

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClickSlot);
        }
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

    private void OnClickSlot()
    {
        inventoryUI.OnClickSlot(slotIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"Hover Enter: {gameObject.name}");
        inventoryUI.OnHoverSlot(slotIndex);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryUI.OnExitSlot(slotIndex);
    }
}
