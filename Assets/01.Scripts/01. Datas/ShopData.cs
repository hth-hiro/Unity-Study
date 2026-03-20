using UnityEngine;

// 상점 아이템 및 재고 데이터

public class ShopData
{
    public ItemStack[] Slots;

    public ShopData(int size)
    {
        Slots = new ItemStack[size];
        for (int i = 0; i < size; i++) Slots[i] = new ItemStack();
    }
}
