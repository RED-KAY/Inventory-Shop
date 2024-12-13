using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image _Rarity;
    public Image _Icon;
    public TextMeshProUGUI _Quantity;
    public Button _Button;
    [SerializeField] GameObject _DisableIcon;

    private ItemEntry _ItemEntry;
    private int _Number;

    bool _Hovering = false;
    PointerEventData _PointerEventData;

    bool _IsShop = false;

    public void SetItem(ItemEntry itemEntry, int num, bool isShop)
    {
        _ItemEntry = itemEntry;
        _Number = num;
        _IsShop = isShop;
    }

    void Update()
    {
        if (_Hovering && _PointerEventData != null)
        {
            GameController.Instance.SetTooltipPosition(_PointerEventData.position);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_Hovering)
        {
            GameController.Instance.SetInfoAndShowTooltip(_ItemEntry, _IsShop, _Number);
            _PointerEventData = eventData;
            _Hovering = true;
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_Hovering == true)
        {
            GameController.Instance.HideTooltip();
            _PointerEventData = null;
            _Hovering = false;
        }
    }

    public void Disable()
    {
        _Button.interactable = false;
        _DisableIcon.SetActive(true);
    }

    public void Enable()
    {
        _Button.interactable = true;
        _DisableIcon.SetActive(false);
    }
}
