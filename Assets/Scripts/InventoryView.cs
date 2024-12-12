using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private GameObject _ItemUIPrefab;
    [SerializeField] private Transform _InventoryContents;
    [SerializeField] private ItemPopupView _ItemPopup;
    [SerializeField] Toggle[] _Toggles;
    [SerializeField] Color _DefaultColor;
    [SerializeField] Color _ActiveColor;

    private InventoryController _Controller;
    int _CurrentActive;

    private void Start()
    {
        foreach (var item in _Toggles)
        {
            item.targetGraphic.color = _DefaultColor;
        }

        _Toggles[0].targetGraphic.color = _ActiveColor;
    }



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
            newItem._Quantity.text = item.Value._Amount.ToString();
            newItem._Rarity.sprite = GameController.Instance._Rarities[((int)item.Value.Details._Rarity)];
            newItem.SetItem(item.Value.Details, item.Value._Amount, false);
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
        _ItemPopup?.Show(item.Details._Id, item.Details._Name, item.Details._SellingPrice, item._Amount, 2);
    }

    public void ChangeFilter(int filter)
    {
        if (_Controller == null) return;

        _Controller.FilterChanged(filter);

        _Toggles[_CurrentActive].targetGraphic.color = _DefaultColor;
        _Toggles[filter].targetGraphic.color = _ActiveColor;

        _CurrentActive = filter;
    }
}
