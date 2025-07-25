using System;
using UnityEngine;

public enum ItemType
{
    Materials = 1,
    Weapons,
    Consumables,
    Treasure
}

public enum Rarity
{
    VeryCommon,
    Common,
    Rare,
    Epic,
    Legendary
}

[CreateAssetMenu(fileName = "New Item", menuName ="Item")]
public class ItemEntry : ScriptableObject, IComparable
{
    public string m_Id;
    public string m_Name;
    public string m_Description;
    public Sprite m_Icon;
    public ItemType m_ItemType;
    public Rarity m_Rarity;
    public float m_Weight;
    public int m_Price;
    public int m_SellingPrice;

    public int CompareTo(object obj)
    {
        var a = this;
        var b = obj as ItemEntry;

        if(((int)a.m_Rarity) > ((int)b.m_Rarity))
        {
            return 1;
        }else if(((int)a.m_Rarity) < ((int)b.m_Rarity))
        {
            return -1;
        }

        return 0;
    }
}
