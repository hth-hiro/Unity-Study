using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BaseSlot : MonoBehaviour
{
    [Header("Visual Elements")]
    [SerializeField] protected Image iconImage;
    [SerializeField] protected TextMeshProUGUI amountText;
    [SerializeField] protected GameObject amountGo;

    protected ItemData currentItem;
    protected int currentAmount;

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
}
