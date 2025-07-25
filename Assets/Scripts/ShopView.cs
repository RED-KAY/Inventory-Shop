using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopView : MonoBehaviour
{
    [SerializeField] private GameObject m_ItemUIPrefab;
    [SerializeField] private Transform m_ShopContents;
    [SerializeField] private ItemPopupView m_ItemPopup;
    [SerializeField] Toggle[] m_Toggles;
    [SerializeField] Color m_DefaultColor;
    [SerializeField] Color m_ActiveColor;
    Button b;

    private ShopController m_Controller;
    int m_CurrentActive;

    private void Start()
    {
        foreach (var item in m_Toggles)
        {
            item.targetGraphic.color = m_DefaultColor;
        }
        m_Toggles[0].targetGraphic.color = m_ActiveColor;
    }

    public void SetController(ShopController controller)
    {
        m_Controller = controller;

        EventService.Instance.m_OnItemsAddedToInventory.AddListener(Refresh);
        EventService.Instance.m_OnItemsRemovedToInventory.AddListener(Refresh);
    }

    public void Populate()
    {
        var itemsToDisplay = m_Controller.GetItemsToDisplay();
        int maxWeight = GameController.Instance.GetMaxWeight();
        int weightAccumulation = GameController.Instance.GetWeightAccumulation();
        int delta = maxWeight - weightAccumulation;

        foreach (var item in itemsToDisplay)
        {
            GameObject newItemGO = Instantiate(m_ItemUIPrefab, m_ShopContents);
            ItemView newItem = newItemGO.GetComponent<ItemView>();
            newItem.m_Button.onClick.AddListener(() => OnItemSelected(item.Value));
            newItem.m_Icon.sprite = item.Value.m_Icon;
            newItem.m_Quantity.text = "$" + item.Value.m_Price.ToString();
            newItem.m_Rarity.sprite = GameController.Instance.m_Rarities[((int)item.Value.m_Rarity)];
            newItem.SetItem(item.Value, item.Value.m_Price, true);

            if (delta < item.Value.m_Weight) { 
                newItem.Disable();
            }
        }

    }

    public void Clear()
    {
        for (int i = 0; i < m_ShopContents.childCount; i++)
        {
            Destroy(m_ShopContents.GetChild(i).gameObject);
        }
    }

    public void Refresh()
    {
        Clear();
        Populate();
    }

    public void OnItemSelected(ItemEntry item)
    {
        m_ItemPopup?.Show(item.m_Id, item.m_Name, item.m_Price);
    }

    public void ChangeFilter(int filter)
    {
        if(m_Controller == null) return;
        m_Controller.FilterChanged(filter);

        m_Toggles[m_CurrentActive].targetGraphic.color = m_DefaultColor;
        m_Toggles[filter].targetGraphic.color = m_ActiveColor;

        m_CurrentActive = filter;
    }

    private void OnDisable()
    {
        EventService.Instance.m_OnItemsAddedToInventory.RemoveListener(Refresh);
        EventService.Instance.m_OnItemsRemovedToInventory.RemoveListener(Refresh);
    }
}
