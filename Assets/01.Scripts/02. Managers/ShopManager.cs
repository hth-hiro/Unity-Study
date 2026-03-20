using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    [SerializeField] private GameObject m_shopUI;
    private ShopData m_shopData;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        m_shopData = new ShopData(9);
    }
    
    public void OnToggleShop(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if (m_shopUI == null) return;

        bool nextState = !m_shopUI.activeSelf;
        m_shopUI.SetActive(nextState);

        PlayerController.Instance?.SetInputBlock(nextState);

        if (nextState)
        {
            ShopUI.Instance?.Refresh();
        }
    }

    public void ProcessPurchase(ItemData item, int amount)
    {
        if (item == null) return;

        int totalPrice = item.buyPrice * amount;

        if (!CurrencyManager.Instance.ConsumeGold(totalPrice))
        {
            Debug.Log("골드가 부족합니다.");
            ShopUI.Instance?.Refresh();
            return;
        }

        int remaining = InventoryManager.Instance.AddItem(item, amount);

        if (remaining == 0)
        {
            Debug.Log("구매 성공 !");
        }
        else
        {
            int refundPrice = remaining * item.buyPrice;

            CurrencyManager.Instance.AddGold(refundPrice);
        }

        ShopUI.Instance?.Refresh();
        InventoryUI.Instance?.RequestRefresh();
    }

    public List<ItemStack> GetShopItems()
    {
        return new List<ItemStack>(m_shopData.Slots);
    }
    
    public ItemStack GetShopItem(int index)
    {
        if (index < 0 || index >= m_shopData.Slots.Length) return null;

        return m_shopData.Slots[index];
    }
}
