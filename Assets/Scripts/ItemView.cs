using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image m_Rarity;
    public Image m_Icon;
    public TextMeshProUGUI m_Quantity;
    public Button m_Button;
    [SerializeField] GameObject m_DisableIcon;

    private ItemEntry m_ItemEntry;
    private int m_Number;

    bool m_Hovering = false;
    PointerEventData m_PointerEventData;

    bool m_IsShop = false;

    public void SetItem(ItemEntry itemEntry, int num, bool isShop)
    {
        m_ItemEntry = itemEntry;
        m_Number = num;
        m_IsShop = isShop;
    }

    void Update()
    {
        if (m_Hovering && m_PointerEventData != null)
        {
            GameController.Instance.SetTooltipPosition(m_PointerEventData.position);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!m_Hovering)
        {
            GameController.Instance.SetInfoAndShowTooltip(m_ItemEntry, m_IsShop, m_Number);
            m_PointerEventData = eventData;
            m_Hovering = true;
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (m_Hovering == true)
        {
            GameController.Instance.HideTooltip();
            m_PointerEventData = null;
            m_Hovering = false;
        }
    }

    public void Disable()
    {
        m_Button.interactable = false;
        m_DisableIcon.SetActive(true);
    }

    public void Enable()
    {
        m_Button.interactable = true;
        m_DisableIcon.SetActive(false);
    }
}
