//using System;
//using Unity.VisualScripting.Antlr3.Runtime.Misc;
//using UnityEngine;
//using UnityEngine.Rendering;

//[System.Serializable]
//public class ItemContainer
//{
//    public ItemStack[] slotDatas;

//    public ItemContainer(int size)
//    {
//        slotDatas = new ItemStack[size];

//        for (int i = 0; i < size; i++)
//        {
//            slotDatas[i] = new ItemStack();
//        }
//    }

//    public void ClearSlot(int index) => slotDatas[index].Clear();

//    public int AddItem(ItemData item, int amount)
//    {
//        int remaining = amount;
//        if (item == null) return remaining;

//        if (item.maxStack > 1)
//        {
//            for (int i = 0; i < slotDatas.Length; i++)
//            {
//                if (!slotDatas[i].IsEmpty() && slotDatas[i].item == item)
//                {
//                    int canAdd = item.maxStack - slotDatas[i].amount;
//                    if (canAdd <= 0) continue;
//                    int addAmount = Mathf.Min(canAdd, remaining);

//                    slotDatas[i].amount += addAmount;
//                    remaining -= addAmount;

//                    if (remaining <= 0) break;
//                }
//            }
//        }

//        if (remaining > 0)
//        {
//            for (int i = 0; i < slotDatas.Length; i++)
//            {
//                if (i >= 36) continue;

//                if (slotDatas[i].IsEmpty())
//                {
//                    int addAmount = Mathf.Min(remaining, item.maxStack);

//                    slotDatas[i].item = item;
//                    slotDatas[i].amount = addAmount;

//                    remaining -= addAmount;

//                    if (remaining <= 0) break;
//                }
//            }
//        }

//        return remaining;   // 0 : 다 들어감, 숫자 : 남음.
//    }

//    public void Swap(int indexA, int indexB)
//    {
//        if (indexA < 0 || indexA >= slotDatas.Length || indexB < 0 || indexB >= slotDatas.Length)
//        {
//            Debug.LogWarning($"[Container] 잘못된 인덱스로 Swap 시도: {indexA}, {indexB}");
//            return;
//        }

//        ItemStack temp = new ItemStack();
//        temp.item = slotDatas[indexA].item;
//        temp.amount = slotDatas[indexA].amount;

//        slotDatas[indexA].item = slotDatas[indexB].item;
//        slotDatas[indexA].amount = slotDatas[indexB].amount;

//        slotDatas[indexB].item = temp.item;
//        slotDatas[indexB].amount = temp.amount;
//    }
//}
