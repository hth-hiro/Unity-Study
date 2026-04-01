using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ContextMenuUI : MonoBehaviour
{
    [SerializeField] private TMP_Text actionText;
    [SerializeField] private GameObject actionButton;
    [SerializeField] private GameObject splitButton;

    private ISlotHandler currentHandler;
    private int targetIndex;

    public void Open(int index, ItemData item, Vector2 position, ISlotHandler handler)
    {
        var cg = GetComponent<CanvasGroup>();
        cg.alpha = 1;

        targetIndex = index;
        currentHandler = handler;

        gameObject.SetActive(true);
        transform.position = position;
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
        
    // 버튼 클릭 이벤트
    public void OnClickAction()
    {
        if (currentHandler == null) return;

        currentHandler.HandleAction(targetIndex);
        Close();
    }

    // 2. 분리
    public void OnClickSplit()
    {
        if (currentHandler == null) return;
        currentHandler.HandleSplit(targetIndex);
        Close();
    }
}
