using UnityEngine;

public class ShopPanel : MonoBehaviour, ISlotHandler
{
    [SerializeField] private BaseSlot[] m_slots;
    [SerializeField] private ItemData[] m_shopItems;

    public ItemContainer ShopContainer;

    void OnDisable()
    {
        if (TooltipManager.Instance != null) TooltipManager.Instance.Hide();

        RefreshAllSlots();
    }

    void Awake()
    {
        m_slots = GetComponentsInChildren<BaseSlot>();
        ShopContainer = new ItemContainer(m_slots.Length);
    }

    void Start()
    {
        InitializeSlotUIs();

        for (int i = 0; i< m_shopItems.Length; i++)
        {
            if (i < ShopContainer.slotDatas.Length)
            {
                ShopContainer.slotDatas[i].item = m_shopItems[i];
                ShopContainer.slotDatas[i].amount = 64;
            }
        }

        RefreshAllSlots();
    }

    void InitializeSlotUIs()
    {
        for (int i = 0; i< m_slots.Length;i++)
        {
            m_slots[i].Initialize(this, i);
        }
    }

    void RefreshAllSlots()
    {
        for (int i = 0; i< m_slots.Length;i++)
        {
            var data = ShopContainer.slotDatas[i];

            if (data.IsEmpty()) m_slots[i].SetEmpty();
            else m_slots[i].SetItem(data.item, data.amount);
        }
    }    

    void BuyItem(int index)
    {
        ItemStack slotData = ShopContainer.slotDatas[index];
        if (slotData.IsEmpty() || slotData.amount <= 0) return;

        ItemData item = slotData.item;

        if (CurrencyManager.Instance.ConsumeGold(item.buyPrice))
        {
            int remaining = InventoryPanel.Instance.AddItem(item, 1);

            if (remaining == 0)
            {
                Debug.Log($"{item.itemName} 구매 성공!");
                slotData.amount--;
                RefreshAllSlots();
            }
            else
            {
                Debug.Log("인벤토리가 가득 찼습니다.");
                CurrencyManager.Instance.AddGold(item.buyPrice);
            }
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    // ISlotHandler
    public void OnClickSlot(int index)
    {
        BuyItem(index);
    }

    public void OnRightClickSlot(int index)
    {
        //if (m_slots[index].IsEmpty()) return;
    }

    public int GetAmount(int index) => 64; // 상점은 항상 수량이 많다고 가정

    public void HandleAction(int index)
    {
        // 상점에서의 Action은 '구매'로 해석
        BuyItem(index);
    }

    public void HandleSplit(int index) { /* 상점은 나누기 기능 필요 없음 */ }

    public void OnHoverSlot(int index)
    {
        if (!ShopContainer.slotDatas[index].IsEmpty())
            TooltipManager.Instance.Show(ShopContainer.slotDatas[index].item);
    }

    public void OnExitSlot(int index)
    {
        TooltipManager.Instance.Hide();
    }
}
