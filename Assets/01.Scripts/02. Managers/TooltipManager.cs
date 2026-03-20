using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance { get; private set; }

    [SerializeField] private TooltipUI tooltipUI;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Show(ItemData item)
    {
        if (tooltipUI != null)
        {
            tooltipUI.Show(item);
            tooltipUI.FollowMouse();
        }
    }

    public void Hide()
    {
        if (tooltipUI != null)
        {
            tooltipUI.Hide();
        }
    }

    void Update()
    {
        var cg = tooltipUI.GetComponent<CanvasGroup>();

        if (cg != null && cg.alpha > 0)
        {
            tooltipUI.FollowMouse();
        }
    }
}
