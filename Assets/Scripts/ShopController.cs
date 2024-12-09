using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopController
{
    ShopModel _Model;
    ShopView _View;

    public ShopController(ShopModel m, ShopView v)
    {
        _Model = m;
        _View = v;

        _Model.SetController(this);
        _View.SetController(this);

        _Model.LoadAllItems();

        PopulateShop();
    }

    public Dictionary<string, ItemEntry> GetAllItemsEntry()
    {
        return _Model.AllItems;
    }

    public void OnItemSelected(string id) { 
    
    }

    public void Buy(string id, int amount)
    {

    }

    public void PopulateShop()
    {
        _View.Refresh();
    }
}


[Serializable]
public class ShopModel
{
    [SerializeField] private Dictionary<string, ItemEntry> _AllItems;
    public Dictionary<string, ItemEntry> AllItems => _AllItems;
    private ShopController _Controller;

    public ShopModel()
    {
        _AllItems = new Dictionary<string, ItemEntry>();
    }

    public void SetController(ShopController controller)
    {
        _Controller = controller;
    }

    public void LoadAllItems()
    {
        ItemEntry[] allItems = Resources.LoadAll<ItemEntry>("Items");

        foreach (ItemEntry item in allItems) { 
            _AllItems.Add(item._Id, item);
        }

        foreach (KeyValuePair<string, ItemEntry> keyValuePair in _AllItems) {
            Debug.Log(keyValuePair.Key);
        }
    }
}