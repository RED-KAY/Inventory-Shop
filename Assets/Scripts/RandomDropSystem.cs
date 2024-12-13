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



        _Popup.SetActive(true);

        Dictionary<string, int> itemsToAdd = new Dictionary<string, int>();
        
        for (int i = 0; i < _DroppedItems.Count; i++)
        {
            string k = _DroppedItems[i]._Id;
            int v = 1;
            if (itemsToAdd.ContainsKey(k))
            {
                itemsToAdd[k] += v;
            }
            else
            {
                itemsToAdd.Add(k, v);
            }
        }

        ItemsAddInfo[] info = new ItemsAddInfo[itemsToAdd.Count];
        int index = 0;
        foreach (var item in itemsToAdd)
        {
            info[index]._Id = item.Key;
            info[index]._Quantity = item.Value;
            index++;
        }

        ItemsAddInfoResult[] itemsAddInfoResults = EventService.Instance._TryAddItems?.Invoke(info);
        int flag = 0;
        for (int i = 0; i < itemsAddInfoResults.Length; i++)
        {
            ItemsAddInfoResult result = itemsAddInfoResults[i];
            for (int j = 0; j < result._Quantity; j++) {
                _ItemNames[flag].text = GameController.Instance.AllItems[result._Id]._Name;
                _Rarities[flag].text = GameController.Instance.AllItems[result._Id]._Rarity.ToString();
                _Statues[flag].text = result._ItemsAdded > 0 ? "added to Inventory" : "cannot be added";
                result._ItemsAdded--;
                flag++;
            }

        }
    }

    public void Okay()
    {

        _Popup.SetActive(false);
    }
}

