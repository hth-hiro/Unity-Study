public readonly struct QuickSlotViewData
{
    public ItemData Item { get; }
    public int Amount { get; }
    public bool IsEmpty { get; }

    public static QuickSlotViewData Empty => new QuickSlotViewData(null, 0);

    public QuickSlotViewData(ItemData item, int amount)
    {
        if (amount < 0) amount = 0;
        Item = item;
        Amount = amount;
        IsEmpty = (item == null || amount <= 0);
    }
}
