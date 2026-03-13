using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private BaseSlot[] slots;

    [Header("Test Items")]
    [SerializeField] private ItemData apple;
    [SerializeField] private ItemData potion;
    [SerializeField] private ItemData sword;
    [SerializeField] private ItemData meat;
    [SerializeField] private ItemData chestplate;

    [SerializeField] private PickedItemUI pickedItemUI;
    [SerializeField] private TooltipUI tooltipUI;
    [SerializeField] private ContextMenuUI contextMenuUI;

    private InventorySlotData[] slotDatas;

    private InventorySlotData pickedSlot = new InventorySlotData();
    private bool hasPickedItem = false;
    private int pickedSlotIndex = -1;

    void Awake()
    {
        slots = GetComponentsInChildren<BaseSlot>();

        for (int i = 0; i < slots.Length; i++)
        {
            Debug.Log($"슬롯 {i}번: {slots[i].name}");
        }

        Initialize();
        InitializeSlotUIs();
    }

    void Start()
    {
        TestAddItems();
        RefreshAllSlots();
    }

    void OnDisable()
    {
        if (tooltipUI != null) tooltipUI.Hide();
        if (contextMenuUI != null) contextMenuUI.Close();
        if (pickedItemUI != null) pickedItemUI.SetEmpty();

        if (slots == null || slotDatas == null) return;

        if (hasPickedItem && !pickedSlot.IsEmpty())
        {
            for (int i = 0; i < slotDatas.Length; i++)
            {
                if (!slotDatas[i].IsEmpty() && slotDatas[i].item == pickedSlot.item)
                {
                    int maxStack = slotDatas[i].item.maxStack;
                    int canAdd = maxStack - slotDatas[i].amount;

                    if (canAdd > 0)
                    {
                        int addAmount = Mathf.Min(canAdd, pickedSlot.amount);
                        slotDatas[i].amount += addAmount;
                        pickedSlot.amount -= addAmount;

                        if (pickedSlot.IsEmpty()) break;
                    }
                }
            }

            if (!pickedSlot.IsEmpty())
            {
                if (pickedSlotIndex != -1 && slotDatas[pickedSlotIndex].IsEmpty())
                {
                    slotDatas[pickedSlotIndex].item = pickedSlot.item;
                    slotDatas[pickedSlotIndex].amount = pickedSlot.amount;
                    pickedSlot.Clear();
                }
                else
                {
                    for (int i = 0; i < slotDatas.Length; i++)
                    {
                        if (slotDatas[i].IsEmpty())
                        {
                            slotDatas[i].item = pickedSlot.item;
                            slotDatas[i].amount = pickedSlot.amount;
                            pickedSlot.Clear();
                            break;
                        }
                    }
                }
            }

            if (!pickedSlot.IsEmpty())
            {
                // TODO : 아이템 생성로직
            }
        }

        pickedSlot.Clear();
        hasPickedItem = false;
        pickedSlotIndex = -1;
        RefreshAllSlots();
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

        slotDatas[6].item = chestplate;
        slotDatas[6].amount = 1;
    }

    public int AddItem(ItemData item, int amount)
    {
        // TODO: 구현부 추가
        return 0;
    }

    private void EquipItem(int index)
    {
        ItemData itemToEquip = slotDatas[index].item;

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

    private void SwapItems(int index, int i)
    {
        InventorySlotData temp = new InventorySlotData();
        temp.item = slotDatas[index].item;
        temp.amount = slotDatas[index].amount;

        slotDatas[index].item = slotDatas[i].item;
        slotDatas[index].amount = slotDatas[i].amount;

        slotDatas[i].item = temp.item;
        slotDatas[i].amount = temp.amount;

        RefreshAllSlots();
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
        Debug.Log($"{index}번 슬롯 클릭됨!");

        InventorySlotData clickedSlot = slotDatas[index];
        BaseSlot targetSlotUI = slots[index];

        if (hasPickedItem)
        {
            if (targetSlotUI is EquipmentSlotUI equipSlot)
            {
                Debug.Log($"장착 슬롯 클릭됨! 아이템 타입: {pickedSlot.item.equipType}, 슬롯 허용 타입: {equipSlot.allowedType}");
                if (!equipSlot.CanEquip(pickedSlot.item))
                {
                    Debug.Log("CanEquip에서 거부됨");
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

        if (slot.item.itemType == ItemType.Equipment)
        {
            EquipItem(index);
            return;
        }

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
