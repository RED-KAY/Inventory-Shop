using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopController
{
    ShopModel m_Model;
    ShopView m_View;

    public ShopController(ShopModel m, ShopView v)
    {
        m_Model = m;
        m_View = v;
    }

    public void Initialize()
    {
        m_Model.SetController(this);
        m_View.SetController(this);

        PopulateShop();
    }

    public Dictionary<string, ItemEntry> GetAllItemsEntry()
    {
        return m_Model.AllItems;
    }

    public Dictionary<string, ItemEntry> GetItemsToDisplay()
    {
        if (m_Model.m_Filter <= 0 || m_Model.m_Filter > 4)
        {
            m_Model.m_Filter = 0;
            return m_Model.AllItems;
        }
        else
        {
            var filteredItems = m_Model.AllItems.Values
                    .Where(item => item.m_ItemType == (ItemType)m_Model.m_Filter)
                        .ToDictionary(item => item.m_Id, item => item);
            return filteredItems;
        }
    }

    public void PopulateShop()
    {
        m_View.Refresh();
    }

    internal void FilterChanged(int filter)
    {
        m_Model.m_Filter = filter;
        PopulateShop();
    }
}

