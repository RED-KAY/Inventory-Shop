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

    string _Id;
    string _ItemName;
    int _PriceOfOne;
    int _Quantity, _MaxQuantity;
    int _TotalPrice;

    public void Yes()
    {
        if (_Mode == 2)
        {
            //Sell
            if (GameController.Instance.CanSell(_Id, _Quantity)) {
                Debug.Log(_Quantity + " " + _ItemName + "(s) Sold!");
                EventService.Instance._OnItemSold?.InvokeEvent(_Id, _Quantity);
            }

        }
        else
        {
            //Buy

            if (PlayerWallet.Instance.TryTransaction(_TotalPrice))
            {
                Debug.Log(_Quantity + " " + _ItemName + "(s) Bought!");
                EventService.Instance._OnItemBought?.InvokeEvent(_Id, _Quantity);
                ItemsAddInfo[] itemsToAdd = new ItemsAddInfo[1];
                itemsToAdd[0]._Id = _Id;
                itemsToAdd[0]._Quantity = _Quantity;
                ItemsAddInfoResult[] result = EventService.Instance._TryAddItems?.Invoke(itemsToAdd);

                if (result != null)
                {
                
                }
            }
        }

        Hide();
    }

    public void Cancel()
    {
        Hide();
    }

    public void Plus()
    {
        _Quantity++;

        if(_Quantity > _MaxQuantity)
        {
            _Quantity = _MaxQuantity;
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

    public void Show(string id, string itemName, int priceOfOne, int quantity = 1, int mode = 1)
    {
        _Id = id;
        _ItemName = itemName;
        _PriceOfOne = priceOfOne;
        _Quantity = quantity;

        if (_Mode == 2) {
            _MaxQuantity = quantity;
        }
        else
        {
            _MaxQuantity = 100;
        }
        
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
