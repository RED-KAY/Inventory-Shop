using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private GameObject m_ItemUIPrefab;
    [SerializeField] private Transform m_InventoryContents;
    [SerializeField] private ItemPopupView m_ItemPopup;
    [SerializeField] Toggle[] m_Toggles;
    [SerializeField] Color m_DefaultColor;
    [SerializeField] Color m_ActiveColor;

    private InventoryController m_Controller;
    int m_CurrentActive;
    [SerializeField] TextMeshProUGUI m_WeightT;

    bool m_Initilized = false;


    private void Start()
    {
        foreach (var item in m_Toggles)
        {
            item.targetGraphic.color = m_DefaultColor;
        }

        m_Toggles[0].targetGraphic.color = m_ActiveColor;
    }

    private void Update()
    {
        if (m_Initilized)
        {
            m_WeightT.text = m_Controller.WeightAccumulation().ToString() + "/" + m_Controller.MaxWeight();
        }
    }


    public void SetController(InventoryController controller)
    {
        m_Controller = controller;
        m_WeightT.text = m_Controller.MaxWeight().ToString();

        m_Initilized = true;
    }

    public void Populate()
    {
        var itemsToDisplay = m_Controller.GetItemsToDisplay();
        foreach (var item in itemsToDisplay)
        {
            GameObject newItemGO = Instantiate(m_ItemUIPrefab, m_InventoryContents);
            ItemView newItem = newItemGO.GetComponent<ItemView>();
            newItem.m_Button.onClick.AddListener(() => OnItemSelected(item.Value));
            newItem.m_Icon.sprite = item.Value.Details.m_Icon;
            newItem.m_Quantity.text = item.Value.m_Amount.ToString();
            newItem.m_Rarity.sprite = GameController.Instance.m_Rarities[((int)item.Value.Details.m_Rarity)];
            newItem.SetItem(item.Value.Details, item.Value.m_Amount, false);
        }

    }

    public void Clear()
    {
        for (int i = 0; i < m_InventoryContents.childCount; i++)
        {
            Destroy(m_InventoryContents.GetChild(i).gameObject);
        }
    }

    public void Refresh()
    {
        Clear();
        Populate();
    }

    public void OnItemSelected(Item item)
    {
        m_ItemPopup?.Show(item.Details.m_Id, item.Details.m_Name, item.Details.m_SellingPrice, item.m_Amount, 2);
    }

    public void ChangeFilter(int filter)
    {
        if (m_Controller == null) return;

        m_Controller.FilterChanged(filter);

        m_Toggles[m_CurrentActive].targetGraphic.color = m_DefaultColor;
        m_Toggles[filter].targetGraphic.color = m_ActiveColor;

        m_CurrentActive = filter;
    }
}
