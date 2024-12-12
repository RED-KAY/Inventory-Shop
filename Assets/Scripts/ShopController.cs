using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopController
{
    ShopModel _Model;
    ShopView _View;

    public ShopController(ShopModel m, ShopView v)
    {
        _Model = m;
        _View = v;


    }

    public void Initialize()
    {
        _Model.SetController(this);
        _View.SetController(this);

        PopulateShop();
    }

    public Dictionary<string, ItemEntry> GetAllItemsEntry()
    {
        return _Model.AllItems;
    }

    public Dictionary<string, ItemEntry> GetItemsToDisplay()
    {
        if (_Model._Filter <= 0 || _Model._Filter > 4)
        {
            _Model._Filter = 0;
            return _Model.AllItems;
        }
        else
        {
            var filteredItems = _Model.AllItems.Values
                    .Where(item => item._ItemType == (ItemType)_Model._Filter)
                        .ToDictionary(item => item._Id, item => item);
            return filteredItems;
        }
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

    internal void FilterChanged(int filter)
    {
        _Model._Filter = filter;
        PopulateShop();
    }
}


[Serializable]
public class ShopModel
{
    [SerializeField] private Dictionary<string, ItemEntry> _AllItems;
    public Dictionary<string, ItemEntry> AllItems => _AllItems;
    private ShopController _Controller;
    public int _Filter = 0; //0: all, 1: materials, 2: weapons, 3: consumables, 4: trasures

    public ShopModel(Dictionary<string, ItemEntry> allItems)
    {
        _AllItems = new Dictionary<string, ItemEntry>();
        _AllItems = allItems;
    }

    public void SetController(ShopController controller)
    {
        _Controller = controller;
    }

}