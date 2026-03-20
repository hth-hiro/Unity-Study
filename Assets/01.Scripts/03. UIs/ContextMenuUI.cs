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

        // 2. 버튼 활성화 여부 결정 (Vertical Layout Group이 있다면 알아서 정렬됩니다)
        bool isIngredient = item.itemType == ItemType.Ingredient;
        bool canSplit = currentHandler.GetAmount(index) > 1;

        // [사용/장착] 버튼: 재료가 아닐 때만 켬
        actionText.transform.parent.gameObject.SetActive(!isIngredient);
        if (!isIngredient)
            actionText.text = item.itemType == ItemType.Equipment ? "장착" : "사용";

        // [분리] 버튼: 수량이 1개보다 많을 때만 켬
        splitButton.SetActive(canSplit);

        // 3. 만약 버튼이 하나도 켜지지 않는다면
        if (isIngredient && !canSplit)
        {
            Close(); // 닫기
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
        
    // 버튼 클릭 이벤트
    public void OnClickAction()
    {
        currentHandler.HandleAction(targetIndex);
        Close();
    }

    // 2. 분리
    public void OnClickSplit()
    {
        currentHandler.HandleSplit(targetIndex);
        Close();
    }
}
