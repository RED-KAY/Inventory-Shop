using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryController
{
    InventoryModel _Model;
    InventoryView _View;

    public InventoryController(InventoryModel m, InventoryView v)
    {
        _Model = m;
        _View = v;
    }

    public void Initialize()
    {
        _Model.SetController(this);
        _View.SetController(this);

        PopulateShop();

        EventService.Instance._OnItemBought.AddListener(OnItemsBought);
        EventService.Instance._OnItemSold.AddListener(OnItemSold);
        EventService.Instance._TryAddItems.AddListener(TryAddItems);
    }

    public Dictionary<string, Item> GetAllItems()
    {
        return _Model.Items;
    }

    public Dictionary<string, Item> GetItemsToDisplay()
    {
        if (_Model._Filter <= 0 || _Model._Filter > 4)
        {
            _Model._Filter = 0;

            var filteredItems = _Model.Items.Values
                .Where(item => item._Amount > 0)
                    .ToDictionary(item => item.Details._Id, item => item);

            return filteredItems;
        }
        else
        {
            var filteredItems = _Model.Items.Values
                    .Where(item => item._Amount > 0 && item.Details._ItemType == (ItemType)_Model._Filter)
                        .ToDictionary(item => item.Details._Id, item => item);
            return filteredItems;
        }
    }

    public void OnItemSelected(string id)
    {

    }

    private void OnItemsBought(string id, int quantity)
    {
        _Model.AddItem(id, quantity);
        Refresh();
    }

    private void OnItemSold(string id, int quantity) { 
    
        _Model.RemoveItem(id, quantity);
        Refresh();

        EventService.Instance._OnItemSoldAddMoney?.InvokeEvent(quantity * _Model.Items[id].Details._SellingPrice);
    }

    public void Buy(string id, int amount)
    {

    }

    ItemsAddInfoResult[] TryAddItems(ItemsAddInfo[] _itemsToAdd)
    {
        return _Model.TryAddItems(_itemsToAdd);
    }

    public void PopulateShop()
    {
        _View.Refresh();
    }


    public void Refresh()
    {
        _View.Refresh();
    }

    internal void FilterChanged(int filter)
    {
        _Model._Filter = filter;
        PopulateShop();
    }

    public bool CanSell(string id, int quantity)
    {
        return _Model.Items[id]._Amount >= quantity;
    }
}

[Serializable]
public class InventoryModel
{
    [SerializeField] private Dictionary<string, Item> _Items;
    public Dictionary<string, Item> Items { get { return _Items; } }
    private InventoryController _Controller;
    public int _Filter = 0; //0: all, 1: materials, 2: weapons, 3: consumables, 4: trasures
    private int _MaxWeight;
    public int MaxWeight { get { return _MaxWeight; } }

    private int _WeightAccumulation = 0;

    public InventoryModel(Dictionary<string, ItemEntry> allItems)
    {
        _Items = new Dictionary<string, Item>();
        foreach (var item in allItems) {
            Item i = new Item(item.Value, 0);
            _Items.Add(item.Key, i);
        }
    }

    public void SetController(InventoryController controller)
    {
        _Controller = controller;
    }

    public void AddItem(string id, int quantity)
    {
        if (Items.ContainsKey(id))
        {
            int newAmount = _Items[id]._Amount;
            newAmount += quantity;
            _Items[id]._Amount = newAmount; 
        }
        else
        {
            Item i = new Item(GameController.Instance.AllItems[id], quantity);
            _Items.Add(id, i);
        }
    }

    public void RemoveItem(string id, int quantity) {
        if (Items.ContainsKey(id))
        {
            int newAmount = _Items[id]._Amount;
            newAmount -= quantity;
            if (newAmount <= 0)
                newAmount = 0;
            _Items[id]._Amount = newAmount;
        }
    }

    public ItemsAddInfoResult[] TryAddItems(ItemsAddInfo[] itemsToAdd)
    {
        List<ItemEntry> items = new List<ItemEntry>();
        foreach (var item in itemsToAdd)
        {
            items.Add(Items[item._Id].Details);
        }

        items.Sort((a, b) =>
        {
            if(a._Weight < b._Weight) return -1;
            else if(a._Weight > b._Weight) return 1;
            return 0;
        });

        foreach (var item in items)
        {
            Debug.Log(item._Weight);
        }

        return null;
    }
}

[Serializable]
public struct ItemsAddInfo
{
    public string _Id;
    public int _Quantity;
}

[Serializable]
public struct ItemsAddInfoResult
{
    public string _Id;
    public int _Quantity;
    public int _ItemsAdded;
    public bool Success { get { return _Quantity == _ItemsAdded; } }
}