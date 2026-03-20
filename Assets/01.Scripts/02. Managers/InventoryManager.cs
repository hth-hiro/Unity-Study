using UnityEngine;
using UnityEngine.InputSystem;

// 가방 칸 계산 0 ~ 35 계산, 아이템 추가/삭제 로직

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [SerializeField] private GameObject m_inventoryUI;

    public InventoryData Data;
    public ItemStack PickedSlot = new ItemStack();
    public bool HasPickedItem => !PickedSlot.IsEmpty();

    private const int m_inventorySize = 40;
    private const int m_storageEndIndex = 36;

    void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
            Data = new InventoryData(m_inventorySize);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnDisable()
    {
        TooltipManager.Instance?.Hide();
        ContextMenuManager.Instance?.Close();

        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.ClearPickedItemUI();
        }

        if (HasPickedItem)
        {
            int remaining = AddItem(PickedSlot.item, PickedSlot.amount);
            PickedSlot.Clear();
            if (!PickedSlot.IsEmpty())
            {
                // TODO : 아이템 생성로직
            }
        }
    }

    public int AddItem(ItemData item, int amount)
    {
        int remaining = amount;

        if (item == null) return remaining;

        if (item.maxStack > 1)
        {
            for (int i = 0; i < m_storageEndIndex; i++)
            {
                if (!Data.Slots[i].IsEmpty() && Data.Slots[i].item == item)
                {
                    int canAdd = item.maxStack - Data.Slots[i].amount;
                    if (canAdd <= 0) continue;
                    int addAmount = Mathf.Min(canAdd, remaining);

                    Data.Slots[i].amount += addAmount;
                    remaining -= addAmount;

                    if (remaining <= 0) break;
                }
            }
        }

        if (remaining > 0)
        {
            for (int i = 0; i < m_storageEndIndex; i++)
            {
                if (Data.Slots[i].IsEmpty())
                {
                    int addAmount = Mathf.Min(remaining, item.maxStack);

                    Data.Slots[i].item = item;
                    Data.Slots[i].amount = addAmount;

                    remaining -= addAmount;

                    if (remaining <= 0) break;
                }
            }
        }

        return remaining;   // 0 : 다 들어감, 숫자 : 남음.
    }

    public void HandleLeftClick(int index)
    {
        if (index < 0 || index >= m_inventorySize) return;

        ItemStack clickedData = Data.Slots[index];

        Debug.Log($"{index}번 슬롯 클릭됨!");

        // 1. 아무것도 안 들고 있을 때
        if (!HasPickedItem)
        {
            if (!clickedData.IsEmpty())
            {
                PickedSlot.item = clickedData.item;
                PickedSlot.amount = clickedData.amount;
                clickedData.Clear();
            }
        }

        else
        {
            if (index >= m_storageEndIndex)
            {
                Debug.Log("놓을 수 없습니다.");
                return;
            }

            if (clickedData.item == PickedSlot.item)
            {
                int maxStack = PickedSlot.item.maxStack;
                int total = clickedData.amount + PickedSlot.amount;

                if (total <= maxStack)
                {
                    clickedData.amount = total;
                    PickedSlot.Clear();
                }
                else
                {
                    clickedData.amount = maxStack;
                    PickedSlot.amount = total - maxStack;
                }
            }

            // 3. 다른 아이템이면 교환
            else
            {
                ItemData tempItem = clickedData.item;
                int tempAmount = clickedData.amount;

                clickedData.item = PickedSlot.item;
                clickedData.amount = PickedSlot.amount;

                PickedSlot.item = tempItem;
                PickedSlot.amount = tempAmount;
            }
        }

        InventoryUI.Instance?.RequestRefresh();
    }

    public void HandleRightClick(int index)
    {
        if (index < 0 || index >= m_inventorySize) return;

        ItemStack slot = Data.Slots[index];
        if (slot.IsEmpty() || HasPickedItem) return;

        TooltipManager.Instance?.Hide();
        Vector2 mousePos = Mouse.current.position.ReadValue();
        ContextMenuManager.Instance.Open(index, slot.item, mousePos, InventoryUI.Instance);
    }

    public void HandleAction(int index)
    {
        ItemStack slot = Data.Slots[index];
        if (slot.IsEmpty()) return;

        if (slot.item.itemType == ItemType.Equipment)
        {
            EquipItem(index);
            return;
        }

        slot.amount -= 1;

        if (slot.amount <= 0)
        {
            slot.Clear();
            TooltipManager.Instance.Hide(); // 아이템이 사라졌으니 툴팁도 끔
        }

        InventoryUI.Instance?.RequestRefresh();
    }

    public void SplitItem(int index)
    {
        ItemStack slot = Data.Slots[index];
        if (slot.amount <= 1 || HasPickedItem) return;

        int splitAmount = slot.amount / 2;

        PickedSlot.item = slot.item;
        PickedSlot.amount = splitAmount;
        slot.amount -= splitAmount;

        InventoryUI.Instance?.RequestRefresh();
    }

    private void EquipItem(int index)
    {
        ItemStack fromSlot = Data.Slots[index];
        if (fromSlot.IsEmpty()) return;

        int equipIndex = GetEquipmentSlotIndex(fromSlot.item);
        if (equipIndex < 0) return;

        SwapItems(index, equipIndex);
        InventoryUI.Instance?.RequestRefresh();
    }

    private int GetEquipmentSlotIndex(ItemData item)
    {
        if (item.itemType != ItemType.Equipment) return -1;

        switch (item.equipType)
        {
            case EquipType.Helmet: return 36;
            case EquipType.Chestplate: return 37;
            case EquipType.Leggings: return 38;
            case EquipType.Boots: return 39;
            default: return -1;
        }
    }

    private void SwapItems(int a, int b)
    {
        ItemData tempItem = Data.Slots[a].item;
        int tempAmount = Data.Slots[a].amount;

        Data.Slots[a].item = Data.Slots[b].item;
        Data.Slots[a].amount = Data.Slots[b].amount;

        Data.Slots[b].item = tempItem;
        Data.Slots[b].amount = tempAmount;
    }

    public void OnToggleInventory(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if (m_inventoryUI == null) return;

        bool nextState = !m_inventoryUI.activeSelf;
        m_inventoryUI.SetActive(nextState);

        if (nextState)
        {
            InventoryUI.Instance?.RequestRefresh();
        }
        else
        {
            TooltipManager.Instance?.Hide();
        }

        PlayerController.Instance.SetInputBlock(nextState);
    }
}
