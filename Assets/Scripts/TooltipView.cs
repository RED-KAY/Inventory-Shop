using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipView : MonoBehaviour
{
    [SerializeField] Image m_Icon;
    [SerializeField] TextMeshProUGUI m_NameT;
    [SerializeField] TextMeshProUGUI m_Description;
    [SerializeField] TextMeshProUGUI m_Price;
    [SerializeField] TextMeshProUGUI m_PriceLabel;
    [SerializeField] Image m_CoinIcon;
    [SerializeField] TextMeshProUGUI m_SellingPrice;
    [SerializeField] TextMeshProUGUI m_Rarity;
    [SerializeField] TextMeshProUGUI m_Weight;

    public void SetInfo(ItemEntry itemEntry, bool isShop = true, int number = 0)
    {
        m_Icon.sprite = itemEntry.m_Icon;
        m_NameT.text = itemEntry.m_Name;
        m_Description.text = itemEntry.m_Description;
        m_Price.text = isShop ? itemEntry.m_Price.ToString() : number.ToString();
        m_PriceLabel.text = isShop ? "price: " : "quantity: ";
        m_SellingPrice.text = itemEntry.m_SellingPrice.ToString();
        m_Rarity.text = "rarity: " + itemEntry.m_Rarity.ToString();
        m_Weight.text = "weight: " + itemEntry.m_Weight.ToString();

        m_CoinIcon.gameObject.SetActive(isShop);
    }
}
