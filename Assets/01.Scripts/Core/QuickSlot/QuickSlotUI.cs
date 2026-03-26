using System.Collections.Generic;
using UnityEngine;

public class QuickSlotUI : MonoBehaviour
{
    [SerializeField] private QuickSlotSlotView[] m_quickSlots;

    public void SetItems(IReadOnlyList<QuickSlotViewData> items)
    {
        if (m_quickSlots == null) return;

        for (int i = 0; i < m_quickSlots.Length; i++)
        {
            if (items != null && i < items.Count)
            {
                m_quickSlots[i].SetData(items[i]);
            }
            else
            {
                m_quickSlots[i].SetEmpty();
            }
        }
    }

    public void Clear()
    {
        if (m_quickSlots == null) return;

        for (int i = 0; i < m_quickSlots.Length; i++)
        {
            m_quickSlots[i].Clear();
        }
    }
}
