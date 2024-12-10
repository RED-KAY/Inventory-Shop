using System.Collections.Generic;
using UnityEngine;

public class GameController : GenericMonoSingleton<GameController>
{
    [SerializeField] private ShopView _ShopView;
    ShopController _ShopController;

    [SerializeField] private InventoryView _InventoryView;
    InventoryController _InventoryController;

    public Sprite[] _Rarities;

    Dictionary<string, ItemEntry> _AllItems;

    public Dictionary<string, ItemEntry> AllItems { get { return _AllItems; } }

    private void Start()
    {
        LoadAllItems();

        ShopModel model = new ShopModel(_AllItems);
        _ShopController = new ShopController(model, _ShopView);

        InventoryModel inventoryModel = new InventoryModel(_AllItems);
        _InventoryController = new InventoryController(inventoryModel, _InventoryView);
    }

    public void LoadAllItems()
    {
        _AllItems = new Dictionary<string, ItemEntry>();

        ItemEntry[] allItems = Resources.LoadAll<ItemEntry>("Items");

        foreach (ItemEntry item in allItems)
        {
            _AllItems.Add(item._Id, item);
        }

        foreach (KeyValuePair<string, ItemEntry> keyValuePair in _AllItems)
        {
            Debug.Log(keyValuePair.Key);
        }
    }

    public bool CanSell(string id, int quantity)
    {
        return _InventoryController.CanSell(id, quantity);
    }

}
