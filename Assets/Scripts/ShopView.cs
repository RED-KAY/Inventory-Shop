using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopView : MonoBehaviour
{
    [SerializeField] private GameObject _ItemUIPrefab;
    [SerializeField] private Transform _ShopContents;
    [SerializeField] private ItemPopupView _ItemPopup;
    [SerializeField] Toggle[] _Toggles;
    [SerializeField] Color _DefaultColor;
    [SerializeField] Color _ActiveColor;
    Button b;

    private ShopController _Controller;
    int _CurrentActive;

    private void Start()
    {
        foreach (var item in _Toggles)
        {
            item.targetGraphic.color = _DefaultColor;
        }
        _Toggles[0].targetGraphic.color = _ActiveColor;
    }

    public void SetController(ShopController controller)
    {
        _Controller = controller;
    }

    public void Populate()
    {
        var itemsToDisplay = _Controller.GetItemsToDisplay();
        foreach (var item in itemsToDisplay)
        {
            GameObject newItemGO = Instantiate(_ItemUIPrefab, _ShopContents);
            ItemView newItem = newItemGO.GetComponent<ItemView>();
            newItem._Button.onClick.AddListener(() => OnItemSelected(item.Value));
            newItem._Icon.sprite = item.Value._Icon;
            newItem._Quantity.text = "$" + item.Value._Price.ToString();
            newItem._Rarity.sprite = GameController.Instance._Rarities[((int)item.Value._Rarity)];
            newItem.SetItem(item.Value, item.Value._Price, true);
        }

    }

    public void Clear()
    {
        for (int i = 0; i < _ShopContents.childCount; i++)
        {
            Destroy(_ShopContents.GetChild(i).gameObject);
        }
    }

    public void Refresh()
    {
        Clear();
        Populate();
    }

    public void OnItemSelected(ItemEntry item)
    {
        _ItemPopup?.Show(item._Id, item._Name, item._Price);
    }

    public void ChangeFilter(int filter)
    {
        if(_Controller == null) return;
        _Controller.FilterChanged(filter);

        _Toggles[_CurrentActive].targetGraphic.color = _DefaultColor;
        _Toggles[filter].targetGraphic.color = _ActiveColor;

        _CurrentActive = filter;
    }
}
