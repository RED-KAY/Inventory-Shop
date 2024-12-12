using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipView : MonoBehaviour
{
    [SerializeField] Image _Icon;
    [SerializeField] TextMeshProUGUI _Name;
    [SerializeField] TextMeshProUGUI _Description;
    [SerializeField] TextMeshProUGUI _Price;
    [SerializeField] TextMeshProUGUI _PriceLabel;
    [SerializeField] Image _CoinIcon;
    [SerializeField] TextMeshProUGUI _SellingPrice;
    [SerializeField] TextMeshProUGUI _Rarity;

    public void SetInfo(ItemEntry itemEntry, bool isShop = true, int number = 0)
    {
        _Icon.sprite = itemEntry._Icon;
        _Name.text = itemEntry._Name;
        _Description.text = itemEntry._Description;
        _Price.text = isShop ? itemEntry._Price.ToString() : number.ToString();
        _PriceLabel.text = isShop ? "price: " : "quantity: ";
        _SellingPrice.text = itemEntry._SellingPrice.ToString();
        _Rarity.text = "rarity: " + itemEntry._Rarity.ToString();

        _CoinIcon.gameObject.SetActive(isShop);
    }
}
