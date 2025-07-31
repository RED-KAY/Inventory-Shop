using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ShopModel
{
    [SerializeField] private Dictionary<string, ItemEntry> m_AllItems;
    public Dictionary<string, ItemEntry> AllItems => m_AllItems;
    private ShopController m_Controller;
    public int m_Filter = 0; //0: all, 1: materials, 2: weapons, 3: consumables, 4: trasures

    public ShopModel(Dictionary<string, ItemEntry> allItems)
    {
        m_AllItems = new Dictionary<string, ItemEntry>();
        m_AllItems = allItems;
    }

    public void SetController(ShopController controller)
    {
        m_Controller = controller;
    }

}