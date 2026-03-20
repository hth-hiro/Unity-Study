using UnityEngine;

// 순수 데이터 클래스

public class InventoryData
{
    public ItemStack[] Slots;

    public InventoryData(int size)
    {
        Slots = new ItemStack[size];
        for (int i = 0; i < size; i++) Slots[i] = new ItemStack();
    }
}
