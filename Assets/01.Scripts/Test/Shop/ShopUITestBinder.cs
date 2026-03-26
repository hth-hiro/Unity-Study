using System.Collections.Generic;
using UnityEngine;

public class ShopUITestBinder : MonoBehaviour
{
    [SerializeField] private ShopUI m_shopUI;
    [SerializeField] private ItemData[] m_charmItems;
    [SerializeField] private ItemData[] m_grimoireItems;
    [SerializeField] private ItemData m_detailItem;
    [SerializeField] private string m_dialogue;
    [SerializeField] private int m_currency;

    private void Start()
    {
        if (m_shopUI == null) return;

        m_shopUI.SetDialogue(m_dialogue);
        m_shopUI.SetCurrency(m_currency);
        m_shopUI.SetDetail(m_detailItem);

        var charmViews = new List<ShopSlotViewData>();
        if (m_charmItems != null)
        {
            foreach (var item in m_charmItems)
            {
                charmViews.Add(new ShopSlotViewData(item)); // Assuming 1 amount
            }
        }
        m_shopUI.SetCharmItems(charmViews);

        var grimoireViews = new List<ShopSlotViewData>();
        if (m_grimoireItems != null)
        {
            foreach (var item in m_grimoireItems)
            {
                grimoireViews.Add(new ShopSlotViewData(item));
            }
        }
        m_shopUI.SetGrimoireItems(grimoireViews);
    }
}
