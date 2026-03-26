using UnityEngine;

public readonly struct ShopSlotViewData
{
    public ItemData Item { get; }
    public bool IsEmpty { get; }

    public ShopSlotViewData(ItemData item)
    {
        Item = item;
        IsEmpty = item == null;
    }

    public static ShopSlotViewData Empty =>
        new ShopSlotViewData(null);
}