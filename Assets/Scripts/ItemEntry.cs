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
    public string _Id;
    public string _Name;
    public string _Description;
    public Sprite _Icon;
    public ItemType _ItemType;
    public Rarity _Rarity;
    public float _Weight;
    public int _Price;
    public int _SellingPrice;

    public int CompareTo(object obj)
    {
        var a = this;
        var b = obj as ItemEntry;

        if(((int)a._Rarity) > ((int)b._Rarity))
        {
            return 1;
        }else if(((int)a._Rarity) < ((int)b._Rarity))
        {
            return -1;
        }

        return 0;
    }
}
