using TMPro;
using UnityEngine;

public class ItemPopupView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _ItemNameT;
    [SerializeField] private TextMeshProUGUI _PriceOfOneT;
    [SerializeField] private TextMeshProUGUI _QuantityT;
    [SerializeField] private TextMeshProUGUI _TotalPriceT;

    [SerializeField] private TextMeshProUGUI _YesButtonT;

    public int _Mode;

    string _ItemName;
    int _PriceOfOne;
    int _Quantity;
    int _TotalPrice;

    public void Yes()
    {

    }

    public void Cancel()
    {
        Hide();
    }

    public void Plus()
    {
        _Quantity++;

        if(_Quantity > 100)
        {
            _Quantity = 100;
        }

        Refresh();
    }

    public void Minus()
    {
        _Quantity--;

        if (_Quantity < 1) { 
            _Quantity = 1;
        }

        Refresh();
    }

    public void Show(string itemName, int priceOfOne, int quantity = 1, int mode = 1)
    {
        _ItemName = itemName;
        _PriceOfOne = priceOfOne;
        _Quantity = quantity;
        _TotalPrice = _PriceOfOne * _Quantity;
        _Mode = mode;

        Refresh();

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    void Refresh()
    {
        _TotalPrice = _Quantity * _PriceOfOne;

        _ItemNameT.text = _ItemName;
        _PriceOfOneT.text = _PriceOfOne.ToString();
        _QuantityT.text = _Quantity.ToString();
        _TotalPriceT.text = _TotalPrice.ToString();


        if (_Mode == 2)
        {
            _YesButtonT.text = "sell";
        }
        else
            _YesButtonT.text = "buy";
    }
}
