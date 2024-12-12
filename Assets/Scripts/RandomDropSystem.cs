using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomDropSystem : GenericMonoSingleton<RandomDropSystem>
{
    public List<ItemEntry> _AllItems { get { return GameController.Instance.ItemsList; } }


    [Header("Rarity Weights")]
    public float _VeryCommonWeight = 50f;
    public float _CommonWeight = 30f;
    public float _RareWeight = 15f;
    public float _EpicWeight = 4f;
    public float _LegendaryWeight = 1f;

    private Dictionary<Rarity, float> _RarityWeights;
    private Dictionary<Rarity, List<ItemEntry>> _ItemsByRarity;

    [SerializeField] TextMeshProUGUI[] _ItemNames;
    [SerializeField] TextMeshProUGUI[] _Rarities;
    [SerializeField] TextMeshProUGUI[] _Statues;

    [SerializeField] GameObject _Popup;
    [SerializeField] int _DropCount = 5;

    List<ItemEntry> _DroppedItems;

    public void Initiliaze()
    {
        _DroppedItems = new List<ItemEntry>(_DropCount);
        _RarityWeights = new Dictionary<Rarity, float>
        {
            { Rarity.VeryCommon, _VeryCommonWeight },
            { Rarity.Common, _CommonWeight },
            { Rarity.Rare, _RareWeight },
            { Rarity.Epic, _EpicWeight },
            { Rarity.Legendary, _LegendaryWeight }
        };

        _ItemsByRarity = new Dictionary<Rarity, List<ItemEntry>>();
        foreach (Rarity r in System.Enum.GetValues(typeof(Rarity)))
        {
            _ItemsByRarity[r] = new List<ItemEntry>();
        }

        foreach (var item in _AllItems)
        {
            _ItemsByRarity[item._Rarity].Add(item);
        }


    }

    public List<ItemEntry> GetRandomDrop(int count)
    {
        List<ItemEntry> result = new List<ItemEntry>();
        for (int i = 0; i < count; i++)
        {
            result.Add(GetRandomItem());
        }
        return result;
    }

    private ItemEntry GetRandomItem()
    {
        float totalWeight = 0f;
        foreach (var kvp in _RarityWeights)
        {
            if (_ItemsByRarity[kvp.Key].Count > 0)
            {
                totalWeight += kvp.Value;
            }
        }

        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0f;
        Rarity chosenRarity = Rarity.VeryCommon; 

        foreach (var kvp in _RarityWeights)
        {
            if (_ItemsByRarity[kvp.Key].Count == 0) continue; 

            cumulative += kvp.Value;
            if (roll < cumulative)
            {
                chosenRarity = kvp.Key;
                break;
            }
        }

        List<ItemEntry> chosenRarityItems = _ItemsByRarity[chosenRarity];
        int randomIndex = Random.Range(0, chosenRarityItems.Count);
        return chosenRarityItems[randomIndex];
    }

    public void OnRandomDropClicked()
    {
        List<ItemEntry> drop = GetRandomDrop(_DropCount);
        _DroppedItems = drop;
        foreach (var droppedItem in drop)
        {
            Debug.Log("Dropped: " + droppedItem._Name + " (" + droppedItem._Rarity + ")");
        }

        for (int i = 0; i < drop.Count; i++)
        {
            _ItemNames[i].text = drop[i]._Name;
            _Rarities[i].text = drop[i]._Rarity.ToString();
            _Statues[i].text = "added to Inventory";
        }

        _Popup.SetActive(true);
    }

    public void Okay()
    {
        List<ItemAddInfo> iai = new List<ItemAddInfo>();
        for (int i = 0; i < _DroppedItems.Count; i++)
        {
            
        }
        _Popup.SetActive(false);
    }
}

