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

    public void OnItemSelected(string id)
    {

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

    public void Buy(string id, int amount)
    {

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

[Serializable]
public class InventoryModel
{
    [SerializeField] private Dictionary<string, Item> m_Items;
    public Dictionary<string, Item> Items { get { return m_Items; } }
    private InventoryController m_Controller;
    public int m_Filter = 0; //0: all, 1: materials, 2: weapons, 3: consumables, 4: trasures
    private int m_MaxWeight;
    public int MaxWeight { get { return m_MaxWeight; } }

    private int m_WeightAccumulation = 0;
    public int WeightAccumulation { get { return m_WeightAccumulation; } }

    public InventoryModel(Dictionary<string, ItemEntry> allItems, int maxWeight)
    {
        m_Items = new Dictionary<string, Item>();
        foreach (var item in allItems) {
            Item i = new Item(item.Value, 0);
            m_Items.Add(item.Key, i);
        }

        m_MaxWeight = maxWeight;
    }

    public void SetController(InventoryController controller)
    {
        m_Controller = controller;
    }

    public void AddItem(string id, int quantity)
    {
        if (Items.ContainsKey(id))
        {
            int newAmount = m_Items[id].m_Amount;
            newAmount += quantity;
            m_Items[id].m_Amount = newAmount; 
        }
        else
        {
            Item i = new Item(GameController.Instance.AllItems[id], quantity);
            m_Items.Add(id, i);
        }

        EventService.Instance.m_OnItemsAddedToInventory?.InvokeEvent();
    }

    public void RemoveItem(string id, int quantity) {
        if (Items.ContainsKey(id))
        {
            int newAmount = m_Items[id].m_Amount;
            newAmount -= quantity;
            if (newAmount <= 0)
                newAmount = 0;
            m_Items[id].m_Amount = newAmount;

            m_WeightAccumulation -= (int)m_Items[id].Details.m_Weight * quantity;

            EventService.Instance.m_OnItemsRemovedToInventory?.InvokeEvent();
        }
    }

    public ItemsAddInfoResult[] TryAddItems(ItemsAddInfo[] itemsToAdd)
    {
        List<Item> items = new List<Item>();
        Dictionary<ItemEntry, int> items2 = new Dictionary<ItemEntry, int>();
        foreach (var item in itemsToAdd)
        {
            Item i = new Item(Items[item.m_Id].Details, item.m_Quantity);
            items.Add(i);
        }

        items.Sort((a, b) =>
        {
            if((a.Details.m_Weight * a.m_Amount) < (b.Details.m_Weight * b.m_Amount)) return -1;
            else if((a.Details.m_Weight * a.m_Amount) > (b.Details.m_Weight * b.m_Amount)) return 1;
            return 0;
        });

        ItemsAddInfoResult[] results = new ItemsAddInfoResult[items.Count];
        int index = 0;
        foreach (var item in items)
        {
            int delta = m_MaxWeight - m_WeightAccumulation;
            int totalItemWeight = (int) item.Details.m_Weight * item.m_Amount;

            results[index].m_Id = item.Details.m_Id;
            results[index].m_Quantity = item.m_Amount;

            if (m_WeightAccumulation + totalItemWeight <= m_MaxWeight)
            {
                m_WeightAccumulation += totalItemWeight;
                results[index].m_ItemsAdded = item.m_Amount;
                AddItem(item.Details.m_Id, item.m_Amount);
            }
            else
            {
                int remainingCapacity = m_MaxWeight - m_WeightAccumulation;
                int maxAddable = (int) (remainingCapacity / item.Details.m_Weight);
                
                if(maxAddable > 0)
                {
                    m_WeightAccumulation += (int) (maxAddable * item.Details.m_Weight);
                    results[index].m_ItemsAdded = maxAddable;
                    AddItem(item.Details.m_Id, maxAddable);
                }
                else
                {
                    results[index].m_ItemsAdded = 0;
                }
            }
            index++;
        }

        return results;
    }

}

[Serializable]
public struct ItemsAddInfo
{
    public string m_Id;
    public int m_Quantity;
}

[Serializable]
public struct ItemsAddInfoResult
{
    public string m_Id;
    public int m_Quantity;
    public int m_ItemsAdded;
    public bool Success { get { return m_Quantity == m_ItemsAdded; } }
}