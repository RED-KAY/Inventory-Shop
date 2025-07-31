using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryController
{
    InventoryModel m_Model;
    InventoryView m_View;

    public InventoryController(InventoryModel m, InventoryView v)
    {
        m_Model = m;
        m_View = v;
    }

    ~InventoryController()
    {
        EventService.Instance.m_OnItemBought.RemoveListener(OnItemsBought);
        EventService.Instance.m_OnItemSold.RemoveListener(OnItemSold);
        EventService.Instance.m_TryAddItems.RemoveListener(TryAddItems);
        EventService.Instance.m_OnItemsAddedToInventory.RemoveListener(Refresh);
        EventService.Instance.m_OnItemsRemovedToInventory.RemoveListener(Refresh);
    }

    public void Initialize()
    {
        m_Model.SetController(this);
        m_View.SetController(this);

        PopulateShop();

        EventService.Instance.m_OnItemBought.AddListener(OnItemsBought);
        EventService.Instance.m_OnItemSold.AddListener(OnItemSold);
        EventService.Instance.m_TryAddItems.AddListener(TryAddItems);
        EventService.Instance.m_OnItemsAddedToInventory.AddListener(Refresh);
        EventService.Instance.m_OnItemsRemovedToInventory.AddListener(Refresh);
    }

    public Dictionary<string, Item> GetAllItems()
    {
        return m_Model.Items;
    }

    public Dictionary<string, Item> GetItemsToDisplay()
    {
        if (m_Model.m_Filter <= 0 || m_Model.m_Filter > 4)
        {
            m_Model.m_Filter = 0;

            var filteredItems = m_Model.Items.Values
                .Where(item => item.m_Amount > 0)
                    .ToDictionary(item => item.Details.m_Id, item => item);

            return filteredItems;
        }
        else
        {
            var filteredItems = m_Model.Items.Values
                    .Where(item => item.m_Amount > 0 && item.Details.m_ItemType == (ItemType)m_Model.m_Filter)
                        .ToDictionary(item => item.Details.m_Id, item => item);
            return filteredItems;
        }
    }

    private void OnItemsBought(string id, int quantity)
    {
        ItemsAddInfo[] itemsAddInfos = new ItemsAddInfo[1];
        itemsAddInfos[0].m_Id = id;
        itemsAddInfos[0].m_Quantity = quantity;
        m_Model.TryAddItems(itemsAddInfos);

        Refresh();
    }

    private void OnItemSold(string id, int quantity) { 
    
        m_Model.RemoveItem(id, quantity);
        Refresh();

        EventService.Instance.m_OnItemSoldAddMoney?.InvokeEvent(quantity * m_Model.Items[id].Details.m_SellingPrice);
    }

    ItemsAddInfoResult[] TryAddItems(ItemsAddInfo[] itemsToAdd)
    {
        return m_Model.TryAddItems(itemsToAdd);
    }

    public void PopulateShop()
    {
        m_View.Refresh();
    }


    public void Refresh()
    {
        m_View.Refresh();
    }

    internal void FilterChanged(int filter)
    {
        m_Model.m_Filter = filter;
        PopulateShop();
    }

    public bool CanSell(string id, int quantity)
    {
        return m_Model.Items[id].m_Amount >= quantity;
    }

    public int MaxWeight()
    {
        return m_Model.MaxWeight;
    }

    public int WeightAccumulation()
    {
        return m_Model.WeightAccumulation;
    }
}

