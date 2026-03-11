using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class TooltipUI : MonoBehaviour
{
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Vector2 offset = new Vector2(20f, -20f);

    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        Hide();
    }

    public void Show(ItemData item)
    {
        if (item == null)
        {
            Hide();
            return;
        }

        gameObject.SetActive(true);
        itemNameText.text = item.itemName;
        descriptionText.text = item.description;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void FollowMouse()
    {
        if (Mouse.current == null)
            return;

        rectTransform.position = Mouse.current.position.ReadValue() + offset;
    }
}
