//using System;
//using System.Xml.Serialization;
//using UnityEngine;
//using UnityEngine.InputSystem;

//// 전체 패널 관리

//public class InventoryUI : MonoBehaviour, ISlotHandler
//{
//    public static InventoryUI Instance { get; private set; }
//    private BaseSlot[] m_uiSlots;

//    [SerializeField] private PickedItemUI pickedItemUI;

//    private void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else { Destroy(gameObject); return; }

//        m_uiSlots = GetComponentsInChildren<BaseSlot>(true);
//        InitializeSlots();
//    }

//    private void OnDestroy()
//    {
//        if (Instance == this) Instance = null;
//    }

//    private void InitializeSlots()
//    {
//        for (int i = 0; i < m_uiSlots.Length; i++)
//        {
//            m_uiSlots[i].Initialize(this, i);
//        }
//    }

//    private void OnEnable() => RequestRefresh();

//    public void RequestRefresh()
//    {
//        if (!gameObject.activeInHierarchy) return;

//        var dataSlots = InventoryManager.Instance.Data.Slots;
//        for (int i = 0; i < m_uiSlots.Length; i++)
//        {
//            if (i < dataSlots.Length)
//            {
//                if (dataSlots[i].IsEmpty()) m_uiSlots[i].SetEmpty();
//                else m_uiSlots[i].SetItem(dataSlots[i].item, dataSlots[i].amount);
//            }
//        }
//    }

//    public void SetActive()
//    {
//        gameObject.SetActive(false);
//    }

//    void Update()
//    {
//        if (InventoryManager.Instance.HasPickedItem)
//        {
//            var picked = InventoryManager.Instance.PickedSlot;
//            pickedItemUI.SetItem(picked.item, picked.amount);
//            pickedItemUI.FollowMouse();
//        }
//        else
//        {
//            pickedItemUI.SetEmpty();
//        }
//    }

//    public void ClearPickedItemUI()
//    {
//        pickedItemUI.SetEmpty();
//    }

//    public void OnClickSlot(int index)
//    {
//        InventoryManager.Instance.HandleLeftClick(index);
//    }

//    public void OnRightClickSlot(int index)
//    {
//        InventoryManager.Instance.HandleRightClick(index);
//    }

//    public void OnHoverSlot(int index)
//    {
//        var slot = InventoryManager.Instance.Data.Slots[index];
//        if (slot != null && !slot.IsEmpty())
//        {
//            TooltipManager.Instance?.Show(slot.item);
//        }
//    }

//    public void OnExitSlot(int index)
//    {
//        TooltipManager.Instance?.Hide();
//    }

//    public int GetAmount(int index)
//    { 
//        var amount = InventoryManager.Instance.Data.Slots[index].amount;
//        return amount;
//    }
    
//    public void HandleAction(int index)
//    {
//        InventoryManager.Instance.HandleAction(index);
//    }

//    public void HandleSplit(int index)
//    {
//        InventoryManager.Instance.SplitItem(index);
//    }
//}
