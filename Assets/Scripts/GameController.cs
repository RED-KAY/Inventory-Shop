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

    [SerializeField] RectTransform _RectTransform;
    [SerializeField] TooltipView _TooltipPrefab;

    private TooltipView _TooltipView;
    public TooltipView TooltipView { get { return _TooltipView; } }

    private void Awake()
    {
        base.Awake();

        InitializeTooltip();
        LoadAllItems();

        ShopModel model = new ShopModel(_AllItems);
        _ShopController = new ShopController(model, _ShopView);

        InventoryModel inventoryModel = new InventoryModel(_AllItems);
        _InventoryController = new InventoryController(inventoryModel, _InventoryView);

        _ShopController.Initialize();
        _InventoryController.Initialize();
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

    private void InitializeTooltip()
    {
        _TooltipView = Instantiate(_TooltipPrefab, _RectTransform) as TooltipView;
        HideTooltip();
    }

    public void SetInfoAndShowTooltip(ItemEntry item, bool isShop = true, int number = 0)
    {
        TooltipView.SetInfo(item, isShop, number);
        TooltipView.gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        TooltipView.gameObject.SetActive(false);
    }

    public void SetTooltipPosition(Vector3 pos)
    {
        TooltipView.transform.position = pos;
    }

    public bool CanSell(string id, int quantity)
    {
        return _InventoryController.CanSell(id, quantity);
    }

}
