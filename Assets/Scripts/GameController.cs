using System.Collections.Generic;
using UnityEngine;

public class GameController : GenericMonoSingleton<GameController>
{
    [SerializeField] private ShopView m_ShopView;
    ShopController m_ShopController;

    [SerializeField] private InventoryView m_InventoryView;
    InventoryController m_InventoryController;

    public Sprite[] m_Rarities;

    Dictionary<string, ItemEntry> m_AllItems;
    List<ItemEntry> m_ItemsList;

    public List<ItemEntry> ItemsList {  get { return m_ItemsList; } }

    public Dictionary<string, ItemEntry> AllItems { get { return m_AllItems; } }

    [SerializeField] RectTransform m_RectTransform;
    [SerializeField] TooltipView m_TooltipPrefab;

    private TooltipView m_TooltipView;
    public TooltipView TooltipView { get { return m_TooltipView; } }

    [SerializeField] int m_MaxWeight;

    private void Awake()
    {
        base.Awake();

        InitializeTooltip();
        LoadAllItems();

        ShopModel model = new ShopModel(m_AllItems);
        m_ShopController = new ShopController(model, m_ShopView);

        InventoryModel inventoryModel = new InventoryModel(m_AllItems, m_MaxWeight);
        m_InventoryController = new InventoryController(inventoryModel, m_InventoryView);

        m_ShopController.Initialize();
        m_InventoryController.Initialize();

        RandomDropSystem.Instance.Initiliaze();
    }

    public void LoadAllItems()
    {
        m_AllItems = new Dictionary<string, ItemEntry>();
        m_ItemsList = new List<ItemEntry>();

        ItemEntry[] allItems = Resources.LoadAll<ItemEntry>("Items");

        foreach (ItemEntry item in allItems)
        {
            m_AllItems.Add(item.m_Id, item);
            m_ItemsList.Add(item);
        }

        m_ItemsList.Sort();

        foreach (KeyValuePair<string, ItemEntry> keyValuePair in m_AllItems)
        {
            Debug.Log(keyValuePair.Key);
        }
    }

    private void InitializeTooltip()
    {
        m_TooltipView = Instantiate(m_TooltipPrefab, m_RectTransform) as TooltipView;
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
        return m_InventoryController.CanSell(id, quantity);
    }

    public int GetMaxWeight()
    {
        return m_InventoryController.MaxWeight();
    }

    public int GetWeightAccumulation()
    {
        return m_InventoryController.WeightAccumulation();
    }

}
