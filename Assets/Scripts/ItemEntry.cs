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
public class ItemEntry : ScriptableObject
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
}
