using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour, ISlotHandler
{
    public static ShopUI Instance { get; private set; }

    private int m_selectedIndex = -1;
    public int SelectedIndex => m_selectedIndex;

    [SerializeField] private ShopSlot[] m_shopSlots;
    [SerializeField] private TMPro.TextMeshProUGUI m_currencyText;  // 보유 금액 표시용
    [SerializeField] private Button m_buyButton;

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); return; }

        m_shopSlots = GetComponentsInChildren<ShopSlot>(true);

        for (int i = 0; i < m_shopSlots.Length; i++) 
        {
            m_shopSlots[i].Initialize(this, i);
        }
    }

    private void Start()
    {
        if (m_buyButton !=null)
        {
            m_buyButton.onClick.RemoveAllListeners();
            m_buyButton.onClick.AddListener(OnClickBuyButton);
        }
    }

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (m_currencyText != null)
        {
            m_currencyText.text = CurrencyManager.Instance.Gold.ToString();
        }

        if (ShopManager.Instance == null) return;

        List<ItemStack> shopItems = ShopManager.Instance.GetShopItems();

        for (int i = 0; i < m_shopSlots.Length; i++)
        {
            if (i < shopItems.Count && shopItems[i] != null && !shopItems[i].IsEmpty())
            {
                m_shopSlots[i].SetItem(shopItems[i].item, shopItems[i].amount);
            }
            else
            {
                m_shopSlots[i].SetEmpty();
            }
        }
    }

    public void HandleAction(int index)
    {
        //
    }

    public void HandleSplit(int index)
    {
        // 
    }

    public void OnClickSlot(int index)
    {
        var slot = ShopManager.Instance.GetShopItem(index);
        if (slot == null || slot.amount <= 0)
        {
            m_selectedIndex = -1;
        }
        else
        {
            m_selectedIndex = index;
        }

        Refresh();
    }

    public void OnRightClickSlot(int index)
    {
        // 필요하면 나중에 다중 구매, 정보창 등으로 확장
    }

    public void OnHoverSlot(int index)
    {
        var slot = ShopManager.Instance.GetShopItem(index);
        if (slot != null && !slot.IsEmpty())
        {
            TooltipManager.Instance?.Show(slot.item);
        }
    }

    public void OnExitSlot(int index)
    {
        TooltipManager.Instance?.Hide();
    }

    public int GetAmount(int index)
    {
        var slot = ShopManager.Instance.GetShopItem(index);
        return slot == null ? 0 : slot.amount;
    }

    private void OnClickBuyButton()
    {
        if (m_selectedIndex < 0) return;

        var slot = ShopManager.Instance.GetShopItem(m_selectedIndex);
        if (slot == null || slot.IsEmpty()) return;

        ShopManager.Instance.ProcessPurchase(slot.item, 1);

        var refreshedSlot = ShopManager.Instance.GetShopItem(m_selectedIndex);
        if (refreshedSlot == null || refreshedSlot.IsEmpty())
        {
            m_selectedIndex = -1;
        }

        Refresh();
    }
}
