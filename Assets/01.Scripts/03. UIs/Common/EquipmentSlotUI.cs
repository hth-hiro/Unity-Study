using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlotUI : BaseSlot
{
    public EquipType allowedType;

    public override void SetItem(ItemData item, int amount)
    {
        base.SetItem(item, amount);
    }

    public override void SetEmpty()
    {
        base.SetEmpty();
    }

    public bool CanEquip(ItemData item)
    {
        return item != null && item.equipType == allowedType;
    }
}