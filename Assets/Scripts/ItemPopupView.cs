using TMPro;
using UnityEngine;

public class ItemPopupView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_ItemNameT;
    [SerializeField] private TextMeshProUGUI m_PriceOfOneT;
    [SerializeField] private TextMeshProUGUI m_QuantityT;
    [SerializeField] private TextMeshProUGUI m_TotalPriceT;

    [SerializeField] private TextMeshProUGUI m_YesButtonT;

    public int m_Mode;

    string m_Id;
    string m_ItemName;
    int m_PriceOfOne;
    int m_Quantity, m_MaxQuantity;
    int m_TotalPrice;

    public void Yes()
    {
        if (m_Mode == 2)
        {
            //Sell
            if (GameController.Instance.CanSell(m_Id, m_Quantity)) {
                Debug.Log(m_Quantity + " " + m_ItemName + "(s) Sold!");
                EventService.Instance.m_OnItemSold?.InvokeEvent(m_Id, m_Quantity);
            }

        }
        else
        {
            //Buy

            if (PlayerWallet.Instance.TryTransaction(m_TotalPrice))
            {
                Debug.Log(m_Quantity + " " + m_ItemName + "(s) Bought!");
                EventService.Instance.m_OnItemBought?.InvokeEvent(m_Id, m_Quantity);
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
        m_Quantity++;

        if(m_Quantity > m_MaxQuantity)
        {
            m_Quantity = m_MaxQuantity;
        }

        Refresh();
    }

    public void Minus()
    {
        m_Quantity--;

        if (m_Quantity < 1) { 
            m_Quantity = 1;
        }

        Refresh();
    }

    public void Show(string id, string itemName, int priceOfOne, int quantity = 1, int mode = 1)
    {
        m_Id = id;
        m_ItemName = itemName;
        m_PriceOfOne = priceOfOne;
        m_Quantity = quantity;
        m_Mode = mode;
        m_TotalPrice = m_PriceOfOne * m_Quantity;

        if (m_Mode == 2) {
            m_MaxQuantity = quantity;
        }
        else
        {
            m_MaxQuantity = 100;
        }

        Refresh();

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    void Refresh()
    {
        m_TotalPrice = m_Quantity * m_PriceOfOne;

        m_ItemNameT.text = m_ItemName;
        m_PriceOfOneT.text = m_PriceOfOne.ToString();
        m_QuantityT.text = m_Quantity.ToString();
        m_TotalPriceT.text = m_TotalPrice.ToString();


        if (m_Mode == 2)
        {
            m_YesButtonT.text = "sell";
        }
        else
            m_YesButtonT.text = "buy";
    }
}
