using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PickedItemUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text amountText;
    private RectTransform rectTransform;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetEmpty()
    {
        gameObject.SetActive(false);
    }

    public void SetItem(ItemData item, int amount)
    {
        if (item == null)
        {
            SetEmpty();
            return;
        }

        gameObject.SetActive(true);
        icon.sprite = item.icon;
        icon.enabled = true;
        amountText.text = amount > 1 ? amount.ToString() : "";
    }

    public void FollowMouse()
    {
        rectTransform.position = Mouse.current.position.ReadValue();
    }
}
