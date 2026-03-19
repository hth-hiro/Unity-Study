using System;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryPanel : MonoBehaviour, ISlotHandler
{
    public static InventoryPanel Instance { get; private set; }

    [SerializeField] private BaseSlot[] slots;

    [Header("Test Items")]
    [SerializeField] private ItemData apple;
    [SerializeField] private ItemData potion;
    [SerializeField] private ItemData sword;
    [SerializeField] private ItemData meat;
    [SerializeField] private ItemData chestplate;

    [SerializeField] private PickedItemUI pickedItemUI;

    public ItemContainer container;

    private InventorySlotData pickedSlot = new InventorySlotData();
    private bool hasPickedItem => pickedSlot != null && !pickedSlot.IsEmpty();
    private int pickedSlotIndex = -1;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        slots = GetComponentsInChildren<BaseSlot>();
        container = new ItemContainer(slots.Length);

        Initialize();

        AddItem(meat, 70);
        AddItem(sword, 1);
        AddItem(potion, 3);
        AddItem(chestplate, 1);
        AddItem(apple, 32);
    }

    void OnDisable()
    {
        if (TooltipManager.Instance != null) TooltipManager.Instance.Hide();
        if (ContextMenuManager.Instance != null) ContextMenuManager.Instance.Close();
        if (pickedItemUI != null) pickedItemUI.SetEmpty();

        if (hasPickedItem)
        {
            int remaining = container.AddItem(pickedSlot.item, pickedSlot.amount);
            pickedSlot.Clear();
            if (!pickedSlot.IsEmpty())
            {
                // TODO : 아이템 생성로직
            }
        }

        pickedSlotIndex = -1;
        RefreshAllSlots();
    }

    void Update()
    {
        UpdatePickedItemUI();
    }

    void Initialize()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Initialize(this, i);
        }
    }

    public int AddItem(ItemData item, int amount)
    {
        int remaining = container.AddItem(item, amount);
        RefreshAllSlots();
        return remaining;   // 0 : 다 들어감, 숫자 : 남음.
    }

    private void EquipItem(int index)
    {
        ItemData itemToEquip = container.slotDatas[index].item;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] is EquipmentSlotUI equipSlot)
            {
                if (equipSlot.CanEquip(itemToEquip))
                {
                    SwapItems(index, i);
                    return;
                }
            }
        }
    }

    private void SwapItems(int indexA, int indexB)
    {
        container.Swap(indexA, indexB);
        RefreshAllSlots();
    }

    void RefreshAllSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (container.slotDatas[i].IsEmpty())
                slots[i].SetEmpty();
            else
                slots[i].SetItem(container.slotDatas[i].item, container.slotDatas[i].amount);
        }
    }

    public int GetAmount(int index)
    {
        return container.slotDatas[index].amount;
    }

    public void OnClickSlot(int index)
    {
        ContextMenuManager.Instance.Close();
        Debug.Log($"{index}번 슬롯 클릭됨!");

        InventorySlotData clickedSlot = container.slotDatas[index];
        BaseSlot targetSlotUI = slots[index];

        if (hasPickedItem)
        {
            if (targetSlotUI is EquipmentSlotUI equipSlot)
            {
                if (!equipSlot.CanEquip(pickedSlot.item))
                {
                    Debug.Log("놓을 수 없습니다.");
                    return;
                }
            }
        }

        // 1. 아무것도 안들고있고, 클릭한 슬롯에 아이템이 있으면 집기
        if (!hasPickedItem)
        {
            if (!clickedSlot.IsEmpty())
            {
                pickedSlot.item = clickedSlot.item;
                pickedSlot.amount = clickedSlot.amount;
                pickedSlotIndex = index;

                clickedSlot.Clear();
                //hasPickedItem = true;

                TooltipManager.Instance.Hide();
            }
        }
        else
        {
            // 2. 손에 들고 있는 상태에서 빈 슬롯 클릭 -> 그대로 놓기
            if (clickedSlot.IsEmpty())
            {
                clickedSlot.item = pickedSlot.item;
                clickedSlot.amount = pickedSlot.amount;

                pickedSlot.Clear();
                //hasPickedItem = false;
                pickedSlotIndex = -1;
            }
            else
            {
                // 3. 같은 아이템이면 합치기
                if (clickedSlot.item == pickedSlot.item)
                {
                    int maxStack = clickedSlot.item.maxStack;
                    int totalAmount = clickedSlot.amount + pickedSlot.amount;

                    if (totalAmount <= maxStack)
                    {
                        clickedSlot.amount = totalAmount;
                        pickedSlot.Clear();
                        //hasPickedItem = false;
                        pickedSlotIndex = -1;
                    }
                    else
                    {
                        clickedSlot.amount = maxStack;
                        pickedSlot.amount = totalAmount - maxStack;
                    }
                }
                else
                {
                    // 4. 다른 아이템이면 교환
                    ItemData tempItem = clickedSlot.item;
                    int tempAmount = clickedSlot.amount;

                    clickedSlot.item = pickedSlot.item;
                    clickedSlot.amount = pickedSlot.amount;

                    pickedSlot.item = tempItem;
                    pickedSlot.amount = tempAmount;

                    //hasPickedItem = true;
                    pickedSlotIndex = index;
                }
            }

            if (!hasPickedItem)
            {
                OnHoverSlot(index);
            }
        }

        RefreshAllSlots();
    }

    public void OnRightClickSlot(int index)
    {
        if (hasPickedItem) return;

        InventorySlotData slot = container.slotDatas[index];
        if (slot.IsEmpty()) return;

        TooltipManager.Instance.Hide();
        ContextMenuManager.Instance.Open(index, slot.item, Mouse.current.position.ReadValue(), this);
    }

    public void OnHoverSlot(int index)
    {
        if (hasPickedItem || ContextMenuManager.Instance.IsVisible)
        {
            TooltipManager.Instance.Hide();
            return;
        }

        InventorySlotData hoveredSlot = container.slotDatas[index];

        if (hoveredSlot.IsEmpty())
        {
            TooltipManager.Instance.Hide();
            return;
        }

        TooltipManager.Instance.Show(hoveredSlot.item);
    }

    public void OnExitSlot(int index)
    {
        TooltipManager.Instance.Hide();
    }

    public void HandleAction(int index)
    {
        InventorySlotData slot = container.slotDatas[index];
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

        RefreshAllSlots();
    }

    public void HandleSplit(int index)
    {
        InventorySlotData slot = container.slotDatas[index];
        if (slot.amount <= 1) return;

        int originAmount = container.slotDatas[index].amount;
        int splitAmount = container.slotDatas[index].amount / 2;

        pickedSlot.item = slot.item;
        pickedSlot.amount = splitAmount;
        //hasPickedItem = true;

        slot.amount = originAmount - splitAmount;

        RefreshAllSlots();
    }

    void UpdatePickedItemUI()
    {
        if (hasPickedItem && !pickedSlot.IsEmpty())
        {
            pickedItemUI.SetItem(pickedSlot.item, pickedSlot.amount);
            pickedItemUI.FollowMouse();
        }
        else
        {
            pickedItemUI.SetEmpty();
        }
    }
}
