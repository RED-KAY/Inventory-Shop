using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    InventoryModel _Model;
    InventoryView _View;

    public InventoryController(InventoryModel m, InventoryView v)
    {
        _Model = m;
        _View = v;

        _Model.SetController(this);
        _View.SetController(this);

        //_Model.LoadAllItems();

        PopulateShop();
    }

    public Dictionary<string, Item> GetAllItems()
    {
        return _Model.AllItems;
    }

    public Dictionary<string, Item> GetItemsToDisplay()
    {
        if (_Model._Filter <= 0 || _Model._Filter > 4)
        {
            _Model._Filter = 0;
            return _Model.AllItems;
        }
        else
        {
            var filteredItems = _Model.AllItems.Values
                    .Where(item => item.Details._ItemType == (ItemType)_Model._Filter)
                        .ToDictionary(item => item.Details._Id, item => item);
            return filteredItems;
        }
    }

    public void OnItemSelected(string id)
    {

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
public class InventoryModel
{
    [SerializeField] private Dictionary<string, Item> _AllItems;
    public Dictionary<string, Item> AllItems => _AllItems;
    private InventoryController _Controller;
    public int _Filter = 0; //0: all, 1: materials, 2: weapons, 3: consumables, 4: trasures

    public InventoryModel()
    {
        _AllItems = new Dictionary<string, Item>();
    }

    public void SetController(InventoryController controller)
    {
        _Controller = controller;
    }

}
