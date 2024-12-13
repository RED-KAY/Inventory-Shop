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

    ~InventoryController()
    {
        EventService.Instance._OnItemBought.RemoveListener(OnItemsBought);
        EventService.Instance._OnItemSold.RemoveListener(OnItemSold);
        EventService.Instance._TryAddItems.RemoveListener(TryAddItems);
        EventService.Instance._OnItemsAddedToInventory.RemoveListener(Refresh);
        EventService.Instance._OnItemsRemovedToInventory.RemoveListener(Refresh);
    }

    public void Initialize()
    {
        _Model.SetController(this);
        _View.SetController(this);

        PopulateShop();

        EventService.Instance._OnItemBought.AddListener(OnItemsBought);
        EventService.Instance._OnItemSold.AddListener(OnItemSold);
        EventService.Instance._TryAddItems.AddListener(TryAddItems);
        EventService.Instance._OnItemsAddedToInventory.AddListener(Refresh);
        EventService.Instance._OnItemsRemovedToInventory.AddListener(Refresh);
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
        ItemsAddInfo[] itemsAddInfos = new ItemsAddInfo[1];
        itemsAddInfos[0]._Id = id;
        itemsAddInfos[0]._Quantity = quantity;
        _Model.TryAddItems(itemsAddInfos);
        //_Model.AddItem(id, quantity);

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

    public int MaxWeight()
    {
        return _Model.MaxWeight;
    }

    public int WeightAccumulation()
    {
        return _Model.WeightAccumulation;
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
    public int WeightAccumulation { get { return _WeightAccumulation; } }

    public InventoryModel(Dictionary<string, ItemEntry> allItems, int maxWeight)
    {
        _Items = new Dictionary<string, Item>();
        foreach (var item in allItems) {
            Item i = new Item(item.Value, 0);
            _Items.Add(item.Key, i);
        }

        _MaxWeight = maxWeight;
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

        EventService.Instance._OnItemsAddedToInventory?.InvokeEvent();
    }

    public void RemoveItem(string id, int quantity) {
        if (Items.ContainsKey(id))
        {
            int newAmount = _Items[id]._Amount;
            newAmount -= quantity;
            if (newAmount <= 0)
                newAmount = 0;
            _Items[id]._Amount = newAmount;

            _WeightAccumulation -= (int)_Items[id].Details._Weight * quantity;

            EventService.Instance._OnItemsRemovedToInventory?.InvokeEvent();
        }
    }

    public ItemsAddInfoResult[] TryAddItems(ItemsAddInfo[] itemsToAdd)
    {
        List<Item> items = new List<Item>();
        Dictionary<ItemEntry, int> items2 = new Dictionary<ItemEntry, int>();
        foreach (var item in itemsToAdd)
        {
            Item i = new Item(Items[item._Id].Details, item._Quantity);
            items.Add(i);
            //items2.Add(Items[item._Id].Details, item._Quantity);
        }

        items.Sort((a, b) =>
        {
            if((a.Details._Weight * a._Amount) < (b.Details._Weight * b._Amount)) return -1;
            else if((a.Details._Weight * a._Amount) > (b.Details._Weight * b._Amount)) return 1;
            return 0;
        });

        ItemsAddInfoResult[] results = new ItemsAddInfoResult[items.Count];
        int index = 0;
        foreach (var item in items)
        {
            int delta = _MaxWeight - _WeightAccumulation;
            int totalItemWeight = (int) item.Details._Weight * item._Amount;

            results[index]._Id = item.Details._Id;
            results[index]._Quantity = item._Amount;

            if (_WeightAccumulation + totalItemWeight <= _MaxWeight)
            {
                _WeightAccumulation += totalItemWeight;
                results[index]._ItemsAdded = item._Amount;
                AddItem(item.Details._Id, item._Amount);
            }
            else
            {
                int remainingCapacity = _MaxWeight - _WeightAccumulation;
                int maxAddable = (int) (remainingCapacity / item.Details._Weight);
                
                if(maxAddable > 0)
                {
                    _WeightAccumulation += (int) (maxAddable * item.Details._Weight);
                    results[index]._ItemsAdded = maxAddable;
                    AddItem(item.Details._Id, maxAddable);
                }
                else
                {
                    results[index]._ItemsAdded = 0;
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