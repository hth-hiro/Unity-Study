using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class TooltipUI : MonoBehaviour
{
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Vector2 offset = new Vector2(25f, -25f);

    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        Hide();
    }

    public void Show(ItemData item)
    {
        if (item == null) return;

        var cg = GetComponent<CanvasGroup>();
        cg.alpha = 1;

        itemNameText.text = item.itemName;
        descriptionText.text = item.description;

        FollowMouse();
    }

    public void Hide()
    {
        var cg = GetComponent<CanvasGroup>();
        cg.alpha = 0;
    }

    public void FollowMouse()
    {
        if (Mouse.current == null)
            return;

        rectTransform.position = Mouse.current.position.ReadValue() + offset;
    }
}
