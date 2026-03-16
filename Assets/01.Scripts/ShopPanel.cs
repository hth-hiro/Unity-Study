using UnityEngine;

public class ShopPanel : MonoBehaviour, ISlotHandler
{
    [SerializeField] private BaseSlot[] slots;
    [SerializeField] private ItemData[] shopItems;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BuyItem(int index)
    {
        ItemData item = shopItems[index];
        InventoryPanel.Instance.AddItem(item, 1);
    }

    // ISlotHandler
    public void OnClickSlot(int index)
    {
        //
    }

    public void OnRightClickSlot(int index)
    {
        if (slots[index].IsEmpty()) return;
    }

    public int GetAmount(int index) => 99; // 상점은 항상 수량이 많다고 가정

    public void HandleAction(int index)
    {
        // 상점에서의 Action은 '구매'로 해석
        BuyItem(index);
    }

    public void HandleSplit(int index) { /* 상점은 나누기 기능 필요 없음 */ }
    public void OnHoverSlot(int index) { /* 툴팁 표시 */ }
    public void OnExitSlot(int index) { /* 툴팁 끄기 */ }
}
