using UnityEngine;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private GameObject _ItemUIPrefab;
    [SerializeField] private Transform _InventoryContents;
    [SerializeField] private ItemPopupView _ItemPopup;

    private InventoryController _Controller;

    public void SetController(InventoryController controller)
    {
        _Controller = controller;
    }

    public void Populate()
    {
        var itemsToDisplay = _Controller.GetItemsToDisplay();
        foreach (var item in itemsToDisplay)
        {
            GameObject newItemGO = Instantiate(_ItemUIPrefab, _InventoryContents);
            ItemView newItem = newItemGO.GetComponent<ItemView>();
            newItem._Button.onClick.AddListener(() => OnItemSelected(item.Value));
            newItem._Icon.sprite = item.Value.Details._Icon;
            newItem._Quantity.text = string.Empty;
            newItem._Rarity.sprite = GameController.Instance._Rarities[((int)item.Value.Details._Rarity)];
        }

    }

    public void Clear()
    {
        for (int i = 0; i < _InventoryContents.childCount; i++)
        {
            Destroy(_InventoryContents.GetChild(i).gameObject);
        }
    }

    public void Refresh()
    {
        Clear();
        Populate();
    }

    public void OnItemSelected(Item item)
    {
        _ItemPopup?.Show(item.Details._Id, item.Details._Name, item.Details._Price);
    }

    public void ChangeFilter(int filter)
    {
        _Controller.FilterChanged(filter);
    }
}
