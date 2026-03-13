using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.GridLayoutGroup;

public class BaseSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Visual Elements")]
    [SerializeField] protected Image iconImage;
    [SerializeField] protected TextMeshProUGUI amountText;
    [SerializeField] protected GameObject amountGo;

    protected ItemData currentItem;
    protected int currentAmount;

    protected ISlotHandler owner;
    protected int index;

    public bool IsEmpty => currentItem == null;

    public virtual void SetItem(ItemData item, int amount)
    {
        currentItem = item;
        currentAmount = amount;

        if (item == null || amount <= 0)
        {
            SetEmpty();
            return;
        }

        iconImage.sprite = item.icon;
        iconImage.gameObject.SetActive(true);

        if (amount > 1)
        {
            amountGo.SetActive(true);
            amountText.text = amount.ToString();
        }
        else
        {
            amountGo.SetActive(false);
        }
    }

    public virtual void SetEmpty()
    {
        currentItem = null;
        currentAmount = 0;
        iconImage.gameObject.SetActive(false);
        amountGo.SetActive(false);
    }

    public virtual void Initialize(ISlotHandler handler, int slotIndex)
    {
        owner = handler;
        index = slotIndex;
    }

    // 모든 슬롯 공통 클릭 처리
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            owner.OnClickSlot(index);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            owner.OnRightClickSlot(index);
        }
    }

    // 모든 슬롯 공통 호버 처리
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        owner.OnHoverSlot(index);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        owner.OnExitSlot(index);
    }
}
