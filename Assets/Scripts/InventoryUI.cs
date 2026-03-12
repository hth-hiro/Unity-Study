using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySlotUI[] slots;

    [Header("Test Items")]
    [SerializeField] private ItemData apple;
    [SerializeField] private ItemData potion;
    [SerializeField] private ItemData sword;
    [SerializeField] private ItemData meat;

    [SerializeField] private PickedItemUI pickedItemUI;
    [SerializeField] private TooltipUI tooltipUI;
    [SerializeField] private ContextMenuUI contextMenuUI;

    private InventorySlotData[] slotDatas;

    private InventorySlotData pickedSlot = new InventorySlotData();
    private bool hasPickedItem = false;
    private int pickedSlotIndex = -1;

    void Awake()
    {
        slots = GetComponentsInChildren<InventorySlotUI>();
    }

    void Start()
    {
        Initialize();
        InitializeSlotUIs();
        TestAddItems();
        RefreshAllSlots();
    }

    void OnDisable()
    {
        if (tooltipUI != null) tooltipUI.Hide();
        if (contextMenuUI != null) contextMenuUI.Close();
        if (pickedItemUI != null) pickedItemUI.SetEmpty();

        InventorySlotData slot = pickedSlot;
        if (hasPickedItem && !pickedSlot.IsEmpty())
        {
            if (slotDatas[pickedSlotIndex].IsEmpty())
            {
                slotDatas[pickedSlotIndex].item = pickedSlot.item;
                slotDatas[pickedSlotIndex].amount = pickedSlot.amount;
            }
            else
            {
                for (int i = 0; i < slotDatas.Length; i++)
                {
                    if (!slotDatas[i].IsEmpty())
                    {
                        slotDatas[i].item = pickedSlot.item;
                        slotDatas[i].amount = pickedSlot.amount;
                        break;
                    }
                }
            }

            pickedSlot.Clear();
            hasPickedItem = false;
            pickedSlotIndex = -1;
            RefreshAllSlots();
        }
    }

    void Update()
    {
        UpdatePickedItemUI();
        UpdateTooltipUI();
    }

    void Initialize()
    {
        slotDatas = new InventorySlotData[slots.Length];
        for (int i = 0; i < slots.Length; i++)
        {
            slotDatas[i]  = new InventorySlotData();
        }
    }

    void InitializeSlotUIs()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Initialize(this, i);
        }
    }

    void TestAddItems()
    {
        slotDatas[0].item = potion;
        slotDatas[0].amount = 1;

        slotDatas[1].item = sword;
        slotDatas[1].amount = 1;

        slotDatas[2].item = apple;
        slotDatas[2].amount = 5;

        slotDatas[3].item = meat;
        slotDatas[3].amount = 62;

        slotDatas[4].item = meat;
        slotDatas[4].amount = 2;

        slotDatas[5].item = meat;
        slotDatas[5].amount = 7;
    }

    public int AddItem(ItemData item, int amount)
    {
        // TODO: 구현부 추가
        return 0;
    }

    void RefreshAllSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slotDatas[i].IsEmpty())
                slots[i].SetEmpty();
            else
                slots[i].SetItem(slotDatas[i].item, slotDatas[i].amount);
        }
    }

    public int GetAmount(int index)
    {
        return slotDatas[index].amount;
    }

    public void OnClickSlot(int index)
    {
        contextMenuUI.Close();

        InventorySlotData clickedSlot = slotDatas[index];

        // 1. 아무것도 안들고있고, 클릭한 슬롯에 아이템이 있으면 집기
        if (!hasPickedItem)
        {
            if (!clickedSlot.IsEmpty())
            {
                pickedSlot.item = clickedSlot.item;
                pickedSlot.amount = clickedSlot.amount;
                pickedSlotIndex = index;

                clickedSlot.Clear();
                hasPickedItem = true;

                tooltipUI.Hide();
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
                hasPickedItem = false;
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
                        hasPickedItem = false;
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

                    hasPickedItem = true;
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

        InventorySlotData slot = slotDatas[index];
        if (slot.IsEmpty()) return;

        tooltipUI.Hide();
        contextMenuUI.Open(index, slot.item, Mouse.current.position.ReadValue(), this);
    }

    public void OnHoverSlot(int index)
    {
        if (hasPickedItem || contextMenuUI.gameObject.activeSelf)
        {
            tooltipUI.Hide();
            return;
        }

        InventorySlotData hoveredSlot = slotDatas[index];

        if (hoveredSlot.IsEmpty())
        {
            tooltipUI.Hide();
            return;
        }

        tooltipUI.Show(hoveredSlot.item);
        tooltipUI.FollowMouse();
    }

    public void OnExitSlot(int index)
    {
        tooltipUI.Hide();
    }

    public void HandleAction(int index)
    {
        InventorySlotData slot = slotDatas[index];
        if (slot.IsEmpty()) return;

        slot.amount -= 1;

        if (slot.amount <= 0)
        {
            slot.Clear();
            tooltipUI.Hide(); // 아이템이 사라졌으니 툴팁도 끔
        }

        RefreshAllSlots();
    }

    public void HandleSplit(int index)
    {
        InventorySlotData slot = slotDatas[index];
        if (slot.amount <= 1) return;

        int originAmount = slotDatas[index].amount;
        int splitAmount = slotDatas[index].amount / 2;

        pickedSlot.item = slot.item;
        pickedSlot.amount = splitAmount;
        hasPickedItem = true;

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

    void UpdateTooltipUI()
    {
        if (tooltipUI.gameObject.activeInHierarchy)
        {
            tooltipUI.FollowMouse();
        }
    }
}
