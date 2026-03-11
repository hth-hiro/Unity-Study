using System.Xml.Serialization;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySlotUI[] slots;

    [Header("Test Items")]
    [SerializeField] private ItemData apple;
    [SerializeField] private ItemData potion;
    [SerializeField] private ItemData sword;

    [SerializeField] private PickedItemUI pickedItemUI;
    [SerializeField] private TooltipUI tooltipUI;

    private InventorySlotData[] slotDatas;

    private InventorySlotData pickedSlot = new InventorySlotData();
    private bool hasPickedItem = false;

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

        slotDatas[1].item = apple;
        slotDatas[1].amount = 63;

        slotDatas[2].item = apple;
        slotDatas[2].amount = 5;
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

    public void OnClickSlot(int index)
    {
        InventorySlotData clickedSlot = slotDatas[index];

        // 1. 아무것도 안들고있고, 클릭한 슬롯에 아이템이 있으면 집기
        if (!hasPickedItem)
        {
            if (!clickedSlot.IsEmpty())
            {
                pickedSlot.item = clickedSlot.item;
                pickedSlot.amount = clickedSlot.amount;

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
            }
            else
            {
                // 3. 같은 아이템이면 합치기
                if (clickedSlot.item == pickedSlot.item)
                {
                    int maxStack = clickedSlot.item.maxStack;
                    int totalAmount = clickedSlot.amount + pickedSlot.amount;

                    if (totalAmount < maxStack)
                    {
                        clickedSlot.amount = totalAmount;
                        pickedSlot.Clear();
                        hasPickedItem = false;
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
                }
            }

            if (!hasPickedItem)
            {
                OnHoverSlot(index);
            }
        }

        RefreshAllSlots();
    }

    public void OnHoverSlot(int index)
    {
        if (hasPickedItem) return;

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
