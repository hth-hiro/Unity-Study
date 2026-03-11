using UnityEngine;
using TMPro;

public class ContextMenuUI : MonoBehaviour
{
    [SerializeField] private TMP_Text actionText;
    private int targetIndex;
    private InventoryUI inventoryUI;

    public void Open(int index, ItemData item, Vector2 position, InventoryUI ui)
    {
        targetIndex = index;
        inventoryUI = ui;

        gameObject.SetActive(true);
        transform.position = position;

        actionText.text = item.itemType == ItemType.Equipment ? "장착" : "사용";
    }

    public void Close() => gameObject.SetActive(false);

    // 버튼 클릭 이벤트
    //public void OnClickAction() { inventoryUI}
}
