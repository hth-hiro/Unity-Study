using UnityEngine;
using UnityEngine.UI;

public class ContextMenuManager : MonoBehaviour
{
    public static ContextMenuManager Instance { get; private set; }
    [SerializeField] private ContextMenuUI contextMenuUI;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    
    public void Open(int slotIndex, ItemData item, Vector2 position, InventoryPanel panel)
    {
        if (contextMenuUI != null)
        {
            contextMenuUI.Open(slotIndex, item, position, panel);
        }
    }

    public void Close()
    {
        if (contextMenuUI != null)
        {
            contextMenuUI.Close();
        }
    }

    public bool IsActive()
    {
        return contextMenuUI != null && contextMenuUI.gameObject.activeSelf;
    }
}
