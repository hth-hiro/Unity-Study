using UnityEngine;

// 어떤 아이템이 몇 개 있는지 정보를 담는 스크립트.

[System.Serializable]
public class ItemStack
{
    public ItemData item;
    public int amount;

    public ItemStack() { }

    public ItemStack(ItemData item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }

    public bool IsEmpty()
    {
        return item == null || amount <= 0;
    }

    public void Clear()
    {
        item = null; 
        amount = 0;
    }

    public bool Add(int count)
    {
        if (item == null) return false;
        amount += count;
        return true;
    }

    public void Remove(int count)
    {
        amount -= count;
        if (amount <= 0) Clear();
    }
}
