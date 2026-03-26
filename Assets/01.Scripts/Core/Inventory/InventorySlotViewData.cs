using UnityEngine;

public readonly struct InventorySlotViewData
{
    public ItemData Item { get; }
    public int Amount { get; }
    public bool IsEmpty { get; }

    public InventorySlotViewData(ItemData item, int amount)
    {
        Item = item;
        Amount = amount;
        IsEmpty = item == null;
    }

    public static InventorySlotViewData Empty => new InventorySlotViewData(null, 0);
}
