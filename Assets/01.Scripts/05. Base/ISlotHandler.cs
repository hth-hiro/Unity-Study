using UnityEngine;

public interface ISlotHandler
{
    void OnClickSlot(int index);
    void OnRightClickSlot(int index);
    void OnHoverSlot(int index);
    void OnExitSlot(int index);

    // 데이터 확인 및 명령 관련 (ContextMenu에서 사용)
    int GetAmount(int index);
    void HandleAction(int index);
    void HandleSplit(int index);
}
