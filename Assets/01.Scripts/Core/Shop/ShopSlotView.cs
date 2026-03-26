using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlotView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image m_iconImage;
    [SerializeField] private TextMeshProUGUI m_amountText;
    [SerializeField] private TextMeshProUGUI m_priceText;
    [SerializeField] private GameObject m_emptyRoot;

    private ShopSlotViewData m_currentData;

    public ShopSlotViewData CurrentData => m_currentData;
    
    public void SetData(ShopSlotViewData data)
    {
        m_currentData = data;

        if (data.IsEmpty)
        {
            SetEmpty();
            return;
        }

        ItemData item = data.Item;

        if (m_iconImage != null)
        {
            m_iconImage.sprite = item.Icon;
            m_iconImage.gameObject.SetActive(item.Icon != null);
        }

        if (m_priceText != null)
        {
            m_priceText.text = item.BuyPrice.ToString("N0");
        }

        if (m_emptyRoot != null)
        {
            m_emptyRoot.gameObject.SetActive(false);
        }
    }

    public void SetEmpty()
    {
        m_currentData = ShopSlotViewData.Empty;

        if (m_iconImage != null)
        {
            m_iconImage.sprite = null;
            m_iconImage.gameObject.SetActive(false);
        }

        if (m_amountText != null)
        {
            m_amountText.text = string.Empty;
        }

        if (m_priceText !=null)
        {
            m_priceText.text = string.Empty;
        }

        if (m_emptyRoot != null)
        {
            m_emptyRoot.gameObject.SetActive(true);
        }
    }

    public void Clear()
    {
        SetEmpty();
    }
}
